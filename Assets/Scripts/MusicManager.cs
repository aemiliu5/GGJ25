using System;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource floriko;
    public AudioSource metal;
    
    public static MusicManager instance;

    private void Start()
    {
        instance = this;
    }
}
