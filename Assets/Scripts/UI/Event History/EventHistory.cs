using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class EventHistory : MonoBehaviour
{
    [SerializeField] private GameObject _defaultHistoryItem;
    [SerializeField] private GameObject _skillRewardItem;
    [SerializeField] private Transform _contentHolder;
    private ScrollRect _scrollRect;
    
    private void Reset()
    {
        var allTransform = GetComponentsInChildren<Transform>(true);

        if (!_contentHolder)
        {
            _contentHolder = allTransform.FirstOrDefault(x => x.name.Contains("Content"));
        }
    }

    private void Awake()
    {
        _scrollRect = GetComponent<ScrollRect>();
    }

    public GameObject CreateMessage(MessageType type = MessageType.Default)
    {
        GameObject message = null;
      
        switch (type)
        {
            case MessageType.Default:
                message = Instantiate(_defaultHistoryItem, _contentHolder);
                break;
            case MessageType.SkillReward:
                message = Instantiate(_skillRewardItem, _contentHolder);
                break;
        }
        _ = WaitAutoScrollBottom();
        
        return message;
    }

    private async UniTask WaitAutoScrollBottom()
    {
        await UniTask.Yield(this.GetCancellationTokenOnDestroy());
        
        if (_scrollRect.verticalNormalizedPosition > 0.01f)
        {
            _scrollRect.DOVerticalNormalizedPos(0, 0.3f);
        }
    }
}

public enum MessageType
{
    Default,
    SkillReward,
}