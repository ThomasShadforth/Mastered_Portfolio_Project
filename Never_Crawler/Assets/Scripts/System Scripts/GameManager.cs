using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IObserver
{
    

    public static GameManager instance;

    private void OnEnable()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Subject>().AddObserver(this);
    }

    void OnSceneUnloaded(Scene scene)
    {
        if(scene.name != "MainMenu")
        {
            Debug.Log("AAAAAAAAA");
        }
    }

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveCharacterData(PlayerStats stats)
    {

    }

    public void OnNotify(CombatActionEnum actionType, CombatActionEnum diceNum = CombatActionEnum.enemy_Died, CombatActionEnum maxDamage = CombatActionEnum.enemy_Died, CombatActionEnum modifier = CombatActionEnum.enemy_Died)
    {
        if(actionType == CombatActionEnum.player_Dead)
        {
            StartCoroutine(LoadGameOverCo());
        }
    }

    public void OnNotify(TutorialEnum tutorialEvent)
    {

    }

    IEnumerator LoadGameOverCo()
    {
        if(UIFade.instance != null)
        {
            UIFade.instance.FadeToBlack();
        }

        yield return new WaitForSeconds(1f);

        SceneManager.LoadSceneAsync("GameOver");
    }
}
