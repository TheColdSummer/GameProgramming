using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyBullet") && gameObject.CompareTag("Player"))
        {
            Bullet bullet = other.GetComponent<Bullet>();
            if (other.gameObject.GetComponent<Collider2D>().IsTouching(transform.Find("Head").GetComponent<Collider2D>()))
            {
                Destroy(other.gameObject);
                gameObject.GetComponent<Player>().CauseHeadDamage(bullet.armorDamage, bullet.bodyDamage);
            }
            else if (other.gameObject.GetComponent<Collider2D>().IsTouching(transform.Find("Body").GetComponent<Collider2D>()))
            {
                Destroy(other.gameObject);
                gameObject.GetComponent<Player>().CauseBodyDamage(bullet.armorDamage, bullet.bodyDamage);
            }
        }
        else if (other.CompareTag("Bullet") && gameObject.CompareTag("Enemy"))
        {
            Bullet bullet = other.GetComponent<Bullet>();
            if (other.gameObject.GetComponent<Collider2D>().IsTouching(transform.Find("Head").GetComponent<Collider2D>()))
            {
                Destroy(other.gameObject);
                gameObject.GetComponent<Enemy>().CauseHeadDamage(bullet.armorDamage, bullet.bodyDamage);
            }
            else if (other.gameObject.GetComponent<Collider2D>().IsTouching(transform.Find("Body").GetComponent<Collider2D>()))
            {
                Destroy(other.gameObject);
                gameObject.GetComponent<Enemy>().CauseBodyDamage(bullet.armorDamage, bullet.bodyDamage);
            }
        }
    }
}
