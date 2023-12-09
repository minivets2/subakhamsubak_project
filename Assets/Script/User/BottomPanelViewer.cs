using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BottomPanelViewer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textNickname;

    public void UpdateNickname()
    {
        textNickname.text = UserInfo.Data.nickname == null ? 
                            UserInfo.Data.gamerId : UserInfo.Data.nickname;
    }
}
