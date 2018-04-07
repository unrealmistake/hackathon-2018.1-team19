using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roughness {
    public enum Direction { nope = 0, left, up, right, down }

    interface IMortal {
        bool IsDead();
        void Die(int die_parametr);
    }

    interface IAbleToMove {
        void Move(Direction direction);
    }
    interface IIsBarrier {
        void setCollision(bool collision); // true - Добавляет свою коллизию в карту коллизий, false убирает
    }
}
