using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestartLevel()
    {
        StartCoroutine(ReloadLevelCo());
    }

    public void QuitToMenu()
    {
        StartCoroutine(QuitToMenuCo());
    }

    IEnumerator ReloadLevelCo()
    {
        if(UIFade.instance != null)
        {
            UIFade.instance.FadeToBlack();
        }

        yield return new WaitForSeconds(1f);

        //Create a class responsible for storing the index of the scene the player died in
        SceneManager.LoadSceneAsync(LevelIndex.index);
    }

    IEnumerator QuitToMenuCo()
    {
        if(UIFade.instance != null)
        {
            UIFade.instance.FadeToBlack();
        }
        yield return new WaitForSeconds(1f);

        SceneManager.LoadSceneAsync("MainMenu");
    }
}
