using UnityEngine;
using UnityEngine.UI;

public class ScrollToTop : MonoBehaviour
{
    private ScrollRect _scrollRect;

    void Start()
    {
        _scrollRect = GetComponent<ScrollRect>();
        if (_scrollRect == null)
        {
            Debug.LogError("ScrollRect component not found on this GameObject.");
            return;
        }
        _scrollRect.verticalNormalizedPosition = 1f;
    }
}
