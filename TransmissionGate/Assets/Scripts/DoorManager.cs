using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorManager : MonoBehaviour
{
    public Transform player; //声明主角
    public Transform mainCamera; //主摄像机
    public Image star; //准星
    Transform[] walls = new Transform[2]; //墙
    public Transform[] substitutes; //替身
    public Transform[] copyCameras; //辅助摄像机

    public Transform[] doorSprites; //传送门
    ParticleSystem.MainModule pm;
    public ParticleSystem effect; //子弹特效
    int number = 0; //计数器
    bool delivery = true; //传送门冷却
    void Start()
    {
        pm = effect.main;
    }
    void LateUpdate()
    {
        SetSubstitutePos();//
        DeliveryPlayer(); //调用传送方法       
        SwitchCameraDepth(); //调用摄像机层级切换方法
    }
    public void AddWall(Transform wall) //获取当前墙
    {
        int i = number % 2;
        if (walls[i] != null) //不为空则将之前隐藏的先显示
            ShowWall(walls[i], true);
        walls[i] = wall;
        if (number > 0)
        {
            ShowWall(walls[i], false); //隐藏当前
            if (number == 1)
                ShowWall(walls[0], false);
        }
        OpenDoor(i); //打开传送门
        number++;
    }
    void ShowWall(Transform wall, bool b) //隐藏墙
    {
        wall.GetComponent<BoxCollider>().isTrigger = !b; //开关触发器
        wall.GetComponent<SpriteRenderer>().enabled = b; //开关渲染器
    }
    void OpenDoor(int i) //打开传送门
    {
        doorSprites[i].position = walls[i].position; //在墙的位置生成传送门
        doorSprites[i].rotation = walls[i].rotation; //跟随门的旋转方向
        doorSprites[i].Rotate(0, i == 1 ? 180 : 0, 0); //单数门要旋转
        doorSprites[i].GetComponent<SpriteRenderer>().flipX = i == 1 ? true : false; //单数门要旋转回图片
        star.color = i == 0 ? Color.red : Color.blue;
        pm.startColor = i == 0 ? Color.red : Color.blue;
    }
    void DeliveryPlayer() //传送主角
    {
        if (number >= 2) //有两道门后可以执行传送
        {
            DeliveryCondition(0, 1, substitutes[0].localPosition.z > 0);
            DeliveryCondition(1, 0, substitutes[1].localPosition.z < 0);
        }
    }
    void DeliveryCondition(int i0, int i1, bool b) //传送条件
    {
        //判断某个一层替身与父物体(传送门)的位置关系
        if (Mathf.Abs(substitutes[i0].localPosition.x) < 1f && Mathf.Abs(substitutes[i0].localPosition.y) < 1 && b)
        {
            //将主角传送至另一个一层替身位置
            player.position = copyCameras[i1].position;
            Quaternion r = copyCameras[i1].rotation;
            player.rotation = new Quaternion(player.rotation.x, r.y, player.rotation.z, r.w);
            print("传送 " + i0);
        }
    }
    public void DeliveryBullet(Transform bullet, Transform wall) //传送子弹
    {
        bullet.parent = wall; //获取该墙成为子弹父物体
        //保存自身本地坐标和旋转
        Vector3 lp = bullet.localPosition;
        Quaternion lr = bullet.localRotation;
        //让另一道门成为子弹父物体
        if (wall == walls[0])
            bullet.parent = walls[1];
        else
            bullet.parent = walls[0];
        //将刚才的本地坐标和旋转再赋予子弹
        bullet.localPosition = new Vector3(-lp.x, lp.y, -lp.z);
        bullet.localRotation = lr;
        bullet.Rotate(0, 180, 0, Space.World);
    }
    void SwitchCameraDepth() //切换第一层摄像机层级
    {
        //如果两个传送门的高度相差不多
        if (Mathf.Abs(doorSprites[0].position.y - doorSprites[1].position.y) < 0.1f)
        {
            //哪个替身离父物体(传送门)近,一起的摄像机层级越低
            if (Mathf.Abs(substitutes[0].localPosition.z) < Mathf.Abs(substitutes[1].localPosition.z))
                SetDepth(-3, -2);
            else
                SetDepth(-2, -3);
        }
        //如果高度相差很大
        else
        {
            //哪个传送门高,它的子物体摄像机层级越高
            if (doorSprites[0].position.y > doorSprites[1].position.y)
                SetDepth(-2, -3);
            else
                SetDepth(-3, -2);
        }
    }
    void SetDepth(int layer1, int layer2) //设置一层摄像机渲染层级
    {
        copyCameras[0].GetComponent<Camera>().depth = layer1;
        copyCameras[1].GetComponent<Camera>().depth = layer2;
    }
    void SetSubstitutePos() //多层摄像机渲染
    {
        //一层替身获取主摄像机坐标旋转
        substitutes[0].position = substitutes[1].position = mainCamera.position;
        substitutes[0].rotation = substitutes[1].rotation = mainCamera.rotation;
        //一层辅助摄像机获取一层替身的本地坐标旋转
        copyCameras[1].localPosition = substitutes[0].localPosition;
        copyCameras[1].localRotation = substitutes[0].localRotation;
        copyCameras[0].localPosition = substitutes[1].localPosition;
        copyCameras[0].localRotation = substitutes[1].localRotation;
        //二层替身获取一层辅助摄像机的坐标旋转
        substitutes[2].position = copyCameras[1].position;
        substitutes[2].rotation = copyCameras[1].rotation;
        substitutes[3].position = copyCameras[0].position;
        substitutes[3].rotation = copyCameras[0].rotation;
        //二层辅助摄像机获取二层替身的本地坐标旋转
        copyCameras[2].localPosition = substitutes[3].localPosition;
        copyCameras[2].localRotation = substitutes[3].localRotation;
        copyCameras[3].localPosition = substitutes[2].localPosition;
        copyCameras[3].localRotation = substitutes[2].localRotation;
    }
}
