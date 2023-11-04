import cv2
import os
import random
import numpy as np
import pandas as pd

import torch
from torch.utils.data import Dataset

device = torch.device("cuda" if torch.cuda.is_available() else "cpu")

print(os.getcwd())
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



img_size = 224

def  generate_dataframes(root_dir):
    train_df = pd.DataFrame(columns=['xray_dir', 'label'])
    test_df  = pd.DataFrame(columns=['xray_dir', 'label'])
    valid_df = pd.DataFrame(columns=['xray_dir', 'label'])
    
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

# train_df, test_df, valid_df = generate_dataframes('inputs/chest_xray/')

# train_df.to_csv('train.csv', index = False)
# test_df.to_csv('test.csv', index = False)
# valid_df.to_csv('valid.csv', index = False)


class PneumoniaDataset(Dataset):
    def __init__(self, data, transform=None, device=device):
        self.device = device
        self.data = pd.read_csv(data)
        self.target = self.data['label']
        self.transform = transform
                
    def __len__(self):
        return len(self.data)
    
    def __getitem__(self, index):
        img_name = self.data['xray_dir'][index]
        x = cv2.imread(img_name)
        y = self.target[index]
        
        #if (y == 0) and self.transform: # check for minority class
        x = self.transform(x)
        
        return x, y
