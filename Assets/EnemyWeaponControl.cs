using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponControl : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public Transform directionPoint;
    public SpriteRenderer spriteRenderer;
    private Weapon _curWeapon;
    private float _lastFireTime;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Vector2 mousePosition = Input.mousePosition;
        // Vector3 mouseScreenPosition = new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane);
        // Vector3 screenPosition = characterCamera.WorldToScreenPoint(transform.position);
        //
        // Vector3 direction = screenPosition - mouseScreenPosition;
        //
        // direction.z = 0;
        //
        // float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // if (angle < -90 || angle > 90)
        // {
        //     Vector2 scale = transform.localScale;
        //     scale.x = -Mathf.Abs(scale.x);
        //     transform.localScale = scale;
        //     transform.rotation = Quaternion.Euler(0, 0, 180 + angle);
        // }
        // else
        // {
        //     Vector2 scale = transform.localScale;
        //     scale.x = Mathf.Abs(scale.x);
        //     transform.localScale = scale;
        //     transform.rotation = Quaternion.Euler(0, 0, angle);
        // }
        //
        // if (_curWeapon != null)
        // {
        //     if (_curWeapon.mode == 0)
        //     {
        //         if (Input.GetMouseButtonDown(0) && Time.time - _lastFireTime >= (float)60 / _curWeapon.RPM)
        //         {
        //             Fire();
        //             _lastFireTime = Time.time;
        //         }
        //     }
        //     else if (_curWeapon.mode == 1)
        //     {
        //         if (Input.GetMouseButton(0) && Time.time - _lastFireTime >= (float)60 / _curWeapon.RPM)
        //         {
        //             Fire();
        //             _lastFireTime = Time.time;
        //         }
        //     }
        // }
    }
    
    void Fire()
    {
        Vector2 direction = (directionPoint.position - firePoint.position).normalized;
        float maxAngle = Mathf.Lerp(5f, 0f, Mathf.Clamp01(_curWeapon.control / 100f));
        float randomAngle = Random.Range(-maxAngle, maxAngle);
        direction = (Quaternion.Euler(0, 0, randomAngle) * direction).normalized;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.Init(direction, 100, _curWeapon.ArmorDmg, _curWeapon.bodyDmg);
    }

    public void ChangeWeapon(Weapon weapon)
    {
        if (weapon == null)
        {
            Debug.LogError("Weapon is null. Cannot change weapon.");
            return;
        }
        if (!_curWeapon || _curWeapon.itemName != weapon.itemName)
        {
            _curWeapon = weapon;
            if (spriteRenderer != null)
            {
                spriteRenderer.GetComponent<SpriteRenderer>().sprite = weapon.sprite;
            }
            else
            {
                Debug.LogWarning("SpriteRenderer component not found on the weapon object.");
            }
            Debug.Log("Weapon changed to: " + _curWeapon);
        }
    }
}
