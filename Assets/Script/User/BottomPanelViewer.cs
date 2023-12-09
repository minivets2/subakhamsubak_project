using TMPro;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;
using BackEnd.GlobalSupport;

public class BottomPanelViewer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textNickname;
    [SerializeField] private Image flagImage;
    [SerializeField] private CountryCode countryCode;
        
    public void UpdateNickname()
    {
        textNickname.text = UserInfo.Data.nickname == null ? 
                            UserInfo.Data.gamerId : UserInfo.Data.nickname;
    }

    public void UpdateFlag()
    {
        Backend.BMember.UpdateCountryCode(countryCode);
    }
}
