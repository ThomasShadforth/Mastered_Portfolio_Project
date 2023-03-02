using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CharacterCreatorMenu : MonoBehaviour
{
    public static CharacterCreatorMenu instance;

    

    [Header("Menus")]
    [SerializeField] ConfirmationPopUp _popupMenu;
    [SerializeField] GameObject _randomStatMenu;
    [SerializeField] GameObject _pointBuyMenu;
    [SerializeField] GameObject _classChoiceMenu;
    [SerializeField] GameObject _statConfigMenu;

    [Header("Class Screen Elements")]
    [SerializeField] TextMeshProUGUI _classNameText;
    [SerializeField] TextMeshProUGUI _classDescriptionText;
    [SerializeField] MenuButton[] _classButtons;

    [Header("Stat Screen Elements")]
    [SerializeField] TextMeshProUGUI[] _abilityScoreTexts;
    [SerializeField] TextMeshProUGUI[] _abilityModifierTexts;
    [SerializeField] MenuButton[] _randomizeButtons;
    [SerializeField] MenuButton[] _positivePointBuyButtons;
    [SerializeField] MenuButton[] _negativePointBuyButtons;
    [SerializeField] TextMeshProUGUI _pointBuyCount;

    [Header("Additional Configs")]
    public int initialPointBuyCount;
    public BaseClassSO[] classes;

    int pointBuyRemaining;
    [SerializeField] float _randomizeTime;


    //Private storage variables
    BaseClassSO chosenClassBrain;


    // Start is called before the first frame update
    void Start()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }

        instance = this;

        //OpenStatConfigMenu();

        SetupButtonValues();
        pointBuyRemaining = initialPointBuyCount;

        _classButtons[0].Press();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SetupButtonValues()
    {
        for(int i = 0; i < _randomizeButtons.Length; i++)
        {
            _randomizeButtons[i].buttonValue = i;
            //Sets up the values for the positive and negative point buy buttons
            _positivePointBuyButtons[i].buttonValue = i;
            _negativePointBuyButtons[i].buttonValue = i;
            
            
        }

        for(int i = 0; i < _classButtons.Length; i++)
        {
            _classButtons[i].buttonValue = i;
            _classButtons[i].buttonText.text = classes[i].className;
        }
    }

    #region Class Choice Menu

    public void FinishClassChoice()
    {
        _classChoiceMenu.SetActive(false);
        _statConfigMenu.SetActive(true);
        OpenStatConfigMenu();
    }

    public void SelectClass(int buttonIndex)
    {
        chosenClassBrain = classes[buttonIndex];
        _classNameText.text = classes[buttonIndex].className;
        _classDescriptionText.text = classes[buttonIndex].classDescription;
        //Set the info text to the class' name and description
    }

    #endregion

    #region Stat Menu
    public void OpenStatConfigMenu()
    {
        _popupMenu.ActivateMenu("Would you like to randomise your stats?", "Roll", "Point Buy", () => {
            OpenRandomStatsMenu();
        },
        () => {
            OpenPointBuyMenu();
        });
    }

    //Called when pressing the switch stat method button in the stat screen
    public void SwitchStatMenu()
    {
        if (_randomStatMenu.activeInHierarchy)
        {
            OpenPointBuyMenu();
        }
        else
        {
            OpenRandomStatsMenu();
        }
    }

    void OpenRandomStatsMenu()
    {
        //Open the menu for randomly rolling stats
        //Also write code that closes the other menu (In case the menu lets the player switch)
        Debug.Log("OPENING RANDOM STATS");
        _pointBuyMenu.SetActive(false);
        _randomStatMenu.SetActive(true);
        pointBuyRemaining = initialPointBuyCount;

        for (int i = 0; i < _abilityScoreTexts.Length; i++)
        {
            //First time pass (Sets the initial values for the point buy menu, which is 8 in traditional DnD
            _abilityScoreTexts[i].text = 8.ToString();
            int modifier = CalculateAbilityModifier(int.Parse(_abilityScoreTexts[i].text));
            SetStatModifierText(modifier, _abilityModifierTexts[i]);
        }
    }

    void OpenPointBuyMenu()
    {
        //Open the menu for using point buy to determine stats
        Debug.Log("OPENING POINT BUY");

        _pointBuyMenu.SetActive(true);
        _randomStatMenu.SetActive(false);

        for(int i = 0; i < _abilityScoreTexts.Length; i++)
        {
            //First time pass (Sets the initial values for the point buy menu, which is 8 in traditional DnD
            _abilityScoreTexts[i].text = 8.ToString();
            int modifier = CalculateAbilityModifier(int.Parse(_abilityScoreTexts[i].text));
            SetStatModifierText(modifier, _abilityModifierTexts[i]);
        }

        _pointBuyCount.text = $"Remaining Points: {pointBuyRemaining}";
    }

    

    //Point buy button methods
    public void ChangeAbilityValue(int index, int valueChange)
    {
        //To Do: Add a counter for total point buy points available
        if(valueChange > 0 && pointBuyRemaining == 0)
        {
            return;
        }

        int storedValue = int.Parse(_abilityScoreTexts[index].text);
        bool canChange = CheckStatValue(storedValue, valueChange);

        if (!canChange)
        {
            return;
        }

        storedValue += valueChange;
        pointBuyRemaining += -valueChange;

        _pointBuyCount.text = $"Remaining Points: {pointBuyRemaining}";

        _abilityScoreTexts[index].text = storedValue.ToString();
        int modifier = CalculateAbilityModifier(storedValue);
        SetStatModifierText(modifier, _abilityModifierTexts[index]);


    }

    //Used for checking whether the changed stat exceeds certain values (higher or lower) (POINT BUY MENU)
    public bool CheckStatValue(int valueToCheck, int valueChange)
    {
        if(valueChange > 0)
        {
            if (valueToCheck < 15)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if(valueToCheck > 8)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    //Random Menu Button Methods
    public void RandomizeValue(int index)
    {
        //Debug.Log(index);
        StartCoroutine(RandomizeCo(index));
    }

    public void RandomizeAll()
    {
        StartCoroutine(DelayedRandomizeCo());
    }

    IEnumerator DelayedRandomizeCo()
    {
        for(int i = 0; i < _abilityScoreTexts.Length; i++)
        {
            StartCoroutine(RandomizeCo(i));

            yield return new WaitForSeconds(0.5f);
        }

        
    }

    IEnumerator RandomizeCo(int index)
    {
        float randomizeTime = _randomizeTime;

        while(randomizeTime > 0)
        {
            //Randomises the text value for a visual effect (May replace with a dice roll animation if possible)
            int randomValue = Random.Range(0, 16);

            _abilityScoreTexts[index].text = randomValue.ToString();

            randomizeTime -= Time.deltaTime;
            yield return null;
        }

        int rolledStat = 0;
        List<int> statRolls = new List<int>();

        for (int i = 0; i < 4; i++)
        {
            int roll = Random.Range(1, 7);
            statRolls.Add(roll);
        }

        statRolls.Sort();
        statRolls.Reverse();

        statRolls.RemoveAt(statRolls.Count - 1);

        //Debug.Log(statRolls.Count);

        for (int i = 0; i < statRolls.Count; i++)
        {
            //Debug.Log(rolledStat);
            //Debug.Log(statRolls[i]);

            rolledStat += statRolls[i];
        }

        _abilityScoreTexts[index].text = rolledStat.ToString();
        SetStatModifierText(CalculateAbilityModifier(rolledStat), _abilityModifierTexts[index]);
    }

    public int CalculateAbilityModifier(int abilityScoreVal)
    {
        int modifier = abilityScoreVal - 10;
        modifier = modifier / 2;


        return modifier;
    }

    void SetStatModifierText(int modifier, TextMeshProUGUI textToSet)
    {
        if(modifier > 0)
        {
            textToSet.text = "(+" + modifier + ")";
        } else if(modifier < 0)
        {
            textToSet.text = "(" + modifier + ")";
        }
        else
        {
            textToSet.text = "(" + modifier + ")";
        }
    }

    public void ReturnToClassChoice()
    {
        _statConfigMenu.SetActive(false);
        _classChoiceMenu.SetActive(true);
    }

    public void FinishCreation()
    {
        for(int i = 0; i < _abilityScoreTexts.Length; i++)
        {
            int abilityScore = int.Parse(_abilityScoreTexts[i].text);

            CreatorDataHandler.statValues[i] = abilityScore;
        }

        CreatorDataHandler.chosenClass = chosenClassBrain;

        StartCoroutine(LoadTutorialCo());

    }

    #endregion

    
    IEnumerator LoadTutorialCo()
    {
        if(UIFade.instance != null)
        {
            UIFade.instance.FadeToBlack();
        }

        yield return new WaitForSeconds(1f);

        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    #region Getter Methods
    //Used to get the point buy or randomize stat menu (Rather than expose them as public variables)
    public GameObject GetRandomMenuStatus()
    {
        return _randomStatMenu;
    }

    public GameObject GetPointBuyMenuStatus()
    {
        return _pointBuyMenu;
    }

    public GameObject GetClassMenuStatus()
    {
        return _classChoiceMenu;
    }

    #endregion
}
