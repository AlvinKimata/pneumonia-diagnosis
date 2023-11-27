
import torch
import torch.nn as nn
import torch.nn.functional as F
import numpy as np
import timm
import pytorch_lightning as pl
from torchvision.models import resnet34

#Hyperparameters.
h = {
    "num_epochs": 10,
    "batch_size": 64,
    "image_size": 224,
    "fc1_size": 512,
    "lr": 0.001,
    "model": "efficientnetv2",
    "scheduler": "CosineAnnealingLR10",
    "balance": True,
    "early_stopping_patience": float("inf"),
    "use_best_checkpoint": False
}

device = 'cuda' if torch.cuda.is_available() else 'cpu'

class PneumoniaModel(pl.LightningModule):
    def __init__(self, h):
        super().__init__()
        self.h = h
        self.model = self._create_model()
        self.criterion = nn.CrossEntropyLoss()
        self.test_outputs = []

    def forward(self, x):
        return self.model(x)

    def training_step(self, batch, batch_idx):
        inputs, labels = batch
        outputs = self(inputs)
        loss = self.criterion(outputs, labels)
        self.log('train_loss', loss, on_epoch=True, on_step=True, prog_bar=True)
        return loss

    def validation_step(self, batch, batch_idx):
        inputs, labels = batch
        outputs = self(inputs)
        loss = self.criterion(outputs, labels)
        acc = (outputs.argmax(dim=1) == labels).float().mean()
        metrics = {"val_loss": loss, "val_acc": acc}
        self.log_dict(metrics, on_epoch=True, on_step=True, prog_bar=True)
        return metrics        

    def configure_optimizers(self):
        optimizer = torch.optim.Adam(self.parameters(), lr=self.h["lr"])
        scheduler_dic = self._configure_scheduler(optimizer)

        if (scheduler_dic["scheduler"]):
            return {
                "optimizer": optimizer,
                "lr_scheduler": scheduler_dic
            }            
        else:
            return optimizer

    def _configure_scheduler(self, optimizer):
        scheduler_name = self.h["scheduler"]
        lr = self.h["lr"]
        if (scheduler_name==""):
            return {
                "scheduler": None
            }
        if (scheduler_name=="CosineAnnealingLR10"):
            scheduler = torch.optim.lr_scheduler.CosineAnnealingLR(optimizer, T_max=h["num_epochs"], eta_min=lr*0.1) #*len(train_loader) if "step"
            return {
                "scheduler": scheduler,
                "interval": "epoch"
            }
        if (scheduler_name=="ReduceLROnPlateau5"):
            scheduler = torch.optim.lr_scheduler.ReduceLROnPlateau(optimizer, mode='min', factor=0.1, patience=5, verbose=True)
            return {
                "scheduler": scheduler,
                "interval": "epoch",
                "monitor": "val_loss",
                "strict": True
            }
        print ("Error. Unknown scheduler name '{scheduler_name}'")
        return None

    def _create_model(self):
        if (self.h["model"]=="efficientnetv2"):
            return timm.create_model("tf_efficientnetv2_b0", pretrained=True, num_classes=2)
        if (self.h["model"]=="fc"):
            return nn.Sequential(
                nn.Flatten(),
                nn.Linear(3 * self.h["image_size"] * self.h["image_size"], self.h["fc1_size"]),
                nn.ReLU(),
                nn.Linear(self.h["fc1_size"], 2)
            )
        if (self.h["model"]=="cnn"):
            return nn.Sequential(
                nn.Conv2d(3, 16, 3, padding=1),
                nn.ReLU(),
                nn.MaxPool2d(2, 2),
                nn.Conv2d(16, 32, 3, padding=1),
                nn.ReLU(),
                nn.MaxPool2d(2, 2),
                nn.Conv2d(32, 64, 3, padding=1),
                nn.ReLU(),
                nn.MaxPool2d(2, 2),
                nn.Flatten(),
                nn.Dropout(0.25),
                nn.Linear(64 * (self.h["image_size"] // 8) * (self.h["image_size"] // 8), 512),
                nn.ReLU(),
                nn.Dropout(0.25),
                nn.Linear(512, 2)
            )
        if (self.h["model"]=="resnet34"):
            model = resnet34(pretrained=True)
            num_features = model.fc.in_features
            model.fc = nn.Linear(num_features, 2)
            return model       


def create_model(best_model_path):
    # model = timm.create_model("tf_efficientnetv2_b0", pretrained=False, num_classes=2)
    best_model = PneumoniaModel.load_from_checkpoint(best_model_path, h=h, map_location = device)
    return best_model