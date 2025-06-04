using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathingEffect : MonoBehaviour
{
    private bool _isPlayerNearby = false;
    private SpriteRenderer _spriteRenderer;
    private Coroutine _breathingCoroutine;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        ResetTransparency();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerNearby = true;
            if (_breathingCoroutine == null)
            {
                _breathingCoroutine = StartCoroutine(Breathing());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerNearby = false;
            if (_breathingCoroutine != null)
            {
                StopCoroutine(_breathingCoroutine);
                _breathingCoroutine = null;
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
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, alpha);
            yield return null;
        }
    }

    private void ResetTransparency()
    {
        _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 0.0f);
    }
}
