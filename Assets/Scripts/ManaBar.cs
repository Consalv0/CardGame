using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    public PlayerHolder player;
    [Header("Mana Bar")]
    public Slider manaSlider;
    public TMPro.TextMeshProUGUI manaText;
    public TMPro.TextMeshProUGUI maxManaText;
    [Header("Steps")]
    [SerializeField]
    private int smallStepSize = 1;
    public Graphic smallStepsRenderer;
    
    private IEnumerator ManaUpdateCoroutine;

    public void UpdateHealthValues()
    {
        if (manaSlider)
        {
            manaSlider.maxValue = player.maxMana;
            manaSlider.value = player.mana;
        }
        if (maxManaText)
        {
            maxManaText.text = player.maxMana.ToString();
        }
        if (manaText)
        {
            manaText.text = player.mana.ToString();
        }
        if (smallStepsRenderer)
        {
            smallStepsRenderer.material.SetFloat("_MaxCount", player.maxMana);
            smallStepsRenderer.material.SetFloat("_Step", smallStepSize);
        }
    }

    private void Start()
    {
        player.OnChangeMana.AddListener(PlayManaChange);
        if (smallStepsRenderer)
            smallStepsRenderer.material = new Material(smallStepsRenderer.material);
        UpdateHealthValues();
    }

    private void PlayManaChange(int count)
    {
        UpdateHealthValues();
    }
}
