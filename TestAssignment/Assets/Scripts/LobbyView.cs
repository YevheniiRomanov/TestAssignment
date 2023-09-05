using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class LobbyView : MonoBehaviour
{
    [SerializeField] Button _quickGameButton;
    [SerializeField] Button _findGameButton;
    [SerializeField] TextMeshProUGUI _inputField;
    [SerializeField] BasicSpawner _basicSpawner;
    
    void Start()
    {
        _quickGameButton.onClick.AddListener(QuickGameClick);
        _findGameButton.onClick.AddListener(FindGameClick);
    }

    async void QuickGameClick()
    {
        //_basicSpawner.StartGame(GameMode.Host, _inputField.text);
        await _basicSpawner.StartGameTest();
        gameObject.SetActive(false);
        // if (_inputField.text.Length <= 1) 
        //     Debug.LogError("EnterRoomName!");
        // else
        // {
        //     //_basicSpawner.StartGame(GameMode.Host, _inputField.text);
        //     await _basicSpawner.JoinGameTest();
        //     gameObject.SetActive(false);
        // }
    }
    
    async void FindGameClick()
    {
        await _basicSpawner.JoinGameTest();
        gameObject.SetActive(false);
        // if (_inputField.text.Length <= 1) 
        //     Debug.LogError("EnterRoomName!");
        // else
        // {
        //     _basicSpawner.StartGame(GameMode.Client, _inputField.text);
        //     gameObject.SetActive(false);
        // }
    }

    private void OnDestroy()
    {
        _quickGameButton.onClick.RemoveAllListeners();
        _findGameButton.onClick.RemoveAllListeners();
    }
}
