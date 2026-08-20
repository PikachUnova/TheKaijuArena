using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHandler : MonoBehaviour
{
    public static UIHandler handler;
    
    public TMP_Text healthText;
    public int health = 100;
    public CharacterStats stats;
    public Slider healthBar;

    public Gradient gradient;
    public Image fill;

    public CanvasGroup canvasGroup;
    private float fadeDuration = 0.3f;

    void Start()
    {
        health = stats.maxHealth;
        healthBar.maxValue = health;
        fill.color = gradient.Evaluate(1f);

        if (UIHandler.handler != null)
        {
            Destroy(this.gameObject);
            return;
        }
        handler = this;
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = health;
        fill.color = gradient.Evaluate(healthBar.normalizedValue);
        healthText.text = "Rex " + health;
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
