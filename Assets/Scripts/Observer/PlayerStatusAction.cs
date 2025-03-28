using System;
using System.Reflection;
using Stats.M_Attribute;
using Stats.stat;
using UnityEngine;
using Attribute = Stats.M_Attribute.Attribute;

namespace Observer
{
    public static class PlayerStatusAction
    {
        //Other
        public static Action<float> OnMoneyChange;
        
        //Attribute
        public static Action<Attribute> OnLvChange;
        public static Action<Attribute> OnHpChange;
        public static Action<Attribute> OnExpChange;
        
        //Stat
        public static Action<Stat> OnMaxHpChange;
        public static Action<Stat> OnAtkChange;
        public static Action<Stat> OnDefChange;
        
        //Init
        public static Action<AttributeEvtArgs> OnInitHp;
        public static Action<AttributeEvtArgs> OnInitExp;
        public static Action<AttributeEvtArgs> OnInitLv;
        public static Action<StatEvtArgs> OnInitAtk;
        public static Action<StatEvtArgs> OnInitDef;

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

    public struct AttributeEvtArgs
    {
        public AttributeType Attribute;
        public float Value;
        public float MaxValue;
    }

    public struct StatEvtArgs
    {
        public StatType Stat;
        public float Value;
    }
}