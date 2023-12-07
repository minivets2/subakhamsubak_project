using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text gameScore;
    [SerializeField] private GameObject warningLine;
    [SerializeField] private GameObject gameOverPopup;
        
    private void OnEnable()
    {
        Warning.warningEvent += SetWarningLine;
    }

    private void OnDisable()
    {
        Warning.warningEvent -= SetWarningLine;
    }

    public void InitUI()
    {
        SetWarningLine(false);
        HideGameOverPopup();
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

    public void ShowGameOverPopup()
    {
        gameOverPopup.SetActive(true);
    }

    public void HideGameOverPopup()
    {
        gameOverPopup.SetActive(false);
    }

    public void NewGameButtonClick()
    {
        GameManager.Instance.StarGame();
    }
}
