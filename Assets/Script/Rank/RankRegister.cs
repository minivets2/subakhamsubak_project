using BackEnd;
using UnityEngine;

public class RankRegister : MonoBehaviour
{
    [SerializeField] private RankType rankType;
    
    public string GetRankUUID()
    {
        string uuid = Constants.RANK_UUID;
        
        if (rankType == RankType.Daily)
        {
            uuid = Constants.DAYLY_RANK_UUID;
        }
        else if (rankType == RankType.Weekly)
        {
            uuid = Constants.WEEKLY_RANK_UUID;
        }
        else if (rankType == RankType.Monthly)
        {
            uuid = Constants.MONTHLY_RANK_UUID;
        }

        return uuid;
    }
    
    public string GetRankType()
    {
        string _rankType = "bestScore";
        
        if (rankType == RankType.Daily)
        {
            _rankType = "bestDailyScore";
        }
        else if (rankType == RankType.Weekly)
        {
            _rankType = "bestWeeklyScore";
        }
        else if (rankType == RankType.Monthly)
        {
            _rankType = "bestMonthlyScore";
        }

        return _rankType;
    }
    
    public void Process(int newScore, string country)
    {
        UpdataMyBestRankData(newScore, country);
    }

    private void UpdateRankData(int newScore, string country)
    {
        string rowInData = string.Empty;

        Backend.GameData.GetMyData(Constants.USER_DATA_TABLE, new Where(), callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"데이터 조회 중 문제가 발생했습니다. : {callback}");
                return;
            }
            
            Debug.Log($"데이터 조회에 성공했습니다. : {callback}");

            if (callback.FlattenRows().Count > 0)
            {
                rowInData = callback.FlattenRows()[0]["inDate"].ToString();
            }
            else
            {
                Debug.LogError("데이터가 존재하지 않습니다");
                return;
            }

            
            
            Param param = new Param()
            {
                { "country", country},
                {GetRankType(), newScore}
            };

            Backend.URank.User.UpdateUserScore(GetRankUUID(), Constants.USER_DATA_TABLE, rowInData, param,
                callback =>
                {
                    if (callback.IsSuccess())
                    {
                        Debug.Log($"랭킹 등록에 성공했습니다. : {callback}");
                    }
                    else
                    {
                        Debug.LogError($"랭킹 등록에 실패했습니다. : {callback}");
                    }
                });
        });
    }

    private void UpdataMyBestRankData(int newScore, string country)
    {
        Backend.URank.User.GetMyRank(GetRankUUID(), callback =>
        {
            if (callback.IsSuccess())
            {
                try
                {
                    LitJson.JsonData rankDataJson = callback.FlattenRows();

                    if (rankDataJson.Count <= 0)
                    {
                        Debug.LogWarning("데이터가 존재하지 않습니다.");
                    }
                    else
                    {
                        int bestScore = int.Parse(rankDataJson[0]["score"].ToString());

                        if (newScore > bestScore)
                        {
                            UpdateRankData(newScore, country);

                            Debug.Log($"최고 점수 갱신 {bestScore} -> {newScore}");
                        }
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError(e);
                }
            }
            else
            {
                if (callback.GetMessage().Contains("userRank"))
                {
                    UpdateRankData(newScore, country);
                    
                    Debug.Log($"새로운 랭킹 데이터 생성 및 등록 : {callback}");
                }
            }
        });
    }
}
