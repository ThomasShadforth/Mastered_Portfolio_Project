using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    int modifier;
    public int diceNum;
    public int maxDamageVal;
    public bool destroyHitGround;

    public bool rotates;

    public bool destroyOverTime;
    public float destroyTime;
    float lifeTime;

    public Vector3 testVelocity;
    Rigidbody _rb;

    encumbranceStates playerState = encumbranceStates.normal;

    public bool offsetPosition;

    GameObject ownerObject;

    public LayerMask groundLayer;
    Attack _attack;
    HitUI damagePrefab;
    // Start is called before the first frame update
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        _attack = new Attack();
        
        //_rb.velocity = testVelocity;
        //transform.position = OffsetProjectileSpawn(ownerObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (destroyOverTime)
        {
            if(lifeTime < destroyTime)
            {
                lifeTime += GamePause.deltaTime;
            }
            else
            {
                if (transform.parent != null)
                {
                    Destroy(transform.parent.gameObject);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }

        if (rotates)
        {
            float angle = -(Mathf.Atan2(_rb.velocity.y, _rb.velocity.z) * Mathf.Rad2Deg);
            transform.rotation = Quaternion.Euler(angle, 0, 0);
        }
    }

    
    private void OnTriggerEnter(Collider other)
    {
        AIThinker AI = other.GetComponent<AIThinker>();
        PlayerController player = other.GetComponent<PlayerController>();

        //Debug.Log("Entity Entered");

        if(AI != null && ownerObject.GetComponent<PlayerController>())
        {
            Debug.Log("HIT");

            bool hasHit;

            HitUI hitText = HitTextObjectPool.instance.GetFromPool().GetComponent<HitUI>();

            if(playerState == encumbranceStates.normal)
            {
                //For now, factor in a normal roll for the normal state
                hasHit = _attack.AttackRoll(modifier, AI.stats.armourClass);
            }
            else
            {
                hasHit = _attack.AttackAdvDisadv(modifier, AI.stats.armourClass, false);
            }

            if (hasHit)
            {
                //
                int damage = _attack.DamageRoll(diceNum, maxDamageVal, modifier);
                //Debug.Log(damage);
                AI.healthSystem.Damage(damage);
                hitText.SetUITextAndPos($"Hit! \n {damage} damage!", other.transform.position);
            }
            else
            {
                hitText.SetUITextAndPos("Missed! Armour Class was too high for roll!", other.transform.position);
            }

            Destroy(gameObject);
        } else if(player != null && ownerObject.GetComponent<AIThinker>())
        {
            //Damage the player
            bool hasHit = false;

            //HitUI hitText = Instantiate(damagePrefab);

            HitUI hitText = HitTextObjectPool.instance.GetFromPool().GetComponent<HitUI>();

            hasHit = _attack.AttackRoll(modifier, player.GetPlayerStats().armourClass);

            if (hasHit)
            {
                int damage = _attack.DamageRoll(diceNum, maxDamageVal, modifier);
                Debug.Log(damage);
                player._healthSystem.Damage(damage);
                hitText.SetUITextAndPos($"Hit! \n {damage} damage!", other.transform.position);
            }
            else
            {
                hitText.SetUITextAndPos("Missed! Armour Class was too high for roll!", other.transform.position);
            }

            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        } else if(other.gameObject.layer == (1 << 7))
        {
            int groundLayer = 7;
            RaycastHit groundHit;

            int bitmask = (1 << groundLayer);

            bool hit = Physics.Raycast(transform.position, Vector3.down, out groundHit, .5f, bitmask);

            /*
            if (hit)
            {
                Destroy(gameObject);
            }*/

            
            
        }
    }

    public Vector3 OffsetProjectileSpawn(GameObject ownerObject)
    {
        Vector3 ownerPos = ownerObject.transform.position;
        Vector3 ownerDirection = ownerObject.transform.forward;
        Quaternion ownerRotation = ownerObject.transform.rotation;
        float offsetPos = 10;

        Vector3 spawnPos = ownerPos + ownerDirection * offsetPos;


        return spawnPos;
    }

    public void SetModifierAndRollValues(int modifierVal, int diceNum, int maxDamageVal, HitUI damageUI, GameObject ownerObject)
    {
        damagePrefab = damageUI;
        modifier = modifierVal;
        this.diceNum = diceNum;
        this.maxDamageVal = maxDamageVal;
        this.ownerObject = ownerObject;
        this.transform.forward = ownerObject.transform.forward;
        
        if(_rb != null)
        {
            
            _rb.velocity = ownerObject.transform.forward * 30f;
            _rb.velocity += new Vector3(0, 2, 0);
        }
        
    }

    public void SetEncumbranceState(encumbranceStates state)
    {
        playerState = state;
    }

    


}
