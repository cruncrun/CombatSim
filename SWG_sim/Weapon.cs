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
        public int CriticalChance { get; set; }

        public Weapon()
        {
            Utils utils = new Utils();
            AttackPower = utils.RandomNumber(0, 10) + 5;
            InitiativeModifier = utils.RandomNumber(0, 10) - 5;
            AttacksPerTurn = utils.RandomNumber(0, 4) + 1;
            //AttacksPerTurn = 1;
            RemainingAttacks = AttacksPerTurn;
            CriticalChance = utils.RandomNumber(4) + utils.RandomNumber(4) + 5;
        }

        public Weapon(int attackPower, int attacksPerTurn, int criticalChance)
        {
            AttackPower = attackPower;
            AttacksPerTurn = attacksPerTurn;
            RemainingAttacks = AttacksPerTurn;
            CriticalChance = criticalChance;
        }
    }
}
