using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class LobbyView : BaseView
{
    [SerializeField] Button _quickGameButton;
    [SerializeField] Button _findGameButton;

    RunnerViewController _runnerViewController;
    UIManager _uiManager;

    public override void Init()
    {
        _runnerViewController = FindObjectOfType<RunnerViewController>();
        _quickGameButton.onClick.AddListener(QuickGameClick);
        _findGameButton.onClick.AddListener(FindGameClick);
        _uiManager = _runnerViewController.GetUIManager();
    }

    async void QuickGameClick()
    {
        _uiManager.OpenPopUp(UIType.Loading);
        await _runnerViewController.StartQuickGame();
        _uiManager.ClosePopUp(UIType.Lobby);
        _uiManager.ClosePopUp(UIType.Loading);
    }

    async void FindGameClick()
    {
        _uiManager.OpenPopUp(UIType.Loading);
        _findGameButton.interactable = false;
        await _runnerViewController.UpdateLobbyList();
        _runnerViewController.OnSessionListUpdate += OnSessionListUpdate;
    }

    void OnSessionListUpdate(List<SessionInfo> list)
    {
        if (list.Count != 0)
        {
            _uiManager.ClosePopUp(UIType.Lobby);
            _uiManager.OpenPopUp(UIType.SessionSelector);
            _uiManager.ClosePopUp(UIType.Loading);
        }
        else
        {
            Debug.LogError("No created room");
            _findGameButton.interactable = true;
            _uiManager.ClosePopUp(UIType.Loading);
        }
        _runnerViewController.OnSessionListUpdate -= OnSessionListUpdate;
    }

    void OnDestroy()
    {
        _quickGameButton.onClick.RemoveAllListeners();
        _findGameButton.onClick.RemoveAllListeners();
    }
}