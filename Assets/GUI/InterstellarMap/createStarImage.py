import matplotlib.pyplot as plt
import numpy as np
from PIL import Image
from PIL import ImageOps

def yellow():
    res = np.zeros((size, size, 4))
    res[..., 0] = 1  # Red channel
    res[..., 1] = gradient # Green channel
    res[..., 2] = 0  # Blue channel
    res[..., 3] = gradient * 1.5  # alpha channel
    return res

def red():
    res = np.zeros((size, size, 4))
    res[..., 0] = 1  # Red channel
    res[..., 1] = gradient # Green channel
    res[..., 2] = gradient  # Blue channel
    res[..., 3] = gradient * 1.5  # alpha channel
    return res

def blue():
    res = np.zeros((size, size, 4))
    res[..., 0] = gradient  # Red channel
    res[..., 1] = gradient # Green channel
    res[..., 2] = 1  # Blue channel
    res[..., 3] = gradient * 1.5  # alpha channel
    return res


# Set up the figure and axis
size = 1000
X, Y = np.meshgrid(np.linspace(-1, 1, size), np.linspace(-1, 1, size))

# Modified radial distance to create a cross effect
weight = 150.0  # Control the cross effect
brightness = 1.15
R = np.sqrt((X**2 + Y**2) + weight * (X**2 * Y**2))

gradient = np.exp(-R * 8 / 2) * brightness
color_map = blue()

final_image = np.zeros_like(color_map)
for i in range(4):
    final_image[..., i] = gradient * color_map[..., i]

fig, ax = plt.subplots(figsize=(8, 8))
ax.imshow(final_image, extent=[-1, 1, -1, 1])
ax.axis('off')

# Save the image
filename = './Star64.png'
plt.savefig(filename, bbox_inches='tight', transparent=True, pad_inches=0)
plt.close(fig)

# Open the saved image, crop, and resize
img = Image.open(filename)
img = ImageOps.crop(img, 150) # Remove 150 pixels from all sides
resized_img = img.resize((64, 64), resample=Image.Resampling.BILINEAR)  # Resize to 64x64
resized_img.save(filename)
