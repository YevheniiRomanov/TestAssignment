using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class LobbyView : MonoBehaviour
{
    [SerializeField] Button _quickGameButton;
    [SerializeField] Button _findGameButton;
    [SerializeField] NetworkRunner _networkRunner;
    [SerializeField] SessionSelectorView _sessionSelectorView;
    [SerializeField] UIManager _uiManager;

    event Action<string> OnSelectSession;
    
    void Start()
    {
        _quickGameButton.onClick.AddListener(QuickGameClick);
        _findGameButton.onClick.AddListener(FindGameClick);
    }

    async void QuickGameClick()
    {
        _uiManager.OpenPopUp(UIType.Loading);
        await _networkRunner.StartGame(new StartGameArgs
        {
            GameMode = GameMode.Shared
        });
        gameObject.SetActive(false);
        _uiManager.ClosePopUp(UIType.Loading);
    }
    
    async void FindGameClick()
    {
        _findGameButton.interactable = false;
        await JoinLobbyForUpdate();
        gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        _quickGameButton.onClick.RemoveAllListeners();
        _findGameButton.onClick.RemoveAllListeners();
    }
    
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        if (sessionList.Count != 0)
        {
            OnSelectSession += SessionSelect;
            _sessionSelectorView.Init(sessionList, OnSelectSession);
            _sessionSelectorView.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("No created room");
            gameObject.SetActive(true);
        }
    }

    async void SessionSelect(string sessionName)
    {
        OnSelectSession -= SessionSelect;
        _uiManager.OpenPopUp(UIType.Loading);

        var result = await _networkRunner.StartGame(new StartGameArgs
        {
            GameMode = GameMode.Shared,
            SessionName = sessionName
        });

        if (!result.Ok)
        {
            Debug.LogError($"Failed to Start: {result.ShutdownReason}");
            gameObject.SetActive(true);
        }
        _uiManager.ClosePopUp(UIType.Loading);
    }

    async Task JoinLobbyForUpdate()
    {
        var result = await _networkRunner.JoinSessionLobby(SessionLobby.Shared);
        
        if (!result.Ok)
        {
            _findGameButton.interactable = false;
            Debug.LogError($"Failed to Start: {result.ErrorMessage}");
        }
    }

}
