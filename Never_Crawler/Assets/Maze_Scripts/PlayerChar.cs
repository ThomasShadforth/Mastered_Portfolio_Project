using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChar : MonoBehaviour
{
    public float moveSpeed;
    Vector3 _moveInput;

    Vector3 _gravForce;
    Vector3 _rayDir = Vector3.down;
    Vector3 _prevVelocity = Vector3.zero;

    [SerializeField]LayerMask _terrainLayer;

    bool _shouldMaintainHeight = true;

    [SerializeField] float _rideHeight = 1.75f;
    [SerializeField] float _rayToGroundLength = 3f;
    [SerializeField] float _rideSpringStrength = 50f;
    [SerializeField] float _rideSpringDamper = 5f;

    Vector3 _desiredVelocity;

    [SerializeField] float _maxSpeed;
    [SerializeField] float _acceleration = 200f;


    [SerializeField] Transform _cam;

    PlayerActionMap _playerActions;
    Rigidbody _rb;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();

        _gravForce = Physics.gravity * _rb.mass;

        _playerActions = new PlayerActionMap();
        _playerActions.Player.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveDir = _playerActions.Player.Movement.ReadValue<Vector2>();

        

        _moveInput = new Vector3(moveDir.x, 0, moveDir.y);
        _moveInput = _moveInput.normalized;

    }

    void RelativeCameraMovement()
    {
        Vector2 moveDir = _playerActions.Player.Movement.ReadValue<Vector2>();
        moveDir = moveDir.normalized;

        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        forward.y = 0;
        right.y = 0;

        forward = forward.normalized;
        right = right.normalized;

        Vector3 relativeForwardVertInput = forward * moveDir.y;
        Vector3 relativeRightInput = right * moveDir.x;

        Vector3 camRelativeMovement = relativeForwardVertInput + relativeRightInput;

        //Debug.Log(camRelativeMovement.x);

        _rb.velocity = new Vector3(camRelativeMovement.x * moveSpeed, _rb.velocity.y, camRelativeMovement.z * moveSpeed);
    }

    private void FixedUpdate()
    {

        RelativeCameraMovement();
        //_rb.velocity = new Vector3(_moveInput.x * moveSpeed, _rb.velocity.y, _moveInput.z * moveSpeed);
    }

    #region Ground Checks

    bool CheckIfGrounded(bool rayHitGround, RaycastHit rayHit)
    {
        bool grounded;
        if (rayHitGround)
        {
            grounded = rayHit.distance <= _rideHeight * 1.3f;
        }
        else
        {
            grounded = false;
        }

        return grounded;
    }

    (bool, RaycastHit) RaycastToGround()
    {
        RaycastHit rayHit;
        Ray rayToGround = new Ray(transform.position, _rayDir);
        bool rayHitGround = Physics.Raycast(rayToGround, out rayHit, _rayToGroundLength, _terrainLayer.value);

        return (rayHitGround, rayHit);
    }

    #endregion

    #region Misc (Height Maintenance)

    void MaintainHeight(RaycastHit rayHit)
    {
        Vector3 vel = _rb.velocity;
        Vector3 otherVel = Vector3.zero;
        Rigidbody hitBody = rayHit.rigidbody;

        if(hitBody != null)
        {
            otherVel = hitBody.velocity;
        }

        float rayDirVel = Vector3.Dot(_rayDir, vel);
        float otherDirVel = Vector3.Dot(_rayDir, otherVel);

        float relVel = rayDirVel - otherDirVel;
        float currHeight = rayHit.distance - _rideHeight;
        float springForce = (currHeight * _rideSpringStrength) - (relVel * _rideSpringDamper);
        Vector3 maintainHeightForce = -_gravForce + springForce * Vector3.down;
        Vector3 oscillationForce = springForce * Vector3.down;
        _rb.AddForce(maintainHeightForce);

        if(hitBody != null)
        {
            hitBody.AddForceAtPosition(-maintainHeightForce, rayHit.point);
        }

    }

    #endregion
}
