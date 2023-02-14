using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    Animator _animator;
    PlayerController _playerController;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _playerController = GetComponentInParent<PlayerController>();

        if(_playerController == null)
        {
            _playerController = GetComponent<PlayerController>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(_animator != null)
        {
            UpdateAnimations();
        }
    }

    private void FixedUpdate()
    {
        
    }

    void UpdateAnimations()
    {
        /*
        if(GetComponent<Rigidbody>().velocity.x != 0 || GetComponent<Rigidbody>().velocity.z != 0)
        {
            //The player is running, set float value to 1
            _animator.SetBool("isMoving", true);
        }
        else
        {
            _animator.SetBool("isMoving", false);
        }*/

        if(_playerController._moveDir != Vector2.zero)
        {
            _animator.SetBool("isMoving", true);
        }
        else
        {
            _animator.SetBool("isMoving", false);
        }
    }

    public void ResetAnimation()
    {
        for(int i = 0; i < _animator.layerCount; i++)
        {
            if(_animator.HasState(i, Animator.StringToHash("Default")))
            {
                _animator.Play(Animator.StringToHash("Default"), i);
            }
        }
        //_animator.Play("Default");
    }
}
