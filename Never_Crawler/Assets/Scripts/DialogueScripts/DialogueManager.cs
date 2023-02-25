using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    public float dialogueTypeSpeed;
    [SerializeField] TextMeshProUGUI _nameText;
    [SerializeField] TextMeshProUGUI _dialogueText;

    Queue<string> _sentence = new Queue<string>();
    [SerializeField] Animator _animator;
    public bool dialogueInProg;

    // Start is called before the first frame update
    void Start()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }

        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartDialogue(Dialogue dialogue)
    {
        StartCoroutine(DisablePlayerMoveCo());
        
        _animator.SetBool("isOpen", true);
        dialogueInProg = true;
        string name = dialogue.speakerName;
        _sentence.Clear();
        foreach(string sentence in dialogue.sentences)
        {
            _sentence.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void EndDialogue()
    {
        _animator.SetBool("isOpen", false);
        //Set animator to false
        dialogueInProg = false;
        FindObjectOfType<PlayerController>()._playerInput.Player.Enable();
        FindObjectOfType<CinemachineFreeLook>().GetComponent<CinemachineInputProvider>().XYAxis.action.Enable();

        Debug.Log(FindObjectOfType<CinemachineFreeLook>().GetComponent<CinemachineInputProvider>().XYAxis.action.enabled);
    }

    public void DisplayNextSentence()
    {
        if(_sentence.Count == 0)
        {
            //End the dialogue
            EndDialogue();
            return;
        }

        string sentence = _sentence.Dequeue();
        StartCoroutine(TypeSentenceCo(sentence));
    }

    IEnumerator DisablePlayerMoveCo()
    {
        yield return new WaitForSeconds(.5f);

        FindObjectOfType<CinemachineFreeLook>().GetComponent<CinemachineInputProvider>().XYAxis.action.Disable();
        FindObjectOfType<PlayerController>()._playerInput.Player.Disable();

    }

    IEnumerator TypeSentenceCo(string sentence)
    {
        _dialogueText.text = "";

        foreach(char letter in sentence.ToCharArray())
        {
            _dialogueText.text += letter;
            yield return new WaitForSeconds(dialogueTypeSpeed);
        }
    }
}
