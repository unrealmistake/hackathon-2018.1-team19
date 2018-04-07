using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpDX.Direct2D1;
using SharpDX.Windows;
using Timer = System.Windows.Forms.Timer;
namespace Roughness {
    // Экземпляр игровой карты, содержит в себе все необходимые ресурсы для отыгрыша одной игровой сессии(от старта до первой смерти игрока)
    public class GameMap {
        public int X { get; set; }
        public int Y { get; set; }
        public RenderForm mainForm;  // Экземпляр формы (Direct2d) на которой идёт отрисовка
        public RenderTarget renderTarget; // RenderTarget 
        public List<RenderingUnit> RenderingUnitsList; // Список объёктов для отрисови на игровом поле
        public Timer game_timer; // Главный таймер игры
        // Списки игровых объектов
        public List<Bomb> bombs = new List<Bomb>();
        public List<Fire> fires = new List<Fire>();
        public List<Wall> walls = new List<Wall>();
        public List<BrickWall> brick_walls = new List<BrickWall>();
        public List<BountyItem> bounty_items = new List<BountyItem>();
        // Битовые карты
        public bool[][] CollisionsMap; //Карта коллизий
        public bool[][] FireMap;
        public bool[][] ItemsMap;
        public bool[][] BombsMap;
        public GameMap(int x, int y, RenderForm main_form, RenderTarget render_target, List<RenderingUnit> rendering_units_list) {
            game_timer = new Timer();
            RenderingUnitsList = rendering_units_list;
            X = x;
            Y = y;
            mainForm = main_form;
            renderTarget = render_target;
            CollisionsMap = new bool[x + 50][];
            FireMap = new bool[x + 50][];
            ItemsMap = new bool[x + 50][];
            BombsMap = new bool[x + 50][];
            for (int i = 0; i < x + 50; i++) {
                CollisionsMap[i] = new bool[y + 50];
                FireMap[i] = new bool[y + 50];
                ItemsMap[i] = new bool[y + 50];
                BombsMap[i] = new bool[y + 50];
            }
        }
    }
    class Game {
        static GameMap curren_map;
        static Player player_one;
        static Player player_two;
        static public void StartGame(GameMap cm) {
            
            curren_map = cm;
            //curren_map = new GameMap((int)cm.ClientSize.Width, (int)cm.ClientSize.Height);

            curren_map.game_timer.Tick += new EventHandler((object sender, EventArgs e) => { GameTick(); });

            player_one = new Player(curren_map, 1, 1, 1, 50, 50, @"player1", true ); // TODO Перед сборпкой обязательно указать новые пути
            player_one.setKeysControl(Keys.Left, Keys.Up, Keys.Right, Keys.Down, Keys.Space);
            player_one.putBomb += ((int x, int y, int bombs_power) => {
                Bomb bomb = new Bomb(curren_map, x, y, 150, bombs_power);
                bomb.EExplosion += () => { player_one.ReturnBomb(); };
                curren_map.bombs.Add(bomb);
            });

            player_two = new Player(curren_map, 2, 600, 500, 50, 50, @"player2", true); // TODO Перед сборпкой обязательно указать новые пути
            player_two.setKeysControl(Keys.A, Keys.W, Keys.D, Keys.S, Keys.F);
            player_two.putBomb += ((int x, int y, int bombs_power) => {
                Bomb bomb = new Bomb(curren_map, x, y, 100, bombs_power);
                bomb.EExplosion += () => { player_two.ReturnBomb(); };
                curren_map.bombs.Add(bomb);
            });

            //curren_form.game_map.brick_walls.Add(new BrickWall(curren_form, "bw", 150, 100, 50, 50, @"..\..\GameRes\Image7.png"));
            for (int x = 50; x <= 600; x += 100) for (int y = 50; y <= 500; y += 100) curren_map.walls.Add(new Wall(curren_map, 0, x, y, 50, 50, @"Image8"));

            Random rd = new Random();
            for (int x = 0; x <= 600; x += 50) for (int y = 0; y <= 500; y += 50) {
                    if ((x == 0) && (y == 0)) continue; // Расчичаем угол первого игрока
                    if ((x == 0) && (y == 50)) continue;
                    if ((x == 50) && (y == 0)) continue;
                    if ((x == 600) && (y == 500)) continue; // Угол второго игрока
                    if ((x == 550) && (y == 500)) continue;
                    if ((x == 600) && (y == 450)) continue;
                    if (curren_map.CollisionsMap[x][y] == true) continue; // Там где уже есть стены

                    if (rd.Next(4) != 3) curren_map.brick_walls.Add(new BrickWall(curren_map, 0, x, y, 50, 50, @"Image7"));
                }

            cm.game_timer.Interval = 10; // Настройка таймера
            cm.game_timer.Start();
        }

        static public void GameTick() { // Этим методом подписываемся на таймер для отсчёта таймеров всех бомб и взрывов
            List<GameObject> objects_for_removal = new List<GameObject>();
            //if (player_two.CheckTemperature()) player_one.Die(0);
            foreach (Bomb i in curren_map.bombs) {
                if (i.CheckTimer()) {
                    i.Explosion();
                    objects_for_removal.Add(i);
                }
            }
            foreach (Fire i in curren_map.fires) {
                if (i.CheckTimer()) {
                    i.RemoveFire();
                    objects_for_removal.Add(i);
                }
            }
            foreach (BrickWall i in curren_map.brick_walls) {
                if (i.CheckTemperature()) {
                    i.Destroy();
                    objects_for_removal.Add(i);
                }
            }
            if (!player_one.IsDead) if (player_one.CheckTemperature()) {
                    player_one.Die(0);
                }
            if (!player_two.IsDead) if (player_two.CheckTemperature()) {
                    player_two.Die(0);
                }
            foreach (GameObject i in objects_for_removal) { // Удаляем объекты из списка
                if (i is Bomb) curren_map.bombs.Remove((Bomb)i);
                if (i is Fire) curren_map.fires.Remove((Fire)i);
                if (i is BrickWall) curren_map.brick_walls.Remove((BrickWall)i);
            }
            objects_for_removal.Clear();
        }

    }

}
