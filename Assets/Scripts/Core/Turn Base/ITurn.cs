using Core.Entities.Common;

namespace Core.TurnBase
{
    public interface ITurn
    {
        public bool IsEndTurn { get; }
        public void HandleTurn(Entity target);
    }
}
