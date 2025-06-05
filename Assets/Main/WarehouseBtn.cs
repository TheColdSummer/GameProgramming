using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WarehouseBtn : MonoBehaviour
{
    void Start()
    {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OpenWarehouseUI);
    }

    private void OpenWarehouseUI()
    {
        try
        {
            SceneManager.LoadScene("Scenes/Warehouse");
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to load Warehouse scene: " + e.Message);
        }
    }
}