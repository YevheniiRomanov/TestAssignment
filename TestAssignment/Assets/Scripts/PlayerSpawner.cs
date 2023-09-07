using Fusion;
using UnityEngine;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined, IPlayerLeft
{
    public GameObject PlayerPrefab;

    NetworkObject networkPlayerObject;

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
            networkPlayerObject = Runner.Spawn(PlayerPrefab, new Vector3(0, 2, 0), Quaternion.identity, player);
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (player == Runner.LocalPlayer) 
            Runner.Despawn(networkPlayerObject);
    }
}