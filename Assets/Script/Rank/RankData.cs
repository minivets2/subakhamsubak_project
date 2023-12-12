using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankData : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textRank;
    [SerializeField] private Image flagImage;
    [SerializeField] private TextMeshProUGUI textNickname;
    [SerializeField] private TextMeshProUGUI textScore;

    private int rank;
    private string countryCode;
    private string nickName;
    private int score;

    public int Rank
    {
        set
        {
            if (value <= Constants.MAX_RANK_LIST)
            {
                rank = value;
                textRank.text = rank.ToString();
            }
            else
            {
                textRank.text = "순위에 없음";
            }
        }
        get => rank;
    }
    
    public string CountryCode
    {
        set
        {
            countryCode = value;
            flagImage.sprite = Resources.Load<Sprite>(countryCode);
            flagImage.SetNativeSize();
        }
        get => countryCode;
    }

    public string Nickname
    {
        set
        {
            nickName = value;
            textNickname.text = nickName;
        }
        get => nickName;
    }

    public int Score
    {
        set
        {
            score = value;
            textScore.text = score.ToString();
        }
        get => score;
    }


}
