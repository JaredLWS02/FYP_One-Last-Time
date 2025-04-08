from PIL import Image, ImageOps
import os

# === CONFIGURATION ===
source_folder = 'target'       # folder with original images
destination_folder = 'output'  # folder to save flipped images
flip_mode = 'horizontal'  # options: 'horizontal' or 'vertical'

# === Ensure destination folder exists ===
os.makedirs(destination_folder, exist_ok=True)

# === Flip and save each image ===
for filename in os.listdir(source_folder):
    if filename.lower().endswith(('.png', '.jpg', '.jpeg', '.bmp', '.gif')):
        src_path = os.path.join(source_folder, filename)
        dst_path = os.path.join(destination_folder, filename)

        try:
            image = Image.open(src_path)
            if flip_mode == 'horizontal':
                flipped = ImageOps.mirror(image)
            elif flip_mode == 'vertical':
                flipped = ImageOps.flip(image)
            else:
                raise ValueError("flip_mode must be 'horizontal' or 'vertical'")

            flipped.save(dst_path)
            print(f"Saved flipped: {dst_path}")

        except Exception as e:
            print(f"Failed to process {filename}: {e}")
