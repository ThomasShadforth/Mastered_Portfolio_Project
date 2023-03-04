using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

public class CombatAnimator : MonoBehaviour, IObserver
{
    public GameObject[] projectileReferences;
    public Subject ownerSubject;



    public GameObject loadedProjectile;

    public HitUI hitPrefab;

    public float applyMoveVelo = 2.5f;

    [SerializeField] Transform projectileSpawnPos;

    bool applyingMove;

    int setModifier;
    int setDiceNum;
    int maxDamage;

    Attack _attack;

    private void OnEnable()
    {
        ownerSubject.AddObserver(this);
    }

    private void OnDisable()
    {
        ownerSubject.RemoveObserver(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        _attack = new Attack();

        //TriggerAttack<PlayerController>(FindObjectOfType<PlayerController>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnProjectile()
    {
        //refactoring: instead of using getcomponent all the time, simply run getcomponent twice - on the object itself, and to check for the component in children
        //If it exists on the object itself, skip the second with a bool/using the projBase variable in a check

        GameObject projectile = Instantiate(loadedProjectile);

        ProjectileBase projBase = projectile.GetComponent<ProjectileBase>();

        if(projBase == null)
        {
            projBase = projectile.GetComponentInChildren<ProjectileBase>();
        }

        if(projBase != null)
        {
            projBase.SetModifierAndRollValues(setModifier, setDiceNum, maxDamage, hitPrefab, gameObject);
            if (GetComponent<PlayerController>())
            {
                projBase.SetEncumbranceState(GetComponent<PlayerController>().encumbranceState);
            }

            if (projectile.GetComponent<Rigidbody>())
            {
                //projectile.GetComponent<Rigidbody>().velocity = transform.forward * 30f;
                //projectile.GetComponent<Rigidbody>().velocity += new Vector3(0, 4, 0);
            }

            projectile.transform.position = projBase.offsetPosition ? projBase.OffsetProjectileSpawn(gameObject) : projectileSpawnPos.position;

        }
    }

    public void StopVelocity()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    public void ResetCombatAnim()
    {
        GetComponent<Animator>().Play("Idle");
    }

    public void LoadProjectile(CombatActionEnum combatAction)
    {
        string[] splitNames = combatAction.ToString().Split('_');

        for(int i = 0; i < projectileReferences.Length; i++)
        {
            //Check the name of the projectile prefab against the enum converted to string
            if(splitNames[1] == projectileReferences[i].name)
            {
                //Load the projectile as the loaded projectile
                
                loadedProjectile = projectileReferences[i];
                i = projectileReferences.Length;
            }
        }
    }

    public void SetDiceNum(CombatActionEnum diceNum)
    {
        string[] splitNum = diceNum.ToString().Split('_');

        int diceNumber = int.Parse(splitNum[1]);

        setDiceNum = diceNumber;

        //Debug.Log("Number of Dice: " + setDiceNum);

    }

    public void SetMaxDamage(CombatActionEnum maxDamage)
    {
        string[] splitDmg = maxDamage.ToString().Split('_');

        int damageMax = int.Parse(splitDmg[1]);

        this.maxDamage = damageMax;

        //Debug.Log("Max Damage: " + this.maxDamage);
    }

    public void ApplyZMovement(int ZForce)
    {
        Vector3 currentPos = transform.position;
        Vector3 currentDirection = transform.forward;

        Vector3 endPos = currentPos + currentDirection * ZForce;

        StartCoroutine(ApplyMoveCo(endPos));

        //GetComponent<Rigidbody>().MovePosition(endPos);
    }

    public void StopMovement()
    {
        applyingMove = false;
    }

    IEnumerator ApplyMoveCo(Vector3 destination)
    {
        applyingMove = true;
        float distanceFromDest = Vector3.Distance(transform.position, destination);

        Vector3 direction = destination - transform.position;

        do
        {
            distanceFromDest = Vector3.Distance(transform.position, destination);

            GetComponent<Rigidbody>().MovePosition(transform.position + direction * applyMoveVelo * Time.deltaTime);

            yield return null;

        } while (distanceFromDest > .5f && applyingMove);
    }

    public void SetModifier(CombatActionEnum modifier, CharacterStats stats)
    {
        string[] splitMod = modifier.ToString().Split('_');

        string modName = splitMod[1];

        switch (modName)
        {
            case "str":
                this.setModifier = stats.strength.GetScoreModifier();
                break;
            case "con":
                this.setModifier = stats.constitution.GetScoreModifier();
                break;
            case "dex":
                this.setModifier = stats.dexterity.GetScoreModifier();
                break;
            case "int":
                this.setModifier = stats.intelligence.GetScoreModifier();
                break;
            case "wis":
                this.setModifier = stats.wisdom.GetScoreModifier();
                break;
            case "char":
                this.setModifier = stats.charisma.GetScoreModifier();
                break;
            default:

                break;
        }

        //Debug.Log(setModifier);
    }

    public void TriggerAttack(PlayerController player = null, AIThinker AI = null)
    {
        int damageToDeal;
        bool hasHit = false;

        Vector3 hitTextPos = player != null ? player.transform.position : AI.transform.position;

        HitUI hitText = HitTextObjectPool.instance.GetFromPool().GetComponent<HitUI>();

        if(player != null)
        {
            hasHit = _attack.AttackRoll(setModifier, player.GetPlayerStats().armourClass);

            if (hasHit)
            {
                damageToDeal = _attack.DamageRoll(setDiceNum, maxDamage, setModifier);

                player._healthSystem.Damage(damageToDeal);
            }
        } else if(AI != null)
        {
            hasHit = _attack.AttackRoll(setModifier, AI.stats.armourClass);

            if (hasHit)
            {
                damageToDeal = _attack.DamageRoll(setDiceNum, maxDamage, setModifier);

                AI.healthSystem.Damage(damageToDeal);
                hitText.SetUITextAndPos($"Hit! {damageToDeal} Damage!", AI.transform.position);
            }
            else
            {
                hitText.SetUITextAndPos($"Missed! Armour Class was Higher!", AI.transform.position);
            }
        }
    }

    public void TriggerAttack(AIThinker enemyToDamage)
    {
        int damageToDeal = _attack.DamageRoll(setDiceNum, maxDamage, setModifier);

        enemyToDamage.healthSystem.Damage(damageToDeal);
    }

    public void OnNotify(TutorialEnum tutorialEvent)
    {

    }

    public void OnNotify(CombatActionEnum actionType, CombatActionEnum diceNum = CombatActionEnum.enemy_Died, CombatActionEnum maxDamage = CombatActionEnum.enemy_Died, CombatActionEnum modifier = CombatActionEnum.enemy_Died)
    {
        //Debug.Log(actionType.ToString());

        if (actionType.ToString().Contains("projectile"))
        {
            LoadProjectile(actionType);
        }
        else
        {

        }

        if (diceNum.ToString().Contains("num"))
        {
            SetDiceNum(diceNum);
        }

        if (maxDamage.ToString().Contains("dice"))
        {
            SetMaxDamage(maxDamage);
        }

        SetModifier(modifier, ownerSubject.GetComponent<CharacterStats>());

        if(ownerSubject.GetType() == typeof(PlayerController))
        {
            PlayerController player = (PlayerController)ownerSubject;
            player.attacking = true;
        }

    }
}
