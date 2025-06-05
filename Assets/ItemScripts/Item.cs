using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public string itemName;
    public string type;
    public int price;
    public int size;
    public Sprite sprite;
    public int id;

    public void Start()
    {
        RefreshItemUI();
    }

    public void RefreshItemUI()
    {
        Transform imageTransform = transform.Find("Image");
        if (imageTransform != null)
        {
            UnityEngine.UI.Image image = imageTransform.GetComponent<UnityEngine.UI.Image>();
            if (image != null)
            {
                image.sprite = sprite;
            }
        }

        Transform textTransform = transform.Find("Text");
        if (textTransform != null)
        {
            Transform nameTransform = textTransform.Find("Name");
            if (nameTransform != null)
            {
                nameTransform.GetComponent<TMPro.TextMeshProUGUI>().text = itemName;
            }
            else
            {
                Debug.LogError("Name object not found in the Text GameObject.");
            }
            Transform sizeTransform = textTransform.Find("Size");
            if (sizeTransform != null)
            {
                sizeTransform.GetComponent<TMPro.TextMeshProUGUI>().text = "Size: " + size;
            }
            else
            {
                Debug.LogError("Size object not found in the Text GameObject.");
            }
            Transform priceTransform = textTransform.Find("Price");
            if (priceTransform != null)
            {
                priceTransform.GetComponent<TMPro.TextMeshProUGUI>().text = "Price: " + price;
            }
            else
            {
                Debug.LogError("Price object not found in the Text GameObject.");
            }
        }
    }

    public virtual string GetSpecificDescription()
    {
        return $"Item Name: {itemName}\nType: {type}\nPrice: {price}\nSize: {size}";
    }
    
    public virtual int GetSellPrice()
    {
        return price;
    }
}
