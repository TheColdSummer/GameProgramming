using UnityEngine;
using System;

public class EnemyCollisionRelay : MonoBehaviour
{
    public Action<Collision2D> onWallCollision;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            onWallCollision?.Invoke(collision);
        }
    }
}