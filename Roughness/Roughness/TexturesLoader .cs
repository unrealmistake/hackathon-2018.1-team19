using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DXGI;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using PixelFormat = SharpDX.Direct2D1.PixelFormat;
namespace Roughness {
    static class TexturesLoader {
        public static Dictionary  <string, Bitmap> GameTextures = new Dictionary<string, Bitmap>();
        public static void LoadTextures(RenderTarget renderTarget) {
            //@"..\..\GameRes\Image10.png"
            GameTextures.Add("Image1", LoadBitmapFromFile(renderTarget, @"..\..\GameRes\Image1.png"));
            GameTextures.Add("Image2", LoadBitmapFromFile(renderTarget, @"..\..\GameRes\Image2.png"));
            GameTextures.Add("Image3", LoadBitmapFromFile(renderTarget, @"..\..\GameRes\Image3.png"));
            GameTextures.Add("Image4", LoadBitmapFromFile(renderTarget, @"..\..\GameRes\Image4.png"));
            GameTextures.Add("Image5", LoadBitmapFromFile(renderTarget, @"..\..\GameRes\Image5.png"));
            GameTextures.Add("Image6", LoadBitmapFromFile(renderTarget, @"..\..\GameRes\Image6.png"));
            GameTextures.Add("Image7", LoadBitmapFromFile(renderTarget, @"..\..\GameRes\Image7.png"));
            GameTextures.Add("Image8", LoadBitmapFromFile(renderTarget, @"..\..\GameRes\Image8.png"));
            GameTextures.Add("Image9", LoadBitmapFromFile(renderTarget, @"..\..\GameRes\Image9.png"));
            //GameTextures.Add("Image10", LoadBitmapFromFile(renderTarget, @"..\..\GameRes\Image10.png"));
            //GameTextures.Add("Image11", LoadBitmapFromFile(renderTarget, @"..\..\GameRes\Image11.png"));
            GameTextures.Add("Image12", LoadBitmapFromFile(renderTarget, @"..\..\GameRes\Image12.png"));
            GameTextures.Add("Image13", LoadBitmapFromFile(renderTarget, @"..\..\GameRes\Image13.png"));
            GameTextures.Add("Image15", LoadBitmapFromFile(renderTarget, @"..\..\GameRes\Image15.png"));
            GameTextures.Add("Image16", LoadBitmapFromFile(renderTarget, @"..\..\GameRes\Image16.png"));

        }
        private static Bitmap LoadBitmapFromFile(RenderTarget renderTarget, string file) {  // Собсвенная реализация загрузчика и конвертации 
            // Loads from file using System.Drawing.Image
            using (var bitmap = (System.Drawing.Bitmap)System.Drawing.Image.FromFile(file)) {
                var sourceArea = new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height);
                var bitmapProperties = new BitmapProperties(new PixelFormat(Format.R8G8B8A8_UNorm, AlphaMode.Premultiplied));
                var size = new Size2(bitmap.Width, bitmap.Height);

                // Transform pixels from BGRA to RGBA
                int stride = bitmap.Width * sizeof(int);
                using (var tempStream = new DataStream(bitmap.Height * stride, true, true)) {
                    // Lock System.Drawing.Bitmap
                    var bitmapData = bitmap.LockBits(sourceArea, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

                    // Convert all pixels 
                    for (int y = 0; y < bitmap.Height; y++) {
                        int offset = bitmapData.Stride * y;
                        for (int x = 0; x < bitmap.Width; x++) {
                            // Not optimized 
                            byte B = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            byte G = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            byte R = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            byte A = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            int rgba = R | (G << 8) | (B << 16) | (A << 24);
                            tempStream.Write(rgba);
                        }
                    }
                    bitmap.UnlockBits(bitmapData);
                    tempStream.Position = 0;
                    return new Bitmap(renderTarget, size, tempStream, stride, bitmapProperties);
                }
            }
        }
    }
}
