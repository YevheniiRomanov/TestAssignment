using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class LobbyView : MonoBehaviour
{
    [SerializeField] Button _quickGameButton;
    [SerializeField] Button _findGameButton;
    [SerializeField] BasicSpawner _basicSpawner;
    [SerializeField] SessionSelectorView _sessionSelectorView;
    [SerializeField] GameObject _loadScreen;
    
    void Start()
    {
        _quickGameButton.onClick.AddListener(QuickGameClick);
        _findGameButton.onClick.AddListener(FindGameClick);
        _basicSpawner.OnSessionListUpdate += OnSessionListUpdate;
    }

    void OnSessionListUpdate(List<SessionInfo> sessionList)
    {
        if (sessionList.Count != 0)
        {
            _sessionSelectorView.Init(sessionList,_basicSpawner);
            _sessionSelectorView.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("No created room");
            gameObject.SetActive(true);
        }
    }

    async void QuickGameClick()
    {
        _loadScreen.SetActive(true);
        await _basicSpawner.StartQuickGame();
        gameObject.SetActive(false);
        _loadScreen.SetActive(false);
    }
    
    async void FindGameClick()
    {
        await _basicSpawner.UpdateLobby();
        gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        _quickGameButton.onClick.RemoveAllListeners();
        _findGameButton.onClick.RemoveAllListeners();
        _basicSpawner.OnSessionListUpdate -= OnSessionListUpdate;
    }
}
