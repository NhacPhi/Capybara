using System;

namespace Stats.stat
{
	public enum StatType
	{
		Atk,
		MaxHp,
		Def,
		ExpToLevelUp,
		MaxLv,
	}

	public static class StatExtensions
	{
		public const string Atk = "ATK";
		public const string MaxHp = "MAXHP";
		public const string Def = "DEF";

		public static string GetName(StatType type)
		{
			switch (type)
			{
				case StatType.Atk:
					return Atk;
				case StatType.MaxHp:
					return MaxHp;
				case StatType.Def:
					return Def;
				default:
					return string.Empty;
			}
		}
	}
}
