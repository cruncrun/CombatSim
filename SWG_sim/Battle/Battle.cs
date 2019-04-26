using System;
using System.Collections.Generic;
using System.Linq;
using Console = Colorful.Console;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;

namespace SWG_sim
{
    class Battle
    {        
        public List<Character> Participants { get; } = new List<Character>();
        public Character Attacker { get; set; }
        public Character Defender { get; set; }
        public bool FightContinues { get; set; }        
        
        public Battle()
        {

        }       

        public void Fight(Battle battle)
        {
            Prepare(battle);
            BattleReport(battle);
            BattleSummary();
        }

        private void BattleReportHeader(List<Character> participants)
        {
            foreach (var character in participants)
            {
                Console.WriteLine(character.Name + " \tI: " + character.Initiative +
                    "\tS: " + character.Strength +
                    " \tObrażenia broni: " + (character.Weapon.BaseAttackPower + character.Weapon.AttackPowerDiceRolls) + "-" +
                    (character.Weapon.BaseAttackPower + (character.Weapon.AttackPowerDiceSides * character.Weapon.AttackPowerDiceRolls)) + " * " + character.Weapon.AttacksPerTurn);                
            }
            System.Console.WriteLine("\r\n");
        }
    
        private void BattleReport(Battle battle)
        {
            //bool FightContinues = true;
            
            BattleReportHeader(Participants);

            for (int turnIterator = 1; turnIterator <= 10 && AreThereAnyParticipantsLeft(Participants); turnIterator++)
            {
                List<Character> aliveParticipants = GetAliveParticipants(Participants);

                Console.WriteLine("Tura " + turnIterator, Color.Green);

                for (int i = 1; AreThereAnyAttacksLeft(aliveParticipants) && AreThereAnyParticipantsLeft(aliveParticipants); i++)
                {                    
                    List<Character> attackingParticipants = InitiativeCheck(aliveParticipants, i);
                    if (attackingParticipants.Count != 0)
                    {
                        foreach (var character in attackingParticipants)
                        {
                            if (character.IsAlive)
                            {
                                Attack attack = new Attack(character, aliveParticipants);
                                attack.PerformAttack(attack);
                                //Thread.Sleep(300);
                            }                             
                        }
                    }
                    else
                    {
                        
                    }
                }
                //TurnSummary(Participants);
                TurnReset(Participants);
                System.Console.WriteLine("\r\n");
            }            
        }

        private List<Character> GetAliveParticipants(List<Character> participants)
        {
            List<Character> aliveParticipants = new List<Character>();
            foreach (var character in participants)
            {
                if (character.IsAlive)
                {
                    aliveParticipants.Add(character);
                }
            }
            return aliveParticipants;
        }

        private void BattleSummary()
        {
            foreach (var character in Participants)
            {
                System.Console.WriteLine(character.Name + " " + character.RemainingHitPoints + "/" + character.HitPoints + ". \tZabitych wrogów: " + character.KillCount + ". \tZadane obrażenia: " + character.DamageDone + ", otrzymane obrażenia: " + character.DamageTaken );
            }
        }
              

        private void TurnReset(List<Character> participants)
        {
            foreach (var character in participants)
            {
                character.Weapon.RemainingAttacks = character.Weapon.AttacksPerTurn;
                character.CummulativeInitiative = character.Initiative;
            }
        }  

        private bool AreThereAnyAttacksLeft(List<Character> participants)
        {
            bool areThereAnyAttacksLeft = false;
            foreach (var character in participants)
            {
                if (character.Weapon.RemainingAttacks != 0 && character.IsAlive)
                {
                    areThereAnyAttacksLeft = true;
                }
            }
            return areThereAnyAttacksLeft;
        }

        private bool AreThereAnyParticipantsLeft(List<Character> participants)
        {
            bool areThereAnyAttackersLeft = false;
            bool areThereAnyDefendersLeft = false;
            
            foreach (var character in participants)
            {
                if (character.IsAlive)
                {
                    if (character.IsAttacker)
                    {
                        areThereAnyAttackersLeft = true;
                    }
                    if (!character.IsAttacker)
                    {
                        areThereAnyDefendersLeft = true;
                    }
                }
            }

            return areThereAnyAttackersLeft && areThereAnyDefendersLeft ? true : false;
        }

        private List<Character> InitiativeCheck(List<Character> participants, int initiative)
        {
            List<Character> attackingParticipants = new List<Character>();

            foreach (var character in participants)
            {
                if ((character.Weapon.RemainingAttacks > 0) && (character.CummulativeInitiative == initiative) && character.IsAlive)
                {
                    attackingParticipants.Add(character);
                }
            }

            return attackingParticipants;
        }

        private void Prepare(Battle battle) // metoda tymczasowo stosowana do debuga
        {
            Utils utils = new Utils();

            int numberOfAttackers = 24;
            int numberOfDefenders = 3;

            for (int i = 0; i < numberOfAttackers; i++)
            {
                Participants.Add(new Character("Atakujący " + i, true));
            }

            /*
            for (int i = 0; i < numberOfDefenders; i++)
            {
                Participants.Add(new Character("Obrońca " + i, false));
            }
            */


            // BOSS

            Character bossAss = new Character("Pomniejszy demon",                     // Name
                                            utils.RandomNumber(1000, 1500),           // HitPoints
                                            1,                                       // ManaPoints
                                            1,                                       // Defence
                                            utils.RandomNumber(50, 100),                                       // Strength
                                            1,                                       // Dexterity
                                            1,                                       // Toughness
                                            utils.RandomNumber(5, 10),                                        // Initiative
                                            new Weapon(utils.RandomNumber(30, 50),                           // BaseAttackPower
                                                       utils.RandomNumber(2, 4),                            // DiceSides
                                                       utils.RandomNumber(40, 70),                            // DiceRolls
                                                       utils.RandomNumber(4, 6),                             // AttacksPerTurn
                                                       utils.RandomNumber(5, 15)),                           // CriticalChance
                                            true,                                     // isAlive
                                            false);                                   // isAttacker
            Participants.Add(bossAss);
            
            Character boss = new Character("Michał, Lord Ciemności",        // Name
                                            utils.RandomNumber(2500, 3500),           // HitPoints
                                            1,                                       // ManaPoints
                                            1,                                       // Defence
                                            utils.RandomNumber(75, 150),                                       // Strength
                                            1,                                       // Dexterity
                                            1,                                       // Toughness
                                            utils.RandomNumber(5, 10),                                        // Initiative
                                            new Weapon(utils.RandomNumber(50, 100),                           // BaseAttackPower
                                                       utils.RandomNumber(1, 4),                            // DiceSides
                                                       utils.RandomNumber(45, 100),                            // DiceRolls
                                                       utils.RandomNumber(4, 6),                             // AttacksPerTurn
                                                       utils.RandomNumber(5, 10)),                           // CriticalChance
                                            true,                  // isAlive
                                            false);                // isAttacker
            Participants.Add(boss);
            

        }
    }
}
