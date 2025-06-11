using UnityEngine;

/*
 * This script is responsible for detecting hits from bullets and applying damage to the player or enemy.
 */
public class HitDetector : MonoBehaviour
{
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

    /*
     * This method is used to apply hit compensation for bullets that hit the player or enemy.
     */
    public void HitCompensation(Bullet bullet, string colliderName, string target)
    {
        if (colliderName == "Head")
        {
            if (target == "Enemy")
                gameObject.GetComponent<Enemy>().CauseHeadDamage(bullet.armorDamage, bullet.bodyDamage);
            else if (target == "Player")
                gameObject.GetComponent<Player>().CauseHeadDamage(bullet.armorDamage, bullet.bodyDamage);
        }
        else if (colliderName == "Body")
        {
            if (target == "Enemy")
                gameObject.GetComponent<Enemy>().CauseBodyDamage(bullet.armorDamage, bullet.bodyDamage);
            else if (target == "Player")
                gameObject.GetComponent<Player>().CauseBodyDamage(bullet.armorDamage, bullet.bodyDamage);
        }
    }
}
