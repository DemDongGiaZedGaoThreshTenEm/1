using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ChangeScene : MonoBehaviour, Interactable
{
    public float delayedTime;
    public string nameScene;
    public bool isLoaded;
    [SerializeField] private string promt;
    public string interactionPromt => promt;
    private Interactable interactable;

    public bool Interact(PlayerInteractions interactor)
    {
        Debug.Log("Teleporting...");
        Vote4Change();
        //interactor.gameObject.SetActive(false);
        ModeSelect();
        return true;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.sceneCount > 0)
        {
            for (int i = 0; i < SceneManager.sceneCount; ++i)
            {
                Scene sc = SceneManager.GetSceneAt(i);
                if (sc.name == nameScene)
                {
                    isLoaded = true;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    private void OnTriggerEnter(Collider c)
    {
        /*PlayerInteractions a = c.gameObject.GetComponent<PlayerInteractions>();
        if(interactable.Interact(a))
        {
            if (a.gameObject.tag == "Player")
            {
                
            }

        }*/
    }
    public void ModeSelect()
    {
        StartCoroutine(LoadAfterDelay());

    }

    IEnumerator LoadAfterDelay()
    {
        if(!isLoaded)
        {
            yield return new WaitForSeconds(delayedTime);
            SceneManager.LoadSceneAsync(nameScene, LoadSceneMode.Single);
            isLoaded = true;
        }
    }

    void Unload()
    {
        if(isLoaded)
        {
            SceneManager.UnloadSceneAsync(nameScene);
            isLoaded = false;
        }    
    }
    public void Vote4Change()
    {

    }
    
}
