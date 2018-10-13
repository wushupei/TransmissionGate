using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Vector3 pos; //位置
    Vector3 scale;//大小
    void Start()
    {
        pos = transform.position; 
        scale = transform.localScale; 
    }
    void Update()
    {
        //位置发生改变则展示由小变大效果
        if (pos != transform.position)
        {
            pos = transform.position;
            transform.localScale *= 0.1f;
        }
        transform.localScale = Vector3.Lerp(transform.localScale, scale, Time.deltaTime*10);
    }
}
