using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            //Load the scene, save stats
            GameManager.instance.SaveCharacterData(other.GetComponent<PlayerStats>());
            StartCoroutine(LoadNextLevelCo());
        }
    }

    IEnumerator LoadNextLevelCo()
    {
        if(UIFade.instance != null)
        {
            UIFade.instance.FadeToBlack();
        }

        yield return new WaitForSeconds(1f);

        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        //Load the scene
    }
}
