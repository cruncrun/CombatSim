using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWG_sim
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Character character = new Character();
                Battle battle = new Battle(character.GetCharacters(5));
                battle.GenerateBattleReport();

                ConsoleWriter cw = new ConsoleWriter();
                cw.GenerateBattleReport(battle);

                Console.ReadKey();
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine("Wyst¹pi³ b³¹d ArgumentNullException w metodzie Main:" + ex);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Wyst¹pi³ b³¹d w metodzie Main:" + ex);
                throw;
            }            
        }
    }
}
