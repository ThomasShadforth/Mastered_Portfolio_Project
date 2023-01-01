using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class ConfirmationPopUp : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] TextMeshProUGUI _displayText;
    [SerializeField] Button _confirmButton;
    [SerializeField] Button _cancelButton;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Activate the confirmation menu (Will display what the player is attempting to do, and assign delegates to the button click events
    public void ActivateMenu(string displayText, string confirmText, string cancelText, UnityAction confirmAction, UnityAction cancelAction)
    {
        this.gameObject.SetActive(true);

        _displayText.text = displayText;
        _confirmButton.GetComponentInChildren<TextMeshProUGUI>().text = confirmText;
        _cancelButton.GetComponentInChildren<TextMeshProUGUI>().text = cancelText;

        _confirmButton.onClick.RemoveAllListeners();
        _cancelButton.onClick.RemoveAllListeners();

        _confirmButton.onClick.AddListener(() => {
            DeactivateMenu();
            confirmAction();
        });

        _cancelButton.onClick.AddListener(() =>
        {
            DeactivateMenu();
            cancelAction();
        });
    }

    void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }
}
