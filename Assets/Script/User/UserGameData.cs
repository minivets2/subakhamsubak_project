using BackEnd;
using BackEnd.GlobalSupport;

[System.Serializable]
public class UserGameData
{
    public string country;
    public int bestDailyScore;
    public int bestWeeklyScore;
    public int bestMonthlyScore;
    public int bestScore;

    public void Reset()
    {
        CountryCode countryCode = (CountryCode)System.Enum.Parse(typeof(CountryCode),
            Backend.LocationProperties.Country.Replace(" ", string.Empty));
        Backend.BMember.UpdateCountryCode(countryCode);
        
        BackendReturnObject bro = Backend.BMember.GetMyCountryCode();
        country = bro.GetReturnValuetoJSON()["country"]["S"].ToString();
        
        bestDailyScore = 0;
        bestWeeklyScore = 0;
        bestMonthlyScore = 0;
        bestScore = 0;
    }
}
