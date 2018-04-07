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

namespace Roughness {
    class GameRender {
        private static D2DFactory d2dFactory;
        private static DWriteFactory dwFactory;
        private static RenderForm mainForm;

        private static WindowRenderTarget renderTarget;
        //Various brushes for our example
        private static SolidColorBrush backgroundBrush;


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

            //Assign render target and brush to our custom renderer
            CreateAndRunGame();  
            RenderLoop.Run(mainForm, () => {
                renderTarget.BeginDraw();
                renderTarget.Clear(bgcolor);
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
            TexturesLoader.LoadTextures(renderTarget); // Загружаем текстуры 
            GameMap game_map = new GameMap(650, 550, mainForm, renderTarget, RenderingUnitsList); // Создаём карту
            Game.StartGame(game_map); // Запускаем логику игры
            mainForm.Select();
        }
        

        private static void CreateResources() {
            if (renderTarget != null) { renderTarget.Dispose(); }
            if (backgroundBrush != null) { backgroundBrush.Dispose(); }

            HwndRenderTargetProperties wtp = new HwndRenderTargetProperties();
            wtp.Hwnd = mainForm.Handle;
            wtp.PixelSize = new Size2(650, 550);
            wtp.PresentOptions = PresentOptions.Immediately;
            renderTarget = new WindowRenderTarget(d2dFactory, new RenderTargetProperties(), wtp);

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
        public RenderingUnit(float x, float y, float size_x, float size_y, RenderTarget renderTarget, string textures_name) {
            X = x;
            Y = y;
            SizeX = size_x;
            SizeY = size_y;
            isVisible = true;
            Body = new RectangleF(X, Y, SizeX, SizeY);
            Texture = TexturesLoader.GameTextures[textures_name];
        }
        public void Hide() {
            isVisible = false;
        }
    }
}
