using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Core.Entities.Common;
using Core.Entities.Enemy;
using Cysharp.Threading.Tasks;
using Observer;
using UnityEngine;
using VContainer.Unity;

namespace Core.TurnBase
{
    public class TurnManager : IInitializable, IDisposable
    {
        public Entity Player;
        private List<Entity> _enemies = new List<Entity>();
        public int CurEnemiesTurnIndex { get; private set; }
        
        public int CurrentRound { get; private set; }
        public const int MaxRound = 30;
        public bool IsEnemeyTurn { get; private set; }
        private CancellationToken _cancellationToken;
        private CancellationTokenSource _cancellationTokenSource;
        private const float _waitTimeToNextTurn = 0.25f;
        private const float _waitTimeToCombatEnd = 1.5f;
        private bool _isEnd;
        
        public void Initialize()
        {
            _isEnd = true;
            GameAction.OnStartCombat += HandleStartCombat;
            GameAction.OnEnemyDead += HandleEnemyDead;
        }

        public void Dispose()
        {
            DisposeSource();
            GameAction.OnEnemyDead -= HandleEnemyDead;
            GameAction.OnStartCombat -= HandleStartCombat;
        }
        
        private void HandleEnemyDead(EnemyCtrl enemy)
        {
            if(!_enemies.Remove(enemy)) return;
            
            CurEnemiesTurnIndex--;
        }
        
        private void HandleStartCombat()
        {
            if(!_isEnd) return;
            
            if (_cancellationTokenSource == null)
            {
                _cancellationTokenSource = new CancellationTokenSource();    
                _cancellationToken = _cancellationTokenSource.Token;
            }
            
            CurEnemiesTurnIndex = 0;
            _isEnd = false;
            CurrentRound = 1;
            GameAction.OnRoundStart?.Invoke(CurrentRound, MaxRound);
            _ = Loop();
        }

        private async UniTaskVoid Loop()
        {
            while (_enemies.Count == 0)
            {
                await UniTask.Yield(_cancellationToken);
            }
            
            Player.HandleTurn(_enemies.FirstOrDefault());
            IsEnemeyTurn = false;
            
            var currentTurn = GetCurrentTurn();
            
            while (CurrentRound < MaxRound)
            {
                if (_enemies.Count == 0)
                {
                    await UniTask.WaitForSeconds(_waitTimeToCombatEnd, cancellationToken: _cancellationToken);
                    EndCombat();
                }
                
                if (!currentTurn.IsEndTurn)
                {
                    await UniTask.Yield(_cancellationToken);
                    continue;
                }

                await UniTask.WaitForSeconds(_waitTimeToNextTurn, cancellationToken: _cancellationToken);
                
                EndTurn();
                currentTurn = GetCurrentTurn();
            }
        }

        private void EndCombat()
        {
            _isEnd = true;
            GameAction.OnCombatEnd?.Invoke();
            DisposeSource();
        }

        private void DisposeSource()
        {
            if(_cancellationTokenSource == null) return;
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;
        }
        
        public void RegisterEnemy(Entity turn)
        {
            _enemies.Add(turn);
        }

        private void EndTurn()
        {
            
            if(IsEnemeyTurn)
            {
                CurEnemiesTurnIndex++;
                
                if (CurEnemiesTurnIndex >= _enemies.Count)
                {
                    CurEnemiesTurnIndex = 0;
                    CurrentRound++;
                    GameAction.OnRoundChange?.Invoke(CurrentRound);
                }
                
                GameAction.OnEndEnemyTurn?.Invoke();
                Player.HandleTurn(_enemies.FirstOrDefault());
                IsEnemeyTurn = false;
                return;
            }
            
            IsEnemeyTurn = true;
            GameAction.OnEndPlayerTurn?.Invoke();
            _enemies[CurEnemiesTurnIndex].HandleTurn(Player);
        }
        
        private ITurn GetCurrentTurn()
        {
            return IsEnemeyTurn ? _enemies[CurEnemiesTurnIndex] : Player;
        }
    }
}
