import gradio as gr
import utils
import torch
import cnn_model

title = "Pneumonia diagnosis."
description = "A web application for diagnosing prescence or abscence of pneumonia in chesy xray images."


image_interface = gr.Interface(
    utils.image_predict,
    gr.Image(),
    'text',
    examples = ['inputs/samples/normal.jpeg',
                'inputs/samples/pneumonia.jpeg'],
    cache_examples=False

)

if __name__ == '__main__':
    image_interface.launch(share = False)