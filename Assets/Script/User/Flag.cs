using BackEnd;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Flag : LoginBase
{
    [System.Serializable]
    public class NicknameEvent : UnityEngine.Events.UnityEvent {}
    public NicknameEvent onNicknameEvent = new NicknameEvent();

    [SerializeField] private Image imageNickname;
    [SerializeField] private TMP_InputField inputFieldNickname;
    [SerializeField] private Button btnUpdateNickname;
}
