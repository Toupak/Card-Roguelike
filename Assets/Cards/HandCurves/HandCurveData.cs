using UnityEngine;

namespace Cards.HandCurves
{
    [CreateAssetMenu(fileName = "HandCurveData", menuName = "Scriptable Objects/HandCurveData")]
    public class HandCurveData : ScriptableObject
    {
        public AnimationCurve positioning;
        public float positioningInfluence;
        public AnimationCurve rotation;
        public float rotationInfluence;
    }
}
