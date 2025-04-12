using System;
using System.Reflection;
using Core.Entities.Enemy;
using UnityEngine;

namespace Observer
{
    public static class GameAction
    {
        public static Action OnNextDay;
        public static Action OnSelectionEvent;
        public static Action<Event_System.EventBase> OnEvent;
        public static Action OnSelectionEventDone;
        public static Action OnPlayerSelect;
        public static Action OnStartCombat;
        public static Action OnCombatEnd;
        public static Action<int, int> OnRoundChange;
        public static Action<EnemyCtrl> OnEnemyDead;
        public static Action OnPlayerTurnStart;
        public static Action OnEndPlayerTurn;
        public static Action OnEnemyTurnStart;
        public static Action OnEndEnemyTurn;
        
#if UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void ResetEvents()
        {
            //C# Reflection 
            FieldInfo[] fields = typeof(PlayerStatusAction).GetFields(BindingFlags.Public | BindingFlags.Static);

            foreach (var field in fields)
            {
                if (typeof(Delegate).IsAssignableFrom(field.FieldType))
                {
                    field.SetValue(null, null);
                }
            }
        }
#endif
    }
}