using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemTextUI : MonoBehaviour
{
    public float destroyTime;
    float lifeTime;
    [SerializeField] TextMeshProUGUI _itemText;
    float _currSmoothVelo;
    public float rotationSmooth;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + 3 * GamePause.deltaTime, transform.position.z);

        if (lifeTime < destroyTime)
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
        //Insert reference to item text object pool
        if(ItemTextObjectPool.instance != null)
        {
            ItemTextObjectPool.instance.AddToPool(gameObject);
        }
    }

    public void SetTextAndPos(string textToSet, Vector3 positionToSet)
    {
        transform.position = positionToSet;
        _itemText.text = textToSet;
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, GetAngleTowardPlayer(), transform.localEulerAngles.z); 
    }

    float GetAngleTowardPlayer()
    {
        Vector3 direction = FindObjectOfType<PlayerController>().transform.position - transform.position;

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currSmoothVelo, rotationSmooth);

        return angle;
    }
}
