using System;
using Stats.stat;
using UnityEngine;

namespace Stats.M_Attribute
{
	[Serializable]
	public class Attribute
	{
		private float _value;
		private float _minValue;
		private Stat _maxValue;
		public float MinValue => _minValue;
		public float MaxValue => _maxValue?.Value ?? 0f;
		public float Value
		{
			get
			{
				_value = Math.Clamp(_value, _minValue, MaxValue);

				return _value;
			}
			set
			{
				var newValue = Math.Clamp(value, _minValue, MaxValue);
				
				if (_value.Equals(newValue)) return;
				
				_value = newValue;
				OnValueChange?.Invoke(this);
			}
		}

		public Action<Attribute> OnValueChange;

		public Attribute(float minValue, Stat maxValue, float startPercent, StatsController controller)
		{
			_minValue = minValue;
			_maxValue = maxValue;

			_value = Mathf.Lerp(minValue, MaxValue, startPercent);
		}

		public void Reset(float startPercent)
		{
			_value = Mathf.Lerp(_minValue, _maxValue.Value, startPercent);
			OnValueChange?.Invoke(this);
		}
		
		public void SetValueWithoutNotify(float value)
		{
			_value = Mathf.Clamp(value, _minValue, MaxValue);
		}
	}
}
