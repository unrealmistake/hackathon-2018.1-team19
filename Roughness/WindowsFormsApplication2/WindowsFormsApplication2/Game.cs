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
    public class GameMap {
        public List<Bomb> bombs = new List<Bomb>();
        public List<Fire> fires = new List<Fire>();
        public List<Wall> walls = new List<Wall>();
        public List<BrickWall> brick_walls = new List<BrickWall>();
        public bool[][] CollisionsMap; //Карта коллизий
        public bool[][] FireMap;
        public GameMap(int x, int y) {
            CollisionsMap = new bool[x + 50][];
            for (int i = 0; i < x + 50; i++) CollisionsMap[i] = new bool[y + 50];
            FireMap = new bool[x + 50][];
            for (int i = 0; i < x + 50; i++) FireMap[i] = new bool[y + 50];
        }
    }
    static class Game {
        static GameForm curren_form;
        static Player player_one;
        static Player player_two;
        static public void start(GameForm cf) {

            curren_form = cf;
            curren_form.game_map = new GameMap((int)cf.ClientSize.Width, (int)cf.ClientSize.Height);

            curren_form.game_timer.Tick += new EventHandler((object sender, EventArgs e) => { GameTick(); });

            player_one = new Player(curren_form, "P1", 1, 1, 50, 50, @"..\..\GameRes\Image1.png"); // TODO Перед сборпкой обязательно указать новые пути
            player_one.setKeysControl(Keys.Left, Keys.Up, Keys.Right, Keys.Down, Keys.Space);
            player_one.putBomb += ((int x, int y) => { curren_form.game_map.bombs.Add(new Bomb(curren_form, x, y, 100, 3)); });

            player_two = new Player(curren_form, "P2", 600, 500, 50, 50, @"..\..\GameRes\Image1.png"); // TODO Перед сборпкой обязательно указать новые пути
            player_two.setKeysControl(Keys.A, Keys.W, Keys.D, Keys.S, Keys.F);
            player_two.putBomb += ((int x, int y) => { curren_form.game_map.bombs.Add(new Bomb(curren_form, x, y, 100, 3)); });

            //curren_form.game_map.brick_walls.Add(new BrickWall(curren_form, "bw", 150, 100, 50, 50, @"..\..\GameRes\Image7.png"));
            for (int x = 50; x <= 600; x += 100) for (int y = 50; y <= 500; y += 100) curren_form.game_map.walls.Add(new Wall(curren_form, "iw", x, y, 50, 50, @"..\..\GameRes\Image8.png"));

            Random rd = new Random();
            for (int x = 0; x <= 600; x += 50) for (int y = 0; y <= 500; y += 50) {
                    if ((x == 0) && (y == 0)) continue; // Расчичаем угол первого игрока
                    if ((x == 0) && (y == 50)) continue;
                    if ((x == 50) && (y == 0)) continue;
                    if ((x == 600) && (y == 500)) continue; // Угол второго игрока
                    if ((x == 550) && (y == 500)) continue;
                    if ((x == 600) && (y == 450)) continue;
                    if (curren_form.game_map.CollisionsMap[x][y] == true) continue; // Там где уже есть стены

                    if (rd.Next(4) != 3) curren_form.game_map.brick_walls.Add(new BrickWall(curren_form, "bw", x, y, 50, 50, @"..\..\GameRes\Image7.png"));
                }

            cf.game_timer.Interval = 10; // Настройка таймера
            cf.game_timer.Start();
        }

        static public void GameTick() { // Этим методом подписываемся на таймер для отсчёта таймеров всех бомб и взрывов
            List<GameObject> objects_for_removal = new List<GameObject>();
            //if (player_two.CheckTemperature()) player_one.Die(0);
            foreach (Bomb i in curren_form.game_map.bombs) {
                if (i.CheckTimer()) {
                    i.Explosion();
                    objects_for_removal.Add(i);
                }
            }
            foreach (Fire i in curren_form.game_map.fires) {
                if (i.CheckTimer()) {
                    i.RemoveFire();
                    objects_for_removal.Add(i);
                }
            }
            foreach (BrickWall i in curren_form.game_map.brick_walls) {
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
                if (i is Bomb) curren_form.game_map.bombs.Remove((Bomb)i);
                if (i is Fire) curren_form.game_map.fires.Remove((Fire)i);
                if (i is BrickWall) curren_form.game_map.brick_walls.Remove((BrickWall)i);
            }
            objects_for_removal.Clear();
        }

    }

}
