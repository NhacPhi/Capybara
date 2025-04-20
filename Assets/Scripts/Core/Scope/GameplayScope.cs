using Core.Entities;
using Core.Entities.Player;
using Core.GameLoop;
using Core.Skill;
using Core.TurnBase;
using Cysharp.Threading.Tasks;
using Stats;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Core.Scope
{
    public class GameplayScope : LifetimeScope
    {
        [SerializeField] private GameObject PlayerPrefab;
        
        public PlayerCtrl PlayerCtrl { get; private set; }
        
        protected override void Configure(IContainerBuilder builder)
        {
           // builder.Register<DataService>(Lifetime.Scoped);
            builder.Register<EventManager>(Lifetime.Scoped);
            builder.Register<SkillDatabase>(Lifetime.Scoped);
            builder.Register<EntitiesStatsDataBase>(Lifetime.Scoped);
            
            //Hierarchy
            builder.RegisterComponentInHierarchy<UIManager>().AsSelf();
                
            //Entry Point
            builder.RegisterEntryPoint<GameplayPreLoad>(Lifetime.Scoped).As<IPreload>();
            builder.RegisterEntryPoint<CombatText>(Lifetime.Scoped);
            builder.RegisterEntryPoint<Gameloop>(Lifetime.Scoped).AsSelf();
            builder.RegisterEntryPoint<EnemyManager>(Lifetime.Scoped).AsSelf();
            builder.RegisterEntryPoint<TurnManager>(Lifetime.Scoped).AsSelf();
            
            builder.RegisterBuildCallback(container =>
            {
                _ = Loading(container);
            });
        }

        private async UniTaskVoid Loading(IObjectResolver objectResolver)
        {
            var loadProgress = objectResolver.Resolve<IPreload>();
            
            while (!loadProgress.IsLoadDone())
            {
                await UniTask.Yield();
            }

            PlayerCtrl = objectResolver.Instantiate(PlayerPrefab).GetComponent<PlayerCtrl>();
            objectResolver.Resolve<TurnManager>().Player = PlayerCtrl;
        }
    }
}
