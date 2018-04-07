using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpDX.Windows;
using System.Threading;

namespace Roughness {
    public partial class MainForm : Form {
        public MainForm() {
            GameSettings.GameSettingsInit(4);
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            this.Hide();
            // Для старта нужно запустить рендер, создание карты и запуск самой игры происходит внутри рендера
            // Это сделанно потому что для старта игры необходимо создать и передать туда уже готовые экземпляры рендера
            List<RenderingUnit> RenderingUnitsList = new List<RenderingUnit>(); // Список всех объектов для отрисовки
            RenderForm mainForm = new RenderForm("GAME");
            GameRender gameRender = new GameRender(ref mainForm, ref RenderingUnitsList);
            Thread RenderThread = new Thread((new ThreadStart(gameRender.StartRender)));
            RenderThread.Start();
        }

        private void button2_Click(object sender, EventArgs e) {
            FormSettings form_settings = new FormSettings(this);
            form_settings.Show(); 
            this.Hide(); 

            
        }

        private void button4_Click(object sender, EventArgs e) {
            MessageBox.Show("                       Coming soon                       ");
        }
    }
}
