using TMPro;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;
using BackEnd.GlobalSupport;

public class BottomPanelViewer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textNickname;
    [SerializeField] private Image flagImage;
    [SerializeField] private TextMeshProUGUI textBestScore;

    private void Awake()
    {
        BackendGameData.Instance.onGameDataLoadEvent.AddListener(UpdateGameData);
    }

    public void UpdateNickname()
    {
        textNickname.text = UserInfo.Data.nickname == null ? 
                            UserInfo.Data.gamerId : UserInfo.Data.nickname;
    }

    public void UpdateFlag()
    {
        BackendReturnObject bro = Backend.BMember.GetMyCountryCode();
        string countryByString = bro.GetReturnValuetoJSON()["country"]["S"].ToString();
        
        flagImage.sprite = Resources.Load<Sprite>(countryByString);
        flagImage.SetNativeSize();
    }

    public void UpdateGameData()
    {
        textBestScore.text = $"{BackendGameData.Instance.UserGameData.bestScore}";
    }
}
