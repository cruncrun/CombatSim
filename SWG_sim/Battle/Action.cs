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
        public Character Target { get; set; }
        public Character Target_EOTValues { get; set; }
        public List<Character> AliveParticipants { get; set; }
        public bool CleanupDone { get; set; }
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
            CleanupDone = false;
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
        public void PerformAction(Action action)
        {
            switch (action.ActionTypeId)
            {
                case ActionType.SingleTargetAttack:
                    action.Target = SelectEnemyTarget(action);

                    if (action.Target != null && action.Target.Name != "Nie ma")
                    {
                        CheckForHit(action.Character.Dexterity, action.Target.Dexterity);
                        if (IsAccurate)
                        {
                            CheckForCritialHit(action.Character.Weapon.CriticalChance);
                            CalculateAttack(action);
                        }
                    }
                    break;

                case ActionType.SingleTargetHealing:
                    action.Target = SelectAlliedTarget(action);

                    if (action.Target != null && action.Target.Name != "Nie ma")
                    {
                        CheckForCritialHit(action.Character.Weapon.CriticalChance);
                        CalculateHealing(action);                        
                    }
                    break;

                case ActionType.MultiTargetAttack:
                    break;

                case ActionType.MultiTargetHealing:
                    break;

                default:
                    break;
            }
            AttackCleanUp(action);

        }

        
        

        public Character SelectEnemyTarget(Action attack)
        {
            Utils utils = new Utils();
            List<Character> opponentsList = GetListOfTargets(attack.Character.IsAttacker, attack.AliveParticipants);
            if (opponentsList.Any())
            {
                Character opponent = opponentsList[utils.RandomNumber(opponentsList.Count)];
                return opponent;
            }
            return new Character("Nie ma");
        }

        public Character SelectAlliedTarget(Action healing)
        {
            Utils utils = new Utils();
            List<Character> opponentsList = GetListOfTargets(!healing.Character.IsAttacker, healing.AliveParticipants);
            if (opponentsList.Any())
            {
                Character opponent = opponentsList[utils.RandomNumber(opponentsList.Count)];
                return opponent;
            }
            return new Character("Nie ma");
        }

        public ActionType GetActionTypeId(Character character)
        {
            Utils utils = new Utils();
            if (utils.RandomNumber(101) <= character.HealingChance)
            {
                return ActionType.SingleTargetHealing;
            }
            else
            {
                return ActionType.SingleTargetAttack;
            }
        }
        #endregion

        #region Private members
        private void CalculateDamageDone(Action attack)
        {
            attack.DamageAmount = (attack.Character.Strength / 2) + GetWeaponDamage(attack.Character.Weapon);
            /*
            TODO: Zdecydować, czy zwiększenie zadawanych obrażeń związanych z trafieniem krytycznym jest rozpatrywane
            przed czy po uwzględnieniu pancerza.
            */
            attack.DamageAmount -= attack.Target.DefencePoints;
            if (attack.IsCritical)
            {
                attack.DamageAmount *= 2;
            }
            
            attack.Character.DamageDone += attack.DamageAmount;
            attack.Target.RemainingHitPoints -= attack.DamageAmount;
            attack.Target.DamageTaken += attack.DamageAmount;
        }

        private void CalculateHealing(Action healing)
        {
            if (healing.Character.IsAlive && healing.Target.IsAlive && Target.RemainingHitPoints < Target.HitPoints)
            {
                healing.HealingAmount = GetWeaponDamage(healing.Character.Weapon);

                if (healing.IsCritical)
                {
                    healing.HealingAmount *= 2;
                }
                healing.Character.HealingDone += healing.HealingAmount;
                healing.Target.RemainingHitPoints += healing.HealingAmount;
                healing.Target.HealingTaken += healing.HealingAmount;
            }
            else
            {
                healing.ActionTypeId = ActionType.SingleTargetAttack;
                PerformAction(healing);
            }
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

        private List<Character> GetListOfTargets(bool isAttacking, List<Character> participants)
        {
            List<Character> aliveTargets = new List<Character>();

            foreach (var character in participants)
            {
                if (character.IsAlive && character.IsAttacker != isAttacking)
                {
                    aliveTargets.Add(character);
                }
            }
            return aliveTargets;
        }

        private void CalculateAttack(Action attack)
        {
            if (attack.Character.IsAlive && attack.Target.IsAlive)
            {
                CalculateDamageDone(attack);
                if (attack.Target.RemainingHitPoints <= 0)
                {
                    //ConsoleWriter.DeathMessage(attack.Opponent.Name);                
                    attack.Character.KillCount++;
                    attack.Target.IsAlive = false;
                }
            }
        }

        private static void AttackCleanUp(Action action)
        {
            if (!action.CleanupDone)
            {
                action.Character.CummulativeInitiative += action.Character.Initiative;
                action.Character.Weapon.RemainingAttacks--;
                action.Target_EOTValues = new Character(action.Target.RemainingHitPoints, action.Target.IsAlive);
                action.CleanupDone = true;
            }
            
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
