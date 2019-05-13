using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWG_sim
{
    public class Character
    {
        #region Properties
        private int remainingHitPoints;
        private int remainingManaPoints;
        private int defencePoints;  
        
        public string Name { get; set; }
        public int HitPoints { get; set; }
        public int RemainingHitPoints
        {
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
                else if (value > HitPoints)
                {
                    remainingHitPoints = HitPoints;
                }  
                else
                {
                    remainingHitPoints = value;
                }
            }
        }
        public int ManaPoints { get; set; }
        public int RemainingManaPoints
        {
            get
            {
                return remainingManaPoints;
            }
            set
            {
                remainingManaPoints = value <= 0 ? 0 : value;
            }
        }
        public int DefencePoints
        {
            get
            {
                return defencePoints;
            }
            set
            {
                defencePoints = value <= 0 ? 0 : value;
            }
        }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        //public int Intelligence { get; set; }
        //public int Wisdom { get; set; }
        public int Toughness { get; set; }
        public int Initiative { get; set; }
        public int CummulativeInitiative { get; set; }
        public Weapon Weapon { get; set; }
        public Armor Armor { get; set; }
        public bool IsAlive { get; set; }
        public bool IsAttacker { get; set; }
        public int DamageDone { get; set; }
        public int DamageTaken { get; set; }
        public int KillCount { get; set; }
        public int HealingDone { get; set; }
        public int HealingTaken { get; set; }
        public int HealingChance { get; set; }
        #endregion

        #region Constructors
        public Character(string name, bool isAttacker)
        {
            Utils utils = new Utils();
            Weapon = new Weapon();
            Armor = new Armor();
            Name = name;
            HitPoints = 150 + utils.RandomNumber(101);            
            RemainingHitPoints = HitPoints;
            ManaPoints = 20;
            RemainingManaPoints = ManaPoints;            
            Strength = 5 + utils.RandomNumber(11) + utils.RandomNumber(6);
            Dexterity = 5 + utils.RandomNumber(11) + utils.RandomNumber(6);
            Toughness = utils.RandomNumber(5, 11) + utils.RandomNumber(6);
            Initiative = 20 + Weapon.InitiativeModifier + Armor.InitiativeModifier;
            DefencePoints = (Toughness + Armor.DefencePoints) / 2;
            //Initiative = 1 + utils.RandomNumber(3);
            CummulativeInitiative = Initiative;
            HealingChance = GetHealing();
            IsAlive = true;
            IsAttacker = isAttacker;
        }      

        // Boss Constructor
        public Character(string name, int hitPoints, int manaPoints, 
            int strength, int dexterity, int toughness, int initiative, Weapon weapon, Armor armor,
            int healingChance, bool isAlive, bool isAttacker)
        {
            Weapon = weapon;
            Armor = armor;
            Name = name;
            HitPoints = hitPoints;
            RemainingHitPoints = HitPoints;
            ManaPoints = manaPoints;            
            Strength = strength;
            Dexterity = dexterity;
            Toughness = toughness;
            DefencePoints = (Toughness + Armor.DefencePoints) / 2;
            Initiative = initiative;
            CummulativeInitiative = Initiative;
            HealingChance = healingChance;
            IsAlive = isAlive;
            IsAttacker = isAttacker;
        }

        public Character(string name)
        {
            Name = name;
        }

        public Character(int hitPoints, int remainigHitPoints, bool isAlive) // End of turn values storage
        {
            HitPoints = hitPoints;
            RemainingHitPoints = remainigHitPoints;
            IsAlive = isAlive;
        }

        public Character()
        {
        }
        #endregion

        #region Public members

        public List<Character> GetCharacters(int amount)
        {
            try
            {
                List<Character> characters = new List<Character>();
                for (int i = 0; i < amount; i++)
                {
                    characters.Add(GenerateCharacter(true, i));
                }
                for (int i = 0; i < amount; i++)
                {
                    characters.Add(GenerateCharacter(false, i));
                }
                return characters;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Wystąpił błąd w metodzie GetCharacters:" + ex);
                throw;
            }
        }

        #endregion

        #region Private members 
        private Character GenerateCharacter(bool isAttacker, int i)
        {
            try
            {
                switch (isAttacker)
                {
                    case true:
                        return new Character("Atakujący " + i, true);
                    case false:
                        return new Character("Obrońca " + i, false);
                    default:
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Wystąpił błąd w metodzie GenerateCharacter:" + ex);
                throw;
            }
        }

        private int GetHealing()
        {
            int healingAbility = 0;
            Utils utils = new Utils();
            if (utils.RandomNumber(101) <= 20)
            {
                healingAbility = 100;
            }
            return healingAbility;
        }
        #endregion
    }
}
