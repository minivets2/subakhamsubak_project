using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UnityEngine;

public class RankRegister : MonoBehaviour
{
    public void Process(int newScore)
    {
        UpdataRankData(newScore);
        UpdataMyBestRankData(newScore);
    }

    private void UpdataRankData(int newScore)
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
                rowInData = callback.FlattenRows()[0]["inData"].ToString();
            }
            else
            {
                Debug.LogError("데이터가 존재하지 않습니다");
                return;
            }

            Param param = new Param()
            {
                { "bestScore", newScore }
            };

            Backend.URank.User.UpdateUserScore(Constants.RANK_UUID, Constants.USER_DATA_TABLE, rowInData, param,
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

    private void UpdataMyBestRankData(int newScore)
    {
        Backend.URank.User.GetMyRank(Constants.RANK_UUID, callback =>
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
                            UpdataRankData(newScore);

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
                    UpdataRankData(newScore);
                    
                    Debug.Log($"새로운 랭킹 데이터 생성 및 등록 : {callback}");
                }
            }
        });
    }
}
