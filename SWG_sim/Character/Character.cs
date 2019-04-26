using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWG_sim
{
    class Character
    {
        private int remainingHitPoints;
        private string v;

        public string Name { get; set; }
        public int HitPoints { get; set; }
        public int RemainingHitPoints {
            get
            {
                return remainingHitPoints;
            }
            set
            {
                if (value <= 0)
                {
                    remainingHitPoints = 0;
                }
                else
                {
                    remainingHitPoints = value;
                }
            }
        }
        public int ManaPoints { get; set; }
        public int RemainingManaPoints { get; set; }
        public int DefencePoints { get; set; }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        //public int Intelligence { get; set; }
        //public int Wisdom { get; set; }
        public int Toughness { get; set; }
        public int Initiative { get; set; }
        public int CummulativeInitiative { get; set; }
        public Weapon Weapon { get; set; }
        public bool IsAlive { get; set; }
        public bool IsAttacker { get; set; }
        public int DamageDone { get; set; }
        public int DamageTaken { get; set; }
        public int KillCount { get; set; }
        //public Armor Armor { get; set; }

        public Character(string name, bool isAttacker)
        {
            Utils utils = new Utils();
            Weapon = new Weapon();
            Name = name;
            HitPoints = 100 + utils.RandomNumber(51);
            //HitPoints = 20;
            RemainingHitPoints = HitPoints;
            ManaPoints = 20;
            RemainingManaPoints = ManaPoints;
            DefencePoints = 0;
            Strength = 6 + utils.RandomNumber(8) + utils.RandomNumber(8);
            Dexterity = 5;
            Toughness = 5;
            Initiative = 10 + Weapon.InitiativeModifier;
            //Initiative = 1 + utils.RandomNumber(3);
            CummulativeInitiative = Initiative;
            IsAlive = true;
            IsAttacker = isAttacker;
        }

        public Character(string name, int hitPoints, int manaPoints, int defencePoints,
            int strength, int dexterity, int toughness, int initiative, Weapon weapon,
            bool isAlive, bool isAttacker)
        {
            Name = name;
            HitPoints = hitPoints;
            RemainingHitPoints = HitPoints;
            ManaPoints = manaPoints;
            DefencePoints = defencePoints;
            Strength = strength;
            Dexterity = dexterity;
            Toughness = toughness;
            Initiative = initiative;
            CummulativeInitiative = Initiative;
            Weapon = weapon;
            IsAlive = isAlive;
            IsAttacker = isAttacker;
        }

        public Character(string v)
        {
            this.v = v;
        }
    }
}
