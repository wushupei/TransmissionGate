using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorManager : MonoBehaviour
{
    public Transform player; //声明主角
    public Transform mainCamera; //主摄像机
    public Transform[] substitutes; //四个替身
    public Transform[] Cameras; //四个辅助摄像机
    public Transform[] doors; //2扇传送门
    public Image star; //准星
    void LateUpdate()
    {
        SetSubstitutePos();//替身及辅助摄像机实时跟随
        DeliveryPlayer(); //调用传送方法       
        SwitchCameraDepth(); //调用摄像机层级切换方法     
    }
    void SetSubstitutePos() //多层空间摄像机渲染
    {
        //一层空间替身获取主摄像机坐标旋转
        substitutes[0].position = substitutes[1].position = mainCamera.position;
        substitutes[0].rotation = substitutes[1].rotation = mainCamera.rotation;
        //二层空间摄像机获取一层空间替身的本地坐标旋转
        Cameras[1].localPosition = substitutes[0].localPosition;
        Cameras[1].localRotation = substitutes[0].localRotation;
        Cameras[0].localPosition = substitutes[1].localPosition;
        Cameras[0].localRotation = substitutes[1].localRotation;
        //二层空间替身获取二层空间摄像机的坐标旋转
        substitutes[2].position = Cameras[1].position;
        substitutes[2].rotation = Cameras[1].rotation;
        substitutes[3].position = Cameras[0].position;
        substitutes[3].rotation = Cameras[0].rotation;
        //三层空间摄像机获取二层空间替身的本地坐标旋转
        Cameras[2].localPosition = substitutes[3].localPosition;
        Cameras[2].localRotation = substitutes[3].localRotation;
        Cameras[3].localPosition = substitutes[2].localPosition;
        Cameras[3].localRotation = substitutes[2].localRotation;
    }
    Transform[] walls = new Transform[2]; //保存开门时被隐藏的墙
    int number = 0; //计数器
    public void GetWall(Transform wall) //获取当前墙,在子弹脚本中调用
    {
        int i = number % 2;
        if (walls[i] != null) //不为空则将之前隐藏的先显示
            ShowWall(walls[i], true);
        walls[i] = wall;
        if (number > 0)
            ShowWall(walls[i], false); //隐藏当前
        if (number == 1)
            ShowWall(walls[0], false);
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
        doors[i].position = walls[i].position;
        doors[i].rotation = walls[i].rotation;
        star.color = i == 0 ? Color.red : Color.blue;
        doors[i].Rotate(0, i == 0 ? 0 : 180, 0);
    }
    void DeliveryPlayer() //传送主角
    {
        if (number >= 2) //有两道门后可以执行传送
        {
            DeliveryCondition(0, substitutes[0].localPosition.z > 0);
            DeliveryCondition(1, substitutes[1].localPosition.z < 0);
        }
    }
    void DeliveryCondition(int i, bool b) //传送主角条件
    {
        int j = Mathf.Abs(i - 1); //另一道门的索引
        //判断某个一层替身与父物体(传送门)的位置关系
        if (Mathf.Abs(substitutes[i].localPosition.x) < 0.3f && b)
        {
            player.GetComponent<Rigidbody>().Sleep();
            //将主角传送至另一道门位置
            player.position = Cameras[j].position;
            Quaternion r = Cameras[j].rotation;
            player.rotation = new Quaternion(player.rotation.x, r.y, player.rotation.z, r.w);
        }
    }
    public void DeliveryBullet(Transform bullet, Transform wall) //传送子弹,在子弹脚本中调用
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
    void SwitchCameraDepth() //切换二层空间摄像机层级
    {
        //如果两个一层替身本地高度差不多，哪个替身离父物体Z轴距离近,一起的摄像机层级越低
        if (Mathf.Abs(substitutes[0].localPosition.y - substitutes[1].localPosition.y) < 0.1f)
        {
            if (Mathf.Abs(substitutes[0].localPosition.z) < Mathf.Abs(substitutes[1].localPosition.z))
            {
                Cameras[0].GetComponent<Camera>().depth = -3;
                Cameras[1].GetComponent<Camera>().depth = -2;
            }
            else
            {
                Cameras[0].GetComponent<Camera>().depth = -2;
                Cameras[1].GetComponent<Camera>().depth = -3;
            }
        }
    }
}
