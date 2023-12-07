using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text gameScore;
    [SerializeField] private GameObject warningLine;
    [SerializeField] private GameObject gameOverPopup;
    [SerializeField] private GameObject pausePopup;
    
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
        HidePausePopup();
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

    public void ShowPausePopup()
    {
        Time.timeScale = 0f;
        pausePopup.SetActive(true);
        GameManager.Instance.SetIsUIOpen(true);
    }

    public void HidePausePopup()
    {
        Time.timeScale = 1f;
        pausePopup.SetActive(false);
        GameManager.Instance.SetIsUIOpen(false);
    }

    public void ExitButtonClick()
    {
        SceneManager.LoadScene("Lobby");
    }
}
