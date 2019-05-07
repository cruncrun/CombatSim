using System.Collections.Generic;

namespace SWG_sim
{
    public class Turn
    {
        #region Properties
        public int TurnIterator { get; set; }
        public List<Action> ActionList { get; } = new List<Action>();
        public List<Character> AliveParticipants { get; set; }
        #endregion

        #region Constructors
        public Turn(int turnIterator, List<Character> aliveParticipants)
        {
            TurnIterator = turnIterator;
            AliveParticipants = aliveParticipants;
        }
        #endregion
    }
}