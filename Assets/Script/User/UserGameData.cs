using BackEnd;
using BackEnd.GlobalSupport;

[System.Serializable]
public class UserGameData
{
    public string country;
    public int bestScore;

    public void Reset()
    {
        CountryCode countryCode = (CountryCode)System.Enum.Parse(typeof(CountryCode),
            Backend.LocationProperties.Country.Replace(" ", string.Empty));
        Backend.BMember.UpdateCountryCode(countryCode);
        
        BackendReturnObject bro = Backend.BMember.GetMyCountryCode();
        country = bro.GetReturnValuetoJSON()["country"]["S"].ToString();
        
        bestScore = 0;
    }
}
