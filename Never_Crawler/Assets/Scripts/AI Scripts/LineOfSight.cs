using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    public float losRadius;
    [Range(0, 360)]
    public float losAngle;

    public GameObject player;

    public LayerMask playerLayer;
    public LayerMask obstructionConfig;

    public bool canSeePlayer;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVCo());
    }

    IEnumerator FOVCo()
    {
        float delay = .2f;

        WaitForSeconds wait = new WaitForSeconds(delay);

        while (true)
        {
            yield return wait;
            FOVCheck();
        }
    }

    private void FOVCheck()
    {
        //
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, losRadius, playerLayer);

        if(rangeChecks.Length != 0)
        {
            Transform targetTransform = rangeChecks[0].transform;
            Vector3 directionToTarget = (targetTransform.position - transform.position).normalized;

            if(Vector3.Angle(transform.forward, directionToTarget) < losAngle / 2){
                float distanceToTarget = Vector3.Distance(transform.position, targetTransform.position);

                if(!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionConfig))
                {
                    canSeePlayer = true;
                }
                else
                {
                    canSeePlayer = false;
                }
            }
            else
            {
                canSeePlayer = false;
            }
        }
        else if(canSeePlayer)
        {
            canSeePlayer = false;
        }
    }
}
