using UnityEngine;
using UnityEditor.PackageManager.UI;
using UnityEngine.SceneManagement;

public class GAMEOVERbacktomain : MonoBehaviour
{
    public GameObject GameOverScreen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Back()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    public void Reload()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameOverScreen.SetActive(false);
    }

}
