using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreMusic : MonoBehaviour
{
    [SerializeField]
    AudioSource audioSource;
    void Start()
    {
        audioSource.time = 1.5f;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
