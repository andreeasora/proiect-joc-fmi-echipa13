using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusic : MonoBehaviour
{
    [SerializeField]
    AudioSource audioSource;
    void Start()
    {
        audioSource.time = 2.0f;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
