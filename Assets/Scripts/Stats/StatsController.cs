using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Stats.M_Attribute;
using Stats.stat;
using Stats.Status_Effect;
using Tech.Composite;
using Tech.Json;
using UnityEngine;
using VContainer;
using Attribute = Stats.M_Attribute.Attribute;

namespace Stats
{
	public class StatsController : CoreComponent, IEffectable
	{
		protected StatsDataHolder statsHolder;
		[field: SerializeField] public string EntityID { get; protected set;}
		[Inject] protected DataService dataService;
		protected Dictionary<StatType, Stat> stats;
		protected Dictionary<AttributeType, Attribute> attributes;
		protected List<StatusEffect> statusEffects = new ();
		protected const string addressKey = "EntitiesStats";
		
		internal Dictionary<StatType, Stat> Stats => stats;
		internal Dictionary<AttributeType, Attribute> Attributes => attributes;
		
		public ReadOnlyCollection<StatusEffect> StatusEffects => statusEffects.AsReadOnly();

#if UNITY_EDITOR
		/// <summary>
		/// Only Work On Editor
		/// </summary>
		internal Action NotifyEditor;
#endif
		
		public override void LoadComponent()
		{
			base.LoadComponent();
			InitAttribute();
		}
		private void InitAttribute()
		{
			if (attributes != null)
			{
				return;
			}

			attributes = new Dictionary<AttributeType, Attribute>();

			if (statsHolder == null)
			{
				if(!dataService.TryGetData<EntitiesStatsDataBase>(addressKey, out var dictData)) return;
				statsHolder = dictData.EntitiesStats[EntityID];
			}

			InitStats();

			foreach (AttributeType key in statsHolder.AttributeItems.Keys)
			{
				AttributeItem attributeItem = statsHolder.GetAttribute(key);
				
				Attribute attribute = new(attributeItem.MinValue, stats[attributeItem.MaxValue], attributeItem.StartPercent, this);
				
#if UNITY_EDITOR
				attribute.OnValueChange += HandleNotifyEditor;
#endif
				
				attributes.Add(key, attribute);
			}
			
#if UNITY_EDITOR
			NotifyEditor?.Invoke();
#endif
		}
		private void InitStats()
		{
			if (stats != null)
			{
				return;
			}
			
			if (statsHolder == null)
			{
				if(!dataService.TryGetData<EntitiesStatsDataBase>(addressKey, out var dictData)) return;
				statsHolder = dictData.EntitiesStats[EntityID];
			}
			
			stats = new Dictionary<StatType, Stat>();

			foreach (StatType key in statsHolder.StatItems.Keys)
			{
				Stat stat = new(statsHolder.StatItems[key]);
#if UNITY_EDITOR
				stat.OnValueChange += HandleNotifyEditor;
#endif
				stats.Add(key, stat);
			}
			
#if UNITY_EDITOR
			NotifyEditor?.Invoke();
#endif
		}
		
		public virtual void Renew()
		{
			foreach (Stat stat in stats.Values)
			{
				stat.ClearAllModifiers();
			}

			foreach (AttributeType key in attributes.Keys)
			{
				var attributeItem = statsHolder.GetAttribute(key);
				attributes[key].Reset(attributeItem.StartPercent);
			}
			
#if UNITY_EDITOR
			NotifyEditor?.Invoke();
#endif
		}
		
		public virtual void AddModifier(StatType type, Modifier modifier)
		{
			if (!stats.TryGetValue(type, out Stat value)) return;
		
			value.AddModifier(modifier);
		}

		public virtual void RemoveModifier(StatType type, Modifier modifier)
		{
			stats[type].RemoveModifier(modifier);
		}

		public virtual void AddModifierWithoutNotify(StatType type, Modifier modifier)
		{
			if (!stats.TryGetValue(type, out Stat value)) return;
		
			value.AddModifierWithoutNotify(modifier);
			
#if UNITY_EDITOR
			NotifyEditor?.Invoke();
#endif
		}
		
		public virtual void RemoveModifierWithoutNotify(StatType type, Modifier modifier)
		{
			stats[type].RemoveModifierWithoutNotify(modifier);
			
#if UNITY_EDITOR
			NotifyEditor?.Invoke();
#endif
		}

		public void RemoveEffect(StatusEffect effect, bool ignoreStack)
		{
			if (!FindEffect(effect, out var cloneEffect)) return;

			if (ignoreStack)
			{
				cloneEffect.Stop();
				statusEffects.Remove(cloneEffect);
				
#if UNITY_EDITOR
				NotifyEditor?.Invoke();
#endif
				return;
			}
			
#if UNITY_EDITOR
			NotifyEditor?.Invoke();
#endif
			if (effect.IsStackable && cloneEffect.RemoveStack() > 0) return;
			
			cloneEffect.Stop();
			statusEffects.Remove(cloneEffect);
		}

		public bool HasEffect<T>() where T : StatusEffect
		{
			foreach (var effect in statusEffects)
			{
				if (effect.GetType() == typeof(T)) return true;
			}
			
			return false;
		}
		
		public void ApplyEffect(StatusEffect effect)
		{
			if(effect == null || effect.MaxStack <= 0) return;

			bool isExist = FindEffect(effect, out var existEffect);
			
			bool stackIsFull = false;

			if (isExist && effect.IsStackable)
			{
				stackIsFull = existEffect.CurStack >= existEffect.MaxStack;
			}
			
			if (effect.IsUnique && isExist)
			{
				if (existEffect.IsStackable && !stackIsFull)
				{
					existEffect.AddStack();
#if UNITY_EDITOR
					NotifyEditor?.Invoke();
#endif
				}
				return;
			}
			
			if (!isExist)
			{
				var clone = effect.Clone();
				statusEffects.Add(clone);
				clone.StartEffect();
#if UNITY_EDITOR
				NotifyEditor?.Invoke();
#endif
				return;
			}

			if (stackIsFull) return;

			existEffect.AddStack();
#if UNITY_EDITOR
			NotifyEditor?.Invoke();
#endif
		}

		//Use id and type to compare Effect
		protected bool FindEffect(StatusEffect effect, out StatusEffect cloneEffect)
		{
			cloneEffect = null;
			if (effect == null) return false;
			
			foreach (var m_effect in statusEffects)
			{
				if (!effect.Equals(m_effect)) continue;
				
				cloneEffect = m_effect;
				return true;
			}
			
			return false;
		}
		
		protected virtual void Update()
		{
			for (int i = statusEffects.Count - 1; i >= 0; i--)
			{
				var effect = statusEffects[i];
				effect.Update();
				if(!effect.IsStop) continue;
				statusEffects.RemoveAt(i);
#if UNITY_EDITOR
				NotifyEditor?.Invoke();
#endif
			}
		}
		
		public Attribute GetAttribute(AttributeType type)
		{
			InitAttribute();
			return attributes.GetValueOrDefault(type);
		}

		public Stat GetStat(StatType type)
		{
			InitStats();
			return stats.GetValueOrDefault(type);
		}
		
#if UNITY_EDITOR
		private void HandleNotifyEditor(object value)
		{
			NotifyEditor?.Invoke();
		}
#endif
	}
}
