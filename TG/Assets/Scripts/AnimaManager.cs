using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimaManager : MonoBehaviour
{
    Player player; //主角
    Animator anim; //动画
    public AnimaManager() //构造函数
    {
        player = FindObjectOfType<Player>();
        anim = player.GetComponentInChildren<Animator>();
        SetWeight();
    }
    void SetWeight()
    {
        anim.SetLayerWeight(1, 1);
    }
    public void MoveAnima(float x, float z)
    {
        anim.SetBool("Left", x < -0.5f);
        anim.SetBool("Right", x > 0.5f);
        anim.SetBool("Run", z > 0.5f);
        anim.SetBool("Back", z < -0.5f);
    }
    public void Attack()
    {
        anim.SetTrigger("Attack");
    }
}
