import sys
import torch
import traceback
from flask import Flask, request, jsonify
import utils

app = Flask(__name__)



@app.route('/classification', methods = ['POST'])
def predict_class():
    if model:
        try:
            json_ = request.json
            print(json_)
            query = 'model query'
            prediction = model.predict(query)
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