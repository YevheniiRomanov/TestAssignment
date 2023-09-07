using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

public class RunnerViewController : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] UIManager _uiManager;
    [SerializeField] NetworkRunner _networkRunner;

    public event Action<List<SessionInfo>> OnSessionListUpdate;
    
    List<SessionInfo> _sessionsInfo;

    void Start() => _uiManager.OpenPopUp(UIType.Lobby);

    public UIManager GetUIManager() => _uiManager;
    public List<SessionInfo> GetSessionList() => _sessionsInfo;
    public void ClearSessionList() => _sessionsInfo = null;

    public async Task StartQuickGame()
    {
        await _networkRunner.StartGame(new StartGameArgs
        {
            GameMode = GameMode.Shared
        });
    }

    public async Task UpdateLobbyList() => 
        await _networkRunner.JoinSessionLobby(SessionLobby.Shared);

    public async Task<bool> ConnectToSession(string sessionName)
    {
        var result = await _networkRunner.StartGame(new StartGameArgs
        {
            GameMode = GameMode.Shared,
            SessionName = sessionName
        });
        return result.Ok;
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        OnSessionListUpdate?.Invoke(sessionList);
        _sessionsInfo = sessionList;
    }

    #region NonUsageCallbacks

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }

    #endregion
}