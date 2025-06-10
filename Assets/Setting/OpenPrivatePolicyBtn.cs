using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenPrivatePolicyBtn : MonoBehaviour
{
    void Start()
    {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OpenPrivatePolicyUI);
    }

    private void OpenPrivatePolicyUI()
    {
        try
        {
            SceneManager.LoadScene("Scenes/PrivatePolicy");
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to load Private Policy scene: " + e.Message);
        }
    }
}