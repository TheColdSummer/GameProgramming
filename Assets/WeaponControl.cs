using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
 * This script controls the weapon system in a game, allowing players to fire weapons, reload, and manage weapon display.
 */
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
    public AudioSource audioSource;
    public AudioClip autoFireClip;
    public AudioClip singleFireClip;
    public AudioClip boltActionClip;
    public AudioClip emptyMagClip;
    public AudioClip reloadClip;
    private Weapon _curWeapon;
    private float _lastFireTime = -1f;
    private bool _reloading;

    void Update()
    {
        AdjustAimDirection();
        LeftClickToFire();
    }

    /*
     * This method handles the left mouse button click to fire the weapon.
     */
    private void LeftClickToFire()
    {
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
                }
            }
            else if (_curWeapon.mode == 1)
            {
                if (Input.GetMouseButton(0) && Time.time - _lastFireTime >= (float)60 / _curWeapon.RPM)
                {
                    Fire();
                }
            }
        }
    }

    /*
     * This method adjusts the aim direction based on the mouse position.
     */
    private void AdjustAimDirection()
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
    }

    /*
     * This method fires the weapon, creating a bullet and playing the appropriate sound effects.
     */
    void Fire()
    {
        // check if the weapon has ammo in the magazine
        if (_curWeapon.currentAmmo <= 0)
        {
            // need reload
            if (SoundSetting.SoundEffectEnabled && Input.GetMouseButtonDown(0) && audioSource != null && emptyMagClip != null)
            {
                audioSource.PlayOneShot(emptyMagClip);
            }
            return;
        }
        Vector2 direction = (directionPoint.position - firePoint.position).normalized;
        float maxAngle = Mathf.Lerp(15f, 0f, Mathf.Clamp01(_curWeapon.control / 100f));
        float randomAngle = Random.Range(-maxAngle, maxAngle);
        direction = (Quaternion.Euler(0, 0, randomAngle) * direction).normalized;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.Init(direction, 100, _curWeapon.ArmorDmg, _curWeapon.bodyDmg, _curWeapon.range * 0.004f);
        _lastFireTime = Time.time;
        
        if (_curWeapon.mode == 1)
        {
            if (SoundSetting.SoundEffectEnabled && autoFireClip != null && audioSource != null)
                audioSource.PlayOneShot(autoFireClip);
        }
        else if (_curWeapon.mode == 0)
        {
            if (SoundSetting.SoundEffectEnabled && singleFireClip != null && audioSource != null)
                audioSource.PlayOneShot(singleFireClip);
        }

        if (_curWeapon.itemName == "AWM" && boltActionClip != null && audioSource != null)
        {
            float delay = 0f;
            if (_curWeapon.mode == 1 && autoFireClip != null)
                delay = autoFireClip.length;
            else if (_curWeapon.mode == 0 && singleFireClip != null)
                delay = singleFireClip.length;

            StartCoroutine(PlayBoltActionAfterDelay(delay));
        }
        
        WeaponFired();
    }
    
    /*
     * This coroutine plays the bolt action sound after a specified delay.
     */
    private IEnumerator PlayBoltActionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (SoundSetting.SoundEffectEnabled && boltActionClip != null && audioSource != null)
            audioSource.PlayOneShot(boltActionClip);
    }

    /*
     * This method changes the current weapon to the specified weapon.
     * If the weapon is null, it clears the current weapon and updates the display accordingly.
     */
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
        if (!_curWeapon || _curWeapon != weapon)
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

    /*
     * This method updates the weapon display with the current weapon's information.
     * If the weapon is null, it sets the display to show zero ammo.
     */
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

    /*
     * This method returns the current weapon.
     */
    public void WeaponFired()
    {
        if (_curWeapon == null)
        {
            Debug.LogError("Current weapon is null. Cannot update ammo.");
            return;
        }
        
        _curWeapon.currentAmmo--;
        
        weaponCurAmmo.text = _curWeapon.currentAmmo.ToString();
    }

    /*
     * This method reloads the current weapon with the specified amount of ammo.
     */
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

    public void PlayReloadAudio()
    {
        if (SoundSetting.SoundEffectEnabled && audioSource != null && reloadClip != null)
        {
            audioSource.PlayOneShot(reloadClip);
        }
    }
}