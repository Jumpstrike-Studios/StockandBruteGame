using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManagementScript : MonoBehaviour
{
    
    public GameObject Boss;

    void Start()
    {
        Actor_Player.OnGameOver += G;
    }

    private void G(bool rly)
    {
    Debug.Log("Player has died. F.");
    }

   void Update()
   {
        if (Boss.IsDestroyed())
        {
             StartCoroutine(LoadAsyncScene());
        }

        
   }

    IEnumerator LoadAsyncScene()
    {
        AsyncOperation asyncload = SceneManager.LoadSceneAsync(2);

        while (!asyncload.isDone)
        {
            yield return null;
        }
    }
}
