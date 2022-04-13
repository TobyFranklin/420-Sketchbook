using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SongPlayer : MonoBehaviour
{

    public AudioClip[] playlist;
    private AudioSource player;

    private int currentTrack;
    // Start is called before the first frame update

    void Start()
    {
        player = GetComponent<AudioSource>();
        PlayTrackRandom();
    }

    public void PlayTrack(int n){

        if(n < 0 || n >= playlist.Length) return;
        player.PlayOneShot(playlist[n]);    
    }

    public void PlayTrackRandom(){
        int r = Random.Range(0, playlist.Length);
        Debug.Log(r);
        PlayTrack(r);
    }

    public void PlayTrackNext(){
        int track = currentTrack + 1;

        if(track >= playlist.Length) track = 0;

        PlayTrack(track);
    }

    // Update is called once per frame
    void Update()
    {
        if(!player.isPlaying){
            PlayTrackNext();
        }
    }
}

//[CustomEditor(typeof(SongPlayer))]

//public class SongPlayer : MonoBehaviour{

//}