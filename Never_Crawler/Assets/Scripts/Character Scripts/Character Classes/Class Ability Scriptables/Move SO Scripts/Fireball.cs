using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FireballSO", menuName = "Scriptable Objects/Class Abilities/Fireball")]
public class Fireball : AbilitySO
{
    public override void UseAbility(int modifier, PlayerController ownerPlayer = null, AIThinker thinker = null)
    {
        if (ownerPlayer != null)
        {
            SpawnProjectile(ownerPlayer.testProjectile, modifier, ownerPlayer);
        } else if(thinker != null)
        {
            //Insert AI projectile method here
            SpawnProjectile(thinker.testProjectile, modifier, null, thinker);
        }

        PlayAnim(ownerPlayer, thinker);
    }

    public override void PlayAnim(PlayerController ownerPlayer = null, AIThinker thinker = null)
    {
        base.PlayAnim(ownerPlayer, thinker);
    }

    void SpawnProjectile(GameObject projectilePrefab, int modifier, PlayerController ownerPlayer = null, AIThinker thinker = null)
    {
        if (ownerPlayer != null)
        {
            GameObject newProjectile = Instantiate(projectilePrefab, new Vector3(ownerPlayer.transform.position.x, ownerPlayer.transform.position.y + 2, ownerPlayer.transform.position.z), Quaternion.identity);
            newProjectile.GetComponent<ProjectileBase>().SetModifierAndRollValues(modifier, diceNum, maxDamageVal, hitTextPrefab, ownerPlayer.gameObject);
            newProjectile.GetComponent<ProjectileBase>().SetEncumbranceState(ownerPlayer.encumbranceState);

            newProjectile.GetComponent<Rigidbody>().velocity = ownerPlayer.transform.forward * 30f;
        } else if(thinker != null)
        {
            GameObject newProjectile = Instantiate(projectilePrefab, new Vector3(thinker.transform.position.x, thinker.transform.position.y + 2.5f, thinker.transform.position.z), thinker.transform.rotation);

            newProjectile.GetComponent<ProjectileBase>().SetModifierAndRollValues(modifier, diceNum, maxDamageVal, hitTextPrefab, thinker.gameObject);

            newProjectile.GetComponent<Rigidbody>().velocity = thinker.transform.forward * 30f;
        }
    }
}
