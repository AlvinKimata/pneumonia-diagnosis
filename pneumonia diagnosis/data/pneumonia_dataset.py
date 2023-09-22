import cv2
import random
import numpy as np
import pandas as pd
from tqdm import tqdm

import torch
import torch.nn as nn
from torch.utils.data import ConcatDataset, Dataset
import torchvision
from torchvision import datasets, transforms


def seed_everything(seed):
    random.seed(seed)
    np.random.seed(seed)
    if device == 'cuda':
        torch.manual_seed(seed)
        torch.cuda.manual_seed(seed)
        torch.backends.cudnn.deterministic = True


SEED = 42 
batch_s = 16
seed_everything(SEED)
device = torch.device("cuda" if torch.cuda.is_available() else "cpu")


train_dir = '../input/chest-xray-pneumonia/chest_xray/train'
valid_dir = '../input/chest-xray-pneumonia/chest_xray/val'
test_dir  = '../input/chest-xray-pneumonia/chest_xray/test'

img_size = 224

# unused (actually used for plotting), better way found (marked as "* better way")
def  generate_dataframes(root_dir):
    train_df = pd.DataFrame(columns=['xray_dir', 'pneumonia (label)'])
    test_df  = pd.DataFrame(columns=['xray_dir', 'pneumonia (label)'])
    valid_df = pd.DataFrame(columns=['xray_dir', 'pneumonia (label)'])
    
    for dirname, _, filenames in os.walk(root_dir):     
        
        # Train
        if dirname == root_dir + 'train/' + 'NORMAL':
            for filename in filenames:
                train_df.loc[len(train_df)] = [dirname + '/' + filename, 0]

        elif dirname == root_dir + 'train/' + 'PNEUMONIA':
            for filename in filenames:
                train_df.loc[len(train_df)] = [dirname + '/' + filename, 1]
        
        # Test
        if dirname == root_dir + 'test/' + 'NORMAL':
            for filename in filenames:
                test_df.loc[len(test_df)] = [dirname + '/' + filename, 0]

        elif dirname == root_dir + 'test/' + 'PNEUMONIA':
            for filename in filenames:
                test_df.loc[len(test_df)] = [dirname + '/' + filename, 1]
        
        # Validation
        if dirname == root_dir + 'val/' + 'NORMAL':
            for filename in filenames:
                valid_df.loc[len(valid_df)] = [dirname + '/' + filename, 0]

        elif dirname == root_dir + 'val/' + 'PNEUMONIA':
            for filename in filenames:
                valid_df.loc[len(valid_df)] = [dirname + '/' + filename, 1]
                
    return train_df, test_df, valid_df

train_df, test_df, valid_df = generate_dataframes('../input/chest-xray-pneumonia/chest_xray/')


class Dataset(Dataset):
    def __init__(self, data, target, transform=None, device=device):
        self.device = device
        self.data = data
        self.target = target
        self.transform = transform
                
    def __len__(self):
        return len(self.data)
    
    def __getitem__(self, index):
        x = self.data[index].to(self.device)
        y = self.target[index].to(self.device)
        
        #if (y == 0) and self.transform: # check for minority class
        x = self.transform(x)
        
        return x, y
    

# Fixing unblaanced dataset with a weighted sampler
# based on code given by https://discuss.pytorch.org/t/balanced-sampling-between-classes-with-torchvision-dataloader/2703/2

def unbalanced_dataset_weights(instances):
    count = [0] * 2
    for item in instances:
        count[item[1]] += 1
        
    class_weight = [0.] * 2
    total = float(sum(count))
    
    for i in range(2):
        class_weight[i] = total/float(count[i])
    
    weight = [0] * len(instances)
    
    for index, value in enumerate(instances):
        weight[index] = class_weight[value[1]]
        
    return weight

# * better way
transform = transforms.Compose([transforms.Resize((img_size, img_size)),
                                transforms.RandomRotation(30),
                                transforms.RandomHorizontalFlip(p=0.5),
                                transforms.ToTensor()])

# train_ds = datasets.ImageFolder(train_dir, transform)
# weights = unbalanced_dataset_weights(train_ds.imgs)
# weights = torch.tensor(weights, dtype=torch.double)
# sampler = torch.utils.data.sampler.WeightedRandomSampler(weights, len(weights))
# train_dataloader = torch.utils.data.DataLoader(train_ds, batch_size=batch_s, sampler= sampler)

# valid_ds = datasets.ImageFolder(valid_dir, transform)
# valid_dataloader = torch.utils.data.DataLoader(valid_ds, batch_size=2, shuffle=False)

# test_ds = datasets.ImageFolder(test_dir, transform)
# test_dataloader = torch.utils.data.DataLoader(test_ds, batch_size=1, shuffle=False)