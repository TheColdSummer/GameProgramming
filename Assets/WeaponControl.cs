using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class WeaponControl : MonoBehaviour
{
    public Camera characterCamera;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public Transform directionPoint;
    public SpriteRenderer spriteRenderer;
    public Image weaponImage;
    public TextMeshProUGUI weaponCurAmmo;
    public TextMeshProUGUI weaponMaxAmmo;
    public GameObject playerInventoryUI;
    private Weapon _curWeapon;
    private float _lastFireTime = -1f;
    private bool _reloading;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector3 mouseScreenPosition = new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane);
        Vector3 screenPosition = characterCamera.WorldToScreenPoint(transform.position);

        Vector3 direction = screenPosition - mouseScreenPosition;

        direction.z = 0;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle < -90 || angle > 90)
        {
            Vector2 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
            transform.rotation = Quaternion.Euler(0, 0, 180 + angle);
        }
        else
        {
            Vector2 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        if (playerInventoryUI == null)
        {
            return;
        }
        if (!playerInventoryUI.activeSelf && _curWeapon != null && !_reloading)
        {
            if (_curWeapon.mode == 0)
            {
                if (Input.GetMouseButtonDown(0) && Time.time - _lastFireTime >= (float)60 / _curWeapon.RPM)
                {
                    Fire();
                    _lastFireTime = Time.time;
                }
            }
            else if (_curWeapon.mode == 1)
            {
                if (Input.GetMouseButton(0) && Time.time - _lastFireTime >= (float)60 / _curWeapon.RPM)
                {
                    Fire();
                    _lastFireTime = Time.time;
                }
            }
        }
    }
    
    void Fire()
    {
        // check if the weapon has ammo in the magazine
        if (_curWeapon.currentAmmo <= 0)
        {
            // need reload
            return;
        }
        Vector2 direction = (directionPoint.position - firePoint.position).normalized;
        float maxAngle = Mathf.Lerp(15f, 0f, Mathf.Clamp01(_curWeapon.control / 100f));
        float randomAngle = Random.Range(-maxAngle, maxAngle);
        direction = (Quaternion.Euler(0, 0, randomAngle) * direction).normalized;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.Init(direction, 100, _curWeapon.ArmorDmg, _curWeapon.bodyDmg);
        WeaponFired();
    }

    public void ChangeWeapon(Weapon weapon)
    {
        if (weapon == null)
        {
            _curWeapon = null;
            if (spriteRenderer != null)
            {
                spriteRenderer.GetComponent<SpriteRenderer>().sprite = null;
                UpdateWeaponDisplay(null);
            }
            else
            {
                Debug.LogWarning("SpriteRenderer component not found on the weapon object.");
            }
            return;
        }
        if (!_curWeapon || _curWeapon.itemName != weapon.itemName)
        {
            _curWeapon = weapon;
            if (spriteRenderer != null)
            {
                spriteRenderer.GetComponent<SpriteRenderer>().sprite = weapon.sprite;
                UpdateWeaponDisplay(weapon);
            }
            else
            {
                Debug.LogWarning("SpriteRenderer component not found on the weapon object.");
            }
            Debug.Log("Weapon changed to: " + _curWeapon);
        }
    }

    public void UpdateWeaponDisplay(Weapon weapon)
    {
        if (weapon == null)
        {
            weaponImage.sprite = Resources.Load<Sprite>("Transparent");
            weaponCurAmmo.text = "0";
            weaponMaxAmmo.text = "0";
            return;
        }
        weaponImage.sprite = weapon.sprite;
        weaponCurAmmo.text = weapon.currentAmmo.ToString();
        weaponMaxAmmo.text = weapon.capacity.ToString();
    }

    public void WeaponFired()
    {
        if (_curWeapon == null)
        {
            Debug.LogError("Current weapon is null. Cannot update ammo.");
            return;
        }
        
        // Decrease current ammo by 1
        _curWeapon.currentAmmo--;
        
        // Update the UI
        weaponCurAmmo.text = _curWeapon.currentAmmo.ToString();
    }

    public void ReLoad(int ammo)
    {
        if (_curWeapon == null)
        {
            Debug.LogError("Current weapon is null. Cannot reload.");
            return;
        }
        
        if (ammo + _curWeapon.currentAmmo > _curWeapon.capacity)
        {
            ammo = _curWeapon.capacity - _curWeapon.currentAmmo;
        }
        
        _curWeapon.currentAmmo += ammo;
        
        weaponCurAmmo.text = _curWeapon.currentAmmo.ToString();
    }

    public void SetReloading(bool b)
    {
        _reloading = b;
    }
    
    public float GetLastFireTime()
    {
        return _lastFireTime;
    }
}