using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image _healthImage;
    public float fillSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHealthFillAmount(float fillPercent)
    {
        StartCoroutine(ChangeHealthFill(fillPercent));
    }

    IEnumerator ChangeHealthFill(float percent)
    {
        while (_healthImage.fillAmount != percent)
        {
            _healthImage.fillAmount = Mathf.MoveTowards(_healthImage.fillAmount, percent, fillSpeed * GamePause.deltaTime);
            yield return null;
        }
    }
}
