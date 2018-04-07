using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Roughness {
    public partial class FormSettings : Form {
        MainForm main_form;

        public FormSettings() {
            InitializeComponent();
        }
        public FormSettings(MainForm form):base() {
            InitializeComponent();
            main_form = form;

            GameSettings.CheckJoysticks();
            int counter = 1;
            foreach (var i in GameSettings.availableGameDevices) comboBox1.Items.Add($"{counter++} {i.joystick.Information.ProductName}");
            comboBox1.Items.Add(Convert.ToString (comboBox1.SelectedIndex));
        }
        private void button1_Click(object sender, EventArgs e) {
            if (comboBox1.SelectedIndex > 0) {
                GameSettings.playersSettings[1].deviceUsed = TypesGamesDevice.joystick;
                GameSettings.playersSettings[1].gameDevice = GameSettings.availableGameDevices[comboBox1.SelectedIndex];
            }

                main_form.Show();
            this.Close(); // скрываем Form1
        }

        private GameKeys current_key_number; // Эти переменные нужны для передачи параметров из обработчика нажатия кнопок в assignKey
        private int current_player_number;

        private void assignDownKey(string key_code) { // Этим методом подписываемся на нажатие клавишь джостика, когда настраиваем их
            int tmp = (int)current_key_number;
            GameSettings.playersSettings[current_player_number].keyDownСodes[tmp] = key_code;
            label_console_line_p1.Text = ($" Was appointed key [down {key_code} ]");
        }
        private void assignUpKey(string key_code) { // Этим методом подписываемся на нажатие клавишь джостика, когда настраиваем их
            int tmp = (int)current_key_number;
            GameSettings.playersSettings[current_player_number].keyUpСodes[tmp] = key_code;
            label_console_line_p1.Text += ($"[up {key_code}]");
        }
        private void assignThisKey(int player_number, GameKeys key_number) { 
            if (comboBox1.SelectedIndex > 0) {
                current_key_number = key_number;
                current_player_number = player_number;
                label_console_line_p1.Text = "Press the corresponding key";
                // Определяем код нажатия клавиши
                GameSettings.availableGameDevices[comboBox1.SelectedIndex].KeyDown += assignDownKey;
                GameSettings.availableGameDevices[comboBox1.SelectedIndex].StartJoystickListenerWithBreak();
                GameSettings.availableGameDevices[comboBox1.SelectedIndex].KeyDown -= assignDownKey;
                
                
                // Определяем код отжатия клавиши
                GameSettings.availableGameDevices[comboBox1.SelectedIndex].KeyUp += assignUpKey;
                GameSettings.availableGameDevices[comboBox1.SelectedIndex].StartJoystickListenerWithBreak();
                GameSettings.availableGameDevices[comboBox1.SelectedIndex].KeyUp -= assignUpKey;
            }
        }
        private void button_put_p1_Click(object sender, EventArgs e) {
            assignThisKey(1, GameKeys.put);
        }
        private void button_up_p1_Click(object sender, EventArgs e) {
            assignThisKey(1, GameKeys.up);
        }
        private void button_left_p1_Click(object sender, EventArgs e) {
            assignThisKey(1, GameKeys.left);
        }
        private void button_right_p1_Click(object sender, EventArgs e) {
            assignThisKey(1, GameKeys.right);
        }
        private void button_down_p1_Click(object sender, EventArgs e) {
            assignThisKey(1, GameKeys.down);
        }
    }
}
