using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimaManager : MonoBehaviour
{

    Player player;
    Animator anim;
    public AnimaManager()
    {
        player = FindObjectOfType<Player>();
        anim = player.GetComponentInChildren<Animator>();
        SetWeight();
    }
    void SetWeight()
    {
        anim.SetLayerWeight(1, 1);
    }
    public void Move(float x, float z)
    {
            anim.SetBool("Left", x < -0.5f);
            anim.SetBool("Right", x > 0.5f);
            anim.SetBool("Run", z > 0.5f);
            anim.SetBool("Back", z < -0.5f);
    }
    public void Jump()
    {
        anim.SetTrigger("Jump");
    }
    public void Attack()
    {
        anim.SetTrigger("Attack");
    }
}
