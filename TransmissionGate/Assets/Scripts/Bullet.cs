using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour //子弹
{
    public float speed;
    Rigidbody rig;
    DoorManager dm; //传送门管理器
    Player player;
    Transform wall;
    public AudioManager am; //声明声音管理器
    //ParticleSystem effect; //自身特效
    //public ParticleSystem boom; //打击特效
    bool open = true; //启动传送门也需要冷却时间
    void OnEnable()
    {
        am.PlayShoot();
    }
    void Start()
    {
        //effect = GetComponentInChildren<ParticleSystem>();
        rig = GetComponent<Rigidbody>();
        dm = FindObjectOfType<DoorManager>();
        player = FindObjectOfType<Player>();
    }
    void Update()
    {
        rig.velocity = transform.forward * Time.deltaTime * speed; //前进
        AutoDisable();
    }
    void OnCollisionEnter(Collision other) //碰撞一次
    {
        if (other.collider.CompareTag("Wall"))
        {
            if (open && wall != other.transform) //已经冷却且撞到的墙不是刚才的墙
            {
                open = false;
                wall = other.transform;
                dm.AddWall(other.transform);
                Invoke("ColdOpen", 0.2f);
                //PlayerEffect(other.transform); //获取碰撞物体位置播放特效
                am.PlayOpen();
            }
            else
                am.PlayOpend();
        }
        else
            am.PlayOpend();
        DisableObj();
    }
    void OnTriggerEnter(Collider other) //触发一次
    {
        wall = other.transform; //获取被触发的墙
        if (wall != transform.parent) //触发的物体(墙)不是自己的父物体
            dm.DeliveryBullet(transform, wall); //传送子弹
    }
    void AutoDisable() //飞出地图一定距离自动禁用
    {
        if (Mathf.Abs(transform.position.z) > 8 || Mathf.Abs(transform.position.x) > 5)
            DisableObj();
    }
    void DisableObj() //禁用并让父物体为空
    {
        gameObject.SetActive(false);
        transform.parent = null;
        player.shoot = true;
    }
    void ColdOpen()
    {
        open = true;
    }
    //void PlayerEffect(Transform hit) //播放撞击特效
    //{
    //if (effect.startColor == Color.red)
    //    boom.startColor = Color.blue;
    //else
    //    boom.startColor = Color.red;
    //
    //Vector3 dir = transform.position - hit.position; //碰撞击物体到子弹的方向
    //boom.transform.position = hit.position+dir*0.5f;
    //boom.transform.rotation = hit.rotation;
    //boom.Play();
    //}
}
