using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RuleBtn : MonoBehaviour
{
    void Start()
    {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OpenRuleUI);
    }

    private void OpenRuleUI()
    {
        try
        {
            SceneManager.LoadScene("Scenes/Rule");
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to load Rule scene: " + e.Message);
        }
    }
}