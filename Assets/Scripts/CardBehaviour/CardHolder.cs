using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHolder : MonoBehaviour
{
    public BaseCard card;

    public AnimationCurves transformAnimationCurves;
    public PlayerHolder player;

    private float m_transitionSpeed = 1.0F;
    private float m_scaleSpeed = 1.0F;
    private Vector3 m_targetPosition;
    private Quaternion m_targetRotation;
    private Vector3 m_targetScale = Vector3.one;
    private Vector3 m_startPosition;
    private Quaternion m_startRotation;
    private Vector3 m_startScale;
    [Range(0.0F, 1.0F)]
    private float m_translationKeyPosition;
    [Range(0.0F, 1.0F)]
    private float m_scaleKeyPosition;
    private string m_translateAnimationName;
    private string m_scaleAnimationName;

    private void Awake()
    {
        card = GetComponent<BaseCard>();
    }

    private void Update()
    {
        if (m_translationKeyPosition < 1.0F)
        {
            AnimationCurve animationCurve = transformAnimationCurves.GetCurveByName(m_translateAnimationName);
            m_translationKeyPosition += Time.deltaTime * m_transitionSpeed;
            transform.position = Vector3.Lerp(m_startPosition, m_targetPosition, animationCurve.Evaluate(m_translationKeyPosition));
            transform.rotation = Quaternion.Lerp(m_startRotation, m_targetRotation, animationCurve.Evaluate(m_translationKeyPosition));
        }
        if (m_scaleKeyPosition < 1.0F)
        {
            AnimationCurve animationCurve = transformAnimationCurves.GetCurveByName(m_scaleAnimationName);
            m_scaleKeyPosition += Time.deltaTime * m_scaleSpeed;
            transform.localScale = Vector3.Lerp(m_startScale, m_targetScale, animationCurve.Evaluate(m_scaleKeyPosition));
        }
    }

    public void Hide()
    {
        card.costText.enabled = false;
        card.descriptionText.enabled = false;
        card.nameText.enabled = false;
        card.backImage.enabled = false;
        card.mainImage.enabled = false;
    }

    public void Show()
    {
        card.costText.enabled = true;
        card.descriptionText.enabled = true;
        card.nameText.enabled = true;
        card.backImage.enabled = true;
        card.mainImage.enabled = true;
    }

    public void UpdateScale(Vector3 targetScale, string animationName, float transitionSpeed = 1.0F)
    {
        m_scaleAnimationName = animationName;
        m_startScale = transform.localScale;
        m_targetScale = targetScale;
        m_scaleKeyPosition = 0;
        m_scaleSpeed = transitionSpeed;
    }

    public void UpdatePath(Vector3 targetPosition, Quaternion targetRotation, string animationName, float transitionSpeed = 1.0F)
    {
        m_translateAnimationName = animationName;
        m_startRotation = transform.rotation;
        m_startPosition = transform.position;
        m_targetPosition = targetPosition;
        m_targetRotation = targetRotation;
        m_translationKeyPosition = 0;
        m_transitionSpeed = transitionSpeed;
    }
}
