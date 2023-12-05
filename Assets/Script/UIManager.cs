using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text gameScore;

    public void SetGameScore(int score)
    {
        gameScore.text = score.ToString();
    }
}
