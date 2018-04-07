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
        public List<Bomb> bombs;
        public List<Fire> fires = new List<Fire>();
        public bool[][] CollisionsMap; //Карта коллизий
        public bool[][] FireMap;
        public GameMap(int x, int y) {
            bombs = new List<Bomb>();
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

            player_two = new Player(curren_form, "P2", 400, 400, 50, 50, @"..\..\GameRes\Image1.png"); // TODO Перед сборпкой обязательно указать новые пути
            player_two.setKeysControl(Keys.A, Keys.W, Keys.D, Keys.S, Keys.F);
            player_two.putBomb += ((int x, int y) => { curren_form.game_map.bombs.Add(new Bomb(curren_form, x, y, 100, 3)); });

            List<Wall> walls = new List<Wall>();
            for (int x = 50; x <= 600; x += 100) for (int y = 50; y <= 500; y += 100) walls.Add(new Wall(curren_form, "iw", x, y, 48, 48));

            cf.game_timer.Interval = 10; // Настройка таймера
            cf.game_timer.Start();
        }

        static public void GameTick() { // Этим методом подписываемся на таймер для отсчёта таймеров всех бомб и взрывов

            if (player_two.CheckTemperature()) player_one.Die(0);
            foreach (Bomb i in curren_form.game_map.bombs) {
                if (i.CheckTimer()) {
                    i.Explosion();
                    curren_form.game_map.bombs.Remove(i);
                    GameTick();  // После удаления объекта нужно заново запускать foreach 
                    return;
                }
            }
            foreach (Fire i in curren_form.game_map.fires) {
                if (i.CheckTimer()) {
                    i.RemoveFire();
                    curren_form.game_map.fires.Remove(i);
                    GameTick();  // После удаления объекта нужно заново запускать foreach 
                    return;
                }
            }
            if (!player_one.IsDead) if (player_one.CheckTemperature()) {
                    player_one.Die(0);
                }
        }

    }

}
