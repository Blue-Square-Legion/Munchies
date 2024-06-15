using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimationSO
{
    [CreateAssetMenu(fileName = "AnimationCurve", menuName = "Animation Curve")]
    public class AnimationCurveSO : ScriptableObject
    {
        public AnimationCurve curve;

        public float Evaluate(float time)
        {
            return curve.Evaluate(time);
        }
    }
}

