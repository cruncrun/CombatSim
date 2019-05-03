using System;
using System.Collections.Generic;
using System.Drawing;
using Console = Colorful.Console;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWG_sim
{
    public class Action
    {
        #region Properties
        private int damageAmount;
        private int healingAmount;

        public ActionType ActionTypeId { get; set; }
        public Character Character { get; set; }
        public Character Opponent { get; set; }
        public Character Opponent_EOTValues { get; set; }
        public List<Character> AliveParticipants { get; set; }
        public int DamageAmount
        {
            get
            {
                return damageAmount;
            }
            set
            {
                if (value <= 0)
                {
                    damageAmount = 1;
                }
                else
                {
                    damageAmount = value;
                }
            }
        }
        public int HealingAmount
        {
            get
            {
                return healingAmount;
            }
            set
            {
                if (value <= 0)
                {
                    healingAmount = 1;
                }
                else
                {
                    healingAmount = value;
                }
            }
        }
        public bool IsAccurate { get; set; }
        public bool IsCritical { get; set; }
        #endregion

        #region Constructors
        public Action(Character character, List<Character> aliveParticipants)
        {
            Character = character;
            AliveParticipants = aliveParticipants;
        }
        #endregion

        #region Enum
        public enum ActionType
        {
            SingleTargetAttack,
            MultiTargetAttack,
            SingleTargetHealing,
            MultiTargetHealing
        }
        #endregion

        #region Public members
        public void PerformAttack(Action action)
        {
            action.Opponent = SelectTarget(action);
            //Opponent = (Character)SelectTarget(action).Clone(); // prowizorka            

            if (action.Opponent != null && action.Opponent.Name != "Nie ma")
            {
                CheckForHit(action.Character.Dexterity, action.Opponent.Dexterity);
                if (IsAccurate)
                {
                    CheckForCritialHit(action.Character.Weapon.CriticalChance);
                    CalculateAttack(action);
                }
                else
                {
                    //ConsoleWriter.MissedAttackMessage(attack);
                }
                AttackCleanUp(action);
            }  
        }        

        public Character SelectTarget(Action attack)
        {
            Utils utils = new Utils();
            List<Character> opponentsList = GetOppisteSide(attack.Character.IsAttacker, attack.AliveParticipants);
            if (opponentsList.Any())
            {
                Character opponent = opponentsList[utils.RandomNumber(opponentsList.Count)];
                return opponent;
            }            
            return new Character("Nie ma");
        }
        #endregion

        #region Private members
        private void CalculateDamageDone(Action attack)
        {            
            attack.DamageAmount = (attack.Character.Strength / 2) + GetWeaponDamage(attack.Character.Weapon);
            
            if (attack.IsCritical)
            {
                attack.DamageAmount *= 2;
            }
            attack.DamageAmount -= attack.Opponent.DefencePoints;
            attack.Character.DamageDone += attack.DamageAmount;
            attack.Opponent.RemainingHitPoints -= attack.DamageAmount;
            attack.Opponent.DamageTaken += attack.DamageAmount;
        }

        private int GetWeaponDamage(Weapon weapon)
        {
            int damage = weapon.BaseAttackPower;
            for (int i = 0; i < weapon.AttackPowerDiceSides; i++)
            {
                Utils utils = new Utils();
                damage += utils.RandomNumber(0, weapon.AttackPowerDiceRolls);
            }
            return damage;
        }

        private List<Character> GetOppisteSide(bool isAttacking, List<Character> participants)
        {
            List<Character> aliveOpponents = new List<Character>();

            foreach (var character in participants)
            {
                if (character.IsAlive && character.IsAttacker != isAttacking)
                {
                    aliveOpponents.Add(character);
                }
            }
            return aliveOpponents;
        }

        private void CalculateAttack(Action attack)
        {
            if (attack.Character.IsAlive && attack.Opponent.IsAlive)
            {
                CalculateDamageDone(attack);
                if (attack.Opponent.RemainingHitPoints <= 0)
                {
                    //ConsoleWriter.DeathMessage(attack.Opponent.Name);                
                    attack.Character.KillCount++;
                    attack.Opponent.IsAlive = false;
                }
            }            
        }

        private static void AttackCleanUp(Action action)
        {
            action.Character.CummulativeInitiative += action.Character.Initiative;
            action.Character.Weapon.RemainingAttacks--;

            action.Opponent_EOTValues = new Character(action.Opponent.RemainingHitPoints, action.Opponent.IsAlive);
        }

        private void CheckForHit(int attackerDex, int defenderDex)
        {
            IsAccurate = false;
            Utils utils = new Utils();
            if (utils.RandomNumber(100) <= 90 + attackerDex - defenderDex)
            {
                IsAccurate = true;
            }
        }

        private void CheckForCritialHit(int weaponCriticalChance)
        {
            IsCritical = false;
            Utils utils = new Utils();
            if (utils.RandomNumber(100) <= weaponCriticalChance)
            {
                IsCritical = true;
            }
        }
        #endregion
    }
}
