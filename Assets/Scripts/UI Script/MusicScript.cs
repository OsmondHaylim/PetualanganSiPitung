using UnityEngine;

public class MusicScript : MonoBehaviour{
    public static MusicScript instance;
    public AudioClip audioClip;
    private AudioSource audioSource;

    private void Awake(){
        if (instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = FindObjectOfType<AudioSource>();
            audioSource.clip = audioClip;
            audioSource.Play();
        }else{
            if (instance.audioSource == null){
                instance.audioSource = FindObjectOfType<AudioSource>();
            }
            if (instance.audioSource.clip != audioClip){
                instance.audioSource.clip = audioClip;
                instance.audioSource.Play();
            }
            Destroy(gameObject);
        }
    }   
}