using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class SessionSelectorView : MonoBehaviour
{
    [SerializeField] Transform _content;
    [SerializeField] SessionSelectorItemView _itemPrefab;
    event Action<string> _onSessionSelect;
    List<SessionSelectorItemView> _items = new();
    public void Init(List<SessionInfo> sessionList, Action<string> onSessionSelect)
    {
        _onSessionSelect = onSessionSelect;
        foreach (var sessionInfo in sessionList)
        {
            var item = Instantiate(_itemPrefab, _content);
            item.Init(sessionInfo.Name);
            item.OnJoinClick += OnJoinClick;
            _items.Add(item);
        }
    }

    void OnJoinClick(string nameSession)
    {
        _onSessionSelect?.Invoke(nameSession);
        Dismiss();
    }

    void Dismiss()
    {
        gameObject.SetActive(false);
        foreach (var item in _items)
        {
            item.OnJoinClick -= OnJoinClick;
            Destroy(item.gameObject);
        }
    }
}