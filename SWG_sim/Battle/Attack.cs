using System;
using System.Collections.Generic;
using System.Drawing;
using Console = Colorful.Console;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWG_sim
{
    public class Attack : IAction
    {
        #region Properties
        private int damageAmount;
        private int healingAmount;

        public ActionType ActionTypeId { get; set; }
        public Character Character { get; set; }
        public Character Target { get; set; }
        public Character Target_EOTValues { get; set; }
        public List<Character> AliveParticipants { get; set; }        
        public int DamageAmount
        {
            get
            {
                return damageAmount;
            }
            set
            {
                damageAmount = value <= 0 ? 1 : value;                
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
                healingAmount = value <= 0 ? 1 : value;
            }
        }
        public bool IsAccurate { get; set; }
        public bool IsCritical { get; set; }
        public bool CleanupDone { get; set; }
        CombatSim_Enum.ActionType IAction.ActionTypeId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        List<Character> IAction.Target { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        #endregion

        #region Constructors
        public Attack(Character character, List<Character> aliveParticipants)
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
        public void PerformAction(Attack action)
        {
            switch (action.ActionTypeId)
            {                
                case ActionType.SingleTargetAttack:
                    action.Target = SelectEnemyTarget(action);

                    if (action.Target != null)
                    {
                        CheckForHit(action.Character.Dexterity, action.Target.Dexterity);
                        if (IsAccurate)
                        {
                            CheckForCritialHit(action.Character.Weapon.CriticalChance);
                            CalculateAttack();
                        }
                    }
                    break;

                case ActionType.SingleTargetHealing:
                    action.Target = SelectAlliedTarget(action);

                    if (action.Target != null)
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

        public Character SelectEnemyTarget(Attack attack)
        {
            Utils utils = new Utils();
            List<Character> opponentsList = GetListOfTargets(attack.Character.IsAttacker, attack.AliveParticipants);
            if (opponentsList.Any())
            {
                Character opponent = opponentsList[utils.RandomNumber(opponentsList.Count)];
                return opponent;
            }
            return null;
        }

        public Character SelectAlliedTarget(Attack healing)
        {
            Utils utils = new Utils();
            List<Character> opponentsList = GetListOfTargets(!healing.Character.IsAttacker, healing.AliveParticipants);
            if (opponentsList.Any())
            {
                Character opponent = opponentsList[utils.RandomNumber(opponentsList.Count)];
                return opponent;
            }
            return null;
        }

        public ActionType GetActionTypeId(Character character)
        {
            //if (character.HealingChance > 0)
            //{
            //    return ActionType.SingleTargetHealing;
            //}
            //else
            //{
            //    return ActionType.SingleTargetAttack;
            //}
            return ActionType.SingleTargetAttack;
        }
        #endregion

        #region Private members
        private void CalculateDamageDone()
        {
            DamageAmount = (Character.Strength / 2) + GetWeaponDamage(Character.Weapon);
            /*
            TODO: Zdecydować, czy zwiększenie zadawanych obrażeń związanych z trafieniem krytycznym jest rozpatrywane
            przed czy po uwzględnieniu pancerza.
            */
            DamageAmount -= Target.DefencePoints;
            if (IsCritical)
            {
                DamageAmount *= 2;
            }
            
            Character.DamageDone += DamageAmount;
            Target.RemainingHitPoints -= DamageAmount;
            Target.DamageTaken += DamageAmount;
        }

        private void CalculateHealing(Attack healing)
        {
            if (healing.Character.IsAlive
                && healing.Target.IsAlive
                && healing.Target.RemainingHitPoints < healing.Target.HitPoints)
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

        private void CalculateAttack()
        {
            if (Character.IsAlive && Target.IsAlive)
            {
                CalculateDamageDone();
                if (Target.RemainingHitPoints <= 0)
                {                                   
                    Character.KillCount++;
                    Target.IsAlive = false;
                }
            }
        }

        private static void AttackCleanUp(Attack action)
        {
            if (!action.CleanupDone)
            {
                action.Character.CummulativeInitiative += action.Character.Initiative;
                action.Character.Weapon.RemainingAttacks--;
                action.Target_EOTValues = new Character(action.Target.HitPoints, action.Target.RemainingHitPoints, action.Target.IsAlive);
                action.CleanupDone = true;
            }
            
        }

        private void CheckForHit(int attackerDex, int defenderDex)
        {
            IsAccurate = false;
            int hitModifier = attackerDex - defenderDex;
            Utils utils = new Utils();
            if (utils.RandomNumber(100) <= 90 + hitModifier)
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

        public void PerformAction()
        {
            if (Target != null)
            {
                CheckForHit(Character.Dexterity, Target.Dexterity);
                if (IsAccurate)
                {
                    CheckForCritialHit(Character.Weapon.CriticalChance);
                    CalculateAttack();
                }
            }
        }

        public void TargetSelection()
        {
            throw new NotImplementedException();
        }

        public void CheckForHit()
        {
            throw new NotImplementedException();
        }

        public void CheckForCriticalHit()
        {
            throw new NotImplementedException();
        }

        public void GetWeaponDamage()
        {
            throw new NotImplementedException();
        }

        public void CalculateAmountDone()
        {
            throw new NotImplementedException();
        }

        public void ActionCleanup()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
