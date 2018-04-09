using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {


    //Allows other scripts to call functions from SoundManager.
    //Also, using to enforce "singelton pattern"
    public static SoundManager instance = null;


    //holds audio source for current song to play
    //public AudioSource currentSongSource;

    //using to play the test song on awake               
    public AudioSource musicSource;

    
    //Might use later for target hit or something
    //Small variation in pitch to change the sound a tiny bit            
    //public float lowPitchRange = .95f;              //The lowest a sound effect will be randomly pitched.
    //public float highPitchRange = 1.05f;            //The highest a sound effect will be randomly pitched.



    //Doing the same "Singleton thing as we did with the GameManager to make sure thetre is only one of these
    void Awake()
    {
        //Check if there is already an instance of SoundManager
        if (instance == null)
            //if not, set it to this.
            instance = this;
        //If instance already exists:
        else if (instance != this)
            //Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
            Destroy(gameObject);

        //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);





        //This isnt quite adding up right, but seems to work fine....

        //Since start y is 8, and bpm is 120, the chords are travelling at 2y unit per sec. 
        //so this gives 1 sec for first 4 y units and .75 for 3 more, then adds the one bar of silence at the 
        //begining of the song to total 8 y units.
        musicSource.PlayDelayed(1.75f);
    }



    


    //Might use this for hitting target or soemthing.

    ////This is just to give a litle variation, so the clips we play over and over dont get toooooo repititive and/or annoying.
    ////RandomizeSfx chooses randomly between various audio clips and slightly changes their pitch.
    //public void RandomizeSfx(params AudioClip[] clips)
    //{
    //    //Generate a random number between 0 and the length of our array of clips passed in.
    //    int randomIndex = Random.Range(0, clips.Length);

    //    //Choose a random pitch to play back our clip at between our high and low pitch ranges.
    //    float randomPitch = Random.Range(lowPitchRange, highPitchRange);

    //    //Set the pitch of the audio source to the randomly chosen pitch.
    //    efxSource.pitch = randomPitch;

    //    //Set the clip to the clip at our randomly chosen index.
    //    efxSource.clip = clips[randomIndex];

    //    //Play the clip.
    //    efxSource.Play();
    //}


}
