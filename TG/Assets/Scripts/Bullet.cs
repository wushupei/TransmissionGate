using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour //子弹
{
    public float speed; //子弹速度
    Rigidbody rig; //刚体
    DoorManager dm; //传送门管理器
    Player player; //主角
    Transform wall; //碰撞到的墙
    void Start()
    {
        rig = GetComponent<Rigidbody>();
        dm = FindObjectOfType<DoorManager>();
        player = FindObjectOfType<Player>();
    }
    void Update()
    {
        //向前飞
        rig.velocity = transform.forward * speed * Time.deltaTime;
        //飞出地图一定距离自动禁用
        if (Mathf.Abs(transform.position.z) > 8 || Mathf.Abs(transform.position.x) > 5)
            gameObject.SetActive(false);
    }
    void OnCollisionEnter(Collision other) //碰撞一次
    {
        if (other.collider.CompareTag("Wall")) //碰到墙
        {
            if (wall != other.transform) //且不是上次的墙
            {
                wall = other.transform; //获取该墙
                dm.GetWall(other.transform); 
            }
        }
        gameObject.SetActive(false);
    }
    void OnTriggerEnter(Collider other) //触发一次
    {
        wall = other.transform; //获取被触发的墙
        if (wall != transform.parent) //触发的物体(墙)不是自己的父物体
            dm.DeliveryBullet(transform, wall); //传送子弹
    }
    void OnDisable()
    {
        transform.parent = null; //父物体设为空
        player.shoot = true; //主角可以开枪了
    }
}
