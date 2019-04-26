using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWG_sim
{
    class Weapon
    {
        public int BaseAttackPower { get; set; }
        public int AttackPowerDiceSides { get; set; }
        public int AttackPowerDiceRolls { get; set; }
        public int InitiativeModifier { get; set; }
        public int AttacksPerTurn { get; set; }
        public int RemainingAttacks { get; set; }
        public int CriticalChance { get; set; }

        public Weapon()
        {
            Utils utils = new Utils();
            BaseAttackPower = utils.RandomNumber(5, 16);
            AttackPowerDiceSides = utils.RandomNumber(2, 5);
            AttackPowerDiceRolls = utils.RandomNumber(1, 15);
            InitiativeModifier = utils.RandomNumber(0, 10) - 5;
            AttacksPerTurn = utils.RandomNumber(1, 5);
            //AttacksPerTurn = 1;
            RemainingAttacks = AttacksPerTurn;
            CriticalChance = utils.RandomNumber(4) + utils.RandomNumber(4) + 5;
        }

        public Weapon(int attackPower, int diceSides, int diceRolls, int attacksPerTurn, int criticalChance)
        {
            BaseAttackPower = attackPower;
            AttackPowerDiceSides = diceSides;
            AttackPowerDiceRolls = diceRolls;
            AttacksPerTurn = attacksPerTurn;
            RemainingAttacks = AttacksPerTurn;
            CriticalChance = criticalChance;
            

        }
    }
}
