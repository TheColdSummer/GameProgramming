using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameBtn : MonoBehaviour
{
    void Start()
    {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(StartGame);
    }

    private void StartGame()
    {
        try
        {
            SceneManager.LoadScene("Scenes/Game");
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to load Game scene: " + e.Message);
        }
    }
}