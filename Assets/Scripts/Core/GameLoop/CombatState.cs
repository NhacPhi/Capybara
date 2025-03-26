using Observer;
using Tech.State_Machine;

namespace Core.GameLoop
{
    public enum Turn
    {
        None,
        EnemyTurn,
        PlayerTurn,
    }
    
    public class CombatState : GameStateBase
    {
        private int _curRound;
        private int maxRound = 30;
        private bool _isPlayerTurn;
        private Turn _currentTurn;
        private Turn _nextTurn;
        
        public CombatState(StateMachine<GameState, GameStateBase> stateMachine) : base(stateMachine)
        {
            _currentTurn = Turn.None;
            GameAction.OnEndPlayerTurn += HandleEndPlayerTurn;
            GameAction.OnEndEnemyTurn += HandleEndEnemyTurn;
            GameAction.OnCombatEnd += HandleCombatEnd;
        }

        ~CombatState()
        {
            GameAction.OnEndPlayerTurn -= HandleEndPlayerTurn;
            GameAction.OnEndEnemyTurn -= HandleEndEnemyTurn;
            GameAction.OnCombatEnd -= HandleCombatEnd;
        }

        private void HandleCombatEnd()
        {
            this.stateMachine.ChangeState(GameState.WaitCombat);
        }
        
        private void HandleEndEnemyTurn()
        {
            _nextTurn = Turn.PlayerTurn;
        }
        
        private void HandleEndPlayerTurn()
        {
            _nextTurn = Turn.EnemyTurn;
        }

        public override void Enter()
        {
            base.Enter();
            _curRound = 1;
            _nextTurn = Turn.PlayerTurn;
            GameAction.OnRoundChange?.Invoke(_curRound, maxRound);
        }

        public override void Exit()
        {
            _nextTurn = _currentTurn = Turn.None;
        }

        public override void OnUpdate()
        {
            if(_currentTurn == _nextTurn) return;

            switch (_nextTurn)
            {
                case Turn.None:
                    return;
                case Turn.EnemyTurn:
                    GameAction.OnEnemyTurnStart?.Invoke();
                    break;
                case Turn.PlayerTurn:
                    if (_currentTurn == Turn.EnemyTurn)
                    {
                        _curRound++;
                        GameAction.OnRoundChange?.Invoke(_curRound, maxRound);
                    }
                    GameAction.OnPlayerTurnStart?.Invoke();
                    break;
            }
            
            _currentTurn = _nextTurn;
        }
    }
}