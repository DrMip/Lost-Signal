using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarFuncs : MonoBehaviour
{
    public Slider healthslider;
    public Slider backgroundHealthSlider;
    public Slider jetslider;

    public Slider wrathslider;
    public Slider backgroundWrathSlider;
    public float delayseconds =0.5f;
    public float bgSpeed = 0.1f;   

    float currentHealth;
    float currentWrath;

    bool crHealth_running;
    bool crWrath_running;




    public void SetMaxHealth(int maxHealth)
    {
        healthslider.maxValue = maxHealth;
        healthslider.value = maxHealth;
        backgroundHealthSlider.maxValue = maxHealth;
        backgroundHealthSlider.value = maxHealth;
    }
    public void SetHealth(int health)
    {
        healthslider.value = health;
        currentHealth = health;
        if(!crHealth_running)
            StartCoroutine(SetHealthInterior(health));
    }
    private IEnumerator SetHealthInterior(int health)
    {
        
        healthslider.value = health;
        if(crHealth_running == false)
            yield return new WaitForSeconds(delayseconds);
        crHealth_running = true;
        while(backgroundHealthSlider.value > currentHealth)
        {
            backgroundHealthSlider.value -= bgSpeed;
            yield return null;

        }
        backgroundHealthSlider.value = currentHealth;
        crHealth_running = false;

    }


    public void SetMaxWrath(int MaxWrath)
    {
        wrathslider.maxValue = MaxWrath;
        wrathslider.value = 0;
        backgroundWrathSlider.maxValue = MaxWrath;
        backgroundWrathSlider.value = 0;
    }
    public void SetWrath(int wrath)
    {
        wrathslider.value = wrath;
        currentWrath = wrath;
        if(!crWrath_running)
            StartCoroutine(SetWrathInterior(wrath));
    }
    private IEnumerator SetWrathInterior(int wrath)
    {
        
        wrathslider.value = wrath;
        if(crWrath_running == false)
            yield return new WaitForSeconds(delayseconds);
        crWrath_running = true;
        while(backgroundWrathSlider.value > currentWrath)
        {
            backgroundWrathSlider.value -= bgSpeed;
            yield return null;

        }
        backgroundWrathSlider.value = currentWrath;
        crWrath_running = false;

    }
    public void SetMaxJet(float maxValue)
    {
        jetslider.maxValue = maxValue;
        jetslider.value = maxValue;
    }
    public void SetJet(float jet)
    {
        jetslider.value = jet;
    }



}
