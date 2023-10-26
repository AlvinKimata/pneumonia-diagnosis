import torch
import torch.nn as nn
import torch.nn.functional as F

import torchvision

"""To design an ensemble framework consisting of three CNN models 
(GoogLeNet, ResNet-18, and DenseNet-121)
for pneumonia detection in chest X-ray images."""

class EnsembleCNN(nn.Module):
    def __init__(self):
        super().__init__()
        self.resnet_model = torchvision.models.resnet101()
        self.efficientnet_model = torchvision.models.efficientnet_b0()
        self.googlenet_model = torchvision.models.googlenet()
    
    def forward(self, x):
        resnet_inputs = self.resnet_model(x)
        efficnetnet_inputs = self.efficientnet_model(x)
        googlenet_inputs = self.googlenet_model(x)

        inputs = torch.concat([resnet_inputs, efficnetnet_inputs, googlenet_inputs])
        return inputs



    

