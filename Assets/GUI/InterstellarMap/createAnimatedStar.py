import matplotlib.pyplot as plt
import numpy as np
from PIL import Image
from PIL import ImageOps
import os

def yellow():
    res = np.zeros((size, size, 4))
    res[..., 0] = 1  # Red channel
    res[..., 1] = gradient # Green channel
    res[..., 2] = 0  # Blue channel
    res[..., 3] = gradient * 1.5  # alpha channel
    return res

def orange():
    res = np.zeros((size, size, 4))
    res[..., 0] = 1  # Red channel
    res[..., 1] = gradient # Green channel
    res[..., 2] = gradient * 0.5  # Blue channel
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



# Create a directory to save the images
if not os.path.exists('pulsing_star'):
    os.makedirs('pulsing_star')

# Set up the figure and axis
size = 1000
X, Y = np.meshgrid(np.linspace(-1, 1, size), np.linspace(-1, 1, size))

# Modified radial distance to create a cross effect
weight = 100.0  # Control the cross effect
R = np.sqrt((X**2 + Y**2) + weight * (X**2 * Y**2))

# Generate and save images with varying brightness
num_frames = 10  # Number of frames in the animation
brightness_variation = np.linspace(0.85, 1.05, num_frames)  # Vary brightness

for i, brightness in enumerate(brightness_variation):
    gradient = np.exp(-R * 8 / 2) * brightness

    color_map = blue()

    final_image = np.zeros_like(color_map)
    for j in range(4):
        final_image[..., j] = gradient * color_map[..., j]

    fig, ax = plt.subplots(figsize=(8, 8))
    ax.imshow(final_image, extent=[-1, 1, -1, 1])
    ax.axis('off')

    # Save the frame
    plt.savefig(f'pulsing_star/frame_{i:02d}.png', bbox_inches='tight', transparent=True, pad_inches=0)
    plt.close(fig)

    # Open the saved image, crop, and resize
    img = Image.open(f'pulsing_star/frame_{i:02d}.png')
    img = ImageOps.crop(img, 150) # Remove 150 pixels from all sides
    resized_img = img.resize((64, 64), resample=Image.Resampling.BILINEAR)  # Resize to 64x64

    resized_img.save(f'pulsing_star/frame_{i:02d}.png')