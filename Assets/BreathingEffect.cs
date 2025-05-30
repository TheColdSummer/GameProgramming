using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathingEffect : MonoBehaviour
{
    private bool isPlayerNearby = false;
    private SpriteRenderer spriteRenderer;
    private Coroutine breathingCoroutine;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            if (breathingCoroutine == null)
            {
                breathingCoroutine = StartCoroutine(Breathing());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            if (breathingCoroutine != null)
            {
                StopCoroutine(breathingCoroutine);
                breathingCoroutine = null;
                ResetTransparency();
            }
        }
    }

    private IEnumerator Breathing()
    {
        float alpha = 0.5f;
        float speed = 0.5f;
        while (true)
        {
            alpha = Mathf.PingPong(Time.time * speed, 1.0f);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
            yield return null;
        }
    }

    private void ResetTransparency()
    {
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.0f);
    }
}
