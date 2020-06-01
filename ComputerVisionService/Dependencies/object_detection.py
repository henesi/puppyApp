from imageai.Detection import ObjectDetection
import os
import sys

dictList = []
keys = ['name', 'percentage_probability']

core_path = os.path.join('results', sys.argv[1], sys.argv[2])

if not os.path.exists(core_path):
    os.makedirs(core_path)

execution_path = os.getcwd()

detector = ObjectDetection()
detector.setModelTypeAsYOLOv3()
detector.setModelPath( os.path.join(execution_path , "yolo.h5"))
detector.loadModel()
detections = detector.detectObjectsFromImage(input_image=os.path.join(core_path , sys.argv[2]), output_image_path=os.path.join(core_path , "image_new.jpg"), minimum_percentage_probability=30)

print(detections)

for eachObject in detections:
	dictList.append(dict(zip(keys, [eachObject['name'], eachObject['percentage_probability']])))

with open(os.path.join(core_path, 'mapping'), 'a') as outfile:
	print(dictList, file=outfile)
