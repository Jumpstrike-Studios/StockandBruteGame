using UnityEngine;
using UnityEditor.PackageManager.UI;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void Play()
    { Game_Manager.instance.RequestReload("Scenes/Stock&BruteGame");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Your game would quit now...");
        
    }
}
