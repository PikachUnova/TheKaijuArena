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

    void Start()
    {
        healthBar.maxValue = 100;
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
}
