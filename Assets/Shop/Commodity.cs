using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Commodity : MonoBehaviour
{
    public Button buyButton;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI priceText;
    public Image image;
    private Item _item;
    private static Warehouse _warehouse;
    private static SaveManager _saveManager;
    private static GameObject _descriptionPanel;
    private static TextMeshProUGUI _descriptionText;

    void Start()
    {
        Button button = gameObject.GetComponent<Button>();
        
        button.onClick.AddListener(ShowDescription);
    }

    private void ShowDescription()
    {
        if (_descriptionText != null)
        {
            _descriptionText.text = _item.GetSpecificDescription();
            _descriptionPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("Description text is not set.");
        }
    }

    public void Init(Item item)
    {
        _item = item;
        nameText.text = item.itemName;
        priceText.text = "Price: " + item.price;
        image.sprite = item.sprite;

        buyButton.onClick.AddListener(BuyItem);
    }

    public static void SetWarehouse(Warehouse warehouse)
    {
        Debug.Log("Setting warehouse: " + warehouse);
        _warehouse = warehouse;
    }
    
    public static void SetSaveManager(SaveManager saveManager)
    {
        _saveManager = saveManager;
    }

    private void BuyItem()
    {
        if (_warehouse == null)
        {
            Debug.LogError("Warehouse is not set. Cannot buy item.");
            return;
        }

        int cash = _warehouse.Cash;
        if (cash < _item.price)
        {
            Debug.LogError("Not enough cash to buy " + _item.itemName);
            return;
        }
        _warehouse.Cash -= _item.price;
        _warehouse.AddToWarehouse(_item);
        _saveManager.SaveWarehouse();
    }

    public static void SetDescriptionPanel(GameObject descriptionPanel)
    {
        _descriptionPanel = descriptionPanel;
        _descriptionText = descriptionPanel.transform.Find("Text").GetComponent<TextMeshProUGUI>();
    }
}
