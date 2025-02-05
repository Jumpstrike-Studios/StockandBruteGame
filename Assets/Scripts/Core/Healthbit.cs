
using UnityEngine;
using UnityEngine.UI;

public class Healthbit : MonoBehaviour
{
   public Sprite State1;
   public Sprite State2;
   public Sprite State3;

   public void Change(Sprite newstate){
    gameObject.GetComponent<Image>().sprite = newstate;
   }
}
