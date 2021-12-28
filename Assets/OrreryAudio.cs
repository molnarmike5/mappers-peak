using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.Audio;

public class OrreryAudio : MonoBehaviour
{

    private AudioSource source;

    public AudioClip powerup;
    public AudioClip powerloop;
    public AudioClip powerdown;

    public AudioMixerSnapshot normal;
    public AudioMixerSnapshot orrerysnap;
    // Start is called before the first frame update

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }

    public void EnterOrrery()
    {
        orrerysnap.TransitionTo(.01f);
        StartCoroutine(PowerUpAndRun());
    }

    public void LeaveOrrery()
    {
        normal.TransitionTo(.01f);
        source.clip = powerdown;
        source.Play();
        source.loop = false;
    }

    IEnumerator PowerUpAndRun()
    {
        source.clip = powerup;
        source.Play();
        yield return new WaitForSeconds(powerup.length);
        source.clip = powerloop;
        source.Play();
        source.loop = true;
    }
}
