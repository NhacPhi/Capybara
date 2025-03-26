using System.Threading;
using Cysharp.Threading.Tasks;
using Tech.Singleton;

namespace Core
{
    public class GameManager : SingletonPersistent<GameManager>
    {
        public static CancellationToken GlobalTokenOnDestroy => 
            Instance.GetCancellationTokenOnDestroy();
        protected override void Awake()
        {
            base.Awake();
        }
    }
}