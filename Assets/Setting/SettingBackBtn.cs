using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingBackBtn : MonoBehaviour
{
    void Start()
    {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OpenMainUI);
    }

    private void OpenMainUI()
    {
        try
        {
            SceneManager.LoadScene("Scenes/Main");
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to load Main scene: " + e.Message);
        }
    }
}