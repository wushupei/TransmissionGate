using UnityEngine;
/// <summary>
/// 挂Player上
/// </summary>
public class Player : MonoBehaviour
{
    public float moveSpeed, rotaSpeed; //声明移动和旋转速度
    Rigidbody rigid; //刚体组件
    public Transform muzzle; //声明枪口
    public GameObject bullet; //声明子弹
    public bool shoot = true; //开枪判定
    AnimaManager am; //动画管理器
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        am = new AnimaManager();
    }
    void Update()
    {
        //旋转
        PlayerRotate(Input.GetAxis("Mouse X"));
        //移动
        Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        //按空格进行攻击
        if (Input.GetKeyDown(KeyCode.Space) && shoot)
        {
            shoot = false;
            am.Attack(); //攻击动画
            Invoke("Shoot", 0.5f);
        }      
    }
    void PlayerRotate(float x) //旋转
    {
        x *= rotaSpeed * Time.deltaTime;
        transform.Rotate(0, x, 0, Space.World);
    }
    void Move(float x, float z) //移动
    {
        rigid.velocity = transform.right * x + transform.forward * z;
        rigid.velocity *= moveSpeed * Time.deltaTime;
        am.MoveAnima(x,z);
    }
    public void Shoot() //开枪
    {
        //如果子弹是禁用状态
        if (bullet.gameObject.activeInHierarchy == false)
        {
            ///子弹来到枪口位置并启用
            bullet.SetActive(true);
            bullet.transform.position = muzzle.position;
            bullet.transform.rotation = muzzle.rotation;
        }
    }
}
