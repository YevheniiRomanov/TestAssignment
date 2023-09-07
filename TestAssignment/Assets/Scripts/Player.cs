using System.Threading.Tasks;
using Fusion;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public Camera Camera;
    public float PlayerSpeed = 2f;
    public float Damage;
    
    public float NetworkedHealth { get; set; } = 100;
    
    CharacterController _controller;
    FirstPersonCamera _firstPersonCamera;
    RunnerViewController _runnerViewController;
    UIManager _uiManager;

    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _runnerViewController = FindObjectOfType<RunnerViewController>();
        _uiManager = _runnerViewController.GetUIManager();
    }

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
        if (NetworkedHealth < 0) 
           return;
        NetworkedHealth -= damage;
        if (NetworkedHealth == 0) 
            ShowEndGame();
        _uiManager.OpenPopUp(UIType.Damage);
        await Task.Delay(500);
        _uiManager.ClosePopUp(UIType.Damage);
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
        _uiManager.OpenPopUp(UIType.EndGame);
        Runner.Disconnect(Runner.LocalPlayer);
    }
}