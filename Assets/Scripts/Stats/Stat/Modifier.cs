using System;

namespace Stats.stat
{
	public enum ModifyType
	{
		BaseConstant,
		Constant,
		Percent,
	}

	public struct Modifier : IEquatable<Modifier>
	{
		public float Value;
		public ModifyType Type;
		public Modifier(float value, ModifyType type = ModifyType.Constant)
		{
			Value = value;
			Type = type;
		}

        public bool Equals(Modifier other)
        {
            return Value.Equals(other.Value) && Type == other.Type;
        }
    }
}
