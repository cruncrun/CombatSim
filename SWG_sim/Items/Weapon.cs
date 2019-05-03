using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWG_sim
{
    public class Weapon
    {
        public int BaseAttackPower { get; set; }
        public int AttackPowerDiceSides { get; set; }
        public int AttackPowerDiceRolls { get; set; }
        public int InitiativeModifier { get; set; }
        public int AttacksPerTurn { get; set; }
        public int RemainingAttacks { get; set; }
        public int CriticalChance { get; set; }
        public int MinimumDamage { get; set; }
        public int MaximumDamage { get; set; }

        public Weapon()
        {
            Utils utils = new Utils();
            BaseAttackPower = utils.RandomNumber(4, 10);
            AttackPowerDiceSides = utils.RandomNumber(2, 4);
            AttackPowerDiceRolls = utils.RandomNumber(6, 10);
            InitiativeModifier = utils.RandomNumber(0, 11) - 5;
            AttacksPerTurn = utils.RandomNumber(1, 5);
            //AttacksPerTurn = 1;
            RemainingAttacks = AttacksPerTurn;
            CriticalChance = utils.RandomNumber(4) + utils.RandomNumber(4) + 5;
            MinimumDamage = BaseAttackPower + AttackPowerDiceRolls;
            MaximumDamage = BaseAttackPower + (AttackPowerDiceSides * AttackPowerDiceRolls);
        }

        public Weapon(int attackPower, int diceSides, int diceRolls, int attacksPerTurn, int criticalChance)
        {
            BaseAttackPower = attackPower;
            AttackPowerDiceSides = diceSides;
            AttackPowerDiceRolls = diceRolls;
            AttacksPerTurn = attacksPerTurn;
            RemainingAttacks = AttacksPerTurn;
            CriticalChance = criticalChance;
            MinimumDamage = BaseAttackPower + AttackPowerDiceRolls;
            MaximumDamage = BaseAttackPower + (AttackPowerDiceSides * AttackPowerDiceRolls);
        }
    }
}
