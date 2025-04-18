import cv2
import json

img = cv2.imread('binary.jpg')
rois = []

# 마우스로 ROI 지정
while True:
    roi = cv2.selectROI("Select Parking Slot (Press ENTER)", img, fromCenter=False, showCrosshair=True)
    if sum(roi) == 0:
        break
    rois.append(roi)  # (x, y, w, h)
    print(f"ROI added: {roi}")

cv2.destroyAllWindows()

# 지정된 ROI를 시각화하고 저장
for idx, (x, y, w, h) in enumerate(rois):
    cv2.rectangle(img, (x, y), (x + w, y + h), (0, 255, 0), 2)
    cv2.putText(img, f"Slot {idx+1}", (x, y - 10), cv2.FONT_HERSHEY_SIMPLEX, 0.5, (0,255,0), 1)

cv2.imshow("All ROIs", img)
cv2.waitKey(0)
cv2.destroyAllWindows()

roi_dicts = [list(roi) for roi in rois]

with open("test.json", "w") as f:
    json.dump(roi_dicts, f) # indent=2 : 들여쓰기 2칸

with open("test.json", "r") as f:
    ROI = json.load(f)

print(ROI)