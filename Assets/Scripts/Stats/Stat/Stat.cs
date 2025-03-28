using System;
using System.Collections.Generic;

namespace Stats.stat
{
	[Serializable]
    public class Stat
    {
    	private float _baseValue;
    
    	public float BaseValue => _baseValue;
    
    	private bool _isDirty = true;
		
    	private float _value;
    	public virtual float Value
    	{
    		get
    		{
    			if (_isDirty)
    			{
    				ReCalculateValue();
    				_isDirty = false;
    			}
    			return _value;
    		}
    	}
    
    	public Action<Stat> OnValueChange;
    
    	private List<Modifier> _statModifiers;
    	public int ModifierCount => _statModifiers.Count;
    
    	public Stat()
    	{
    		_statModifiers = new List<Modifier>();
    	}
    
    	public Stat(float baseValue) : this()
    	{
    		_baseValue = baseValue;
    	}

		public void AddModifierWithoutNotify(Modifier mod)
		{
			_isDirty = true;
    		_statModifiers.Add(mod);
		}

        public void AddModifier(Modifier mod)
    	{
			_statModifiers.Add(mod);
			ReCalculateValue();
    		OnValueChange?.Invoke(this);
    	}

    	public bool RemoveModifier(Modifier mod)
    	{
		    if (!_statModifiers.Remove(mod)) return false;
		    
		    ReCalculateValue();
		    OnValueChange?.Invoke(this);
		    return true;
	    }

		public bool RemoveModifierWithoutNotify(Modifier mod)
		{
			if (_statModifiers.Remove(mod))
    		{
    			_isDirty = true;
    			return true;
    		}
    		return false;
		}

    	public void ClearAllModifiers()
    	{
    		_statModifiers.Clear();
    		_value = _baseValue;
    		OnValueChange?.Invoke(this);
    	}

    	protected virtual void ReCalculateValue()
    	{
		    float baseConstant = 0f;
		    float constant = 0f;
		    float percent = 0f;

		    foreach (Modifier modifier in _statModifiers)
		    {
                switch (modifier.Type)
                {
                    case ModifyType.BaseConstant:
						baseConstant += modifier.Value; continue;
                    case ModifyType.Constant:
                        constant += modifier.Value; continue;
                    case ModifyType.Percent:
                        percent += modifier.Value; continue;
                };
            }

            float _finalValue = (BaseValue + baseConstant) * (1f + percent) + constant;

		    _value = (float)Math.Round(_finalValue, 4);
        }
    }
}
