using System;
using Unity.Mathematics;
using UnityEngine;

public class Whisp_Wheel : Projectile_Base
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject[] Whisps;
    public float RotateSpeed;
    private float ExpandTime;
    public float FireAngle=Mathf.PI*0.5f;
    private float AnimationTimer=0;

    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(Mathf.Sin(FireAngle),Mathf.Cos(FireAngle),0f)*Time.deltaTime*WalkSpeed;
        AnimationTimer+=Time.deltaTime;
        int count = Whisps.Length;
        int I =0;
        foreach (GameObject item in Whisps)
        {
            float WAngle = -math.PI*(AnimationTimer*2.4f)+(math.PI*2/count*I);
            item.transform.position = transform.position+new Vector3(Mathf.Sin(WAngle)*0.5f,Mathf.Cos(WAngle)*0.5f,0f)*Mathf.Clamp(AnimationTimer,0f,1f);
            item.transform.rotation = Quaternion.Euler(0,0,-WAngle*Mathf.Rad2Deg+90);
            I++;
        }
        if (AnimationTimer>15f){
            Destroy(gameObject);
        }
    }
}
