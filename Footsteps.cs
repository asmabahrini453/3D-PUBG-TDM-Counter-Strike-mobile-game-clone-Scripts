using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    public AudioSource audioSource;

    [Header("Footsteps Sources")]
    public AudioClip[] footstepsSound;

    private AudioClip GetRandomFootStep() //function to slect random footstep sound from the audio array
    {
        return footstepsSound[Random.Range(0, footstepsSound.Length)]; 
    }

    //to play the sound //the "Step" name of the func must mutch the name of the event we created for the sound in the animation 
    private void Step()
    {
        AudioClip clip = GetRandomFootStep();
        audioSource.PlayOneShot(clip);
    }


}
