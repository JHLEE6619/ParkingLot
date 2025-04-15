import io
import json
from PIL import Image
from ultralytics import YOLO

class Car_Detection:
    def __init__(self):
        self.model = YOLO('best.pt')
        self.rois = self.rois_xyxy(self.load_rois())

    def binary_to_img(self,binary):
        img = Image.open(io.BytesIO(binary)).convert("RGB")
        return img

    def extract_box(self, img):
        results = self.model(img)
        boxes = []
        for result in results:
            boxes = result.boxes.xyxy  # Boxes object for bounding box outputs'

        return boxes

    def load_rois(self):
        with open("rois.json", "r") as f:
            rois = json.load(f)
        return rois

    def box_centerPoint(self, boxes):
        center = []
        for idx, [x1, y1, x2, y2] in enumerate(boxes):
            center_x = (x1+x2)/2
            center_y = (y1+y2)/2
            center.append((center_x,center_y))
        return center

    #ROI : [x,y,w,h] , box = [x,y,x,y]
    def rois_xyxy(self, rois):
        xyxy = []
        for idx, [x, y, w, h] in enumerate(rois):
            x2 = x + w
            y2 = y + h
            xyxy.append((x,y,x2,y2))
        return xyxy

    def judge_vacantSeat(self, binary):
        img = self.binary_to_img(binary = binary)
        boxes = self.extract_box(img = img)
        boxesCenterPt = self.box_centerPoint(boxes = boxes)
        seat = []
        seat_cnt = 21
        for i in range(seat_cnt):
            seat.append(0)

        for center in boxesCenterPt:
            for idx, (x1,y1,x2,y2) in enumerate(self.rois):
                # 중심점이 ROI 안에 있으면 1, 없으면 0
                if x1 <= center[0] <= x2 and y1 <= center[1] <= y2:
                    seat[idx] = 1

        return seat

