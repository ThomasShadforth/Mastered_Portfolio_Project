using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FireballSO", menuName = "Scriptable Objects/Class Abilities/Fireball")]
public class Fireball : AbilitySO
{
    public override void UseAbility(PlayerController ownerPlayer, int modifier)
    {
        SpawnProjectile(ownerPlayer, ownerPlayer.testProjectile, modifier);
    }

    void SpawnProjectile(PlayerController ownerPlayer, GameObject projectilePrefab, int modifier)
    {
        GameObject newProjectile = Instantiate(projectilePrefab, new Vector3(ownerPlayer.transform.position.x, ownerPlayer.transform.position.y + 2, ownerPlayer.transform.position.z), Quaternion.identity);
        newProjectile.GetComponent<ProjectileBase>().SetModifierAndRollValues(modifier, diceNum, maxDamageVal, hitTextPrefab);
        newProjectile.GetComponent<Rigidbody>().velocity = ownerPlayer.transform.forward * 30f;
    }
}
