using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject _settingsMenu;
    [SerializeField] GameObject _mainMenu;
    Animator _animator;

    bool _openingSettings;
    bool _startingGame;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        CharacterData.firstLoadDone = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        _startingGame = true;
        _openingSettings = false;

        _animator.Play("FadeOut");
    }


    public void OpenSettings()
    {
        _openingSettings = true;
        _startingGame = false;
        //Play animation here if applicable
        _animator.Play("FadeOut");
    }

    public void ActivateMenu()
    {
        _mainMenu.SetActive(true);
        _animator.Play("FadeIn");
        _settingsMenu.SetActive(false);
    }

    public void FadeOutEnd()
    {
        if (_openingSettings)
        {
            _mainMenu.SetActive(false);
            _settingsMenu.SetActive(true);
        } else if (_startingGame)
        {
            StartCoroutine(StartGameCo());
        }
    }

    public void EndGame()
    {
        Application.Quit();
    }

    IEnumerator StartGameCo()
    {
        if(UIFade.instance != null)
        {
            UIFade.instance.FadeToBlack();
        }
        yield return new WaitForSeconds(.5f);

        SceneManager.LoadSceneAsync("CharacterCreation");
    }
}
