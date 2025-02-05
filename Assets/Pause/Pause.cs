using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public GameObject Pausemenu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Pausemenu.SetActive(true);
            Time.timeScale = 0;
                   
        }
        if (Input.GetKey(KeyCode.LeftShift)) 
        {
            Pausemenu.SetActive(false);
            Time.timeScale = 1;
        }
        
        
        
    }
}
