using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Achievement : MonoBehaviour
{
    public TextMeshProUGUI cash;
    public Bar achievementBar;
    public Image stage1;
    public Image stage2;
    public Image stage3;
    public Image stage4;
    public Sprite bronze;
    public Sprite silver;
    public Sprite gold;
    public Sprite diamond;
    private int _maxValue = 4000000;
    
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SaveLoadManager saveLoadManager = new SaveLoadManager();
        saveLoadManager.SetPath(Application.persistentDataPath + "/cash.json");
        int loadedCash = saveLoadManager.LoadInt();
        cash.text = loadedCash.ToString();
        SetAchievementBar(loadedCash);
    }
    
    public void SetAchievementBar(int currentValue)
    {
        if (currentValue < 0)
        {
            return;
        }
        achievementBar.SetValue(currentValue, _maxValue);
        if (currentValue >= _maxValue)
        {
            stage1.sprite = bronze;
            stage2.sprite = silver;
            stage3.sprite = gold;
            stage4.sprite = diamond;
        }
        else if (currentValue >= _maxValue * 0.75f)
        {
            stage1.sprite = bronze;
            stage2.sprite = silver;
            stage3.sprite = gold;
        }
        else if (currentValue >= _maxValue * 0.5f)
        {
            stage1.sprite = bronze;
            stage2.sprite = silver;
        }
        else if (currentValue >= _maxValue * 0.25f)
        {
            stage1.sprite = bronze;
        }
    }
}