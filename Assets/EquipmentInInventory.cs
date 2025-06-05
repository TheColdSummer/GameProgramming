using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentInInventory : MonoBehaviour
{
    public Item equipment;
    private GameObject _correspondingItemObject;
    public string type;
    public bool template = false;
    public static Sprite defaultImage;
    void OnEnable()
    {
        if (template)
        {
            defaultImage = GetComponentInChildren<Image>().GetComponent<Image>().sprite;
        }
    }

    public GameObject ChangeEquipment(GameObject newEquipment)
    {
        if (newEquipment == null)
        {
            Component image = GetComponentInChildren<UnityEngine.UI.Image>();
            if (image != null)
            {
                image.GetComponent<UnityEngine.UI.Image>().sprite = defaultImage;
            }

            GameObject ret = _correspondingItemObject;
            _correspondingItemObject = null;
            equipment = null;
            return ret;
        }
        
        Item item = newEquipment.GetComponent<Item>();
        if (item != null)
        {
            if (item.type != type)
            {
                Debug.Log("Cannot equip item: " + item.name + " to position: " + type);
                return null;
            }
            GameObject ret = _correspondingItemObject;
            _correspondingItemObject = newEquipment;
            newEquipment.transform.SetParent(transform, false);
            equipment = _correspondingItemObject.GetComponent<Item>();
            newEquipment.SetActive(false);
            Debug.Log("Equipped item: " + item.name + " to position: " + type);
            Component image = GetComponentInChildren<UnityEngine.UI.Image>();
            if (image != null)
            {
                image.GetComponent<UnityEngine.UI.Image>().sprite = item.sprite;
            }
            else
            {
                Debug.LogWarning("No Image component found in children to update sprite.");
            }
            return ret;
        }
        return null;
    }
}
