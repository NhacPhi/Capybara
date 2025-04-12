using Core.Entities;
using Core.Entities.Player;
using Core.GameLoop;
using Core.Skill;
using Cysharp.Threading.Tasks;
using Stats;
using Tech.Json;
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
            builder.RegisterEntryPoint<TextCombat>(Lifetime.Scoped).AsSelf();
            builder.RegisterEntryPoint<Gameloop>(Lifetime.Scoped).AsSelf();
            builder.RegisterEntryPoint<EnemyManager>(Lifetime.Scoped).AsSelf();
            
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

            var enemyManager = objectResolver.Resolve<EnemyManager>();
            PlayerCtrl = objectResolver.Instantiate(PlayerPrefab).GetComponent<PlayerCtrl>();
            enemyManager.Player = PlayerCtrl;
        }
    }
}
