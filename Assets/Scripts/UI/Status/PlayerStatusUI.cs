using System;
using System.Reflection;
using Observer;
using Stats.M_Attribute;
using Stats.stat;

namespace UI
{
    using UnityEngine;
    public class PlayerStatusUI : MonoBehaviour
    {
        public BaseAttributeUI[] attributes { get; private set; }
        public BaseStatUI[] stats { get; private set; }

        private void Awake()
        {
            attributes = GetComponentsInChildren<BaseAttributeUI>();
            stats = GetComponentsInChildren<BaseStatUI>();
            InitEvent();
        }

        private void OnDestroy()
        {
            DisposeEvent();
        }

        private void InitEvent()
        {
            Type type = typeof(PlayerStatusAction);
            FieldInfo[] fields = type.GetFields(BindingFlags.Static | BindingFlags.Public);

            foreach (var field in fields)
            {
                Type fieldType = field.FieldType;

                if (fieldType == typeof(Action<AttributeEvtArgs>))
                {
                    var evt = (Action<AttributeEvtArgs>)field.GetValue(null);
                    evt += InitAttribute;
                    field.SetValue(null, evt);
                }
                else if (fieldType == typeof(Action<StatEvtArgs>))
                {
                    var evt = (Action<StatEvtArgs>)field.GetValue(null);
                    evt += InitStat;
                    field.SetValue(null, evt);
                }
            }
        }

        private void DisposeEvent()
        {
            Type type = typeof(PlayerStatusAction);
            FieldInfo[] fields = type.GetFields(BindingFlags.Static | BindingFlags.Public);

            foreach (var field in fields)
            {
                switch (field.FieldType.IsGenericType)
                {
                    case true when field.FieldType.GetGenericTypeDefinition() == typeof(Action<AttributeEvtArgs>):
                    {
                        var evt = (Action<AttributeEvtArgs>)field.GetValue(null);
                        evt -= InitAttribute;
                        field.SetValue(null, evt); 
                        continue;
                    }
                    case true when field.FieldType.GetGenericTypeDefinition() == typeof(Action<StatEvtArgs>):
                    {
                        var evt = (Action<StatEvtArgs>)field.GetValue(null);
                        evt -= InitStat;
                        field.SetValue(null, evt); 
                        continue;
                    }
                }
            }
        }
        
        private void InitAttribute(AttributeEvtArgs args)
        {
            var baseAttributeUI = SearchFirstAttribute(args.Attribute);
            
            if(!baseAttributeUI) return;
            
            baseAttributeUI.Init(args.Value, args.MaxValue);
        }

        private void InitStat(StatEvtArgs args)
        {
            var baseStatUI = SearchFirstStat(args.Stat);
            
            if(!baseStatUI) return;
            
            baseStatUI.Init(args.Value);
        }

        private BaseStatUI SearchFirstStat(StatType statType)
        {
            foreach (var stat in stats)
            {
                if(stat.StatID != statType) continue;
                
                return stat;
            }
            
            return null;
        }
        
        private BaseAttributeUI SearchFirstAttribute(AttributeType attributeType)
        {
            foreach (var attribute in attributes)
            {
                if(attribute.AttributeID != attributeType) continue;
                
                return attribute;
            }
            
            return null;
        }
    }
}