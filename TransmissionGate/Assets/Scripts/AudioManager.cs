using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip shoot;
    public AudioClip opend;
    public AudioClip open;
    public void PlayShoot() //发射
    {
        AudioSource.PlayClipAtPoint(shoot, transform.position, 0.3f);      
    }
    public void PlayOpend()
    {
        AudioSource.PlayClipAtPoint(opend, transform.position, 0.3f);
    }
    public void PlayOpen()
    {
        AudioSource.PlayClipAtPoint(open, transform.position, 0.3f);
    }
}
