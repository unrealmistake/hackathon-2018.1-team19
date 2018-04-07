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
            int counter = 0;
            foreach (var i in GameSettings.availableGameDevices) {
                comboBox_p1.Items.Add($"{++counter} {i.joystick.Information.ProductName}");
                comboBox_p2.Items.Add($"{counter} {i.joystick.Information.ProductName}");
            }
            
        }
        private void button1_Click(object sender, EventArgs e) {
            if (comboBox_p1.SelectedIndex > 0) {
                GameSettings.playersSettings[1].deviceUsed = TypesGamesDevice.joystick;
                GameSettings.playersSettings[1].gameDevice = GameSettings.availableGameDevices[comboBox_p1.SelectedIndex-1];
            }
            if (comboBox_p2.SelectedIndex > 0) {
                GameSettings.playersSettings[2].deviceUsed = TypesGamesDevice.joystick;
                GameSettings.playersSettings[2].gameDevice = GameSettings.availableGameDevices[comboBox_p2.SelectedIndex-1];
            }

            main_form.Show();
            this.Close(); // скрываем Form1
        }

        private GameKeys current_key_number; // Эти переменные нужны для передачи параметров из обработчика нажатия кнопок в assignKey
        private int current_player_number;

        private void assignDownKey(string key_code) { // Этим методом подписываемся на нажатие клавишь джостика, когда настраиваем их
            int tmp = (int)current_key_number;
            GameSettings.playersSettings[current_player_number].keyDownСodes[tmp] = key_code;
            if (current_player_number == 1) label_console_line_p1.Text = ($" Was appointed key [down {key_code} ]");
            else label_console_line_p2.Text = ($" Was appointed key [down {key_code} ]");
        }
        private void assignUpKey(string key_code) { // Этим методом подписываемся на нажатие клавишь джостика, когда настраиваем их
            int tmp = (int)current_key_number;
            GameSettings.playersSettings[current_player_number].keyUpСodes[tmp] = key_code;
            if (current_player_number == 1) label_console_line_p1.Text += ($"[up {key_code}]");
            else label_console_line_p2.Text += ($"[up {key_code}]");
        }
        private void assignThisKeyP1(GameKeys key_number) { 
            if (comboBox_p1.SelectedIndex > 0) {
                current_key_number = key_number;
                current_player_number = 1;
                label_console_line_p1.Text = "Press the corresponding key";
                // Определяем код нажатия клавиши
                GameSettings.availableGameDevices[comboBox_p1.SelectedIndex - 1].KeyDown += assignDownKey;
                GameSettings.availableGameDevices[comboBox_p1.SelectedIndex - 1].StartJoystickListenerWithBreak();
                GameSettings.availableGameDevices[comboBox_p1.SelectedIndex - 1].KeyDown -= assignDownKey;
                
                
                // Определяем код отжатия клавиши
                GameSettings.availableGameDevices[comboBox_p1.SelectedIndex - 1].KeyUp += assignUpKey;
                GameSettings.availableGameDevices[comboBox_p1.SelectedIndex - 1].StartJoystickListenerWithBreak();
                GameSettings.availableGameDevices[comboBox_p1.SelectedIndex - 1].KeyUp -= assignUpKey;
            }
        }
        private void assignThisKeyP2(GameKeys key_number) {
            if (comboBox_p2.SelectedIndex > 0) {
                current_key_number = key_number;
                current_player_number = 2;
                label_console_line_p2.Text = "Press the corresponding key";
                // Определяем код нажатия клавиши
                GameSettings.availableGameDevices[comboBox_p2.SelectedIndex - 1].KeyDown += assignDownKey;
                GameSettings.availableGameDevices[comboBox_p2.SelectedIndex - 1].StartJoystickListenerWithBreak();
                GameSettings.availableGameDevices[comboBox_p2.SelectedIndex - 1].KeyDown -= assignDownKey;


                // Определяем код отжатия клавиши
                GameSettings.availableGameDevices[comboBox_p2.SelectedIndex - 1].KeyUp += assignUpKey;
                GameSettings.availableGameDevices[comboBox_p2.SelectedIndex - 1].StartJoystickListenerWithBreak();
                GameSettings.availableGameDevices[comboBox_p2.SelectedIndex - 1].KeyUp -= assignUpKey;
            }
        }
        // Управление первого игрока
        private void button_left_p1_Click(object sender, EventArgs e) {
            assignThisKeyP1(GameKeys.left);
        }

        private void button_up_p1_Click(object sender, EventArgs e) {
            assignThisKeyP1(GameKeys.up);
        }

        private void button_right_p1_Click(object sender, EventArgs e) {
            assignThisKeyP1(GameKeys.right);
        }
        private void button_down_p1_Click(object sender, EventArgs e) {
            assignThisKeyP1(GameKeys.down);
        }
        private void button_put_p1_Click(object sender, EventArgs e) {
            assignThisKeyP1(GameKeys.put);
        }
        // Управление второго игрока
        private void button_left_p2_Click(object sender, EventArgs e) {
            assignThisKeyP2(GameKeys.left);
        }
        private void button_up_p2_Click(object sender, EventArgs e) {
            assignThisKeyP2(GameKeys.up);
        }
        private void button_right_p2_Click(object sender, EventArgs e) {
            assignThisKeyP2(GameKeys.right);
        }
        private void button_down_p2_Click(object sender, EventArgs e) {
            assignThisKeyP2(GameKeys.down);
        }
        private void button_put_p2_Click(object sender, EventArgs e) {
            assignThisKeyP2(GameKeys.put);
        }
    }
}
