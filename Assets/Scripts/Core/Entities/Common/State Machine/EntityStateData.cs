using System;
using System.Threading;
using Core.Entities.Player;
using Cysharp.Threading.Tasks;
using Tech.Composite;
using Tech.State_Machine;
using UnityEngine;

namespace Core.Entities.Common
{
    public abstract class EntityStateData : CoreComponent
    {
        public StateMachine<EntityState, EntityStateBase> StateManager { get; protected set; }
        public AnimationSystemBase Anim { get; protected set; }
        public Entity Entity { get; protected set; }
        public EntityStats StatsManager { get; protected set; }
        public EntitySkill SkillsManger { get; protected set; }

        [field: SerializeField] public float TimeToReadyMoveAttack { get; protected set; } = 0.3f;
        [field: SerializeField] public float TimeToEndTurn { get; protected set; } = 0.3f;
        [field: SerializeField] public float MoveSpeed { get; protected set; } = 8f;
        [field: SerializeField] public Vector3 OffsetToTarget { get; protected set; } = Vector3.right;

        [NonSerialized] public Entity CurrentTarget;
        [NonSerialized] public Vector3 MovePosition;
        public Vector3 RootPosition { get; protected set; }
        
        public CancellationToken Token => Entity.transform.GetCancellationTokenOnDestroy();

        [field: Header("Animmation Parameters")] 
        [field: SerializeField] public string IdleAnim { get; protected set; } = "Idle";
        [field: SerializeField] public string MoveAnim { get; protected set; } = "Move";
        [field: SerializeField] public string AttackAnim { get; protected set; } = "Attack";
        [field: SerializeField] public string HitAnim { get; protected set; } = "Take Hit";

        public override void LoadComponent()
        {
            Entity = core as Entity;
            StateManager = Entity.StateManager;
            Anim = Entity.GetCoreComponent<AnimationSystemBase>();
            StatsManager = Entity.GetCoreComponent<EntityStats>();
            SkillsManger = Entity.GetCoreComponent<EntitySkill>();
            RootPosition = Entity.transform.position;
        }

        public virtual void HandleTurn()
        {
            _ = WaitToReadyAttack();
        }

        protected virtual async UniTaskVoid WaitToReadyAttack()
        {
            await UniTask.WaitForSeconds(TimeToReadyMoveAttack, cancellationToken: Token);
            MovePosition = CurrentTarget.transform.position + OffsetToTarget;
            StateManager.ChangeState(EntityState.MOVE_UP);
        }

        public void EndTurn()
        {
            Entity.IsEndTurn = true;
        }
    }
}
