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
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.Remoting.Contexts;


namespace Game {

    public class Program {
        [STAThread]
        static void Main(string[] args) {

            //RenderingUnit a = new RenderingUnit(50, 50, 50, 50, renderTarget, "image1.png");
            //RenderingUnit b = new RenderingUnit(50, 100, 50, 50, renderTarget, "image1.png");
            //RenderingUnitsList.Add(a);
            //RenderingUnitsList.Add(b);
            List<RenderingUnit> RenderingUnitsList = new List<RenderingUnit>();
            RenderForm mainForm = new RenderForm("GAME");
           

            GameRender gameRender = new GameRender(ref mainForm, ref RenderingUnitsList);
            Thread RenderThread = new Thread((new ThreadStart(gameRender.StartRender)));
            RenderThread.Start();

            //GameMap gameMap = new GameMap(650, 550, render_target);

        }
    }


}
