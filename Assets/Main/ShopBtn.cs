using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopBtn : MonoBehaviour
{
    void Start()
    {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OpenShopUI);
    }

    private void OpenShopUI()
    {
        try
        {
            SceneManager.LoadScene("Scenes/Shop");
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to load Shop scene: " + e.Message);
        }
    }
}