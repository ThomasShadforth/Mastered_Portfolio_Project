using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlacement : MonoBehaviour
{
    public int numberOfEnemies;
    public int patrolPointNum;

    public GameObject[] enemyTypes;
    public GameObject patrolPoint;

    public GameObject GetEnemyToSpawn()
    {
        return enemyTypes[Random.Range(0, enemyTypes.Length)];
    }
}
