using UnityEngine;
using System.Collections;

public class ExtractionPoint : MonoBehaviour
{
    private Coroutine _extractCoroutine;
    private GameObject _eventSystem;
    private float _extractDuration = 10f;

    void Start()
    {
        _eventSystem = GameObject.Find("EventSystem");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _extractCoroutine = StartCoroutine(ExtractCountdown());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && _extractCoroutine != null)
        {
            StopCoroutine(_extractCoroutine);
            _extractCoroutine = null;
            Extract extract = _eventSystem.GetComponent<Extract>();
            if (extract != null)
            {
                extract.HideTime();
            }
        }
    }

    private IEnumerator ExtractCountdown()
    {
        float timeLeft = _extractDuration;
        Extract extract = _eventSystem.GetComponent<Extract>();
        while (timeLeft > 0f)
        {
            if (extract != null)
            {
                extract.SetExtractTime(timeLeft);
            }
            yield return null;
            timeLeft -= Time.deltaTime;
        }
        if (extract != null)
        {
            extract.SetExtractTime(0f);
            extract.Gameover(true);
        }
    }
}