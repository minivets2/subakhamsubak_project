using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text gameScore;
    [SerializeField] private GameObject warningLine;
    //[SerializeField] private 
        
    private void OnEnable()
    {
        Warning.warningEvent += SetWarningLine;
    }

    private void OnDisable()
    {
        Warning.warningEvent -= SetWarningLine;
    }

    private void Start()
    {
        InitUI();
    }

    private void InitUI()
    {
        SetWarningLine(false);
        SetGameScore(0);
    }

    private void SetWarningLine(bool isWarning)
    {
        warningLine.SetActive(isWarning);
    }

    public void SetGameScore(int score)
    {
        gameScore.text = score.ToString();
    }
}
