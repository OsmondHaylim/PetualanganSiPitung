using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinishLevelManager : MonoBehaviour
{
    public RectTransform finishLevelNotification;
    public RectTransform finishLevelText;
    public RectTransform finishLevelDisplayPosition;
    public Image fadeOutPanel;
    public string sceneToLoad;
    public string key;
    public int level;
    private GameManager gameManager;


    public void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        fadeOutPanel.gameObject.SetActive(false);
        finishLevelDisplayPosition.gameObject.SetActive(false);
    }

    public IEnumerator OnLevelCompleted()
    {
        float step = 0.5f * Time.fixedDeltaTime;

        for (float t = 0; t <= 1.0f; t += step) {
            finishLevelNotification.transform.position = Vector3.Lerp(finishLevelNotification.transform.position, finishLevelDisplayPosition.transform.position, t);
            yield return new WaitForEndOfFrame();
        }
        step = 1.5f * Time.fixedDeltaTime;
        Vector2 prevSize = new Vector2(5, 100);
        Vector2 newSize = new Vector2(400, 100);
        for (float t = 0; t <= 1.0f; t += step) {
            finishLevelNotification.sizeDelta = Vector2.Lerp(prevSize, newSize, t);
            finishLevelText.sizeDelta = Vector2.Lerp(prevSize, newSize, t);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(3);

        fadeOutPanel.gameObject.SetActive(true);

        step = 0.6f * Time.fixedDeltaTime;
        Color prevColor = new Color(0, 0, 0, 0);
        Color newColor = new Color(0, 0, 0, 1);

        for (float t = 0; t <= 1.0f; t += step) {
            Color transitionColor = Color.Lerp(prevColor, newColor, t);
            fadeOutPanel.color = transitionColor;
            yield return new WaitForEndOfFrame();
        }
        Cursor.lockState = CursorLockMode.None;
        if (PlayerPrefs.GetInt("Level") < level){
            PlayerPrefs.SetInt("Level", level);
        }
        if (PlayerPrefs.GetInt(key) < gameManager.currentWine){
            PlayerPrefs.SetInt(key, gameManager.currentWine);
        }
        SceneManager.LoadScene(sceneToLoad);
    }
}

