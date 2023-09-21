import torch
import torch.nn as nn

import torchvision


class SeparableConv2d(nn.Module):
    def __init__(self, in_channels, out_channels, depth, kernel_size, bias=False):
        super(SeparableConv2d, self).__init__()
        self.depthwise = nn.Conv2d(in_channels, out_channels*depth, kernel_size=kernel_size, groups=in_channels, bias=bias)
        self.pointwise = nn.Conv2d(out_channels*depth, out_channels, kernel_size=1, bias=bias)

    def forward(self, x):
        out = self.depthwise(x)
        out = self.pointwise(out)
        #print(out.size())
        return out

class conv_v1(nn.Module):
    def __init__(self):
        super(conv_v1, self).__init__()
        
        #self.input_size = 100
        
        """convs = [
            SeparableConv2d(in_channels=1, out_channels=32, depth=3, kernel_size=(32,32)),
            SeparableConv2d(in_channels=32, out_channels=64, depth=3, kernel_size=(32,32)),
            SeparableConv2d(in_channels=64, out_channels=64, depth=3, kernel_size=(32,32)),
            nn.GELU(),
            nn.GroupNorm(8, 64),
            SeparableConv2d(in_channels=64, out_channels=64, depth=3, kernel_size=(16,16)),
            SeparableConv2d(in_channels=64, out_channels=64, depth=3, kernel_size=(16,16)),
            SeparableConv2d(in_channels=64, out_channels=128, depth=3, kernel_size=(16,16)),
            nn.GELU(),
            nn.MaxPool2d(2),
            nn.GroupNorm(16, 128),
            SeparableConv2d(in_channels=128, out_channels=128, depth=3, kernel_size=(8,8)),
            SeparableConv2d(in_channels=128, out_channels=128, depth=3, kernel_size=(8,8)),
            SeparableConv2d(in_channels=128, out_channels=256, depth=3, kernel_size=(8,8)),
            nn.GELU(),
            nn.GroupNorm(32, 256),
            SeparableConv2d(in_channels=256, out_channels=256, depth=3, kernel_size=(8,8)),
            SeparableConv2d(in_channels=256, out_channels=512, depth=3, kernel_size=(8,8)),
            nn.GELU(),
            nn.GroupNorm(32, 512),
            SeparableConv2d(in_channels=512, out_channels=512, depth=3, kernel_size=(4,4)),
            SeparableConv2d(in_channels=512, out_channels=1024, depth=3, kernel_size=(4,4)),
            nn.GELU(),
            nn.GroupNorm(32, 1024),
            SeparableConv2d(in_channels=1024, out_channels=1024, depth=1, kernel_size=(2,2)),
            nn.GELU(),
            nn.GroupNorm(32, 1024)
        ]
        
        self.conv = nn.Sequential(*convs)"""
        
        """convs = [
            nn.Conv2d(1 , 32, kernel_size=32, stride=1),
            nn.Conv2d(32, 64, kernel_size=16, stride=1),
            nn.MaxPool2d(2),
            nn.Conv2d(64, 128, kernel_size=8, stride=2, dilation=1),
            nn.Conv2d(128, 256, kernel_size=4, stride=1, dilation=2),
            nn.MaxPool2d(2),
            nn.Conv2d(256, 512, kernel_size=4, stride=1, dilation=1),
            nn.Conv2d(512, 1024, kernel_size=4, stride=1, dilation=2),
            nn.MaxPool2d(2),
            nn.Conv2d(1024, 2048, kernel_size=3, stride=2),
            nn.Conv2d(2048, 4096, kernel_size=1, stride=1)
        ]
        
        self.conv = nn.Sequential(*convs)"""
        
        self.resnext = torchvision.models.resnext50_32x4d()
        self.wide_resnet = torchvision.models.wide_resnet50_2()
        
        self.linear1 = nn.Linear(1000, 512)
        self.linear2 = nn.Linear(512, 128)
        self.linear3 = nn.Linear(256, 1)
            
        self.gelu = nn.GELU()
        #self.gelu_threshold = 0.7
        
        self.sigmoid = nn.Sigmoid()
        
    def forward(self, x):
        """feature_maps = self.conv(x)"""
        feature_maps1 = self.resnext(x)
        feature_maps2 = self.wide_resnet(x)
        
        #print(torch.flatten(feature_maps, start_dim=1).size())
        #print(torch.flatten(feature_maps2, start_dim=1).size())
        
        hidden1 = self.gelu(self.linear1(torch.flatten(feature_maps1, start_dim=1)))
        hidden1 = self.gelu(self.linear2(hidden1))
        
        hidden2 = self.gelu(self.linear1(torch.flatten(feature_maps2, start_dim=1)))
        hidden2 = self.gelu(self.linear2(hidden2))
        
        hidden = torch.cat((hidden1, hidden2), 1)
        
        #out = hidden
        out = self.sigmoid(self.linear3(hidden))
        out = torch.squeeze(out)
        out = out.type(torch.float)
        return out