using System.Threading.Tasks;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : NetworkBehaviour
{
    private CharacterController _controller;

    public Camera Camera;
    public float PlayerSpeed = 2f;
    public float Damage;
    
    //[Networked(OnChanged = nameof(NetworkedHealthChanged))]
    public float NetworkedHealth { get; set; } = 100;
    public GameObject PopupHp;
    public GameObject EndGame;
    public Button GoToLobby;

    
    FirstPersonCamera FirstPersonCamera;

    void Awake() =>
        _controller = GetComponent<CharacterController>();

    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            Camera = Camera.main;
            Camera.GetComponent<FirstPersonCamera>().Target = GetComponent<NetworkTransform>().InterpolationTarget;
        }
    }
    
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public async void DealDamageRpc(float damage)
    {
        NetworkedHealth -= damage;
        if (NetworkedHealth == 0) 
            ShowEndGame();
        PopupHp.SetActive(true);
        await Task.Delay(500);
        PopupHp.SetActive(false);
    }

    public override void FixedUpdateNetwork()
    {
        if (Camera == null || HasStateAuthority == false)
            return;

        var cameraRotationY = Quaternion.Euler(0, Camera.transform.rotation.eulerAngles.y, 0);
        Vector3 move = cameraRotationY * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) *
                       Runner.DeltaTime * PlayerSpeed;

        _controller.Move(move);

        if (move != Vector3.zero)
            gameObject.transform.forward = move;
    }

    void Update()
    {
        if (Camera == null || HasStateAuthority == false)
            return;

        Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
        ray.origin += Camera.transform.forward;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Runner.GetPhysicsScene().Raycast(ray.origin, ray.direction, out var hit))
            {
                if (hit.transform.TryGetComponent<Player>(out var player)) 
                    player.DealDamageRpc(Damage);
            }
        }
    }

    void ShowEndGame()
    {
        EndGame.SetActive(true);
        GoToLobby.onClick.AddListener(GoToLobbyClick);
        Runner.Disconnect(Runner.LocalPlayer);
    }

    void GoToLobbyClick()
    {
        EndGame.SetActive(false);
        SceneManager.LoadScene(0);
    }

    // static void NetworkedHealthChanged(Changed<Health> changed)
    // {
    //     //Debug.Log($"Health changed to: {changed.Behaviour.NetworkedHealth}");
    // }
}