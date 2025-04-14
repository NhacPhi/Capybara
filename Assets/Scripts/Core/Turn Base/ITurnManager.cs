namespace Core.TurnBase
{
    public interface ITurnManager
    {
        public int CurrentTurnIndex { get;}
        public int CurrentRound { get;}

        public void EndCombat(); 
        public void RegisterTurn(ITurn turn);
        public void UnregisterTurn(ITurn turn);
    }
}
