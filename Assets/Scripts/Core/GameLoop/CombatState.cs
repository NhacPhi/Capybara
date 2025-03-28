using Observer;
using Tech.State_Machine;
using Unity.VisualScripting;

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
        public static int CurRound { get; private set; }
        public static int maxRound { get; private set; } = 30;
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
            CurRound = 1;
            _nextTurn = Turn.PlayerTurn;
            GameAction.OnRoundChange?.Invoke(CurRound, maxRound);
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
                        CurRound++;
                        GameAction.OnRoundChange?.Invoke(CurRound, maxRound);
                    }
                    GameAction.OnPlayerTurnStart?.Invoke();
                    break;
            }
            
            _currentTurn = _nextTurn;
        }
    }
}