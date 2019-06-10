using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public PlayerHolder player;
    [Header("Health Bar")]
    public Slider healthSlider;
    public TMPro.TextMeshProUGUI healthText;
    public TMPro.TextMeshProUGUI maxHealthText;
    public float addSpeed = 1.0F;
    [Header("Damage Bar")]
    public Slider damageSlider;
    public Graphic damageGraphic;
    public float stunDamageTime = 0.2F;
    public float substractSpeed = 1.0F;
    [Header("Steps")]
    [SerializeField]
    private int smallStepSize = 100;
    public Graphic smallStepsRenderer;
    [SerializeField]
    private int bigStepSize = 1000;
    public Graphic bigStepsRenderer;

    private Color m_damageColor;
    private IEnumerator HealthUpdateCoroutine;

    public void UpdateHealthValues()
    {
        if (healthSlider)
        {
            healthSlider.maxValue = player.stats.maxHealth;
            healthSlider.value = player.stats.GetHealth();
        }
        if (damageSlider)
        {
            damageSlider.maxValue = player.stats.maxHealth;
            damageSlider.value = player.stats.GetHealth();
        }
        if (maxHealthText)
        {
            maxHealthText.text = player.stats.maxHealth.ToString();
        }
        if (healthText)
        {
            healthText.text = player.stats.GetHealth().ToString();
        }
        if (smallStepsRenderer)
        {
            smallStepsRenderer.material.SetFloat("_MaxCount", player.stats.maxHealth);
            smallStepsRenderer.material.SetFloat("_Step", smallStepSize);
        }
        if (bigStepsRenderer)
        {
            bigStepsRenderer.material.SetFloat("_MaxCount", player.stats.maxHealth);
            bigStepsRenderer.material.SetFloat("_Step", bigStepSize);
        }
    }

    private void Start()
    {
        player.stats.OnDamage.AddListener(PlayDamage);
        player.stats.OnHeal.AddListener(PlayHeal);
        player.stats.OnChangeHealth.AddListener(PlayHealthChange);
        if (smallStepsRenderer)
            smallStepsRenderer.material = new Material(smallStepsRenderer.material);
        if (bigStepsRenderer)
            bigStepsRenderer.material = new Material(bigStepsRenderer.material);
        if (damageGraphic)
        {
            damageGraphic.material = new Material(damageGraphic.material);
            m_damageColor = damageGraphic.material.color;
        }
        UpdateHealthValues();
    }

    private void PlayHealthChange(int count)
    {
        UpdateHealthValues();
    }

    private void PlayHeal(HealInfo info)
    {
        if (!healthSlider) UpdateHealthValues();
        if (HealthUpdateCoroutine != null)
            StopCoroutine(HealthUpdateCoroutine);
        StartCoroutine(HealthUpdateCoroutine = OnHeal());
    }

    private void PlayDamage(DamageInfo info)
    {
        float damageSliderInitialSize = healthSlider ? healthSlider.value : 0.0F;
        UpdateHealthValues();

        if (!damageSlider) return;
        damageSlider.value = damageSliderInitialSize;
        healthSlider.value = player.stats.GetHealth();

        if (HealthUpdateCoroutine != null)
            StopCoroutine(HealthUpdateCoroutine);
        StartCoroutine(HealthUpdateCoroutine = OnDamage());
    }

    private IEnumerator OnDamage()
    {
        if (damageGraphic)
        {
            float stunDamageTimePassed = stunDamageTime;
            while (stunDamageTimePassed > 0)
            {
                stunDamageTimePassed -= Time.deltaTime;
                damageGraphic.material.color =
                    m_damageColor + (new Color(1, 1, 1, 0) * Mathf.Lerp(0, 0.8F, stunDamageTimePassed % (stunDamageTime / 1.5F) / stunDamageTime));
                yield return new WaitForSeconds(Time.deltaTime);
            }
        } else
        {
            yield return new WaitForSeconds(stunDamageTime);
        }
        while (damageSlider.value > healthSlider.value)
        {
            damageSlider.value -= player.stats.maxHealth * substractSpeed * (Time.deltaTime * 100);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        UpdateHealthValues();
    }

    private IEnumerator OnHeal()
    {
        float currentHealth = healthSlider.value;
        UpdateHealthValues();
        while (currentHealth < player.stats.GetHealth())
        {
            currentHealth += player.stats.maxHealth * addSpeed * (Time.deltaTime * 100);
            currentHealth = Mathf.Min(currentHealth, player.stats.GetHealth());
            if (damageSlider)
                damageSlider.value = currentHealth;
            if (healthSlider)
                healthSlider.value = currentHealth;
            if (healthText)
                healthText.text = ((int)currentHealth).ToString();
            yield return new WaitForSeconds(Time.deltaTime);
        }
        UpdateHealthValues();
    }
}
