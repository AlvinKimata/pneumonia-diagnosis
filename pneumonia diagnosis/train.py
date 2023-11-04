import os
import torch
import random
import argparse
import numpy as np
from tqdm import tqdm
import torch.nn as nn
import torch.optim as optim
from data import pneumonia_dataset as ds
from models.cnn_ensemble import EnsembleCNN
from torchvision import  transforms

#Define arguments to parse when training the model.
def get_args(parser):
    parser.add_argument("--batch_size", type=int, default=2)
    parser.add_argument("--train_ds", type=str, default="pneumonia diagnosis/inputs/train.csv")
    parser.add_argument("--val_ds", type=str, default="pneumonia diagnosis/inputs/valid.csv")
    parser.add_argument("--test_ds", type=str, default="pneumonia diagnosis/inputs/test.csv")
    parser.add_argument("--lr", type=float, default=1e-4)
    parser.add_argument("--epochs", type=int, default=20)
    parser.add_argument("--savedir", type=str, default="pneumonia diagnosis/savepath/")
    parser.add_argument("--device", type=str, default='cpu')

    #Dataset args.
    parser.add_argument("--augment_dataset", type = bool, default = True)

#Prepare the dataset.
def prepare_dataset(args):
    #Dataset transformations.
    train_transform = transforms.Compose([
        transforms.ToTensor(),
        transforms.Resize((224, 224)),
        transforms.RandomHorizontalFlip(p=0.5),
        transforms.RandomVerticalFlip(p=0.5),
        transforms.GaussianBlur(kernel_size=(5, 9), sigma=(0.1, 5)),
        transforms.RandomRotation(degrees=(30, 70)),
        transforms.Normalize(
            mean=[0.5, 0.5, 0.5],
            std=[0.5, 0.5, 0.5]
        )
    ])

    #Test and validation transforms.
    valid_transform = transforms.Compose([
        transforms.ToTensor(),
        transforms.Resize((224, 224)),
        transforms.Normalize(
            mean=[0.5, 0.5, 0.5],
            std=[0.5, 0.5, 0.5]
        )
    ])

    train_ds = ds.PneumoniaDataset(data = args.train_ds, transform=train_transform)
    train_dataloader = torch.utils.data.DataLoader(train_ds, batch_size=args.batch_size, shuffle = True)

    valid_ds = ds.PneumoniaDataset(data = args.val_ds, transform = valid_transform)
    valid_dataloader = torch.utils.data.DataLoader(valid_ds, batch_size=args.batch_size, shuffle=False)

    test_ds = ds.PneumoniaDataset(data = args.test_ds, transform = valid_transform)
    test_dataloader = torch.utils.data.DataLoader(test_ds, batch_size=args.batch_size, shuffle=False)

    return train_dataloader, test_dataloader, valid_dataloader


def prepare_model():
    model = EnsembleCNN()
    return model

def main(args):
    #Main python script for training the model.
    #Prepare the model.
    model = prepare_model()
    criterion = nn.BCELoss()
    optimizer = optim.Adam(model.parameters(), lr=args.lr, weight_decay=1e-5)

    #Prepare the dataset.
    train_ds, test_ds, valid_ds = prepare_dataset(args)


    for epoch in range(args.epochs):
        running_loss = 0.0
        train_losses = []
        model.train()

        print(f"Epoch {epoch} of {args.epochs}")

        for index, batch in tqdm(enumerate(train_ds)):
            images, labels = batch
            
            images = images.to(args.device)
            labels = labels.to(args.device)
            labels = labels.view(-1, 1)

            # zero the parameter gradients
            optimizer.zero_grad()

            outputs = model(images)
            outputs = torch.abs(outputs)
            loss = criterion(outputs, labels.float())

            loss.backward()
            optimizer.step()

            #Print statistics.
            running_loss += loss.item()

            if index % 10 == 0:    # print every 10 mini-batches
                print(f'[{epoch + 1}, {index + 1:5d}] loss: {running_loss / 2000:.3f}')
                running_loss = 0.0

            train_losses.append(loss.item())


if __name__ == "__main__":
    import warnings
    warnings.filterwarnings("ignore")

    parser = argparse.ArgumentParser(description="Train Model")
    get_args(parser)
    args, remaining_args = parser.parse_known_args()
    main(args)