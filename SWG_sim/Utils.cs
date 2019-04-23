using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWG_sim
{
    class Utils
    {
        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();

        public int RandomNumber(int min, int max)
        {
            lock (syncLock)
            { // synchronize
                return random.Next(min, max);
            }
        }

        public int RandomNumber(int max)
        {
            lock (syncLock)
            { // synchronize
                return random.Next(max);
            }
        }
    }
}
