using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour, IObserver
{
    [SerializeField] int enemiesDefeated = 0;
    int enemyCount;
    int currentCheckpointNum;

    [SerializeField] Dialogue _startDialogue;
    [SerializeField] Dialogue[] _checkpointDialogue;
    [SerializeField] Dialogue _enemyDeathDialogue;

    List<TutorialObject> _tutorialObjects = new List<TutorialObject>();

    // Start is called before the first frame update
    void Start()
    {
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        Debug.Log(enemyCount.ToString());

        TutorialObject[] existingObjects = FindObjectsOfType<TutorialObject>();

        foreach(TutorialObject tutObj in existingObjects)
        {
            _tutorialObjects.Add(tutObj);
        }

        foreach(TutorialObject tutObj in _tutorialObjects)
        {
            tutObj.AddObserver(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateTutorial(TutorialEnum tutorialEvent)
    {
        if (tutorialEvent.ToString().Contains("Started"))
        {
            
            if(DialogueManager.instance != null)
            {
                
                DialogueManager.instance.StartDialogue(_startDialogue);
            }
        }
        else if (tutorialEvent.ToString().Contains("Enemy"))
        {
            enemiesDefeated++;

            if(enemiesDefeated >= enemyCount)
            {
                if (DialogueManager.instance != null)
                {
                    //Trigger dialogue for defeating enemies
                    DialogueManager.instance.StartDialogue(_enemyDeathDialogue);
                }
            }
        }
        else if (tutorialEvent.ToString().Contains("Checkpoint"))
        {
            
            if (DialogueManager.instance != null)
            {
                if (currentCheckpointNum < _checkpointDialogue.Length)
                {
                    DialogueManager.instance.StartDialogue(_checkpointDialogue[currentCheckpointNum]);
                    currentCheckpointNum++;
                }
            }
            
        }
    }

    public void OnNotify(TutorialEnum tutorialEvent)
    {
        //Debug.Log(tutorialEvent.ToString());
        UpdateTutorial(tutorialEvent);
    }

    public void OnNotify(CombatActionEnum actionType, CombatActionEnum diceNum, CombatActionEnum maxDamage, CombatActionEnum modifier)
    {

    }

}
