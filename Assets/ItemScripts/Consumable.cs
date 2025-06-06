using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Consumable : Item
{
    public int useTime;
    protected IUseStrategy UseStrategy;

    void Start()
    {
        base.Start();

    }
    
    public void Use(Player player)
    {
        bool used = UseStrategy != null && UseStrategy.Use(player, this);
        if (used)
        {
            Debug.Log("Item used: " + itemName);
        }
        else
        {
            Debug.Log("Failed to use item: " + itemName);
        }
    }
    
    public void UseAnimation()
    {
        Destroy(gameObject.GetComponent<HorizontalLayoutGroup>());
        GameObject useAnimation = Instantiate(Resources.Load<GameObject>("LoadingAnimation"), gameObject.transform);

        RectTransform rectTransform = useAnimation.GetComponent<RectTransform>();
        float parentWidth = gameObject.transform.parent.GetComponent<RectTransform>().rect.width;
        if (rectTransform != null)
        {
            Vector2 offsetMax = rectTransform.offsetMax;
            offsetMax.x = parentWidth;
            rectTransform.offsetMax = offsetMax;
        }
        
        useAnimation.SetActive(true);
    }
    
    public override string GetSpecificDescription()
    {
        return base.GetSpecificDescription() + "\n" +
               "Use Time: " + useTime + " seconds";
    }
}
