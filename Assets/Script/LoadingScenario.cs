using UnityEngine;

public class LoadingScenario : MonoBehaviour
{
    [SerializeField] private Progress progress;
    [SerializeField] private SceneNames nextScene;

    private void Awake()
    {
        SystemSetup();
    }

    private void SystemSetup()
    {
        Application.runInBackground = true;

        int width = Screen.width;
        int height = (int)(Screen.width * 16 / 9);
        Screen.SetResolution(width, height, true);

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        
        progress.Play(OnAfterProgress);
    }

    private void OnAfterProgress()
    {
        Utils.LoadScene(nextScene);
    }
}
