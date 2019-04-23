using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWG_sim
{
    class Weapon
    {
        public int AttackPower { get; set; }
        public int InitiativeModifier { get; set; }
        public int AttacksPerTurn { get; set; }
        public int RemainingAttacks { get; set; }

        public Weapon()
        {
            Utils utils = new Utils();
            AttackPower = utils.RandomNumber(0, 10) + 5;
            InitiativeModifier = utils.RandomNumber(0, 10) - 5;
            AttacksPerTurn = utils.RandomNumber(0, 3) + 1;
            //AttacksPerTurn = 1;
            RemainingAttacks = AttacksPerTurn;
        }

        public Weapon(int attackPower, int attacksPerTurn)
        {
            AttackPower = attackPower;
            AttacksPerTurn = attacksPerTurn;
            RemainingAttacks = AttacksPerTurn;
        }
    }
}
