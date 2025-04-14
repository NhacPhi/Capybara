namespace Core
{
    public struct DamageBonus
    {
        public float FlatValue;
        public float DamageMultiplier;
        
        public static DamageBonus GetDefault()
        {
            return new DamageBonus()
            {
                FlatValue = 0f,
                DamageMultiplier = 1f
            };
        }
    }
}