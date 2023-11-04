import os
import torch
import random
import argparse
import numpy as np
from data import pneumonia_dataset as ds
from models.cnn_ensemble import EnsembleCNN
from torchvision import datasets, transforms

#Define arguments to parse when training the model.
def get_args(parser):
    parser.add_argument("--batch_size", type=int, default=1)
    parser.add_argument("--data_dir", type=str, default="pneumonia diagnosis/inputs/chest_xray/")
    parser.add_argument("--lr", type=float, default=1e-4)
    parser.add_argument("--epochs", type=int, default=20)
    parser.add_argument("--savedir", type=str, default="pneumonia diagnosis/savepath/")
    parser.add_argument("--device", type=str, default='cpu')

    #Dataset args.
    parser.add_argument("--augment_dataset", type = bool, default = True)

#Prepare the dataset.
def prepare_dataset(args):
    train_dir = os.path.join(args.data_dir, 'train')
    test_dir = os.path.join(args.data_dir, 'test')
    valid_dir = os.path.join(args.data_dir, 'val')

    #Dataset transformations.
    train_transform = transforms.Compose([
        transforms.Resize((224, 224)),
        transforms.RandomHorizontalFlip(p=0.5),
        transforms.RandomVerticalFlip(p=0.5),
        transforms.GaussianBlur(kernel_size=(5, 9), sigma=(0.1, 5)),
        transforms.RandomRotation(degrees=(30, 70)),
        transforms.ToTensor(),
        transforms.Normalize(
            mean=[0.5, 0.5, 0.5],
            std=[0.5, 0.5, 0.5]
        )
    ])

    #Validation transforms
    valid_transform = transforms.Compose([
        transforms.Resize((224, 224)),
        transforms.ToTensor(),
        transforms.Normalize(
            mean=[0.5, 0.5, 0.5],
            std=[0.5, 0.5, 0.5]
        )
    ])

    train_ds = datasets.ImageFolder(train_dir, transform = train_transform)
    train_dataloader = torch.utils.data.DataLoader(train_ds, batch_size=args.batch_size)

    valid_ds = datasets.ImageFolder(valid_dir, transform = valid_transform)
    valid_dataloader = torch.utils.data.DataLoader(valid_ds, batch_size=args.batch_size, shuffle=False)

    test_ds = datasets.ImageFolder(test_dir, transform = valid_transform)
    test_dataloader = torch.utils.data.DataLoader(test_ds, batch_size=args.batch_size, shuffle=False)

    return train_dataloader, test_dataloader, valid_dataloader


def prepare_model(args):

parser = argparse.ArgumentParser(description="Train Model")
get_args(parser)
args, remaining_args = parser.parse_known_args()
train_dataloader, test_dataloader, valid_dataloader = prepare_dataset(args)
