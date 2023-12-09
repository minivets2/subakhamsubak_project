using TMPro;
using UnityEngine;
using BackEnd;
using BackEnd.GlobalSupport;

public class PopupUpdateProfileViewer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textNickname;
    [SerializeField] private TextMeshProUGUI textGamerID;
    [SerializeField] private TextMeshProUGUI countName;

    public void UpdateNickname()
    {
        textNickname.text = UserInfo.Data.nickname == null ? UserInfo.Data.gamerId : UserInfo.Data.nickname;

        textGamerID.text = UserInfo.Data.gamerId;

        countName.text = Backend.LocationProperties.Country;
    }
}
