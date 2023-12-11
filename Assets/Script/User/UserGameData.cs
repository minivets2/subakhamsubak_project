using UnityEngine.Serialization;

[System.Serializable]
public class UserGameData
{
    public int bestScore;

    public void Reset()
    {
        bestScore = 0;
    }
}
