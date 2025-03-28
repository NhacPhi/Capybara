using Core.Entities.Player;
using Stats.stat;

namespace Event_System
{
    public static class EventUtil
    {
        public static void HandleModify(BaseModifyValue modifier, PlayerStats playerStats)
        {
            switch (modifier)
            {
                case AttributeModify attributeModify:
                    HandleAttribute(attributeModify,playerStats);
                    break;
                case StatModify statModify:
                    HandleStat(statModify, playerStats);
                    break;
                case MoneyModify moneyModify:
                    HandleMoney(moneyModify, playerStats);
                    break;
            }
        }

        public static void HandleAttribute(AttributeModify attributeModify, PlayerStats playerStats)
        {
            var attribute = playerStats.GetAttribute(attributeModify.AttributeType);

            if (attributeModify.ModType == ModifyType.Percent)
            {
                attribute.Value *= (1 * attributeModify.Value);
                return;
            }
            
            attribute.Value += attributeModify.Value;
        }

        public static void HandleStat(StatModify statModify, PlayerStats playerStats)
        {
            playerStats.AddModifier(statModify.StatType, new Modifier(statModify.Value, statModify.ModType));
        }

        public static void HandleMoney(MoneyModify moneyModify, PlayerStats playerStats)
        {
            if (moneyModify.ModType == ModifyType.Percent)
            {
                playerStats.Money *= (1 * moneyModify.Value);
                return;
            }
            
            playerStats.Money += moneyModify.Value;
        }
    }
}
