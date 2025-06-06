using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHp;
    public int currentHp;
    public Backpack backpack;
    public Weapon weapon;
    public ChestRig chestRig;
    public Helmet helmet;
    public BodyArmor bodyArmor;
    public EnemyWeaponControl weaponControl;
    public GameObject visualRange;
    public GameObject footStepRange;
    public GameObject gunFireRange;
    public GameObject attackRange;
    public GameObject containers;
    public GameObject question;
    public GameObject exclaimRed;
    public GameObject exclaimYellow;
    private static GameObject _player;
    private float _visualRadius;
    private float _attackRadius;
    private GameObject _idleRange;
    private FSM _fsm;

    // Start is called before the first frame update
    void Start()
    {
        InitEnemy();
        _fsm = GetComponent<FSM>();
    }

    private void InitEnemy()
    {
        if (_player == null)
        {
            _player = GameObject.Find("Player");
            if (_player == null)
            {
                Debug.LogError("Player object not found in the scene.");
            }
        }

        if (visualRange != null)
        {
            _visualRadius = visualRange.GetComponent<CircleCollider2D>().radius * visualRange.transform.lossyScale.x;
        }

        if (attackRange != null)
        {
            _attackRadius = attackRange.GetComponent<CircleCollider2D>().radius * attackRange.transform.lossyScale.x;
        }

        int p = Random.Range(0, 100);
        int level;
        if (p < 50)
        {
            level = 1;
        }
        else if (p < 80)
        {
            level = 2;
        }
        else if (p < 90)
        {
            level = 3;
        }
        else if (p < 95)
        {
            level = 4;
        }
        else
        {
            level = 5;
        }

        InitBackpack(backpack, level);
        InitChestRig(chestRig, level);
        InitHelmet(helmet, level);
        InitBodyArmor(bodyArmor, level);


        int w = Random.Range(1, 8);
        switch (w)
        {
            case 1:
                InitWeapon(weapon, "AKM");
                break;
            case 2:
                InitWeapon(weapon, "AWM");
                break;
            case 3:
                InitWeapon(weapon, "M4A1");
                break;
            case 4:
                InitWeapon(weapon, "M249");
                break;
            case 5:
                InitWeapon(weapon, "QBZ95");
                break;
            case 6:
                InitWeapon(weapon, "SCAR");
                break;
            case 7:
                InitWeapon(weapon, "SVD");
                break;
        }

        ChangeWeapon(weapon);
        if (containers == null)
        {
            containers = GameObject.Find("Map/Containers");
            if (containers == null)
            {
                Debug.LogError("Containers object not found in the scene.");
            }
        }
    }

    private void InitWeapon(Weapon w, string wName)
    {
        Weapon wm = Resources.Load<Weapon>("Weapon/" + wName);
        if (wm != null)
        {
            w.itemName = wm.itemName;
            w.type = wm.type;
            w.price = wm.price;
            w.size = wm.size;
            w.sprite = wm.sprite;
            w.id = wm.id;
            w.bodyDmg = wm.bodyDmg;
            w.ArmorDmg = wm.ArmorDmg;
            w.range = wm.range;
            w.RPM = wm.RPM;
            w.capacity = wm.capacity;
            w.reloadTime = wm.reloadTime;
            w.control = wm.control;
            w.mode = wm.mode;
            w.ammoType = wm.ammoType;
            w.currentAmmo = wm.currentAmmo;
        }
        else
        {
            Debug.LogError("Weapon not found: " + wName);
        }
    }

    private void InitBodyArmor(BodyArmor b, int level)
    {
        BodyArmor bm = Resources.Load<BodyArmor>("Armor/Armor" + level);
        if (bm != null)
        {
            b.itemName = bm.itemName;
            b.type = bm.type;
            b.price = bm.price;
            b.size = bm.size;
            b.sprite = bm.sprite;
            b.id = bm.id;
            b.durability = bm.durability;
            b.maxDurability = bm.maxDurability;
        }
        else
        {
            Debug.LogError("BodyArmor not found for level " + level);
        }
    }

    private void InitHelmet(Helmet h, int level)
    {
        Helmet hm = Resources.Load<Helmet>("Helmet/Helmet" + level);
        if (hm != null)
        {
            h.itemName = hm.itemName;
            h.type = hm.type;
            h.price = hm.price;
            h.size = hm.size;
            h.sprite = hm.sprite;
            h.id = hm.id;
            h.durability = hm.durability;
            h.maxDurability = hm.maxDurability;
        }
        else
        {
            Debug.LogError("Helmet not found for level " + level);
        }
    }

    private void InitChestRig(ChestRig c, int level)
    {
        ChestRig cm = Resources.Load<ChestRig>("ChestRig/ChestRig" + level);
        if (cm != null)
        {
            c.itemName = cm.itemName;
            c.type = cm.type;
            c.price = cm.price;
            c.size = cm.size;
            c.sprite = cm.sprite;
            c.id = cm.id;
            c.innerSize = cm.innerSize;
        }
        else
        {
            Debug.LogError("ChestRig not found for level " + level);
        }
    }

    private void InitBackpack(Backpack b, int level)
    {
        Backpack bm = Resources.Load<Backpack>("Backpack/Backpack" + level);

        if (bm != null)
        {
            b.itemName = bm.itemName;
            b.type = bm.type;
            b.price = bm.price;
            b.size = bm.size;
            b.sprite = bm.sprite;
            b.id = bm.id;
            b.innerSize = bm.innerSize;
        }
        else
        {
            Debug.LogError("Backpack not found for level " + level);
        }
    }

    void Update()
    {
        if (_player == null || visualRange == null || _fsm == null)
            return;

        float dist = Vector2.Distance(transform.position, _player.transform.position);

        if (dist > _visualRadius && _fsm.CurrentState() != StateType.Idle && _fsm.CurrentState() != StateType.Alert)
        {
            _fsm.TransitionToState(StateType.Idle);
        }
    }

    public void CauseHeadDamage(int bulletArmorDamage, int bulletBodyDamage)
    {
        if (helmet != null)
        {
            if (helmet.durability > 0)
            {
                helmet.ChangeDurabilityDelta(-bulletArmorDamage);
                ChangeHealthDelta((int)(-bulletBodyDamage * 1.5 / 10));
            }
            else
            {
                ChangeHealthDelta((int)(-bulletBodyDamage * 1.5));
            }
        }
        else
        {
            ChangeHealthDelta((int)(-bulletBodyDamage * 1.5));
        }
    }

    public void CauseBodyDamage(int bulletArmorDamage, int bulletBodyDamage)
    {
        if (bodyArmor != null)
        {
            if (bodyArmor.durability > 0)
            {
                bodyArmor.ChangeDurabilityDelta(-bulletArmorDamage);
                ChangeHealthDelta((int)(-bulletBodyDamage * 1.5 / 10));
            }
            else
            {
                ChangeHealthDelta(-bulletBodyDamage);
            }
        }
        else
        {
            ChangeHealthDelta(-bulletBodyDamage);
        }
    }

    public void ChangeHealthDelta(int delta)
    {
        currentHp += delta;
        if (currentHp <= 0)
        {
            currentHp = 0;
            Die();
        }

        if (currentHp > maxHp)
        {
            currentHp = maxHp;
        }
    }

    public void ChangeWeapon(Item equipment)
    {
        if (equipment is Weapon weaponItem)
        {
            weaponControl.ChangeWeapon(weaponItem);
        }
        else
        {
            Debug.LogError("Provided item is not a Weapon.");
        }
    }

    public void Die()
    {
        Destroy(weaponControl);
        Destroy(gameObject.GetComponent<HitDetector>());
        Destroy(gameObject.GetComponent<Rigidbody2D>());
        Destroy(gameObject.GetComponent<BoxCollider2D>());
        Destroy(visualRange);
        Destroy(footStepRange);
        Destroy(gunFireRange);
        Destroy(attackRange);
        Destroy(_idleRange);

        FSM fsm = gameObject.GetComponent<FSM>();
        fsm.TransitionToState(StateType.Die);

        transform.Rotate(0, 0, 90);

        GameObject container = Instantiate(Resources.Load<GameObject>("Container"), containers.transform);
        container.transform.position = transform.position;
        Container containerScript = container.GetComponent<Container>();
        if (containerScript == null)
        {
            Debug.LogError("Container script not found on the instantiated container.");
            return;
        }

        List<Item> items = new List<Item>();
        if (backpack != null)
        {
            items.Add(backpack);
        }

        if (chestRig != null)
        {
            items.Add(chestRig);
        }

        if (helmet != null)
        {
            items.Add(helmet);
        }

        if (bodyArmor != null)
        {
            items.Add(bodyArmor);
        }

        if (weapon != null)
        {
            weapon.currentAmmo = Random.Range(0, weapon.capacity);
            items.Add(weapon);
        }

        containerScript.AddItems(items);
        
        Destroy(this);
    }

    public void SetIdleRange(GameObject idleRangeObject)
    {
        if (idleRangeObject == null)
        {
            Debug.LogError("Idle range object is null. Cannot set idle range.");
            return;
        }

        _idleRange = idleRangeObject;
        _idleRange.SetActive(true);
    }

    public GameObject GetIdleRange()
    {
        if (_idleRange == null)
        {
            Debug.LogError("Idle range is not set. Returning null.");
            return null;
        }

        return _idleRange;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Vector2 enemyPos = transform.position;
            Vector2 playerPos = other.transform.position;
            if (visualRange == null)
            {
                Debug.LogError("Visual range is not set. Cannot check visibility.");
                return;
            }

            if (visualRange.GetComponent<CircleCollider2D>() == null)
            {
                Debug.LogError("Visual range collider is not set. Cannot check visibility.");
                return;
            }

            float vDist = Vector2.Distance(enemyPos, playerPos);
            if (vDist > _visualRadius)
            {
                return;
            }

            RaycastHit2D hit = Physics2D.Linecast(enemyPos, playerPos, LayerMask.GetMask("Wall"));
            if (hit.collider == null || !hit.collider.CompareTag("Wall"))
            {
                StateType currentState = _fsm.CurrentState();
                if (attackRange != null)
                {
                    float dist = Vector2.Distance(enemyPos, playerPos);

                    if (dist <= _attackRadius &&
                        (currentState == StateType.Idle || currentState == StateType.Alert ||
                         currentState == StateType.Chase))
                    {
                        _fsm.TransitionToState(StateType.Attack);
                    }
                    else if (dist > _attackRadius * 1.8f &&
                             (currentState == StateType.Idle || currentState == StateType.Alert ||
                              currentState == StateType.Attack))
                    {
                        _fsm.TransitionToState(StateType.Chase);
                    }
                }
            }
            else if (hit.collider.CompareTag("Wall"))
            {
                if (_fsm.CurrentState() != StateType.Alert && _fsm.CurrentState() != StateType.Idle)
                {
                    _fsm.TransitionToState(StateType.Idle);
                }
            }
        }
    }

    public void NoQuestion()
    {
        question.SetActive(false);
        exclaimRed.SetActive(false);
        exclaimYellow.SetActive(false);
    }

    public void Question()
    {
        question.SetActive(true);
        exclaimRed.SetActive(false);
        exclaimYellow.SetActive(false);
    }

    public void ExclaimRed()
    {
        question.SetActive(false);
        exclaimRed.SetActive(true);
        exclaimYellow.SetActive(false);
    }

    public void ExclaimYellow()
    {
        question.SetActive(false);
        exclaimRed.SetActive(false);
        exclaimYellow.SetActive(true);
    }
}