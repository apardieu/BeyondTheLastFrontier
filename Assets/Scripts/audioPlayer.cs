using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioPlayer : MonoBehaviour
{

    public List<AudioClip> audioClips;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
   
        
    }


    public void playSound(int clipIndex , bool stopCurrentSound, float volume)
    {
        if (audioSource.isPlaying && stopCurrentSound)
            audioSource.Stop();
        
            if(!audioSource.isPlaying && clipIndex!=-1)
            {
                audioSource.clip = audioClips[clipIndex];
                audioSource.Play();
                audioSource.volume = volume;
            }
    }
    public void setPitch(float pitch)
    {
        if(audioSource != null)
           audioSource.pitch = pitch;
    
    }

    public void stopSound()
    {
        audioSource.Stop();
    }

    public AudioClip getClip(int clipIndex)
    {
        return audioClips[clipIndex];
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
