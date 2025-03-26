using System;

namespace Stats.M_Attribute
{
	public enum AttributeType
	{
		Hp,
		Exp,
		Lv,
	}
	
	public static class AttributeExtensions
	{
		public const string Hp = "HP";
		public const string Exp = "EXP";
		public const string Lv = "LV";
		
		public static string GetName(AttributeType type)
		{
			switch (type)
			{
				case AttributeType.Hp:
					return Hp;
				case AttributeType.Exp:
					return Exp;
				case AttributeType.Lv:
					return Lv;
			}	
			
			return string.Empty;
		}
	}
}

