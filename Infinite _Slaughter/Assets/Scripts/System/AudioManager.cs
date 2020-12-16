using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip Music1;
    public AudioClip Music2;
    public AudioClip Explode1;
    public AudioClip Laser1;
    // Start is called before the first frame update
    void Start()
    {
        ServiceLocator.Register<AudioManager>(this);
    }


}
