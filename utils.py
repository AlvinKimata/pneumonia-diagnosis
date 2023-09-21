#Perform model inference.
import torch
import cnn_model
import cv2

device = 'cuda' if torch.cuda.is_available() else 'cpu'

model = cnn_model.conv_v1()
model = model.to(device)

#Load model ckpt.
print('Loading checkpoint.')
ckpt = torch.load('models/best.pth', map_location = device)
model.load_state_dict(ckpt)

#Evaluation mode.
model = model.eval()


def preprocess_img(image_path):
    image = cv2.imread(image_path)
    image = image / 255
    image = cv2.resize(image, (256, 256))
    image = image.transpose(2, 0, 1) #(W, H, C) -> (C, W, H)
    image_pt = torch.unsqueeze(torch.Tensor(image), dim = 0) 
    return image_pt


def deepfakes_image_predict(input_image, model):
    image = preprocess_img(input_image)

    out = model.forward(image)
    
    return out

pneumonia = 'inputs/samples/pneumonia.jpeg'
normal = 'inputs/samples/normal.jpeg'

pneumonia_grads = deepfakes_image_predict(input_image=pneumonia, model = model)
normal_grads = deepfakes_image_predict(input_image=normal, model = model)
print(normal_grads)
print(pneumonia_grads)
