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
    [Header("Damage Bar")]
    public Slider damageSlider;
    public Image damageImage;
    public float stunDamageTime = 0.2F;
    public float dividendStepSize = 60.0F;
    [Header("Steps")]
    [SerializeField]
    private int smallStepSize = 100;
    public Image smallStepsRenderer;
    [SerializeField]
    private int bigStepSize = 1000;
    public Image bigStepsRenderer;

    private Color m_damageColor;
    private IEnumerator OnDamageCoroutine;

    public void UpdateHealthValues()
    {
        healthSlider.maxValue = player.stats.maxHealth;
        healthSlider.value = player.stats.GetHealth();
        damageSlider.maxValue = player.stats.maxHealth;
        damageSlider.value = player.stats.GetHealth();
        maxHealthText.text = player.stats.maxHealth.ToString();
        healthText.text = player.stats.GetHealth().ToString();
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
        if (damageImage)
        {
            damageImage.material = new Material(damageImage.material);
            m_damageColor = damageImage.material.color;
        }
        UpdateHealthValues();
    }

    private void PlayHealthChange(int count)
    {
        UpdateHealthValues();
    }

    private void PlayHeal(HealInfo info)
    {
        UpdateHealthValues();
    }

    private void PlayDamage(DamageInfo info)
    {
        float damageSliderInitialSize = healthSlider.value;

        UpdateHealthValues();
        damageSlider.value = damageSliderInitialSize;
        healthSlider.value = player.stats.GetHealth();

        if (OnDamageCoroutine != null)
            StopCoroutine(OnDamageCoroutine);
        StartCoroutine(OnDamageCoroutine = OnDamage());
    }

    private IEnumerator OnDamage()
    {
        float stunDamageTimePassed = stunDamageTime;
        while (stunDamageTimePassed > 0)
        {
            stunDamageTimePassed -= Time.deltaTime;
            damageImage.material.color =
                m_damageColor + (new Color(1, 1, 1, 0) * Mathf.Lerp(0, 0.8F, stunDamageTimePassed % (stunDamageTime / 1.5F) / stunDamageTime));
            yield return new WaitForSeconds(Time.deltaTime);
        }
        while (damageSlider.value > healthSlider.value)
        {
            damageSlider.value -= player.stats.maxHealth / dividendStepSize * (Time.deltaTime * 100);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        UpdateHealthValues();
    }
}
