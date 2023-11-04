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
    parser.add_argument("--batch_size", type=int, default=8)
    parser.add_argument("--data_dir", type=str, default="datasets/train/fakeavceleb*")
    parser.add_argument("--lr", type=float, default=1e-4)
    parser.add_argument("--epochs", type=int, default=20)
    parser.add_argument("--savedir", type=str, default="./savepath/")
    parser.add_argument("--device", type=str, default='cpu')

    #Dataset args.
    parser.add_argument("--augment_dataset", type = bool, default = True)
    parser.add_argument("--img_size", type=int, default=224)

#Prepare the dataset.
def prepare_dataset(args):
    img_size = args.img_size
    train_df, test_df, valid_df = ds.generate_dataframes(args.data_dir)
    train_dir = os.path.join(args.data_dir, 'train')
    test_dir = os.path.join(args.data_dir, 'test')
    valid_dir = os.path.join(args.data_dir, 'val')

    #Dataset transformations.
    transform = transforms.Compose([transforms.Resize((img_size, img_size)),
                                    transforms.RandomRotation(30),
                                    transforms.RandomHorizontalFlip(p=0.5),
                                    transforms.ToTensor()])

    train_ds = datasets.ImageFolder(train_dir, transform)
    weights = ds.unbalanced_dataset_weights(train_ds.imgs)
    weights = torch.tensor(weights, dtype=torch.double)
    sampler = torch.utils.data.sampler.WeightedRandomSampler(weights, len(weights))
    train_dataloader = torch.utils.data.DataLoader(train_ds, batch_size=args.batch_size, sampler= sampler)

    valid_ds = datasets.ImageFolder(valid_dir, transform)
    valid_dataloader = torch.utils.data.DataLoader(valid_ds, batch_size=args.batch_size, shuffle=False)

    test_ds = datasets.ImageFolder(test_dir, transform)
    test_dataloader = torch.utils.data.DataLoader(test_ds, batch_size=args.batch_size, shuffle=False)

    return train_dataloader, test_dataloader, valid_dataloader

parser = argparse.ArgumentParser(description="Train Model")
get_args(parser)
args, remaining_args = parser.parse_known_args()
print(args)
print(remaining_args)
# assert remaining_args == [], remaining_args
# #Test data loader.
# train_loader, test_loader, valid_loader = prepare_dataset
# # #Prepare the model.
# # def prepare_model(args):

