#Perform model inference.
import os
import cv2
import torch
import numpy as np
from PIL import Image
from models import cnn_model
# from models import efficientnetv2


device = 'cuda' if torch.cuda.is_available() else 'cpu'

def load_model():
    model = cnn_model.conv_v1()
    model = model.to(device)

    # Load model ckpt.
    ckpt = torch.load('..\\ckpt\\model_weights.pth', map_location = device) #Load model using an endpoint.
    model.load_state_dict(ckpt, strict = True)

    #Evaluation mode.
    model.eval()
    return model


def preprocess_img(image):
    image = image / 255
    image = cv2.resize(image, (256, 256))
    image = image.transpose(2, 0, 1) #(W, H, C) -> (C, W, H)
    image_pt = torch.unsqueeze(torch.Tensor(image), dim = 0) 
    return image_pt


def image_classification(image_bn, model):
    img = decode_image_binary(image_bn)
    img = preprocess_img(img)
    grads = model.forward(img)
    out = grads.detach().cpu().numpy()
    out = np.round(out * 100, 2)
    # if out > 0.5:
    #     print(out)
    return f"{out}% probability of prescence of pneumonia."
    
    # else:
    #     # out = 1 - out
    #     print(out)
    #     return f"{out}% probability of abscence of pneumonia."


def decode_image_binary(image_bn):
    '''Reads image in binary and returns it np.array format'''
    img_buff = np.frombuffer(image_bn, np.uint8)
    image_decoded = cv2.imdecode(img_buff, cv2.IMREAD_COLOR)
    image_array = np.array(image_decoded)
    return image_array
