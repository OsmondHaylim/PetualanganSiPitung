using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public string sceneToLoad; 
    public AudioClip buttonClickSound;
    private AudioSource audioSource;
    private void Start(){
        audioSource = GetComponent<AudioSource>();
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }
    public void OnClick(){
        if (buttonClickSound != null && audioSource != null)
        {
            audioSource.clip = buttonClickSound;
            audioSource.Play();
        }
        StartCoroutine(LoadSceneAfterSound(0.5f));
        LoadScene();
    }

    IEnumerator LoadSceneAfterSound(float delay)
    {
        // Wait for the length of the audio clip before loading the scene.
        yield return new WaitForSeconds(delay);
        LoadScene();
    }

    void LoadScene()
    {
        // Load the specified scene.
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            Time.timeScale = 1f; 
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Quit();
        }
    }
    private void Quit(){
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
