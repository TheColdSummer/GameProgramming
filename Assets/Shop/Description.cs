using UnityEngine;
using UnityEngine.UI;

public class Description : MonoBehaviour
{
    public GameObject descriptionPanel;

    void Start()
    {
        if (descriptionPanel == null)
        {
            Debug.LogError("No description panel found");
            return;
        }

        descriptionPanel.transform.Find("Button").GetComponent<Button>().onClick.AddListener(() =>
        {
            descriptionPanel.SetActive(false);
            descriptionPanel.transform.Find("Text").GetComponent<TMPro.TextMeshProUGUI>().text = string.Empty;
        });
        Commodity.SetDescriptionPanel(descriptionPanel);
    }
}