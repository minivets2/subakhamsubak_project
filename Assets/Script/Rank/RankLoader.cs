using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RankLoader : MonoBehaviour
{
    [SerializeField] private GameObject rankDataPrefab;
    [SerializeField] private Scrollbar scrollbar;
    [SerializeField] private Transform rankDataParent;
    [SerializeField] private RankData myRankData;

    private List<RankData> _rankDataList;

    private void Awake()
    {
        _rankDataList = new List<RankData>();

        for (int i = 0; i < Constants.MAX_RANK_LIST; ++i)
        {
            GameObject clone = Instantiate(rankDataPrefab, rankDataParent);
            _rankDataList.Add(clone.GetComponent<RankData>());
        }
    }

    private void OnEnable()
    {
        scrollbar.value = 1;
        GetRankList();
        GetMyRank();
    }

    private void GetRankList()
    {
        Backend.URank.User.GetRankList(Constants.RANK_UUID, Constants.MAX_RANK_LIST, callback =>
        {
            if (callback.IsSuccess())
            {
                try
                {
                    Debug.Log($"랭킹 조회에 성공했습니다 : {callback}");

                    LitJson.JsonData rankDataJson = callback.FlattenRows();

                    if (rankDataJson.Count <= 0)
                    {
                        for (int i = 0; i < Constants.MAX_RANK_LIST; ++i)
                        {
                            SetRankData(_rankDataList[i], i + 1, "KR","-", 0 );
                        }
                        
                        Debug.LogWarning("데이터가 존재하지 않습니다");
                    }
                    else
                    {
                        int rankerCount = rankDataJson.Count;

                        for (int i = 0; i < rankerCount; ++i)
                        {
                            _rankDataList[i].Rank = int.Parse(rankDataJson[i]["rank"].ToString());
                            _rankDataList[i].Score = int.Parse(rankDataJson[i]["score"].ToString());
                            _rankDataList[i].Nickname = rankDataJson[i].ContainsKey("nickname") == true ?
                                rankDataJson[i]["nickname"]?.ToString() : UserInfo.Data.gamerId;
                            _rankDataList[i].CountryCode = rankDataJson[i]["country"].ToString();
                        }

                        for (int i = rankerCount; i < Constants.MAX_RANK_LIST; ++i)
                        {
                            SetRankData(_rankDataList[i], i + 1, "KR","-", 0);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            else
            {
                for (int i = 0; i < Constants.MAX_RANK_LIST; ++i)
                {
                    SetRankData(_rankDataList[i], i + 1, "KR","-", 0);
                }
                
                Debug.LogError($"랭킹 조회 중 오류가 발생했습니다 : {callback}");
            }
        });
    }

    private void GetMyRank()
    {
        Backend.URank.User.GetMyRank(Constants.RANK_UUID, callback =>
        {
            string nickName = UserInfo.Data.nickname == null ? UserInfo.Data.gamerId : UserInfo.Data.nickname;

            if (callback.IsSuccess())
            {
                try
                {
                    LitJson.JsonData rankDataJson = callback.FlattenRows();

                    if (rankDataJson.Count <= 0)
                    {
                        SetRankData(myRankData, 1000000000, "KR", nickName, 0);
                        
                        Debug.LogWarning("데이터가 존재하지 않습니다.");
                    }
                    else
                    {
                        myRankData.Rank = int.Parse(rankDataJson[0]["rank"].ToString());
                        myRankData.CountryCode = rankDataJson[0]["country"].ToString();
                        myRankData.Score = int.Parse(rankDataJson[0]["score"].ToString());

                        myRankData.Nickname = rankDataJson[0].ContainsKey("nickname") == true ?
                            rankDataJson[0]["nickname"]?.ToString() : UserInfo.Data.gamerId;
                    }
                }
                catch (Exception e)
                {
                    SetRankData(myRankData, 1000000000, "KR", nickName, 0);
                    
                    Debug.LogError(e);
                }
            }
            else
            {
                if (callback.GetMessage().Contains("userRank"))
                {
                    SetRankData(myRankData, 1000000000, "KR", nickName, 0);
                }
            }

        });
    }
    
    private void SetRankData(RankData rankData, int rank, string countryCode, string nickname, int score)
    {
        rankData.Rank = rank;
        rankData.CountryCode = countryCode;
        rankData.Nickname = nickname;
        rankData.Score = score;
    }
}
