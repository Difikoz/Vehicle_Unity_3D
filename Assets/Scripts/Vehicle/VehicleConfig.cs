using UnityEngine;

namespace WinterUniverse
{
    [CreateAssetMenu(fileName = "Vehicle", menuName = "Winter Universe/Vehicle/New Vehicle")]
    public class VehicleConfig : BasicInfoConfig
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private float _accelerationForce;
        [SerializeField] private float _decelerationForce;
        [SerializeField] private float _brakeForce;
        [SerializeField] private float _maxForwardSpeed;
        [SerializeField] private float _maxBackwardSpeed;
        [SerializeField] private float _turnAngle;
        [SerializeField] private float _turnSpeed;
        [SerializeField] private AnimationCurve _accelerationCurve;
        [SerializeField] private AnimationCurve _decelerationCurve;
        [SerializeField] private AnimationCurve _turnCurve;

        public GameObject Prefab => _prefab;
        public float AccelerationForce => _accelerationForce;
        public float DecelerationForce => _decelerationForce;
        public float BrakeForce => _brakeForce;
        public float MaxForwardSpeed => _maxForwardSpeed;
        public float MaxBackwardSpeed => _maxBackwardSpeed;
        public float TurnAngle => _turnAngle;
        public float TurnSpeed => _turnSpeed;
        public AnimationCurve AccelerationCurve => _accelerationCurve;
        public AnimationCurve DecelerationCurve => _decelerationCurve;
        public AnimationCurve TurnCurve => _turnCurve;
    }
}