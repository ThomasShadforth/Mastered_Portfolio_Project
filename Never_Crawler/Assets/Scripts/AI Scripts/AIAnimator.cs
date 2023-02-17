using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAnimator : MonoBehaviour
{
    AIThinker _thinker;
    Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _thinker = GetComponent<AIThinker>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(_thinker.agent.velocity.magnitude);

        UpdateAnimations();
    }

    private void FixedUpdate()
    {
       
    }

    void UpdateAnimations()
    {
        if(_thinker.agent.velocity.magnitude != 0 || GetComponent<Rigidbody>().velocity.magnitude != 0)
        {
            //Set the walking bool in the animator
            _animator.SetBool("isMoving", true);
        }
        else
        {
            //Set walking bool to false
            _animator.SetBool("isMoving", false);
        }
        if (_thinker.canSeePlayer)
        {
            //Set running bool to true
            _animator.SetBool("isChasing", true);

        }
        else
        {
            //Set to false
            _animator.SetBool("isChasing", false);
        }



    }
}
