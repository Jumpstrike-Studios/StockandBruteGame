using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchToLevelOne : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)) 
        {
            SceneManager.LoadScene("Scenes/Stock&BruteGame");
        }

    }
}
