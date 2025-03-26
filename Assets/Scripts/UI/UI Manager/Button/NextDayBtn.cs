using Observer;

public class NextDayBtn : ButtonBase
{
    public override void OnAwake()
    {
        Btn.onClick.AddListener(HandleClick);
    }

    private void HandleClick()
    {
        GameAction.OnNextDay?.Invoke();
    }
}