using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Roughness {

    public abstract class GameObject {
        protected const int COLLISIONS_COEFFICIENT = 3;
        public PictureBox body; // Тело отрисовки
        private int m_curr_position_x; // Текущая позиция на экране в пикселях, при отображении реально находиться в левом верхнем углу
        private int m_curr_position_y;
        protected readonly int m_size_x; // Размер, используеться и при отрисовке и при взаимодействии
        protected readonly int m_size_y;
        protected int m_center_x; // Центр, каждый раз при изменений х и у пересчитываеться
        protected int m_center_y;
        protected Direction m_direction; // Текущее направление
        protected string m_id;
        protected GameForm curren_form; // Форма на каторой всё рисуетсья
        public Point m_location;  // Обьект необходимый для отрисовки текущей позиии на форме

        protected GameObject(GameForm cf, string id, int x, int y, int x_size, int y_size) : this(cf, id, x, y, x_size, y_size, null) { }
        public GameObject(GameForm cf, string id, int x, int y, int x_size, int y_size, string image_path) {
            curren_form = cf;
            m_location = new System.Drawing.Point(x, y);
            body = new PictureBox();
            this.x = x;
            this.y = y;
            m_size_x = x_size;
            m_size_y = y_size;

            m_center_x = x + (m_size_x / 2);
            m_center_y = y + (m_size_y / 2);
            body.Size = new System.Drawing.Size(x_size, y_size);
            body.ClientSize = body.Size;
            body.Location = m_location;
            body.Name = id;
            body.Text = "";
            body.TabIndex = 0;
            m_id = id;

            if (image_path != null) body.Image = Image.FromFile(image_path);
            else body.BackColor = Color.Bisque;
            body.SizeMode = PictureBoxSizeMode.StretchImage; // Растягивание текстуры
            cf.Controls.Add(body);
        }
        public int x {
            get {
                return m_curr_position_x;
            }
            set {// Если < 0 то будем выходить за пределы массивов при использовании x
                if ((value < 1) || (value > curren_form.ClientSize.Width - m_size_x)) return;

                m_location.X = value;
                body.Location = m_location;
                m_center_x = x + (m_size_x / 2);
                m_curr_position_x = value;
            }
        }
        public int y {
            get {
                return m_curr_position_y;
            }
            set {// Если < 0 то будем выходить за пределы массивов при использовании y
                if ((value < 1) || (value > curren_form.ClientSize.Height - m_size_y)) return;
                m_location.Y = value;
                body.Location = m_location;
                m_center_y = y + (m_size_y / 2);
                m_curr_position_y = value;
            }
        }
    }

    public class Player : GameObject, IMortal, IAbleToMove, ICanExplode {
        private int move_speed { get; set; } // Скорость передвижения игрока
        private List<Keys> keys_control = new List<Keys>(); // Клавиши управления
        private Dictionary<Keys, bool> keys_now_presed = new Dictionary<Keys, bool>(); // Флаги нажатых в данный момент клавиш управления
        public event Action<int, int> putBomb;

        public Player(GameForm cf, string id, int x, int y, int x_size, int y_size, string image_path) : base(cf, id, x, y, x_size, y_size, image_path) {
            curren_form.KeyDown += new KeyEventHandler(ActionByKeyDown);
            curren_form.KeyUp += new KeyEventHandler(ActionByKeyUp);
            curren_form.game_timer.Tick += new EventHandler((object sender, EventArgs e) => { Move(m_direction); });
            move_speed = 3;
            IsDead = false;
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
        void ActionByKeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == keys_control[0]) {
                keys_now_presed[keys_control[0]] = true;
                m_direction = Direction.left;
            }
            if (e.KeyCode == keys_control[1]) {
                keys_now_presed[keys_control[1]] = true;
                m_direction = Direction.up;
            }
            if (e.KeyCode == keys_control[2]) {
                keys_now_presed[keys_control[2]] = true;
                m_direction = Direction.right;
            }
            if (e.KeyCode == keys_control[3]) {
                keys_now_presed[keys_control[3]] = true;
                m_direction = Direction.down;
            }
            if (e.KeyCode == keys_control[4]) {
                if (putBomb != null) putBomb(x, y);
            }

        }
        void ActionByKeyUp(object sender, KeyEventArgs e) {
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

            if (keys_now_presed[keys_control[0]] == true) {
                if (!((curren_form.game_map.CollisionsMap[x - 1][y + COLLISIONS_COEFFICIENT] == true) || // Проверка на коллизию 1 края
                    (curren_form.game_map.CollisionsMap[x - 1][y + m_size_y - COLLISIONS_COEFFICIENT] == true) || // Другого края
                    (curren_form.game_map.CollisionsMap[x - 1][y + m_size_y / 2 - COLLISIONS_COEFFICIENT] == true) // Центра (когда коллизия с маленьким обьектом)
                    ))
                    x -= move_speed;
            }
            if (keys_now_presed[keys_control[1]] == true) {
                if (!((curren_form.game_map.CollisionsMap[x + COLLISIONS_COEFFICIENT][y - 1] == true) ||
                    (curren_form.game_map.CollisionsMap[x + m_size_x - COLLISIONS_COEFFICIENT][y - 1] == true) ||
                    (curren_form.game_map.CollisionsMap[x + m_size_x / 2 - COLLISIONS_COEFFICIENT][y - 1] == true)))
                    y -= move_speed;

            }
            if (keys_now_presed[keys_control[2]] == true) {
                if (!((curren_form.game_map.CollisionsMap[x + m_size_x][y + COLLISIONS_COEFFICIENT] == true) ||
                    (curren_form.game_map.CollisionsMap[x + m_size_x][y + m_size_y - COLLISIONS_COEFFICIENT] == true) ||
                    (curren_form.game_map.CollisionsMap[x + m_size_x][y + m_size_y / 2 - COLLISIONS_COEFFICIENT] == true)))
                    x += move_speed;
            }
            if (keys_now_presed[keys_control[3]] == true) {
                if (!((curren_form.game_map.CollisionsMap[x + COLLISIONS_COEFFICIENT][y + m_size_y] == true) ||
                    (curren_form.game_map.CollisionsMap[x + m_size_x - COLLISIONS_COEFFICIENT][y + m_size_y] == true) ||
                    (curren_form.game_map.CollisionsMap[x + m_size_x / 2 - COLLISIONS_COEFFICIENT][y + m_size_y] == true)))
                    y += move_speed;
            }
        }
        public bool CheckTemperature() {
            const int DEPTH = 10;
            bool t1 = curren_form.game_map.FireMap[x + DEPTH][y + DEPTH];
            bool t2 = curren_form.game_map.FireMap[x + m_size_x - DEPTH][y + DEPTH];
            bool t3 = curren_form.game_map.FireMap[x + DEPTH][y + m_size_y - DEPTH];
            bool t4 = curren_form.game_map.FireMap[x + m_size_x - DEPTH][y + m_size_y - DEPTH];
            return t1 || t2 || t3 || t4;
        }
        public bool IsDead { set; get; }
        public void Die(int die_parametr) {
            IsDead = true;
            MessageBox.Show($"Player {this.m_id} проиграл");
        }

    }
    public class Wall : GameObject, IIsBarrier {
        public Wall(GameForm cf, string id, int x, int y, int x_size, int y_size, string image_path) : base(cf, id, x, y, x_size, y_size, image_path) {
            setCollision(true);
        }

        public void setCollision(bool collision) {
            int end_x = x + m_size_x;
            int end_y = y + m_size_y;
            for (int ix = x; ix < end_x; ix++)
                for (int iy = y; iy < end_y; iy++)
                    curren_form.game_map.CollisionsMap[ix][iy] = collision;
        }
    }

    public class BrickWall : Wall, IIsBarrier, ICanExplode {
        public BrickWall(GameForm cf, string id, int x, int y, int x_size, int y_size, string image_path) : base(cf, id, x, y, x_size, y_size, image_path) {

        }
        public bool CheckTemperature() {
            const int DEPTH = 10;
            bool t1 = curren_form.game_map.FireMap[x + DEPTH][y + DEPTH];
            bool t2 = curren_form.game_map.FireMap[x + m_size_x - DEPTH][y + DEPTH];
            bool t3 = curren_form.game_map.FireMap[x + DEPTH][y + m_size_y - DEPTH];
            bool t4 = curren_form.game_map.FireMap[x + m_size_x - DEPTH][y + m_size_y - DEPTH];
            return t1 || t2 || t3 || t4;
        }
        public void Destroy() {
            setCollision(false);
            this.body.Hide();
        }
    }

    public class Bomb : GameObject, IIsBarrier, ICanExplode {
        int m_timer_fuse;   // Счётчик времени до взрыва
        int m_explosion_radius; // Радиус взрыва
        public Bomb(GameForm cf, int x, int y, int time_before_explosion, int explosion_radius) : base(cf, "b", x, y, 40, 40, @"..\..\GameRes\Image3.png") {
            setCollision(true);
            m_explosion_radius = explosion_radius;
            m_timer_fuse = time_before_explosion;
        }

        public void setCollision(bool collision) {
            const int COLLISIONS_DEPTH = 10; // TODO Обязательно сделать ромбовидную карту коллизии бомбы
            int end_x = x + m_size_x - COLLISIONS_DEPTH;
            int end_y = y + m_size_y - COLLISIONS_DEPTH;
            for (int ix = x + COLLISIONS_DEPTH; ix < end_x; ix++)
                for (int iy = y + COLLISIONS_DEPTH; iy < end_y; iy++)
                    curren_form.game_map.CollisionsMap[ix][iy] = collision;
        }

        public bool CheckTimer() { // 
            if (CheckTemperature()) return true; //Если высокая температура то взрываем заранее
            m_timer_fuse--;
            if (m_timer_fuse <= 0) return true;
            return false;
        }
        public bool CheckTemperature() {
            bool t1 = curren_form.game_map.FireMap[x][y];
            bool t2 = curren_form.game_map.FireMap[x + m_size_x][y];
            bool t3 = curren_form.game_map.FireMap[x][y + m_size_y];
            bool t4 = curren_form.game_map.FireMap[x + m_size_x][y + m_size_y];
            return t1 || t2 || t3 || t4;
        }
        public void Explosion() {
            setCollision(false);
            this.body.Hide();
            curren_form.game_map.fires.Add(new Fire(curren_form, x, y, Direction.nope, m_explosion_radius, 50, @"..\..\GameRes\Image4.png"));
        }
    }

    public class Fire : GameObject { // Если direction nope то объект порождает 4 новых c power-1 во всех направлениях, иначе только в своём с power-1 
        int m_timer;        // Счётчик времени

        public Fire(GameForm cf, int x, int y, Direction direction, int power, int time, string image_path) : base(cf, "f", x, y, 50, 50, image_path) {
            m_timer = time; // 2 итерации анимации
            m_direction = direction;
            setFieldFire(true);
            if (power > 1) {
                if (direction == Direction.nope) {
                    curren_form.game_map.fires.Add(new Fire(cf, x - 50, y, Direction.left, power - 1, time, @"..\..\GameRes\Image5.png"));
                    curren_form.game_map.fires.Add(new Fire(cf, x, y - 50, Direction.up, power - 1, time, @"..\..\GameRes\Image6.png"));
                    curren_form.game_map.fires.Add(new Fire(cf, x + 50, y, Direction.right, power - 1, time, @"..\..\GameRes\Image5.png"));
                    curren_form.game_map.fires.Add(new Fire(cf, x, y + 50, Direction.down, power - 1, time, @"..\..\GameRes\Image6.png"));
                }
                if (direction == Direction.left) curren_form.game_map.fires.Add(new Fire(cf, x - 50, y, Direction.left, power - 1, time, @"..\..\GameRes\Image5.png"));
                if (direction == Direction.up) curren_form.game_map.fires.Add(new Fire(cf, x, y - 50, Direction.up, power - 1, time, @"..\..\GameRes\Image6.png"));
                if (direction == Direction.right) curren_form.game_map.fires.Add(new Fire(cf, x + 50, y, Direction.right, power - 1, time, @"..\..\GameRes\Image5.png"));
                if (direction == Direction.down) curren_form.game_map.fires.Add(new Fire(cf, x, y + 50, Direction.down, power - 1, time, @"..\..\GameRes\Image6.png"));
            }

        }
        public bool CheckTimer() { // 
            m_timer--;
            if (m_timer <= 0) return true;
            return false;
        }
        public void RemoveFire() {
            setFieldFire(false);
            this.body.Hide();
        }
        public void setFieldFire(bool fire) {
            int end_x = x + m_size_x;
            int end_y = y + m_size_y;
            for (int ix = x; ix < end_x; ix++)
                for (int iy = y; iy < end_y; iy++)
                    curren_form.game_map.FireMap[ix][iy] = fire;
        }

    }


}
