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
        string country = Backend.LocationProperties.Country.Replace(" ", string.Empty);
        countryCode = (CountryCode)System.Enum.Parse(typeof(CountryCode), country);
        Backend.BMember.UpdateCountryCode(countryCode);
        
        BackendReturnObject bro = Backend.BMember.GetMyCountryCode();
        string countryByString = bro.GetReturnValuetoJSON()["country"]["S"].ToString();
        
        flagImage.sprite = Resources.Load<Sprite>(countryByString);
    }
}