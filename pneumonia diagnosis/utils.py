#Perform model inference.
import torch
import cnn_model
import cv2
import numpy as np

device = 'cuda' if torch.cuda.is_available() else 'cpu'

model = cnn_model.conv_v1()
model = model.to(device)

#Load model ckpt.
print('Loading checkpoint.')
ckpt = torch.load('models/best.pth', map_location = device)
model.load_state_dict(ckpt, strict = True)

#Evaluation mode.
model = model.eval()


def preprocess_img(image):
    # image = cv2.imread(image_path)
    image = image / 255
    image = cv2.resize(image, (256, 256))
    image = image.transpose(2, 0, 1) #(W, H, C) -> (C, W, H)
    image_pt = torch.unsqueeze(torch.Tensor(image), dim = 0) 
    return image_pt


def image_predict(input_image):
    image = preprocess_img(input_image)

    out = model.forward(image)
    out = out.detach().cpu().numpy()
    out = np.round(out, 4)
    if out > 0.5:
        return f"{out * 100}% probability of prescence of pneumonia in image."
    
    else:

        out = 1 - out
        return f"{out * 100}% probability of abscence of pneumonia in image."
