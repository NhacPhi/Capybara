using Stats.Status_Effect;

namespace Stats
{
    public interface IEffectable
    {
        public void ApplyEffect(StatusEffect effect);
        public void RemoveEffect(StatusEffect effect, bool ignoreStack);
        public bool HasEffect<T>() where T : StatusEffect;
    }
}