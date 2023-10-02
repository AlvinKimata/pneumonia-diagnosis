import sys
import os
import torch
import pathlib
import traceback
from flask import Flask, request, jsonify
import numpy as np
import cv2



#Setting path.
cwd = pathlib.Path(__name__).parent.resolve()
cwd = str(cwd).split(sep = '/')[:-1]
cwd = '/'.join(cwd)

sys.path.append(cwd)
print(cwd)
import utils


app = Flask(__name__)

@app.route('/classification', methods = ['POST'])
def predict_class():
    if model:
        try:
            #Check request content type.
            content_type = request.content_type
            if content_type == "image/jpeg":
                #Read image binary data.
                image_data = request.data
                prediction = utils.image_classification(image_data, model = model)
                return jsonify({'prediction': str(prediction)})
        except:
            return jsonify({'trace': traceback.format_exc()})
    else:
        print("Model not loaded yet!")


if __name__ == "__main__":
    try:
        port = int(sys.argv[1])
    except:
        port = 12345

    print("Loading model checkpoint...")
    model = utils.load_model()
    print("Loaded model.")

    app.run(debug = True, port = port)