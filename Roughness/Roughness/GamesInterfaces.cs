using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roughness {
    public enum TypesGamesDevice {keyboard = 1, joystick }
    public enum Direction { nope = 0, left, up, right, down }
    public enum Bonuses { nope = 0, power, bomb }
    public enum GameKeys { left = 0, up, right, down, put }
    interface IMortal {
        bool IsDead { set; get; }
        void Die(int die_parametr);
    }
    interface ICanExplode {
        bool CheckTemperature();
    }
    interface IAbleToMove {
        void Move(Direction direction);
    }
    interface IIsBarrier {
        void setCollision(bool collision); // true - Добавляет свою коллизию в карту коллизий, false убирает
    }
}
