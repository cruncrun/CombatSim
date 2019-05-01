using System;
using System.Collections.Generic;
using System.Drawing;
using Console = Colorful.Console;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Colorful;

namespace SWG_sim
{
    static class ConsoleWriter
    {
        static public void DeathMessage(string name)
        {
            string message = name + " umiera na śmierć.";
            Console.WriteLine(message, Color.Red);
        }

        static public void TurnNumberMessage(int turnIterator)
        {
            Console.WriteLine("Tura " + turnIterator, Color.LawnGreen);
        }

        static public void ParticipantDetails(Character character)
        {
            string baseText = "{0} \tI: {1}\tS: {2}\tObrażenia broni: {3}-{4} * {5}";
            Formatter[] elements = new Formatter[]
            {
                new Formatter(character.Name, GetCharacterNameColor(character)),
                new Formatter(character.Initiative, Color.White),
                new Formatter(character.Strength, Color.White),
                new Formatter(character.Weapon.MinimumDamage, Color.White),
                new Formatter(character.Weapon.MaximumDamage, Color.White),
                new Formatter(character.Weapon.AttacksPerTurn, Color.White),
            };
            Console.WriteLineFormatted(baseText, Color.LightGray, elements);
        }

        static public void RegularHitMesage(Attack attack)
        {
            string baseText = "{0} zadaje atakiem {1} punktów obrażeń. {2} ma {3}/{4} punktów życia.";
            Formatter[] elements = new Formatter[]
            {
                new Formatter(attack.Character.Name, GetCharacterNameColor(attack.Character)),
                new Formatter(attack.Damage, Color.White),
                new Formatter(attack.Opponent.Name, GetCharacterNameColor(attack.Opponent)),
                new Formatter(attack.Opponent.RemainingHitPoints, Color.White),
                new Formatter(attack.Opponent.HitPoints, Color.White)
            };
            Console.WriteLineFormatted(baseText, Color.LightGray, elements);            
        }

        static public void CriticalHitMessage(Attack attack)
        {
            string baseText = "{0} trafia krytycznie zadając {1} punktów obrażeń. {2} ma {3}/{4} punktów życia.";
            Formatter[] elements = new Formatter[]
            {
                new Formatter(attack.Character.Name, GetCharacterNameColor(attack.Character)),
                new Formatter(attack.Damage, Color.DeepPink),
                new Formatter(attack.Opponent.Name, GetCharacterNameColor(attack.Opponent)),
                new Formatter(attack.Opponent.RemainingHitPoints, Color.White),
                new Formatter(attack.Opponent.HitPoints, Color.White)
            };
            Console.WriteLineFormatted(baseText, Color.HotPink, elements);
        }

        static public void MissedAttackMessage(Attack attack)
        {
            string baseText = "{0} próbuje zaatakować {1}, ale nie trafia...";
            Formatter[] elements = new Formatter[]
            {
                new Formatter(attack.Character.Name, GetCharacterNameColor(attack.Character)),
                new Formatter(attack.Opponent.Name, GetCharacterNameColor(attack.Opponent))
            };
            Console.WriteLineFormatted(baseText, Color.DimGray, elements);            
        }

        static public void BattleSummary(List<Character> participants)
        {
            foreach (var character in participants)
            {
                string baseText = "{0} \t{1}/{2}\tZabitych wrogów: {3}. \tZadane obrażenia: {4}, otrzymane obrażenia: {5}";
                Formatter[] elements = new Formatter[]
                {
                    new Formatter(character.Name, GetCharacterNameColor(character)),
                    new Formatter(character.RemainingHitPoints, Color.LightGray),
                    new Formatter(character.HitPoints, Color.LightGray),
                    new Formatter(character.KillCount, Color.LightGray),
                    new Formatter(character.DamageDone, Color.LightGray),
                    new Formatter(character.DamageTaken, Color.LightGray)
                };
                Console.WriteLineFormatted(baseText, Color.LightGray, elements);
            }
        }

        static public Color GetCharacterNameColor(Character character)
        {
            return character.IsAttacker ? Color.Tomato : Color.SteelBlue;
        }
    }
}
