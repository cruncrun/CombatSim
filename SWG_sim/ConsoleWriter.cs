using System;
using System.Collections.Generic;
using System.Drawing;
using Console = Colorful.Console;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Console.WriteLine(character.Name +
                " \tI: " + character.Initiative +
                "\tS: " + character.Strength +
                " \tObrażenia broni: " + (character.Weapon.BaseAttackPower + character.Weapon.AttackPowerDiceRolls) + "-" +
                    (character.Weapon.BaseAttackPower + (character.Weapon.AttackPowerDiceSides * character.Weapon.AttackPowerDiceRolls)) + " * " + character.Weapon.AttacksPerTurn);
        }

        static public void RegularHitMesage(Attack attack)
        {
            Console.WriteLine(attack.Character.Name + " zadaje " + attack.Damage + " punktów obrażeń. " +
                       attack.Opponent.Name + " ma " + attack.Opponent.RemainingHitPoints + "/" + attack.Opponent.HitPoints + " punktów życia.");
        }

        static public void CriticalHitMessage(Attack attack)
        {
            Console.WriteLine(attack.Character.Name + " trafia krytycznie zadając " + attack.Damage + " punktów obrażeń! " +
                       attack.Opponent.Name + " ma " + attack.Opponent.RemainingHitPoints + "/" + attack.Opponent.HitPoints + " punktów życia.", Color.DeepPink);
        }

        static public void MissedAttackMessage(Attack attack)
        {
            Console.WriteLine(attack.Character.Name + " próbuje zaatakować " + attack.Opponent.Name + ", ale nie trafia...", Color.WhiteSmoke);
        }

        static public void BattleSummary(List<Character> participants)
        {
            foreach (var character in participants)
            {
                System.Console.WriteLine(character.Name + " " + character.RemainingHitPoints + "/" + character.HitPoints + ". \tZabitych wrogów: " + character.KillCount + ". \tZadane obrażenia: " + character.DamageDone + ", otrzymane obrażenia: " + character.DamageTaken);
            }
        }
    }
}
