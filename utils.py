#Perform model inference.
import torch
import cnn_model


device = 'cuda' if torch.cuda.is_available() else 'cpu'

model = cnn_model.conv_v1()
model = model.to(device)

#Load model ckpt.
print('Loading checkpoint.')
ckpt = torch.load('models/best.pth', map_location = device)
model.load_state_dict(ckpt)

print('ckpt loaded') 