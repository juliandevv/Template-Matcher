# Object Detection Via Template Matching
This project is a complete one-shot solution for detecting objects in an image.
## Process
As an example this sharpie marker will be detected in the image below.
![alt text](https://github.com/juliandevv/Template-Matcher/blob/master/Template%20Matcher/examples/Search.jpg)
The program takes a single example image of the object. This is called the template image.

![alt text](https://github.com/juliandevv/Template-Matcher/blob/master/Template%20Matcher/examples/Template.jpg)

### Image Pyramid
The template matching operation is an intensive calculation so the image is downsized for ROI selection and then later the full size image is used for accurate detection.

![alt text](https://github.com/juliandevv/Template-Matcher/blob/master/Template%20Matcher/examples/Pyramid.jpg)

### Coarse Matching and Rotation
To detect all possible orientations of the object, the template image is rotated in discrete steps.

![alt text](https://github.com/juliandevv/Template-Matcher/blob/master/Template%20Matcher/examples/Rotate.jpg)

The template matching operation is then performed with all the downsized and rotated images. The operation computes a normalized cross-correlation of every possible position of the template image within the search image.
This covers all possible x-y translations of the object. The result is an image that shows where the cross-correlation operation had the highest value.

![alt text](https://github.com/juliandevv/Template-Matcher/blob/master/Template%20Matcher/examples/InitialMatches.jpg)

### ROI Selection
The peaks of the previous image are detected and regions are draw around them based on the size of the template image. This region is then selected from the full-size search image

![alt text](https://github.com/juliandevv/Template-Matcher/blob/master/Template%20Matcher/examples/ROI.jpg)

### Refined Match
The matching operation is performed again, this time with the full-size images and a series of more granular rotations. The result is defined by a bounding box the same size as the template image

![alt text](https://github.com/juliandevv/Template-Matcher/blob/master/Template%20Matcher/examples/Result.jpg)

## Limitations
This algorithm is very useful due to it's one-shot capabilities however, it works under certain assumptions.
- The lighting must be identical in the template image and search image
- Perspective warp is not considered, the object must have only undergone a 2D affine transform in the search image
- The object cannot be occluded
Despite these limiations this algorithm is useful for object detection in controlled environments such as pick and place operations on conveyors.

## Improvements
A few improvments could be made to increase the speed and accuracy of this algorithm:
- All rotate and match operations are independent of one-another and could therefore be computed concurrently.
- The image downsizing uses a mean filter for decimation which is fast but leads to more inaccuracy due to aliasing. A gaussian filter could possibly improve this operation.
- The template image rotations could be computed offline to reduce the amount of online operations required.
- Sub-pixel accuracy could be achieved by computing the gradient of the match result and finding continuous peaks.

