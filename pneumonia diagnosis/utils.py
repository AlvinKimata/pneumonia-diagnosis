#Perform model inference.
import os
import cv2
import torch
import numpy as np
from PIL import Image
from models import cnn_model
from models import efficientnetv2


device = 'cuda' if torch.cuda.is_available() else 'cpu'

def load_model():
    model = cnn_model.conv_v1()
    model = model.to(device)

    # Load model ckpt.
    ckpt = torch.load('../ckpt/best_1.pth', map_location = device) #Load model using an endpoint.
    # ckpt = torch.load('ckpt/best_1.pth', map_location = device) #Load model using gradio webapp.
    model.load_state_dict(ckpt, strict = True)

    #Evaluation mode.
    model = model.eval()
    return model


def preprocess_img(image):
    image = image / 255
    image = cv2.resize(image, (256, 256))
    image = image.transpose(2, 0, 1) #(W, H, C) -> (C, W, H)
    image_pt = torch.unsqueeze(torch.Tensor(image), dim = 0) 
    return image_pt


def image_predict(input_image):
    # model = load_model()
    model = efficientnetv2.create_model(best_model_path='ckpt/efficientnetv2.ckpt')

    #Load ckpt.
    # ckpt = torch.load('ckpt/efficientnetv2.ckpt', map_location=device)
    # model.load_state_dict(ckpt, strict = False)
    #Set model to evaluation mode

    model.eval()

    image = preprocess_img(input_image)

    out = model.forward(image)
    # out = out.detach().cpu().numpy()
    # out = np.round(out, 4)
    return out
    # if out > 0.5:
    #     return f"{out * 100}% probability of prescence of pneumonia."
    
    # else:

    #     out = 1 - out
    #     return f"{out * 100}% probability of abscence of pneumonia."


def image_classification(image_bn, model):
    img = decode_image_binary(image_bn)
    img = preprocess_img(img)
    grads = model.forward(img)
    out = grads.detach().cpu().numpy()
    out = np.round(out * 100, 4)
    return out
    # if out > 0.5:
    #     return f"{out}% probability of prescence of pneumonia."
    
    # else:
    #     out = 1 - out
    #     return f"{out}% probability of abscence of pneumonia."


def decode_image_binary(image_bn):
    '''Reads image in binary and returns it np.array format'''
    img_buff = np.frombuffer(image_bn, np.uint8)
    image_decoded = cv2.imdecode(img_buff, cv2.IMREAD_COLOR)
    image_array = np.array(image_decoded)
    return image_array
