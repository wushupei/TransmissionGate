using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Player : MonoBehaviour //主角
{
    public float moveSpeed, downSpeed, jumpPower, rotaSpeed; //移动下落起跳旋转速度
    CharacterController cc; //角色控制器
    Vector3 v; //速度变量
    public Transform muzzle; //枪口
    public Bullet bullet; //子弹
    public bool shoot = true;
    AnimaManager am; //动画管理器
    public Transform mainCamera;
    void Start()
    {
        Cursor.visible = false; //禁用鼠标指针
        cc = GetComponent<CharacterController>();
        am = new AnimaManager();
    }
    void Update()
    {
        if (cc.isGrounded)
        {
            //旋转
            PlayerRotate(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            //移动
            Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            //跳跃
            if (Input.GetKeyDown(KeyCode.Space) && shoot)
            {
                //Jump();
                shoot = false;
                am.Attack(); //攻击动画
                Invoke("Shoot", 0.5f);
            }
            //子弹只有一颗,当处于禁用状态时才可以开枪调用子弹
            //if (Input.GetMouseButtonDown(0) && shoot)
            //{
            //    shoot = false;
            //    am.Attack(); //攻击动画
            //    Invoke("Shoot", 0.5f);
            //}
        }
        else
            Down(); //重力       
        cc.Move(v * Time.deltaTime);
    }
    void PlayerRotate(float x, float y) //镜头旋转
    {
        x *= rotaSpeed * Time.deltaTime;
        y *= rotaSpeed * Time.deltaTime;
        transform.Rotate(0, x, 0, Space.World);
        mainCamera.Rotate(-y, 0, 0);
    }
    void Move(float x, float z) //移动
    {
        am.Move(x, z);
        v = (transform.forward * z + transform.right * x) * moveSpeed + transform.up * v.y;
    }
    void Jump() //跳跃
    {
        am.Jump();
        v.y = jumpPower;
    }
    void Down() //重力
    {
        v.y -= downSpeed * Time.deltaTime;
    }
    public void Shoot() //开枪
    {
        //子弹来到枪口位置并启用
        if (bullet.gameObject.activeInHierarchy == false)
        {
            bullet.gameObject.SetActive(true);
            bullet.transform.position = muzzle.position;
            bullet.transform.rotation = muzzle.rotation;
        }
    }
}
