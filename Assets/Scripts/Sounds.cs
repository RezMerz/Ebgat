using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour {
    protected AudioSource audioSource;
    protected CharacterAttributesClient charStatsClient;
    public AudioClip[] attackSounds;
    public AudioClip[] jumpSounds;

	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
        charStatsClient = GetComponent<CharacterAttributesClient>();
	}

    public virtual void HandState(string value)
    {
        if (value == "2")
        {
            audioSource.clip = attackSounds[charStatsClient.attackNumber];
            audioSource.Play();
        }
    }

    public virtual void FeetState(string value)
    {
        if (value == "3")
        {
            // Jump Sound
        }
        else if (value == "5")
        {
            // Double Jump Sound

        }
    }


}
