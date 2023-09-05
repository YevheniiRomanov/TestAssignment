using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SessionSelectorItemView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _nameTMP;
    [SerializeField] Button _joinButton;

    public event Action<string> OnJoinClick;

    public void Init(string nameSession)
    {
        _nameTMP.text = nameSession;
        _joinButton.onClick.AddListener(JoinClick);
    }

    void JoinClick() => OnJoinClick?.Invoke(_nameTMP.text);

    void OnDestroy() => _joinButton.onClick.RemoveAllListeners();
}