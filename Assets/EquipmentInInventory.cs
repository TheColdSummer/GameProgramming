using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentInInventory : MonoBehaviour
{
    public Item equipment;
    private GameObject _correspondingItemObject;
    public string type;

    public static Sprite defaultImage;
    // Start is called before the first frame update
    void Start()
    {
        _correspondingItemObject = null;
        equipment = null;
        defaultImage = GetComponentInChildren<UnityEngine.UI.Image>().GetComponent<UnityEngine.UI.Image>().sprite;
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
