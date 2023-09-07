using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class SessionSelectorView : BaseView
{
    [SerializeField] Transform _content;
    [SerializeField] SessionSelectorItemView _itemPrefab;
    
    RunnerViewController _runnerViewController;
    UIManager _uiManager;
    List<SessionInfo> _sessionList;
    List<SessionSelectorItemView> _items = new();
    
    public override void Init()
    {
        _runnerViewController = FindObjectOfType<RunnerViewController>();
        _uiManager = _runnerViewController.GetUIManager();
        _sessionList = _runnerViewController.GetSessionList();
        if (_sessionList == null)
        {
            Debug.LogError("List empty!");
            _uiManager.ClosePopUp(UIType.SessionSelector);
            _uiManager.OpenPopUp(UIType.Lobby);
        }
        else
            CreateElementsOfList();
    }

    void CreateElementsOfList()
    {
        foreach (var sessionInfo in _sessionList)
        {
            var item = Instantiate(_itemPrefab, _content);
            item.Init(sessionInfo.Name);
            item.OnJoinClick += OnJoinClick;
            _items.Add(item);
        }
    }

    async void OnJoinClick(string nameSession)
    {
        _uiManager.OpenPopUp(UIType.Loading);
        var success= await _runnerViewController.ConnectToSession(nameSession);
        
        if (success) 
            _uiManager.ClosePopUp(UIType.SessionSelector);
        else
            _uiManager.OpenPopUp(UIType.Lobby);
        Dismiss();
        _uiManager.ClosePopUp(UIType.Loading);
    }

    void Dismiss()
    {
        gameObject.SetActive(false);
        foreach (var item in _items)
        {
            item.OnJoinClick -= OnJoinClick;
            Destroy(item.gameObject);
        }
        _runnerViewController.ClearSessionList();
    }
}