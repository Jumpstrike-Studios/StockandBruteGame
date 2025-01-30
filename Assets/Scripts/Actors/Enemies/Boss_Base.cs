using UnityEngine;
/// <summary>
/// The basis of evil, and MORE EVIL
/// </summary>
public class Boss_Base : Enemy_Base
{
    public bool Enraged=false;
    public string levelpath = "";
    public delegate void PrepNextLevel(string path);
    public static event PrepNextLevel? On_PrepNextLevel;
    public new void Start()
    {
        base.Start();
        Health=new ActorVitals(2500);
        Debug.Log(Health.Health);
        Debug.Log(Health.MaxHealth);

    }

    // Update is called once per frame
    public new void Update()
    {
        base.Update();
        Enraged = Health.Health<=Health.MaxHealth/2;
        GetComponent<SpriteRenderer>().color = Enraged?Color.red:Color.white;
        GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r,GetComponent<SpriteRenderer>().color.g,GetComponent<SpriteRenderer>().color.b,IFrame_Ticker%2==1?0.8f:1f);
    }
    public new void Die(){
        base.Die();
        On_PrepNextLevel?.Invoke(levelpath);
    }

}