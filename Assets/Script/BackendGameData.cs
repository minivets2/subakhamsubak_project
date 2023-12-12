using BackEnd;
using UnityEngine;
using UnityEngine.Events;

public class BackendGameData : Singleton<BackendGameData>
{
    [System.Serializable]
    public class GameDataLoadEvent : UnityEngine.Events.UnityEvent { }
    public GameDataLoadEvent onGameDataLoadEvent = new GameDataLoadEvent();
    
    private UserGameData userGameData = new UserGameData();
    public UserGameData UserGameData => userGameData;

    private string gameDataRowInData = string.Empty;

    public void GameDataInsert()
    {
        userGameData.Reset();

        Param param = new Param()
        {
            {"country", userGameData.country},
            {"bestScore", userGameData.bestScore}
        };

        Backend.GameData.Insert("USER_DATA", param, callback =>
        {
            if (callback.IsSuccess())
            {
                gameDataRowInData = callback.GetInDate();
                
                Debug.Log($"게임 정보 데이터 삽입에 성공했습니다. : {callback}");
            }
            else
            {
                Debug.Log($"게임 정보 데이터 삽입에 실패했습니다. : {callback}");
            }
        });
    }

    public void GameDataLoad()
    {
        Backend.GameData.GetMyData("USER_DATA", new Where(), callback =>
        {
            if (callback.IsSuccess())
            {
                Debug.Log($"게임 정보 데이터 불러오기에 성공했습니다. : {callback}");

                try
                {
                    LitJson.JsonData gameDataJson = callback.FlattenRows();

                    if (gameDataJson.Count <= 0)
                    {
                        Debug.LogWarning("데이터가 존재하지 않습니다.");
                    }
                    else
                    {
                        gameDataRowInData = gameDataJson[0]["inDate"].ToString();

                        userGameData.country = gameDataJson[0]["country"].ToString();
                        userGameData.bestScore = int.Parse(gameDataJson[0]["bestScore"].ToString());

                        onGameDataLoadEvent?.Invoke();
                    }
                }
                catch (System.Exception e)
                {
                    userGameData.Reset();
                    
                    Debug.Log(e);
                }
            }
            else
            {
                Debug.LogError($"게임 정보 데이터 불러오기에 실패했습니다 : {callback}");
            }
    
        });
    }

    public void GameDataUpdate(UnityAction action = null)
    {
        if (userGameData == null)
        {
            Debug.LogError("서버에서 다운받거나 새로 삽입한 데이터가 존재하지 않습니다." + 
                           "Insert 혹은 Load를 통해 데이터를 생성해주세요.");

            return;
        }

        Param param = new Param()
        {
            {"country", userGameData.country},
            {"bestScore", userGameData.bestScore}
        };

        if (string.IsNullOrEmpty(gameDataRowInData))
        {
            Debug.LogError("유저의 inData 정보가 없어 게임 정보 데이터 수정에 실패했습니다.");
        }
        else
        {
            Debug.Log($"{gameDataRowInData}의 게임 정보 데이터 수정을 요청합니다.");

            Backend.GameData.UpdateV2("USER_DATA", gameDataRowInData, Backend.UserInDate, param, callback =>
            {
                if (callback.IsSuccess())
                {
                    Debug.Log($"게임 정보 데이터 수정에 성공했습니다. : {callback}");

                    action?.Invoke();
                }
                else
                {
                    Debug.LogError($"게임 정보 데이터 수정에 실패했습니다. : {callback}");
                }
            });
        }
    }
}
