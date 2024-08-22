using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    private float health;
    private float lerpTimer;

    [Header("Health Bar")]
    public float maxHealth = 100;
    public float chipSpeed = 2f;
    public Image frontHealthBar;
    public Image backHealthBar;
    public TextMeshProUGUI healthText;

    [Header("Health Overlay")]
    public Image overlay;
    public float duration;
    public float fadeSpeed;
    private float durationTimer;

    [Header("Game Over")]
    public GameObject gameOverScreen;

    void Start()
    {
        health = maxHealth;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);
        gameOverScreen.SetActive(false); 
    }

    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();

        if (overlay.color.a > 0)
        {
            if (health < 30)
                return;

            durationTimer += Time.deltaTime;
            if (durationTimer > duration)
            {
                float tempAlpha = overlay.color.a;
                tempAlpha -= fadeSpeed * Time.deltaTime;
                overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, tempAlpha);
            }
        }

        if (health <= 0)
        {
           // Debug.Log("Health is 0 or less, triggering GameOver()"); 
            GameOver();
        }
    }

    public void UpdateHealthUI()
    {
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFraction = health / maxHealth;

        if (fillB > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }

        if (fillF < hFraction)
        {
            backHealthBar.color = Color.green;
            backHealthBar.fillAmount = hFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, backHealthBar.fillAmount, percentComplete);
        }
    }

        public void TakeDamage(float damage)
        {
            //Debug.Log("Taking damage: " + damage); // Log the damage amount
            health -= damage;
            health = Mathf.Clamp(health, 0, maxHealth); 
          // Debug.Log("Current Health after damage: " + health); 
            if (health <= 0)
            {
                //Debug.Log("Health has dropped to 0 or below inside TakeDamage()");
                GameOver();
            }
            lerpTimer = 0;
            durationTimer = 0;
            overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 1);
        }

    public void RestoreHealth(float healAmount)
    {
        health += healAmount;
      //  Debug.Log("Restoring health: " + healAmount); 
       // Debug.Log("Current Health after healing: " + health); 
        lerpTimer = 0f;
    }

    void GameOver()
    {
        Debug.Log("Game Over triggered"); 
        gameOverScreen.SetActive(true); 
      //  Debug.Log("Game Over screen should be visible now");
        Time.timeScale = 0f; 
    }

}
