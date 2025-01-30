using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// <para>This class handles all global variables, properties, events, and more. This includes level loading
/// </para>
///<c>Unmodified code sourced from: https://learn.unity.com/tutorial/level-generation?projectId=5c514a00edbc2a0020694718#5c7f8528edbc2a002053b6f7</c>
/// </summary>

    public class Game_Manager : MonoBehaviour
    {
    //Events set here need to have the "static" property, otherwise they can't be called by other scripts
    #region Events
       
    #endregion

    #region Parameters
            #region General
        //Static instance of GameManager which allows it to be accessed by any other script.
        public static Game_Manager instance = null;
        
            #endregion

            #region Gameplay


            #endregion
    #endregion

        //Awake is always called before any Start functions
        void Awake()
        {
            //Check if instance already exists
            if (instance == null)

                //if not, set instance to this
                instance = this;

            //If instance already exists and it's not this:
            else if (instance != this)

                //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                Destroy(gameObject);    

            //Sets this to not be destroyed when reloading scene
            DontDestroyOnLoad(gameObject);
            //Events and Delis
            Actor_Player.OnGameOver += G;
            Boss_Base.On_PrepNextLevel += OnPrepNextLevel;
            //Call the InitGame function to initialize the first level 
        }
        private void OnPrepNextLevel(string level){
                StartCoroutine(LoadAsyncScene(level));

        }
         IEnumerator LoadAsyncScene(string level)
    {
        AsyncOperation asyncload = SceneManager.LoadSceneAsync(level);

        while (!asyncload.isDone)
        {
            yield return null;
        }
    }

    private void G(bool rly)
    {
    Debug.Log("Player has died. F.");
    }


    }