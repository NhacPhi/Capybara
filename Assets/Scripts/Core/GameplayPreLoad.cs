using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Stats;
using System.Threading;
using Core;
using Core.Entities;
using Core.Skill;
using Tech.Json;
using Tech.Pooling;
using VContainer;
using VContainer.Unity;

public class GameplayPreLoad : IAsyncStartable
{
    [Inject] private DataService _dataService;
    [Inject] private EventManager _eventManager;
    [Inject] private SkillDatabase _skillDatabase;
    [Inject] private EnemyManager _enemyManager;
    [Inject] private IObjectResolver _objectResolver;
    
    public bool IsLoadDone;

    public async UniTask StartAsync(CancellationToken cancellation = default)
    {
        IsLoadDone = false;

        await UniTask.WaitUntil(() => AddressablesManager.Instance && GameManager.Instance
            && PoolManager.Instance, cancellationToken: cancellation);

        var tasks = new List<UniTask>()
        {
            _dataService.LoadDataAsync<EntitiesStatsDataBase>(AddressConstant.EntitiesStats, cancellation),
            _skillDatabase.Init(cancellation),
            _enemyManager.Init(cancellation),
        };
        
        _eventManager.Init(tasks, cancellation);
        
        await UniTask.WhenAll(tasks);
        
        _objectResolver.Inject(PoolManager.Instance);
        _eventManager.OnAfterLoadDone();
        IsLoadDone = true;
    }
}
