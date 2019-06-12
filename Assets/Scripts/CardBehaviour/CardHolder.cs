using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHolder : MonoBehaviour
{
    public BaseCard card;

    public AnimationCurves transformAnimationCurves;
    public PlayerHolder player;

    private float m_transitionSpeed = 1.0F;
    private Vector3 m_targetPosition;
    private Quaternion m_targetRotation;
    private Vector3 m_startPosition;
    private Quaternion m_startRotation;
    [Range(0.0F, 1.0F)]
    private float m_animationKeyPosition;
    private string m_animationName;

    private void Awake()
    {
        card = GetComponent<BaseCard>();
    }

    private void Update()
    {
        if (m_animationKeyPosition < 1.0F)
        {
            AnimationCurve animationCurve = transformAnimationCurves.GetCurveByName(m_animationName);
            m_animationKeyPosition += Time.deltaTime * m_transitionSpeed;
            transform.position = Vector3.Lerp(m_startPosition, m_targetPosition, animationCurve.Evaluate(m_animationKeyPosition));
            transform.rotation = Quaternion.Lerp(m_startRotation, m_targetRotation, animationCurve.Evaluate(m_animationKeyPosition));
        }
    }

    public void UpdatePath(Vector3 targetPosition, Quaternion targetRotation, string animationName, float transitionSpeed = 1.0F)
    {
        m_animationName = animationName;
        m_startRotation = transform.rotation;
        m_startPosition = transform.position;
        m_targetPosition = targetPosition;
        m_targetRotation = targetRotation;
        m_animationKeyPosition = 0;
        m_transitionSpeed = transitionSpeed;
    }
}
