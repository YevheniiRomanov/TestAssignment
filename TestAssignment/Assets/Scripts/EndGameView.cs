using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameView : BaseView
{
    [SerializeField]Button GoToLobby;
    
    RunnerViewController _runnerViewController;
    UIManager _uiManager;

    public override void Init()
    {
        _runnerViewController = FindObjectOfType<RunnerViewController>();
        _uiManager = _runnerViewController.GetUIManager();
        GoToLobby.onClick.AddListener(GoToLobbyClick);
    }

    void GoToLobbyClick()
    {
        _uiManager.ClosePopUp(UIType.EndGame);
        SceneManager.LoadScene(0);
    }
}