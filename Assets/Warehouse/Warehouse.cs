using TMPro;
using UnityEngine;

public class Warehouse : MonoBehaviour
{
    public GameObject warehouseContent;
    public TextMeshProUGUI cashText;
    private int _cash;
    
    public int Cash
    {
        get => _cash;
        set
        {
            if (value < 0)
            {
                Debug.LogWarning("Cash cannot be negative. Setting to 0.");
                _cash = 0;
            }
            else
            {
                _cash = value;
            }
            if (cashText != null)
            {
                cashText.text = _cash.ToString();
            }
            else
            {
                Debug.LogError("Cash text UI element is not set.");
            }
        }
    }

    public void AddToWarehouse(Item item)
    {
        if (item == null)
        {
            Debug.LogError("Item is null. Cannot add to warehouse.");
            return;
        }
        if (warehouseContent == null)
        {
            Debug.LogError("Warehouse content is not set. Cannot add item to warehouse.");
            return;
        }

        SaveLoadManager manager = new SaveLoadManager();
        GameObject itemObject = manager.ConstructGameObjectFromItemData(manager.ToItemData(item));
        itemObject.transform.SetParent(warehouseContent.transform, false);
        itemObject.SetActive(true);
    }

    public void SellItem(GameObject item)
    {
        int price = item.GetComponent<Item>().GetSellPrice();
        Cash += (int)(price * 0.9f);    // 10% fee for selling
        Destroy(item);
    }
}