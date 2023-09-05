using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class SessionSelectorView : MonoBehaviour
{
    [SerializeField] Transform _content;
    [SerializeField] SessionSelectorItemView _itemPrefab;
    [SerializeField] GameObject _loadScreen;

    BasicSpawner _basicSpawner;
    List<SessionSelectorItemView> _items = new();
    public void Init(List<SessionInfo> sessionList, BasicSpawner basicSpawner)
    {
        _basicSpawner = basicSpawner;
        foreach (var sessionInfo in sessionList)
        {
            var item = Instantiate(_itemPrefab, _content);
            item.Init(sessionInfo.Name);
            item.OnJoinClick += OnJoinClick;
            _items.Add(item);
        }
    }

    async void OnJoinClick(string nameSession)
    {
        _loadScreen.SetActive(true);
        await _basicSpawner.JoinSession(nameSession);
        Dismiss();
        gameObject.SetActive(false);
        _loadScreen.SetActive(false);
    }

    void Dismiss()
    {
        foreach (var item in _items)
        {
            item.OnJoinClick -= OnJoinClick;
            Destroy(item.gameObject);
        }
    }
}