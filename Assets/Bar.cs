using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    public Image foregroundImage;
    public int maxValue;
    public int currentValue;
    public GameObject text;

    public void SetValue(int c, int m)
    {
        if (m < 0)
        {
            Debug.LogError("Max health must be greater than or equal as zero.");
            return;
        }
        currentValue = c;
        maxValue = m;
        if (currentValue > maxValue)
        {
            currentValue = maxValue;
        }
        else if (currentValue < 0)
        {
            currentValue = 0;
        }
        UpdateBar();
    }
    
    public void UpdateValueDelta(int v)
    {
        currentValue += v;
        if (currentValue > maxValue)
        {
            currentValue = maxValue;
        }
        else if (currentValue < 0)
        {
            currentValue = 0;
        }
        UpdateBar();
    }

    private void UpdateBar()
    {
        foregroundImage.fillAmount = maxValue == 0 ? 0.0f : (float)currentValue / maxValue;
        if (text != null)
        {
            text.GetComponent<TMPro.TextMeshProUGUI>().text = $"{currentValue}/{maxValue}";
        }
    }
}
