using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    public AudioSource audioSound;

    void Start()
    {
        audioSound = GetComponent<AudioSource>();
    }

    //Om spelaren går
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
                audioSound.volume = 0.4f;
                audioSound.PlayOneShot(audioSound.clip);
        }
    }
}