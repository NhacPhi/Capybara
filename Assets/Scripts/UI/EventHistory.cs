using System.Linq;
using UnityEngine;

public class EventHistory : MonoBehaviour
{
    [SerializeField] private EventHistoryItem _prefab;
    [SerializeField] private Transform _contentHolder;
    
    private void Reset()
    {
        var allTransform = GetComponentsInChildren<Transform>(true);

        if (!_contentHolder)
        {
            _contentHolder = allTransform.FirstOrDefault(x => x.name.Contains("Content"));
        }
    }

    public EventHistoryItem CreateMessage()
    {
        //Dont Need Pool Here Message Will Not Delete
        return Instantiate(_prefab, _contentHolder);
    }
}
