using Core.Entities.Player;
using Core.TurnBase;
using Tech.State_Machine;
using VContainer;

namespace Core.Entities.Common
{
    public abstract class Entity : Tech.Composite.Core, ITurn
    {
        public StateMachine<EntityState, EntityStateBase> StateManager { get; protected set; }
        [Inject] protected IObjectResolver objectResolver;
        public bool IsEndTurn { get; set; }
        protected EntityStateData entityStateData;
        
        protected override void LoadComponent()
        {
            InitStateMachine();
        }

        protected virtual void InitStateMachine()
        {
            entityStateData = GetCoreComponent<EntityStateData>();
            StateManager = new StateMachine<EntityState, EntityStateBase>();
            AddStateToStateMachine(entityStateData);
            objectResolver.Inject(entityStateData);
        }

        protected virtual void AddStateToStateMachine(EntityStateData entityStateData)
        {
            StateManager.AddNewState(EntityState.IDLE, new EntityIdle(entityStateData));
            StateManager.AddNewState(EntityState.MOVE_UP, new EntityMoveUp(entityStateData));
            StateManager.AddNewState(EntityState.MOVE_DOWN, new EntityMoveDown(entityStateData));
            StateManager.AddNewState(EntityState.ATTACK, new EntityAttack(entityStateData));
            StateManager.AddNewState(EntityState.HIT, new EntityTakeHit(entityStateData));
        }
        
        protected virtual void Start()
        {
            StateManager.Initialize(EntityState.IDLE);
        }

        protected virtual void Update()
        {
            StateManager.CurrentState.LogicUpdate();
        }

        public virtual void HandleTurn(Entity target)
        {
            IsEndTurn = false;
            entityStateData.CurrentTarget = target;
            entityStateData.HandleTurn();
        }
    }
}
