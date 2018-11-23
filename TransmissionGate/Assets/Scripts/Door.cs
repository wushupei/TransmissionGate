using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Vector3 pos; //位置
    Vector3 scale;//大小
    public float angle; //旋转角度
    public Transform CopyDoor; //把假门拖进去
    void Start()
    {
        //记录初始位置和大小
        pos = transform.position;
        scale = transform.localScale;
    }
    void Update()
    {
        //位置发生改变
        if (pos != transform.position)
        {             
            //更新位置和旋转信息
            pos = transform.position;
            transform.localScale = Vector3.zero;
        }
        //真门变大
        transform.localScale = Vector3.Lerp(transform.localScale, scale, Time.deltaTime*10 );

        if (CopyDoor.gameObject.activeInHierarchy) //如果假门被启用调用展示效果方法
            ShowPrefabDoor();
    }
    public void OpenDoor(Vector3 pos, Quaternion rota) //打开传送门
    {
        DisplayDoor(); //给与假门自身位置与旋转
        //获取新的位置和旋转
        transform.position = pos;
        transform.rotation = rota;
        transform.Rotate(0, angle, 0);
    }
    void DisplayDoor() //显示假门
    {
        //首先复制真门的位置旋转尺寸，然后启用
        CopyDoor.position = pos;
        CopyDoor.rotation = transform.rotation;
        CopyDoor.localScale = scale;
        CopyDoor.gameObject.SetActive(true);
    }
    void ShowPrefabDoor() //展示假门效果
    {
        //假门变小
        CopyDoor.localScale = Vector3.Lerp(CopyDoor.localScale, Vector3.zero, Time.deltaTime*10 );
        if (CopyDoor.localScale.x < 0.1f) //小到一定程度就禁用
            CopyDoor.gameObject.SetActive(false);
    }   
}
