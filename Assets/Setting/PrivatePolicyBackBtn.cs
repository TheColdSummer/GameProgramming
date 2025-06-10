using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrivatePolicyBackBtn : MonoBehaviour
{
    void Start()
    {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OpenSettingUI);
    }

    private void OpenSettingUI()
    {
        try
        {
            SceneManager.LoadScene("Scenes/Setting");
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to load Setting scene: " + e.Message);
        }
    }
}