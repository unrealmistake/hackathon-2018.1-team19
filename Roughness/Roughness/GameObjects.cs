using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace Roughness {

    public abstract class GameObject {
        protected const int COLLISIONS_COEFFICIENT = 3;
        public RenderingUnit body; // Тело отрисовки
        private int m_curr_position_x; // Текущая позиция на экране в пикселях, при отображении реально находиться в левом верхнем углу
        private int m_curr_position_y;
        public readonly int m_size_x; // Размер, используеться и при отрисовке и при взаимодействии
        public readonly int m_size_y;
        protected int m_center_x; // Центр, каждый раз при изменений х и у пересчитываеться
        protected int m_center_y;
        private Direction direction;
        protected Direction Direction {
            get { return direction; }
            set {
                direction = value;
                if (isAnimated == true) body.Direction = value;
            }
        } // Текущее направление
        protected bool isAnimated;
        protected int m_id;
        protected GameMap curren_map; // Форма на каторой всё рисуетсья

        protected GameObject(GameMap cm, int id, int x, int y, int x_size, int y_size) : this(cm, id, x, y, x_size, y_size, null) { }
        public GameObject(GameMap cm, int id, int x, int y, int x_size, int y_size, string textures_name, bool is_animated = false) {
            curren_map = cm;
            body = new RenderingUnit(x, y, x_size, y_size, cm.renderTarget, textures_name, is_animated);
            this.x = x;
            this.y = y;
            m_size_x = x_size;
            m_size_y = y_size;

            m_center_x = this.x + (m_size_x / 2);
            m_center_y = this.y + (m_size_y / 2);
            
            body.X = this.x;
            body.Y = this.y;
            body.SizeX = x_size;
            body.SizeY = y_size;  
            m_id = id;
            cm.RenderingUnitsList.Add(body);
            isAnimated = is_animated;
        }
        public int x {
            get {
                return m_curr_position_x;
            }
            set {// Если < 0 то будем выходить за пределы массивов при использовании x

                if ((value < 1) || (value > 0x0000ffff)) {
                    value = 1;
                }
                if (value > curren_map.X - m_size_x) return;
                body.X = value;
                m_center_x = x + (m_size_x / 2);
                m_curr_position_x = value;
            }
        }
        public int y {
            get {
                return m_curr_position_y;
            }
            set {// Если < 0 то будем выходить за пределы массивов при использовании y
                if ((value < 1) || (value > 0x0000ffff)) {
                    value = 1;
                }
                if (value > curren_map.Y - m_size_y) return;
                body.Y = value;
                m_center_y = y + (m_size_y / 2);
                m_curr_position_y = value;
            }
        }
    }

    public class Player : GameObject, IMortal, IAbleToMove, ICanExplode {
        private int m_currentAnimationStep;
        private int m_currentAnimationCounter; // Счётчик который считает когда нужно сменить анимацию
        private const int CHANGE_ANIMATED_PICTURE = 10; // Колличество циклов через которое смениться картинка анимации
        private int CurrentAnimationStep {// Текущая картинка анимации [1-4]
            get { return m_currentAnimationStep; }
            set {
                m_currentAnimationStep = value;
                body.CurrentAnimationStep = value;
            }
        }
        private int move_speed { get; set; } // Скорость передвижения игрока
        private List<Keys> keys_control = new List<Keys>(); // Клавиши управления
        private Dictionary<Keys, bool> keys_now_presed = new Dictionary<Keys, bool>(); // Флаги нажатых в данный момент клавиш управления
        public event Action<int, int, int> putBomb;
        private int bombs_power; // Дальность взрыва бомб
        private int number_of_bombs; // Запас бомб
        private PlayersSettings players_setting;
        public Thread gamepad_thread; // Поток в котором будет запущен слушатель геймпада

        public Player(GameMap cm, int id, int x, int y, int x_size, int y_size, string textures_name,bool is_animated) : base(cm, id, x, y, x_size, y_size, textures_name, is_animated) {
            curren_map.game_timer.Tick += new EventHandler((object sender, EventArgs e) => { Move(Direction); });
            move_speed = 3;
            bombs_power = 2;
            number_of_bombs = 2;
            CurrentAnimationStep = 1;
            m_currentAnimationCounter = 0;
            IsDead = false;
            players_setting = GameSettings.playersSettings[id];
            if (players_setting.deviceUsed == TypesGamesDevice.keyboard) {
                curren_map.mainForm.KeyDown += new KeyEventHandler(ActionByKeyboardKeyDown);
                curren_map.mainForm.KeyUp += new KeyEventHandler(ActionByKeyboardKeyUp);
            }
            if (players_setting.deviceUsed == TypesGamesDevice.joystick) {

                //curren_map.mainForm.KeyDown += new KeyEventHandler(ActionByKeyDown);
                //curren_map.mainForm.KeyUp += new KeyEventHandler(ActionByKeyUp);
                players_setting.gameDevice.KeyDown += ActionByGamepadKeyDown;
                players_setting.gameDevice.KeyUp += ActionByGamepadKeyUp;
                gamepad_thread = new Thread(new ThreadStart(players_setting.gameDevice.StartJoystickListener));
                gamepad_thread.IsBackground = true;
                gamepad_thread.Start();
            }

        }
        public void setKeysControl(Keys left, Keys up, Keys right, Keys down, Keys put) {
            keys_control.Clear();
            keys_now_presed.Clear();
            keys_control.Add(left);
            keys_now_presed.Add(left, false);
            keys_control.Add(up);
            keys_now_presed.Add(up, false);
            keys_control.Add(right);
            keys_now_presed.Add(right, false);
            keys_control.Add(down);
            keys_now_presed.Add(down, false);
            keys_control.Add(put);
            keys_now_presed.Add(put, false);
        }
        void ActionByGamepadKeyDown(string command) {
            if (command == players_setting.keyDownСodes[0]) {
                keys_now_presed[keys_control[0]] = true;
                Direction = Direction.left;
            }
            if (command == players_setting.keyDownСodes[1]) {
                keys_now_presed[keys_control[1]] = true;
                Direction = Direction.up;
            }
            if (command == players_setting.keyDownСodes[2]) {
                keys_now_presed[keys_control[2]] = true;
                Direction = Direction.right;
            }
            if (command == players_setting.keyDownСodes[3]) {
                keys_now_presed[keys_control[3]] = true;
                Direction = Direction.down;
            }
            if (command == players_setting.keyDownСodes[4]) {
                if ((!CheckBombsField()) && (number_of_bombs > 0)) {
                    number_of_bombs--;
                    putBomb(x, y, bombs_power);
                }
            }
        }
        void ActionByGamepadKeyUp(string command) {
            if (command == players_setting.keyUpСodes[0]) {
                keys_now_presed[keys_control[0]] = false;
            }
            if (command == players_setting.keyUpСodes[1]) {
                keys_now_presed[keys_control[1]] = false;
            }
            if (command == players_setting.keyUpСodes[2]) {
                keys_now_presed[keys_control[2]] = false;
            }
            if (command == players_setting.keyUpСodes[3]) {
                keys_now_presed[keys_control[3]] = false;
            }
            if (keys_now_presed[keys_control[0]] || keys_now_presed[keys_control[1]] || keys_now_presed[keys_control[2]] || keys_now_presed[keys_control[3]]) CurrentAnimationStep = 1; // Меняем анимаю на стойку прямо
        }
        void ActionByKeyboardKeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == keys_control[0]) {
                keys_now_presed[keys_control[0]] = true;
                Direction = Direction.left;
            }
            if (e.KeyCode == keys_control[1]) {
                keys_now_presed[keys_control[1]] = true;
                Direction = Direction.up;
            }
            if (e.KeyCode == keys_control[2]) {
                keys_now_presed[keys_control[2]] = true;
                Direction = Direction.right;
            }
            if (e.KeyCode == keys_control[3]) {
                keys_now_presed[keys_control[3]] = true;
                Direction = Direction.down;
            }
            if (e.KeyCode == keys_control[4]) {
                if ((!CheckBombsField()) && (number_of_bombs > 0)) {
                    number_of_bombs--;
                    putBomb(x, y, bombs_power);
                }
            }
        }
        void ActionByKeyboardKeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == keys_control[0]) {
                keys_now_presed[keys_control[0]] = false;
            }
            if (e.KeyCode == keys_control[1]) {
                keys_now_presed[keys_control[1]] = false;
            }
            if (e.KeyCode == keys_control[2]) {
                keys_now_presed[keys_control[2]] = false;
            }
            if (e.KeyCode == keys_control[3]) {
                keys_now_presed[keys_control[3]] = false;
            }
        }
        public void Move(Direction direction) {
            if (keys_now_presed[keys_control[0]] || keys_now_presed[keys_control[1]] || keys_now_presed[keys_control[2]] || keys_now_presed[keys_control[3]]) { // Меняем анимацию если надо
                if (m_currentAnimationCounter == CHANGE_ANIMATED_PICTURE) {
                m_currentAnimationCounter = 0;
                if (CurrentAnimationStep != 4) CurrentAnimationStep++;
                else CurrentAnimationStep = 2;
            } else m_currentAnimationCounter++;
        } else CurrentAnimationStep = 1;
            // Передвижение
            if (keys_now_presed[keys_control[0]] == true) {
                if (!((curren_map.CollisionsMap[x - 1][y + COLLISIONS_COEFFICIENT] == true) || // Проверка на коллизию 1 края
                    (curren_map.CollisionsMap[x - 1][y + m_size_y - COLLISIONS_COEFFICIENT] == true) || // Другого края
                    (curren_map.CollisionsMap[x - 1][y + m_size_y / 2 - COLLISIONS_COEFFICIENT] == true))) { // Центра (когда коллизия с маленьким обьектом)
                    x -= move_speed;
                    if (CheckItems()) PickUpItem();
                }
            }
            if (keys_now_presed[keys_control[1]] == true) {
                if (!((curren_map.CollisionsMap[x + COLLISIONS_COEFFICIENT][y - 1] == true) ||
                    (curren_map.CollisionsMap[x + m_size_x - COLLISIONS_COEFFICIENT][y - 1] == true) ||
                    (curren_map.CollisionsMap[x + m_size_x / 2 - COLLISIONS_COEFFICIENT][y - 1] == true))) {
                    y -= move_speed;
                    if (CheckItems()) PickUpItem();
                }
            }
            if (keys_now_presed[keys_control[2]] == true) {
                if (!((curren_map.CollisionsMap[x + m_size_x][y + COLLISIONS_COEFFICIENT] == true) ||
                    (curren_map.CollisionsMap[x + m_size_x][y + m_size_y - COLLISIONS_COEFFICIENT] == true) ||
                    (curren_map.CollisionsMap[x + m_size_x][y + m_size_y / 2 - COLLISIONS_COEFFICIENT] == true))) {
                    x += move_speed;
                    if (CheckItems()) PickUpItem();
                }

            }
            if (keys_now_presed[keys_control[3]] == true) {
                if (!((curren_map.CollisionsMap[x + COLLISIONS_COEFFICIENT][y + m_size_y] == true) ||
                    (curren_map.CollisionsMap[x + m_size_x - COLLISIONS_COEFFICIENT][y + m_size_y] == true) ||
                    (curren_map.CollisionsMap[x + m_size_x / 2 - COLLISIONS_COEFFICIENT][y + m_size_y] == true))) {
                    y += move_speed;
                    if (CheckItems()) PickUpItem();
                }

            }
        }
        public bool CheckTemperature() {
            const int DEPTH = 10;
            bool t1 = curren_map.FireMap[x + DEPTH][y + DEPTH];
            bool t2 = curren_map.FireMap[x + m_size_x - DEPTH][y + DEPTH];
            bool t3 = curren_map.FireMap[x + DEPTH][y + m_size_y - DEPTH];
            bool t4 = curren_map.FireMap[x + m_size_x - DEPTH][y + m_size_y - DEPTH];
            return t1 || t2 || t3 || t4;
        }
        private bool CheckItems() {
            const int DEPTH = 5;
            bool t1 = curren_map.ItemsMap[x + DEPTH][y + DEPTH];
            bool t2 = curren_map.ItemsMap[x + m_size_x - DEPTH][y + DEPTH];
            bool t3 = curren_map.ItemsMap[x + DEPTH][y + m_size_y - DEPTH];
            bool t4 = curren_map.ItemsMap[x + m_size_x - DEPTH][y + m_size_y - DEPTH];
            return t1 || t2 || t3 || t4;
        }
        private bool CheckBombsField() {
            const int DEPTH = 1;
            bool t1 = curren_map.BombsMap[x + DEPTH][y + DEPTH];
            bool t2 = curren_map.BombsMap[x + m_size_x - DEPTH][y + DEPTH];
            bool t3 = curren_map.BombsMap[x + DEPTH][y + m_size_y - DEPTH];
            bool t4 = curren_map.BombsMap[x + m_size_x - DEPTH][y + m_size_y - DEPTH];
            return t1 || t2 || t3 || t4;
        }
        private void PickUpItem() {
            int tmp_x = 0;
            int tmp_y = 0;
            for (int i = x; i <= x + m_size_x; i++)
                for (int j = y; j <= y + m_size_y; j++) {
                    if (curren_map.ItemsMap[i][j] == true) {
                        tmp_x = i;
                        tmp_y = j;
                        i = x + m_size_x + 1; // Для выхода из внечнего цикла
                        break;
                    }
                }
            foreach (BountyItem i in curren_map.bounty_items) {
                if ((tmp_x >= i.x) && (tmp_x <= i.x + i.m_size_x))
                    if ((tmp_y >= i.y) && (tmp_y <= i.y + i.m_size_y)) {
                        Bonuses tmp_b;
                        tmp_b = i.PickUp();
                        switch (tmp_b) {
                            case Bonuses.power: { bombs_power++; break; }
                            case Bonuses.bomb: { number_of_bombs++; break; }
                        }
                        return;
                    }
            }
        }
        public void ReturnBomb() {
            number_of_bombs++;
        }
        public bool IsDead { set; get; }
        public void Die(int die_parametr) {
            IsDead = true;
            MessageBox.Show($"Player {this.m_id} проиграл");
        }

    }
    public class Wall : GameObject, IIsBarrier {
        public Wall(GameMap cm, int id, int x, int y, int x_size, int y_size, string textures_name) : base(cm, id, x, y, x_size, y_size, textures_name) {
            setCollision(true);
        }

        public void setCollision(bool collision) {
            int end_x = x + m_size_x;
            int end_y = y + m_size_y;
            for (int ix = x; ix < end_x; ix++)
                for (int iy = y; iy < end_y; iy++)
                    curren_map.CollisionsMap[ix][iy] = collision;
        }
    }

    public class BrickWall : Wall, IIsBarrier, ICanExplode {
        Bonuses m_bonus;
        protected Random obj_random;
        public BrickWall(GameMap cm, int id, int x, int y, int x_size, int y_size, string textures_name) : base(cm, id, x, y, x_size, y_size, textures_name) {
            const int HOW_OFTEN_DROPS = 4; //Чем меньше тем меньше выпадает
            obj_random = new Random(x + y);
            m_bonus = Bonuses.nope;
            if (obj_random.Next(0, HOW_OFTEN_DROPS) == 1) m_bonus = Bonuses.power;
            if (obj_random.Next(0, HOW_OFTEN_DROPS) == 2) m_bonus = Bonuses.bomb;
        }
        public bool CheckTemperature() {
            const int DEPTH = 10;
            bool t1 = curren_map.FireMap[x + DEPTH][y + DEPTH];
            bool t2 = curren_map.FireMap[x + m_size_x - DEPTH][y + DEPTH];
            bool t3 = curren_map.FireMap[x + DEPTH][y + m_size_y - DEPTH];
            bool t4 = curren_map.FireMap[x + m_size_x - DEPTH][y + m_size_y - DEPTH];
            return t1 || t2 || t3 || t4;
        }
        public void Destroy() {
            if (m_bonus == Bonuses.power) curren_map.bounty_items.Add(new BountyItem(curren_map, x, y, m_bonus, @"Image15"));
            if (m_bonus == Bonuses.bomb) curren_map.bounty_items.Add(new BountyItem(curren_map, x, y, m_bonus, @"Image16"));

            setCollision(false);
            this.body.Hide();
        }
    }

    public class Bomb : GameObject, IIsBarrier, ICanExplode {
        int m_timer_fuse;   // Счётчик времени до взрыва
        int m_explosion_radius; // Радиус взрыва
        public delegate void emptyFunction();
        public event emptyFunction EExplosion;
        public Bomb(GameMap cm, int x, int y, int time_before_explosion, int explosion_radius) : base(cm, 0, x + 5, y + 5, 40, 40, @"Image9") {
            setCollision(true);
            setBombsField(true);
            m_explosion_radius = explosion_radius;
            m_timer_fuse = time_before_explosion;
        }

        public void setCollision(bool collision) {
            const int COLLISIONS_DEPTH = 10; // TODO Обязательно сделать ромбовидную карту коллизии бомбы
            int end_x = x + m_size_x - COLLISIONS_DEPTH;
            int end_y = y + m_size_y - COLLISIONS_DEPTH;
            for (int ix = x + COLLISIONS_DEPTH; ix < end_x; ix++)
                for (int iy = y + COLLISIONS_DEPTH; iy < end_y; iy++)
                    curren_map.CollisionsMap[ix][iy] = collision;
        }
        public void setBombsField(bool bombs_field) {
            const int LEDGE = 5; // Нужен для компенсации маленького размера бомбы
            int end_x = x + m_size_x + LEDGE;
            int end_y = y + m_size_y + LEDGE;
            for (int ix = x - LEDGE; ix < end_x; ix++)
                for (int iy = y - LEDGE; iy < end_y; iy++)
                    curren_map.BombsMap[ix][iy] = bombs_field;
        }

        public bool CheckTimer() { // 
            if (CheckTemperature()) return true; //Если высокая температура то взрываем заранее
            m_timer_fuse--;
            if (m_timer_fuse <= 0) return true;
            return false;
        }
        public bool CheckTemperature() {
            bool t1 = curren_map.FireMap[x][y];
            bool t2 = curren_map.FireMap[x + m_size_x][y];
            bool t3 = curren_map.FireMap[x][y + m_size_y];
            bool t4 = curren_map.FireMap[x + m_size_x][y + m_size_y];
            return t1 || t2 || t3 || t4;
        }
        public void Explosion() {
            setCollision(false);
            setBombsField(false);
            this.body.Hide();
            curren_map.fires.Add(new Fire(curren_map, x - 5, y - 5, Direction.nope, m_explosion_radius, 50, @"Image4"));
            // Проигрываем звук взрыва
            AudioEngine ae = new AudioEngine();
            Thread MusicThread = new Thread(new ParameterizedThreadStart(ae.PlaySound));
            MusicThread.IsBackground = true;
            MusicThread.Start("explosion");
            // Создаём событие
            EExplosion();
        }
    }

    public class Fire : GameObject { // Если direction nope то объект порождает 4 новых c power-1 во всех направлениях, иначе только в своём с power-1 
        int m_timer;        // Счётчик времени

        public Fire(GameMap cm, int cx, int cy, Direction direction, int power, int time, string textures_name) : base(cm, 0, cx, cy, 50, 50, textures_name) {
            m_timer = time;
            base.Direction = direction;
            //setFieldFire(true);

            const int DEPTH = 10; // Как глубоко от краёв пламени проверяеться кооллизия  
            bool t1 = curren_map.CollisionsMap[x + DEPTH][y + DEPTH];
            bool t2 = curren_map.CollisionsMap[x + m_size_x - DEPTH][y + DEPTH];
            bool t3 = curren_map.CollisionsMap[x + DEPTH][y + m_size_y - DEPTH];
            bool t4 = curren_map.CollisionsMap[x + m_size_x - DEPTH][y + m_size_y - DEPTH];
            if (t1 || t2 || t3 || t4 == true) { // Вроверка на столкновения огня с препятсвиями
                setLittleFieldFire(true);
                return;
            } else {
                setFieldFire(true);
            }
            if (power > 1) {
                if (direction == Direction.nope) {
                    curren_map.fires.Add(new Fire(cm, x - 50, y, Direction.left, power - 1, time, @"Image5"));
                    curren_map.fires.Add(new Fire(cm, x, y - 50, Direction.up, power - 1, time, @"Image6"));
                    curren_map.fires.Add(new Fire(cm, x + 50, y, Direction.right, power - 1, time, @"Image5"));
                    curren_map.fires.Add(new Fire(cm, x, y + 50, Direction.down, power - 1, time, @"Image6"));
                }
                if (direction == Direction.left) curren_map.fires.Add(new Fire(cm, x - 50, y, Direction.left, power - 1, time, @"Image5"));
                if (direction == Direction.up) curren_map.fires.Add(new Fire(cm, x, y - 50, Direction.up, power - 1, time, @"Image6"));
                if (direction == Direction.right) curren_map.fires.Add(new Fire(cm, x + 50, y, Direction.right, power - 1, time, @"Image5"));
                if (direction == Direction.down) curren_map.fires.Add(new Fire(cm, x, y + 50, Direction.down, power - 1, time, @"Image6"));
            }

        }
        public bool CheckTimer() {
            m_timer--;
            if (m_timer <= 0) return true;
            return false;
        }
        public void RemoveFire() {
            setFieldFire(false);
            this.body.Hide();
        }
        public void setFieldFire(bool isFire) {
            int end_x = x + m_size_x;
            int end_y = y + m_size_y;
            for (int ix = x; ix < end_x; ix++)
                for (int iy = y; iy < end_y; iy++)
                    curren_map.FireMap[ix][iy] = isFire;
        }
        public void setLittleFieldFire(bool isFire) {
            const int REDUCTION = 10;
            int end_x = x + m_size_x - REDUCTION;
            int end_y = y + m_size_y - REDUCTION;
            for (int ix = x + REDUCTION; ix < end_x; ix++)
                for (int iy = y + REDUCTION; iy < end_y; iy++)
                    curren_map.FireMap[ix][iy] = isFire;
        }
    }

    public class BountyItem : GameObject {
        Bonuses m_bonus;
        public BountyItem(GameMap cm, int start_x, int start_y, Bonuses bonus, string textures_name) : base(cm, 0, start_x, start_y, 50, 50, textures_name) {
            m_bonus = bonus;
            setItemsField(true);
        }
        public Bonuses PickUp() {
            // Проигрываем звук поднятия предмета
            AudioEngine ae = new AudioEngine();
            Thread MusicThread = new Thread(new ParameterizedThreadStart(ae.PlaySound));
            MusicThread.IsBackground = true;
            MusicThread.Start("pickup");

            body.Hide();
            setItemsField(false);
            curren_map.bounty_items.Remove(this);
            return m_bonus;
        }
        public void setItemsField(bool isItem) {
            int end_x = x + m_size_x;
            int end_y = y + m_size_y;
            for (int ix = x; ix < end_x; ix++)
                for (int iy = y; iy < end_y; iy++)
                    curren_map.ItemsMap[ix][iy] = isItem;
        }
    }
}
