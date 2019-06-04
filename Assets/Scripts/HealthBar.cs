using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public PlayerHolder player;
    public Slider healthSlider;
    public Slider damageSlider;
    public TMPro.TextMeshProUGUI healthText;
    public TMPro.TextMeshProUGUI maxHealthText;

    private IEnumerator OnDamageCoroutine;

    private void Start()
    {
        player.stats.OnDamage.AddListener(PlayDamage);
        player.stats.OnHeal.AddListener(PlayHeal);
        player.stats.OnChangeHealth.AddListener(PlayHealthChange);
        healthSlider.maxValue = player.stats.maxHealth;
        healthSlider.value = player.stats.GetHealth();
        damageSlider.maxValue = player.stats.maxHealth;
        damageSlider.normalizedValue = player.stats.GetHealth();
        maxHealthText.text = player.stats.maxHealth.ToString();
        healthText.text = player.stats.GetHealth().ToString();
    }

    private void PlayHealthChange(int count)
    {
        healthSlider.value = count;
        healthText.text = player.stats.GetHealth().ToString();
    }

    private void PlayHeal(HealInfo info)
    {
        healthSlider.value += info.count;
    }

    private void PlayDamage(DamageInfo info)
    {
        damageSlider.value = healthSlider.value;
        healthSlider.value -= info.count;

        if (OnDamageCoroutine != null)
            StopCoroutine(OnDamageCoroutine);
        StartCoroutine(OnDamageCoroutine = OnDamage());
    }

    private IEnumerator OnDamage()
    {
        yield return new WaitForSeconds(0.2F);
        while (damageSlider.value > healthSlider.value)
        {
            damageSlider.value -= player.stats.maxHealth / 100.0F;
            yield return new WaitForSeconds(0.01F);
        }
        damageSlider.value = healthSlider.value;
    }
}
