using System;
using System.Collections.Generic;
using SharpDX;
using SharpDX.DXGI;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.Windows;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using D2DFactory = SharpDX.Direct2D1.Factory;
using DWriteFactory = SharpDX.DirectWrite.Factory;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Bitmap = SharpDX.Direct2D1.Bitmap;
using PixelFormat = SharpDX.Direct2D1.PixelFormat;
using RectangleF = SharpDX.RectangleF;
using Color = SharpDX.Color;
using FontStyle = SharpDX.DirectWrite.FontStyle;


namespace Game {
    class GameRender {
        private static D2DFactory d2dFactory;
        private static DWriteFactory dwFactory;
        private static RenderForm mainForm;

        private static WindowRenderTarget renderTarget;
        //Various brushes for our example
        private static SolidColorBrush backgroundBrush;
        private static SolidColorBrush defaultBrush;
        private static SolidColorBrush greenBrush;
        private static SolidColorBrush redBrush;

        List<RenderingUnit> RenderingUnitsList;
        //This one is only a measured region
        public GameRender(ref RenderForm form, ref List<RenderingUnit> rendering_units_list) {
            mainForm = form;
            RenderingUnitsList = rendering_units_list;
        }
       public void StartRender() {
            //List<RenderingUnit> RenderingUnitsList = new List<RenderingUnit>();
            d2dFactory = new D2DFactory();
            dwFactory = new DWriteFactory(SharpDX.DirectWrite.FactoryType.Shared);
            CreateResources();
            var bgcolor = new Color4(0.1f, 0.1f, 0.1f, 1.0f);

            //This is the offset where we start our text layout
            Vector2 offset = new Vector2(202.0f, 250.0f);
            //Assign render target and brush to our custom renderer
            RectangleF tmp = new RectangleF(10, 10, 10, 10);

            RectangleF tmp_bitmap_rect = new RectangleF(50, 50, 50, 50);
            CreateAndRunGame();
            RenderLoop.Run(mainForm, () => {
                renderTarget.BeginDraw();
                renderTarget.Clear(bgcolor);

                if (tmp.X > 400) tmp.X = 4;
                tmp.X++;
                renderTarget.FillRectangle(tmp, redBrush);
                for (int i = RenderingUnitsList.Count - 1; i>-1; i--) { // for для того что бы список отрисовывался задом наперёд (Тогда главные объекты отрисовываютсья поверх второстепенных)
                    if (RenderingUnitsList[i].isVisible) renderTarget.DrawBitmap(RenderingUnitsList[i].Texture, RenderingUnitsList[i].Body, 1, BitmapInterpolationMode.Linear);
                };
                try {
                    renderTarget.EndDraw();
                } catch {
                    CreateResources();
                }
            });
            d2dFactory.Dispose();
            dwFactory.Dispose();
            renderTarget.Dispose();

        }
        public void CreateAndRunGame() {
            GameMap game_map = new GameMap(650, 550, mainForm, renderTarget, RenderingUnitsList);
            Game.StartGame(game_map);
            mainForm.Select();
        }
        

        private static void CreateResources() {
            if (renderTarget != null) { renderTarget.Dispose(); }
            if (defaultBrush != null) { defaultBrush.Dispose(); }
            if (greenBrush != null) { greenBrush.Dispose(); }
            if (redBrush != null) { redBrush.Dispose(); }
            if (backgroundBrush != null) { backgroundBrush.Dispose(); }

            HwndRenderTargetProperties wtp = new HwndRenderTargetProperties();
            wtp.Hwnd = mainForm.Handle;
            wtp.PixelSize = new Size2(650, 550);
            wtp.PresentOptions = PresentOptions.Immediately;
            renderTarget = new WindowRenderTarget(d2dFactory, new RenderTargetProperties(), wtp);

            defaultBrush = new SolidColorBrush(renderTarget, Color.White);
            greenBrush = new SolidColorBrush(renderTarget, Color.Green);
            redBrush = new SolidColorBrush(renderTarget, Color.Red);
            backgroundBrush = new SolidColorBrush(renderTarget, new Color4(0.3f, 0.3f, 0.3f, 0.5f));
        }
    }
    public class RenderingUnit { //Являетсья единицой отрисовывающейся на экране
        private float x;
        private float y;
        public bool isVisible { get; set; } // 1 - Visible, 0 - Hidden
        public float X {
            get { return x; } 
            set {
                x = value;
                Body.X = x;
            }
        }
        public float Y {
            get { return y; }
            set {
                y = value;
                Body.Y = y;
            }
        }
        public float SizeX { get; set; }
        public float SizeY { get; set; }
        public RectangleF Body;
        public Bitmap Texture;
        public RenderingUnit() { }
        public RenderingUnit(float x, float y, float size_x, float size_y, RenderTarget renderTarget, string textures_file_name) {
            X = x;
            Y = y;
            SizeX = size_x;
            SizeY = size_y;
            isVisible = true;
            Body = new RectangleF(X, Y, SizeX, SizeY);
            Texture = LoadBitmapFromFile(renderTarget, textures_file_name);
        }
        public void Hide() {
            isVisible = false;
        }
        private Bitmap LoadBitmapFromFile(RenderTarget renderTarget, string file) {  // Собсвенная реализация загрузчика и конвертации 
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
