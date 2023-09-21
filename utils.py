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


def preprocess_img(image):
    image = image / 255
    image = cv2.resize(image, (256, 256))
    image = image.transpose(2, 0, 1) #(W, H, C) -> (C, W, H)
    image_pt = torch.unsqueeze(torch.Tensor(image), dim = 0) 
    return image_pt


def deepfakes_image_predict(input_image):
    face = preprocess_img(input_image)

    img_grads = img_model.forward(face)
    multimodal_grads = multimodal.clf_rgb[0].forward(img_grads)

    out = nn.Softmax()(multimodal_grads)
    max = torch.argmax(out, dim=-1) #Index of the max value in the tensor.
    max = max.cpu().detach().numpy()
    max_value = out[max] #Actual value of the tensor.
    max_value = np.argmax(out[max].detach().numpy())

    if max_value > 0.5:
        preds = round(100 - (max_value*100), 3)
        text2 = f"The image is REAL."

    else:
        preds = round(max_value*100, 3)
        text2 = f"The image is FAKE."

    return text2
