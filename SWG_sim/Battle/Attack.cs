using System;
using System.Collections.Generic;
using System.Drawing;
using Console = Colorful.Console;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWG_sim
{
    class Attack
    {
        public Character Character { get; set; }
        public Character Opponent { get; set; }
        public List<Character> AliveParticipants { get; set; }
        public int Damage { get; set; }
        public bool IsAccurate { get; set; }
        public bool IsCritical { get; set; }

        public Attack(Character character, List<Character> aliveParticipants)
        {
            Character = character;
            AliveParticipants = aliveParticipants;
        }

        public void PerformAttack(Attack attack)
        {   
            attack.Opponent = SelectTarget(attack); // prowizorka 
            if (attack.Opponent != null && attack.Opponent.Name != "Nie ma")
            {
                CheckForHit();
                if (IsAccurate)
                {
                    CheckForCritialHit(attack.Character.Weapon.CriticalChance);
                    CalculateAttack(attack);
                }
                else
                {
                    ConsoleWriter.MissedAttackMessage(attack);
                }
                AttackCleanUp(attack);
            }  
        }        

        public Character SelectTarget(Attack attack)
        {
            Utils utils = new Utils();
            List<Character> opponentsList = GetOppisteSide(attack.Character.IsAttacker, attack.AliveParticipants);
            if (opponentsList.Any())
            {
                Character opponent = opponentsList[utils.RandomNumber(opponentsList.Count)];
                return opponent;
            }
            //else
            //{
            //    return new Character("Nie ma");
            //}
            return new Character("Nie ma");
        }

        private void PrintAttack(Attack attack)
        {
            if (attack.IsCritical)
            {
                ConsoleWriter.CriticalHitMessage(attack);                
            }
            else
            {
                ConsoleWriter.RegularHitMesage(attack);                
            }
            if (attack.Opponent.RemainingHitPoints <= 0)
            {
                ConsoleWriter.DeathMessage(attack.Opponent.Name);                
                attack.Character.KillCount++;
                attack.Opponent.IsAlive = false;
            }
        }

        private void CalculateDamageDone(Attack attack)
        {            
            attack.Damage = (attack.Character.Strength / 2) + GetWeaponDamage(attack.Character.Weapon);
            
            if (attack.IsCritical)
            {
                attack.Damage *= 2;
            }
            attack.Character.DamageDone += attack.Damage;
            attack.Opponent.RemainingHitPoints -= attack.Damage;
            attack.Opponent.DamageTaken += attack.Damage;
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

        private void CalculateAttack(Attack attack)
        {
            if (attack.Character.IsAlive && attack.Opponent.IsAlive)
            {
                CalculateDamageDone(attack);
                PrintAttack(attack);
            }            
        }

        private static void AttackCleanUp(Attack attack)
        {
            attack.Character.CummulativeInitiative += attack.Character.Initiative;
            attack.Character.Weapon.RemainingAttacks--;
        }

        private void CheckForHit()
        {
            IsAccurate = false;
            Utils utils = new Utils();
            if (utils.RandomNumber(100) <= 95)
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
    }
}
