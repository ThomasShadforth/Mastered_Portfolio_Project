using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

public class CombatAnimator : MonoBehaviour, IObserver
{
    public GameObject[] projectileReferences;
    public Subject ownerSubject;

    public LayerMask enemyLayer;

    public GameObject loadedProjectile;

    public HitUI hitPrefab;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnProjectile()
    {
        GameObject projectile = Instantiate(loadedProjectile, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.Euler(0, transform.rotation.y, 0));

        projectile.GetComponent<ProjectileBase>().SetModifierAndRollValues(setModifier, setDiceNum, maxDamage, hitPrefab, transform.parent.gameObject);

        if (transform.parent.gameObject.GetComponent<PlayerController>())
        {

            projectile.GetComponent<ProjectileBase>().SetEncumbranceState(transform.parent.GetComponent<PlayerController>().encumbranceState);
        }

        projectile.GetComponent<Rigidbody>().velocity = transform.parent.transform.forward * 30f;

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

    public void TriggerAttack(AIThinker enemyToDamage)
    {
        int damageToDeal = _attack.DamageRoll(setDiceNum, maxDamage, setModifier);

        enemyToDamage.healthSystem.Damage(damageToDeal);
    }

    public void OnNotify(CombatActionEnum actionType, CombatActionEnum diceNum = CombatActionEnum.enemy_Died, CombatActionEnum maxDamage = CombatActionEnum.enemy_Died, CombatActionEnum modifier = CombatActionEnum.enemy_Died)
    {
        Debug.Log(actionType.ToString());

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
    }
}
