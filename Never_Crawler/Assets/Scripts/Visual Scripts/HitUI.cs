using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class HitUI : MonoBehaviour
{
    public float destroyTime;
    public float rotationSmooth;
    [SerializeField] TextMeshProUGUI _damageUI;
    float _currSmoothVelo;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyObject", destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        //Set rotation, move up
        transform.rotation = Quaternion.Euler(0, GetAngleTowardPlayer(), 0);
        transform.position = new Vector3(transform.position.x, transform.position.y + 3 * GamePause.deltaTime, transform.position.z);
    }

    public void SetUITextAndPos(string textToSet, Vector3 objectPos)
    {
        transform.position = objectPos;
        _damageUI.text = textToSet;
    }

    //Use for now, try and make object pool later
    void DestroyObject()
    {
        Destroy(gameObject);
    }

    float GetAngleTowardPlayer()
    {
        Vector3 direction = FindObjectOfType<PlayerController>().transform.position - transform.position;

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currSmoothVelo, rotationSmooth);

        return angle;
    }
}
