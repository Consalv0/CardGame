using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimationCurves", menuName = "Animation/Animation Curves")]
public class AnimationCurves : ScriptableObject
{
    [System.Serializable]
    public struct Curve
    {
        public string name;
        public AnimationCurve animationCurve;
    }

    public List<Curve> animationCurves;

    public AnimationCurve GetCurveByName(string name)
    {
        if (animationCurves.Exists(curve => curve.name == name))
        {
            Curve animCurve = animationCurves.Find(curve => curve.name == name);
            return animCurve.animationCurve;
        } else
        {
            return new AnimationCurve();
        }
    }
}
