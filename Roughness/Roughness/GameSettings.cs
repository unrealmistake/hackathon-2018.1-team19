using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.DirectInput;

namespace Roughness {

    static class GameSettings {
        static public int sound_volume;
        static public List<PlayersSettings> playersSettings = new List<PlayersSettings>();
        static public List<GameDevice> availableGameDevices = new List<GameDevice>();

        static DirectInput directInput = new DirectInput();
        static Guid joystickGuid = Guid.Empty;
        static public void GameSettingsInit(int number_of_players) {
            for (int i = 0; i <= number_of_players; i++) { // <= Потому что элементов должно быть на 1 больше чем реальных игроков 
                playersSettings.Add(new PlayersSettings());
            }
        }
        static public void CheckJoysticks() {
            directInput = new DirectInput();
            joystickGuid = Guid.Empty;
            Joystick joystick;
            foreach (var deviceInstance in directInput.GetDevices(DeviceType.Gamepad, DeviceEnumerationFlags.AllDevices)) {
                joystickGuid = deviceInstance.InstanceGuid;
                MessageBox.Show("Внимание, у нас тут неожиданно появился геймпад -_-, срочно пишите https://vk.com/id12488633");
            }
            foreach (var deviceInstance in directInput.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AllDevices)) {
                joystickGuid = deviceInstance.InstanceGuid;
                joystick = new Joystick(directInput, joystickGuid);
                joystick.Properties.BufferSize = 128;
                joystick.Acquire();
                availableGameDevices.Add(new GameDevice(joystick));
            }
        }


    }

    class PlayersSettings {
        public TypesGamesDevice deviceUsed;
        public string[] keyDownСodes;
        public string[] keyUpСodes;
        public GameDevice gameDevice { get; set; }
        public PlayersSettings() {
            keyDownСodes = new string[6];
            keyUpСodes = new string[6];
            deviceUsed = TypesGamesDevice.keyboard;
        }
        public void SetKeyDownCode(string key, int key_number) {
            keyDownСodes[key_number] = key;
        }
        public void SetKeyUpCode(string key, int key_number) {
            keyUpСodes[key_number] = key;
        }
    }
    class GameDevice {
        public delegate void DJoystickEvent(string value);
        public event DJoystickEvent KeyDown;
        public event DJoystickEvent KeyUp;
        public Joystick joystick;

        public GameDevice(Joystick joystick) {
            this.joystick = joystick;
        }
        public void StartJoystickListener() // Слушатель геймпада
        {
            joystick.Poll();
            while (true) {
                var datas = joystick.GetBufferedData();
                foreach (var state in datas) {
                    if (KeyDown != null){
                        if ((state.Value > 0) && (state.Offset != JoystickOffset.X) && (state.Offset != JoystickOffset.Y)) {// Down обычной кнопки
                            joystick.GetBufferedData();
                            KeyDown(state.Value.ToString() + state.Offset.ToString());
                        }
                        if (((state.Offset == JoystickOffset.X) || (state.Offset == JoystickOffset.Y)) && ((state.Value == 0) || (state.Value > 65000))) {// Down перекрестия
                            joystick.GetBufferedData();
                            KeyDown(state.Value.ToString() + state.Offset.ToString());
                        }
                    }
                    if (KeyUp != null) {
                        if ((state.Value == 0) && (state.Offset != JoystickOffset.X) && (state.Offset != JoystickOffset.Y)) {// Up обычной кнопки
                            joystick.GetBufferedData();
                            KeyUp(state.Value.ToString() + state.Offset.ToString());
                        }
                        if (((state.Offset == JoystickOffset.X) || (state.Offset == JoystickOffset.Y)) && ((state.Value > 0) && (state.Value < 65000))) {// Up перекрестия
                            joystick.GetBufferedData();
                            KeyUp(state.Value.ToString() + state.Offset.ToString());
                        }
                    }
                }
            }
        }
        public void StartJoystickListenerWithBreak() // Слушатель геймпада с выходом после первой нажатой кнопки
        {
            joystick.Poll();
            bool break_flag = false;
            while (!break_flag) {
                var datas = joystick.GetBufferedData();
                foreach (var state in datas) {
                    if (KeyDown != null) {
                        if ((state.Value > 0) && (state.Offset != JoystickOffset.X) && (state.Offset != JoystickOffset.Y)) {// Down обычной кнопки
                            joystick.GetBufferedData();
                            KeyDown(state.Value.ToString() + state.Offset.ToString());
                            break_flag = true;
                            break;
                        }
                        if (((state.Offset == JoystickOffset.X) || (state.Offset == JoystickOffset.Y)) && ((state.Value == 0) || (state.Value > 65000))) {// Down перекрестия
                            joystick.GetBufferedData();
                            KeyDown(state.Value.ToString() + state.Offset.ToString());
                            break_flag = true;
                            break;
                        }
                    }
                    if (KeyUp != null) {
                        if ((state.Value == 0) && (state.Offset != JoystickOffset.X) && (state.Offset != JoystickOffset.Y)) {// Up обычной кнопки
                            joystick.GetBufferedData();
                            KeyUp(state.Value.ToString() + state.Offset.ToString());
                            break_flag = true;
                            break;
                        }
                        if (((state.Offset == JoystickOffset.X) || (state.Offset == JoystickOffset.Y)) && ((state.Value > 0) && (state.Value < 65000))) {// Up перекрестия
                            joystick.GetBufferedData();
                            KeyUp(state.Value.ToString() + state.Offset.ToString());
                            break_flag = true;
                            break;
                        }
                    }
                }
            }
        }
    }
}
