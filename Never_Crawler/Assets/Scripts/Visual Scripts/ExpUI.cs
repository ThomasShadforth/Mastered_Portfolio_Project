using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ExpUI : MonoBehaviour
{
    public float destroyTime;
    float lifeTime;
    public float rotationSmooth;
    [SerializeField] TextMeshProUGUI _expUI;
    float currSmoothVelocity;

    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.Euler(0, GetAngleTowardPlayer(), 0);
        transform.position += new Vector3(0, 3, 0) * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(lifeTime < destroyTime)
        {
            lifeTime += GamePause.deltaTime;
        }
        else
        {
            lifeTime = 0;
            ReturnToPool();
        }
    }

    void ReturnToPool()
    {
        ExpTextObjectPool.instance.AddToPool(gameObject);
    }

    public void SetTextandPosition(string textToSet, Vector3 position)
    {
        _expUI.text = textToSet;
        transform.position = position;
    }

    float GetAngleTowardPlayer()
    {
        Vector3 direction = FindObjectOfType<PlayerController>().transform.position - transform.position;

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currSmoothVelocity, rotationSmooth);

        return angle;
    }

}
