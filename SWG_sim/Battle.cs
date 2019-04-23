using System;
using System.Collections.Generic;
using System.Linq;
using Console = Colorful.Console;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

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
                Console.WriteLine(character.Name + " \tInicjatywa: " + character.Initiative + "\tSiła: " + character.Strength + " \tMoc broni: " + character.Weapon.AttackPower + " \tIlość ataków: " + character.Weapon.AttacksPerTurn);                
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

        private void Prepare(Battle battle)
        {
            int numberOfAttackers = 24;
            int numberOfDefenders = 4;

            for (int i = 0; i < numberOfAttackers; i++)
            {
                Participants.Add(new Character("Atakujący " + i, true));
            }


            //for (int i = 0; i < numberOfDefenders; i++)
            //{
            //    Participants.Add(new Character("Obrońca " + i, false));
            //}



            // BOSS
            
            Character pomiot = new Character("Pomiot Szatana",
                                             1250,
                                             50,
                                             50,
                                             30,
                                             50,
                                             50,
                                             9,
                                             new Weapon(25, 6, 8),
                                             true,
                                             false);
            Participants.Add(pomiot);
            
            Character boss2 = new Character("Szatan diaboł",
                                            3000,
                                            50,
                                            50,
                                            50,
                                            50,
                                            50,
                                            7,
                                            new Weapon(100, 4, 10),
                                            true,
                                            false);
            Participants.Add(boss2);
            

        }
    }
}
