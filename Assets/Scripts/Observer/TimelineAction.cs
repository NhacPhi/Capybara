using System;
using System.Reflection;
using UnityEngine;

namespace Observer
{
    public static class TimelineAction
    {
        public static Action<int, int> OnInitTimeline;
        
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