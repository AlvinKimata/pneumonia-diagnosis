import cv2
import sys
import pathlib
import traceback
from flask import Flask, request, jsonify, render_template, current_app

#Setting path.
cwd = pathlib.Path(__name__).parent.resolve()
if sys.platform.startswith('win'):
    cwd = str(cwd).split(sep = '\\')[:-1]
    cwd = '\\'.join(cwd)
    sys.path.append(cwd)

else:
    # Use '/' for Linux and other platforms
    cwd = str(cwd).split(sep = '/')[:-1]
    cwd = '/'.join(cwd)
    sys.path.append(cwd)
    print(cwd)

# def file_format_not_supported(e = None):
#     if e:
#         current_app.logger.info(f"{e.name} error {e.code} at {request.url}")
#     return "Error! File format is not supported"

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
                return str(prediction)
            else:
                return utils.file_format_not_supported()
        except Exception as e:
            return jsonify({'trace': traceback.format_exc(), 'error': str(e)})
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
    app.register_error_handler(400, utils.file_format_not_supported)
    app.register_error_handler(cv2.error, utils.opencv_error)

    app.run(debug = True, port = port)