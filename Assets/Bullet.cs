using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector2 _direction;
    private float _speed;
    public int armorDamage;
    public int bodyDamage;

    public void Init(Vector2 direction, float speed, int armorDmg, int bodyDmg)
    {
        _direction = direction;
        _speed = speed;
        armorDamage = armorDmg;
        bodyDamage = bodyDmg;
        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        
        Destroy(gameObject, 2f);
    }

    void Update()
    {
        transform.Translate(_direction * _speed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
