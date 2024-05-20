using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int currentWine;
    public int currentHealth;
    public int maxHealth;
    public int fallDamage;
    public bool startAtMaxHealth;
    public bool deathFall;
    public float yLimit;
    public float invicibilityTime;
    private float invicibilityCounter;
    public float flashTime = 0.1f;
    private float flashCounter;
    private bool isRespawning;
    private bool isPaused = false;
    private Vector3 respawnPoint; 
    public float respawnTime;
    private bool fade;
    private bool emerge;
    public float fadeSpeed;
    public float fadeWaitTime;
    private float originalTimeScale;

    public PlayerController monitoredPlayer;
    public TextMeshProUGUI wineText;
    public TextMeshProUGUI healthText;
    public Renderer playerRenderer;
    public GameObject respawner;
    public ParticleSystem deathEffect;
    public Image blackScreen;
    public GameObject PauseMenu;
    public GameObject DeathMenu;
    public CameraController cameraController;


    void Start()
    {
        isPaused = false;
        if(monitoredPlayer == null){
            monitoredPlayer = FindObjectOfType<PlayerController>();
        }
        if (startAtMaxHealth){
            currentHealth = maxHealth;
        }
        healthText.text = "Health : " + currentHealth + "/" + maxHealth;
        if(respawner == null){
            respawnPoint = monitoredPlayer.transform.position;
        }else{
            respawnPoint = respawner.transform.position;
        }
        originalTimeScale = Time.timeScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(invicibilityCounter > 0){
            invicibilityCounter -= Time.deltaTime;
            flashCounter -= Time.deltaTime;

            if(flashCounter <= 0){
                playerRenderer.enabled = !playerRenderer.enabled;
                flashCounter = flashTime;
            }

            if(invicibilityCounter <= 0){
                playerRenderer.enabled = true;
            }
        }
        if(!isRespawning && monitoredPlayer.controller.transform.position.y <= yLimit){
            if(deathFall){
                currentHealth = 0;
            }else{
                currentHealth -= fallDamage;
            }
            
            healthText.text = "Health : " + currentHealth + "/" + maxHealth;
             
            playerRenderer.enabled = false;
            flashCounter = flashTime;
            Respawn();
            invicibilityCounter = respawnTime + 1; 
        }
        
        if(emerge){
            blackScreen.color =  new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 1f, fadeSpeed * Time.deltaTime));
            if(blackScreen.color.a == 1f){
                emerge = false;
            }
        }

        if(fade){
            blackScreen.color =  new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 0f, fadeSpeed * Time.deltaTime));
            if(blackScreen.color.a == 0f){
                fade = false;
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape)){
            Pause();
        }
    }

    public void AddMore(int wineAmount){
        currentWine += wineAmount;
        wineText.text = "Wine : " + currentWine;
    }
    public void HurtPlayer(int damageGiven, float timeKnocked, float force, Vector3 direction){
        if(invicibilityCounter <= 0){
            currentHealth -= damageGiven;
            healthText.text = "Health : " + currentHealth + "/" + maxHealth;
            if(currentHealth <= 0){
                Respawn();
            }else{
                monitoredPlayer.anim.SetTrigger("Hurt");
                monitoredPlayer.KnockBack(timeKnocked, direction, force);
                invicibilityCounter = invicibilityTime;
                
                playerRenderer.enabled = false;
                flashCounter = flashTime;
                
            }
            
        }
    }

    public void HealPlayer(int heal){
        currentHealth += heal;
        if (currentHealth > maxHealth){
            currentHealth = maxHealth;
            healthText.text = "Health : " + currentHealth + "/" + maxHealth;
        }
    }

    public void Respawn(){
        if(!isRespawning){
            StartCoroutine("RespawnCo");
        }
    }

    public void Restart(){
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Death(){
        Cursor.lockState = CursorLockMode.None;
        originalTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        DeathMenu.SetActive(true);
        if (cameraController != null){
            cameraController.enabled = false;
        }
    }
    public void Pause(){
        isPaused = !isPaused;
        if(isPaused){
            Cursor.lockState = CursorLockMode.None;
            originalTimeScale = Time.timeScale;
            Time.timeScale = 0f;
            PauseMenu.SetActive(true);
            if (cameraController != null){
                cameraController.enabled = false;
            }
        }else{
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = originalTimeScale;
            PauseMenu.SetActive(false);
            if (cameraController != null){
                cameraController.enabled = true;
            }
        }
    }

    public IEnumerator RespawnCo(){
        isRespawning = true;
        if(monitoredPlayer.controller.transform.position.y > yLimit){
            monitoredPlayer.controller.enabled = false;
            Instantiate(deathEffect, monitoredPlayer.controller.transform.position, monitoredPlayer.controller.transform.rotation);
            monitoredPlayer.anim.SetTrigger("Dead");
            yield return new WaitForSeconds(respawnTime);
            
            monitoredPlayer.gameObject.SetActive(false);
        }else{
            monitoredPlayer.gameObject.SetActive(false);
            Instantiate(deathEffect, monitoredPlayer.controller.transform.position, monitoredPlayer.controller.transform.rotation);
            yield return new WaitForSeconds(respawnTime);
        }

        emerge = true;
        yield return new WaitForSeconds(fadeWaitTime);
        if(currentHealth <= 0){
            Cursor.lockState = CursorLockMode.None;
            originalTimeScale = Time.timeScale;
            Time.timeScale = 0f;
            DeathMenu.SetActive(true);
            if (cameraController != null){
                cameraController.enabled = false;
            }
        }else{
            emerge = false;
            fade = true;
            
            isRespawning = false;

            monitoredPlayer.controller.enabled = false;
            monitoredPlayer.transform.position = respawnPoint;
            monitoredPlayer.controller.enabled = true;
            currentHealth = maxHealth;
            healthText.text = "Health : " + currentHealth + "/" + maxHealth;
            monitoredPlayer.gameObject.SetActive(true);

            invicibilityCounter = invicibilityTime;
            playerRenderer.enabled = false;
            flashCounter = flashTime;
        }
        
        
    }
}
