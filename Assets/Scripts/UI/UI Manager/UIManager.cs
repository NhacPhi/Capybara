using System.Collections.Generic;
using System;
using Tech.Logger;
using UnityEngine;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

[DefaultExecutionOrder(-100)]
public class UIManager : MonoBehaviour
{
    [Inject] private IObjectResolver _objectResolver;
    private readonly Dictionary<string, PanelBase> _panelDictionary = new();
    
    private void Awake()
    {
        foreach (var panel in GetComponentsInChildren<PanelBase>(true))
        {
            _objectResolver?.InjectGameObject(panel.gameObject);
            _panelDictionary.Add(panel.name, panel);
        }
    }

    public async UniTask<PanelBase> CreatePanelAsync(string panelName, Transform parent = null, Action<PanelBase> onComplete = null)
    {
        if (_panelDictionary.ContainsKey(panelName)) return null;

        while (!AddressablesManager.Instance)
        {
            await UniTask.Yield();
        }
        
        var go = await AddressablesManager.Instance.InstantiateAsync(panelName, parent? parent: transform, false);
        go.name = panelName;
        _objectResolver.InjectGameObject(go);
        
        if (!go.TryGetComponent(out PanelBase panel))
        {
            LogCommon.LogError(go.name + "No Has Panel Component");
            return null;
        }
        onComplete?.Invoke(panel);

        _panelDictionary.Add(panelName, panel);
        return panel;
    }

    public void RemovePanel(string panelName)
    {
        _panelDictionary.Remove(panelName);
        AddressablesManager.Instance.RemoveAsset(panelName);
    }

    public T GetPanel<T>(string panelName) where T : PanelBase
    {
        return (T)_panelDictionary.GetValueOrDefault(panelName);
    }

    public T GetFirstPanelOfType<T>()
    {
        foreach (var panel in _panelDictionary.Values)
        {
            if (panel is T tPanel) return tPanel;
        }

        return default;
    }
    
    public T ShowPanel<T>(string panelName) where T : PanelBase
    {
        if (!_panelDictionary.TryGetValue(panelName, out var panel))
        {
            return null;
        }

        panel.Show();
        return panel as T;
    }

    public void HidePanel(string panelName)
    {
        if (!_panelDictionary.TryGetValue(panelName, out var panel))
        {
            _ = CreatePanelAsync(panelName, onComplete:x => x.Hide());
            return;
        }

        panel.Hide();
    }
}
