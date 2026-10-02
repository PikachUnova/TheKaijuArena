using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHandler : MonoBehaviour
{
    public static UIHandler handler;
    
    // Health
    public TMP_Text healthText;
    private int currentHealth = 0;
    public CharacterStats stats;
    public Slider healthBar;
    public Gradient gradient;
    public Image fill;


    // Energy Points
    private int maxEnergyPoints = 10;
    [SerializeField] private int energyPoints = 10;
    public Slider energyBar;
    private float energyTimer = 0.0f;
    private float secondsPerEnergy = 2.0f;

    // Special Points
    private int maxSpecialPoints = 30;
    [SerializeField] private int specialPoints = 0;
    public Slider specialBar;
    public GameObject specialAttacksPanel;



    // Weapon Icons
    public RawImage currentIcon;
    public Texture2D [] textureIcons;

    //Fade
    public CanvasGroup canvasGroup;
    private float fadeDuration = 0.3f;

    void Start()
    {
        currentHealth = stats.maxHealth;
        healthBar.maxValue = currentHealth;
        fill.color = gradient.Evaluate(1f);

        energyPoints = maxEnergyPoints;
        energyBar.maxValue = maxEnergyPoints;
        energyBar.value = maxEnergyPoints;

        specialBar.maxValue = maxSpecialPoints;
        specialBar.value = 0; // start from 0 points

        if (UIHandler.handler != null)
        {
            Destroy(this.gameObject);
            return;
        }
        handler = this;
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        healthBar.value = currentHealth;
        fill.color = gradient.Evaluate(healthBar.normalizedValue);
        healthText.text = "Rex " + currentHealth;

        energyBar.value = energyPoints;
        specialBar.value = specialPoints;

        if (energyPoints < maxEnergyPoints)
            energyTimer += Time.deltaTime; 
        if (energyTimer >= secondsPerEnergy) 
        {
            energyPoints++;
            energyTimer -= secondsPerEnergy; 
        }

    }

    public int GetHP()
    {
        return currentHealth;
    }
    public void SetHP(int hp)
    {
        currentHealth = hp;
    }

    public int GetEnergyPoints()
    {
        return energyPoints;
    }
    public void SetEnergyPoints(int points)
    {
        energyPoints = points;
    }
    public void UseEnergyPoints()
    {
        energyPoints -= 1;
    }


    public void UpdateSpecialPoints(int points)
    {
        specialPoints += points;

        if (specialPoints > maxSpecialPoints) // Do not exceed the maximum points
            specialPoints = maxSpecialPoints;

        if (specialPoints < 0) // No negative points
            specialPoints = 0;
    }

    public bool CheckSpecialPoints(int required)
    {
        if (specialPoints >= required)
            return true;
        return false;
    }

    public void SetPanelActive()
    {
        if (!specialAttacksPanel.activeSelf)
        {
            specialAttacksPanel.SetActive(true);
            Time.timeScale = 0.2f;
        }
        else
        {
            specialAttacksPanel.SetActive(false);
            Time.timeScale = 1.0f;
        }
    }


    public void FadeIn()
    {
        StartCoroutine(Fade(canvasGroup, canvasGroup.alpha, 0f, fadeDuration));
    }
    public void FadeOut()
    {
        StartCoroutine(Fade(canvasGroup, canvasGroup.alpha, 1f, fadeDuration));
    }

    private IEnumerator Fade(CanvasGroup cg, float start , float end, float duration)
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            cg.alpha = Mathf.Lerp(start, end, elapsedTime / duration);
            yield return null;
        }
        cg.alpha = end;
    }

}
