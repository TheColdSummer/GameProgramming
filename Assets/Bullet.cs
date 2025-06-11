using UnityEngine;

/*
 * This bullet class handles the behavior of bullets in the game.
 */
public class Bullet : MonoBehaviour
{
    private Vector2 _direction;
    private float _speed;
    public int armorDamage;
    public int bodyDamage;
    private Vector2 _lastPosition;

    private void Start()
    {
        _lastPosition = transform.position;
    }

    public void Init(Vector2 direction, float speed, int armorDmg, int bodyDmg, float flyTime)
    {
        _direction = direction;
        _speed = speed;
        armorDamage = armorDmg;
        bodyDamage = bodyDmg;
        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        Destroy(gameObject, flyTime);
    }

    void Update()
    {
        Vector2 currentPosition = transform.position;

        // the following code checks for collisions with characters and enemies and applies hit compensation if necessary
        RaycastHit2D[] hits = Physics2D.LinecastAll(_lastPosition, currentPosition, LayerMask.GetMask("Character", "Enemy"));
        foreach (var hit in hits)
        {
            if (hit.collider == null) continue;
            if (hit.collider.CompareTag("PlayerBody") && CompareTag("EnemyBullet"))
            {
                if (hit.collider.name == "Head" || hit.collider.name == "Body")
                {
                    HitDetector detector = hit.collider.transform.root.GetComponent<HitDetector>();
                    if (detector != null)
                    {
                        detector.HitCompensation(this, hit.collider.name, "Player");
                        Destroy(gameObject);
                        return;
                    }
                }
            }
            else if (hit.collider.CompareTag("EnemyBody") && CompareTag("Bullet"))
            {
                if (hit.collider.name == "Head" || hit.collider.name == "Body")
                {
                    HitDetector detector = hit.collider.transform.parent.GetComponent<HitDetector>();
                    if (detector != null)
                    {
                        detector.HitCompensation(this, hit.collider.name, "Enemy");
                        Destroy(gameObject);
                        return;
                    }
                }
            }
        }

        transform.Translate(_direction * _speed * Time.deltaTime, Space.World);
        _lastPosition = currentPosition;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}