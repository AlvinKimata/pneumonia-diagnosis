import torch
import torch.nn as nn
import torch.nn.functional as F

import torchvision
import googlenet

"""To design an ensemble framework consisting of three CNN models 
(GoogLeNet, ResNet-18, and DenseNet-121)
for pneumonia detection in chest X-ray images."""

class EnsembleCNN(nn.Module):
    def __init__(self):
        super().__init__()
        self.resnet_model = torchvision.models.resnet101()
        self.efficientnet_model = torchvision.models.efficientnet_b0()
        self.googlenet_model = googlenet.GoogLeNet()
    
    def forward(self, x):
        resnet_inputs = self.resnet_model(x)
        efficnetnet_inputs = self.efficientnet_model(x)
        googlenet_outputs = self.googlenet_model(x)


        inputs = torch.concat([resnet_inputs, efficnetnet_inputs, googlenet_outputs[0]], dim = 0)
        x = nn.Linear(1000, 500)(inputs)
        x = F.relu(x)
        x = nn.Dropout(0.3)(x)
        x = nn.Linear(500, 200)(x)
        x = F.relu(x)
        x = nn.Dropout(0.3)(x)
        x = nn.Linear(200, 1)(x)
        out_mean = torch.mean(x)

        return out



    

