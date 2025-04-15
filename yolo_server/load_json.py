import json
with open("rois.json", "r") as f:
    ROI = json.load(f)

print(ROI)