import numpy as np
import matplotlib.pyplot as plt
import random

# Image dimensions
width = 2560
height = 2560

# Number of stars
num_stars = 2000

# Create an empty image with a black background
image = np.zeros((height, width), dtype=np.float32)

# Function to create a blurred star
def create_star(size):
    star = np.zeros((size*2+1, size*2+1), dtype=np.float32)
    for i in range(size*2+1):
        for j in range(size*2+1):
            distance = np.sqrt((i-size)**2 + (j-size)**2)
            intensity = np.exp(-distance**2 / (2*(size/2)**2))
            star[i, j] = intensity
    return star

# Add stars to the image
for _ in range(num_stars):
    x = random.randint(0, width - 1)
    y = random.randint(0, height - 1)
    size = random.randint(1, 3)
    star = create_star(size)
    
    x_start = max(0, x - size)
    x_end = min(width, x + size + 1)
    y_start = max(0, y - size)
    y_end = min(height, y + size + 1)
    
    star_x_start = max(0, size - x)
    star_x_end = star_x_start + (x_end - x_start)
    star_y_start = max(0, size - y)
    star_y_end = star_y_start + (y_end - y_start)
    
    image[y_start:y_end, x_start:x_end] += star[star_y_start:star_y_end, star_x_start:star_x_end]

# Clip the values to be in the range [0, 1]
image = np.clip(image, 0, 1)

# Display the image
fig, ax = plt.subplots(figsize=(width / 100, height / 100), dpi=100)
ax.imshow(image, cmap='gray')
ax.axis('off')

# Save the image
plt.savefig('./stars.png', bbox_inches='tight', pad_inches=0)
plt.show()
