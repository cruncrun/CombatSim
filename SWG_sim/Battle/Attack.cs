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
        public bool isAccurate { get; set; }
        public bool isCritical { get; set; }

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
                if (isAccurate)
                {
                    CheckForCritialHit(attack.Character.Weapon.CriticalChance);
                    CalculateAttack(attack);
                }
                else
                {
                    PrintMissedAttack(attack);
                }
                AttackCleanUp(attack);
            }  
        }

        private void PrintMissedAttack(Attack attack)
        {
            Console.WriteLine(attack.Character.Name + " próbuje zaatakować " + attack.Opponent.Name + ", ale nie trafia...", Color.WhiteSmoke);
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
            if (attack.isCritical)
            {
                Console.WriteLine(attack.Character.Name + " trafia krytycznie zadając " + attack.Damage + " punktów obrażeń! " +
                       attack.Opponent.Name + " ma " + attack.Opponent.RemainingHitPoints + "/" + attack.Opponent.HitPoints + " punktów życia.", Color.DeepPink);
            }
            else
            {
                Console.WriteLine(attack.Character.Name + " zadaje " + attack.Damage + " punktów obrażeń. " +
                       attack.Opponent.Name + " ma " + attack.Opponent.RemainingHitPoints + "/" + attack.Opponent.HitPoints + " punktów życia.");
            }
            if (attack.Opponent.RemainingHitPoints <= 0)
            {                
                Console.WriteLine(attack.Opponent.Name + " umiera na śmierć.", Color.Red);
                attack.Character.KillCount++;
                attack.Opponent.IsAlive = false;
            }
        }

        private void CalculateDamageDone(Attack attack)
        {
            Utils utils = new Utils();
            attack.Damage = (attack.Character.Strength / 2) + GetWeaponDamage(attack.Character.Weapon);
            
            if (attack.isCritical)
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
            isAccurate = false;
            Utils utils = new Utils();
            if (utils.RandomNumber(100) <= 95)
            {
                isAccurate = true;
            }
        }

        private void CheckForCritialHit(int weaponCriticalChance)
        {
            isCritical = false;
            Utils utils = new Utils();
            if (utils.RandomNumber(100) <= weaponCriticalChance)
            {
                isCritical = true;
            }
        }
    }
}
