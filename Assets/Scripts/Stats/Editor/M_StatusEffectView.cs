#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using Stats.Status_Effect;
using UnityEngine;
using UnityEngine.UIElements;

namespace Stats.Editor
{
	
	public class M_StatusEffectView : M_ItemView
	{
		private const string _statusEffect = "Status Effect";
		private readonly Color backgroundColor = new Color(0.345098f, 0.345098f, 0.345098f);
		private List<StatusEffect> m_statusEffects = new();
		
		public M_StatusEffectView(StatsController stats) : base(stats)
		{
			statsController.NotifyEditor -= Rebuild;
			statsController.NotifyEditor += Rebuild;
		}

		~M_StatusEffectView()
		{
			if (statsController == null) return;
			statsController.NotifyEditor -= Rebuild;
		}
		
		protected override void SetTitle()
		{
			title = _statusEffect;
		}

		protected override void Rebuild()
		{
			if (statsController == null) return;

			Body.Clear();

			if (statsController.StatusEffects.Count == 0)
			{
				var root = new VisualElement
				{
					style =
					{
						flexDirection = FlexDirection.Row,
						justifyContent = Justify.SpaceBetween,
						alignItems = Align.Center,
						backgroundColor = backgroundColor,
						marginBottom = 3
					}
				};
				SetBorderColor(root, Color.black, 1);
				
				Label label = new Label($"None")
				{
					style =
					{
						paddingLeft = 8,
						fontSize = 12,
						unityFontStyleAndWeight = FontStyle.Bold
					}
				};
				root.Add(label);
				Body.Add(root);
				return;
			}
			
			foreach (var m_effect in statsController.StatusEffects)
			{
				m_statusEffects.Add(m_effect);
				
				var root = new VisualElement
				{
					style =
					{
						flexDirection = FlexDirection.Row,
						justifyContent = Justify.SpaceBetween,
						alignItems = Align.Center,
						backgroundColor = backgroundColor,
						marginBottom = 3
					}
				};

				SetBorderColor(root, Color.black, 1);
		
				var tmp = m_effect.Data.Name == String.Empty ? "No Name ???" : m_effect.Data.Name;
				
				Label label = new Label($"{tmp}")
				{
					style =
					{
						paddingLeft = 8,
						fontSize = 12,
						unityFontStyleAndWeight = FontStyle.Bold
					}
				};
				string stack = m_effect.Data.IsStackable ? $"Stack : {m_effect.CurStack}" : string.Empty; 
				Label label2 = new Label($"{stack} Duration : {m_effect.Data.Duration}s")
				{
					style =
					{
						paddingRight = 8,
						fontSize = 12,
						unityFontStyleAndWeight = FontStyle.Bold
					}
				};
				
				root.Add(label);
				root.Add(label2);
				Body.Add(root);
			}
		}

		private void SetBorderColor(VisualElement root, Color color, float width)
		{
			root.style.borderTopColor = color;
			root.style.borderTopWidth = width;
			root.style.borderBottomColor = color;
			root.style.borderBottomWidth = width;
			root.style.borderLeftColor = color;
			root.style.borderLeftWidth = width;
			root.style.borderRightColor = color;
			root.style.borderRightWidth = width;
		}
	}
}
#endif