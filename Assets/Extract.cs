using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Extract : MonoBehaviour
{
    public GameObject gameoverObj;
    public GameObject successObj;
    public GameObject failObj;
    public GameObject extractTimerObj;
    public TextMeshProUGUI extractTime;
    public Button quitBtn;
    
    public void Gameover(bool success)
    {
        if (success)
        {
            if (gameoverObj != null)
            {
                gameoverObj.SetActive(true);
                successObj.SetActive(true);
                failObj.SetActive(false);
            }
        }
        else
        {
            if (gameoverObj != null)
            {
                gameoverObj.SetActive(true);
                failObj.SetActive(true);
                successObj.SetActive(false);
            }
        }
        
        Time.timeScale = 0f;

        if (quitBtn != null)
        {
            quitBtn.onClick.RemoveAllListeners();
            quitBtn.onClick.AddListener(OnQuitButtonClicked);
        }
    }
    
    private void OnQuitButtonClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Scenes/Main");  // Switch to the main scene
    }
    
    public void SetExtractTime(float time)
    {
        if (extractTime != null)
        {
            extractTimerObj.SetActive(true);
            extractTime.text = time.ToString("F2") + " s";
        }
        else
        {
            Debug.LogError("Extract Time TextMeshProUGUI is not assigned.");
        }
    }
    
    public void HideTime()
    {
        if (extractTime != null)
        {
            extractTimerObj.SetActive(false);
        }
        else
        {
            Debug.LogError("Extract Time TextMeshProUGUI is not assigned.");
        }
    }
}