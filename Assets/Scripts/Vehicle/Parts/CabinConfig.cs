using UnityEngine;

namespace WinterUniverse
{
    [CreateAssetMenu(fileName = "Cabin", menuName = "Winter Universe/Vehicle/New Cabin")]
    public class CabinConfig : VehiclePartConfig
    {
        [SerializeField] private float _accelerationForce = 5000f;
        [SerializeField] private float _decelerationForce = 1000f;
        [SerializeField] private float _maxForwardSpeed = 50f;
        [SerializeField] private float _maxBackwardSpeed = 25f;
        [SerializeField] private AnimationCurve _accelerationCurve;
        [SerializeField] private AnimationCurve _decelerationCurve;
        [SerializeField] private AudioClip _engineClip;

        public float AccelerationForce => _accelerationForce;
        public float DecelerationForce => _decelerationForce;
        public float MaxForwardSpeed => _maxForwardSpeed;
        public float MaxBackwardSpeed => _maxBackwardSpeed;
        public AnimationCurve AccelerationCurve => _accelerationCurve;
        public AnimationCurve DecelerationCurve => _decelerationCurve;
        public AudioClip EngineClip => _engineClip;
    }
}