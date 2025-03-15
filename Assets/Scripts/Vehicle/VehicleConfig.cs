using UnityEngine;

namespace WinterUniverse
{
    [CreateAssetMenu(fileName = "Vehicle", menuName = "Winter Universe/Vehicle/New Vehicle")]
    public class VehicleConfig : BasicInfoConfig
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private float _accelerationForce;
        [SerializeField] private float _decelerationForce;
        [SerializeField] private float _maxForwardSpeed;
        [SerializeField] private float _maxBackwardSpeed;
        [SerializeField] private float _turnAngle;
        [SerializeField] private float _turnSpeed;

        public GameObject Prefab => _prefab;
        public float AccelerationForce => _accelerationForce;
        public float DecelerationForce => _decelerationForce;
        public float MaxForwardSpeed => _maxForwardSpeed;
        public float MaxBackwardSpeed => _maxBackwardSpeed;
        public float TurnAngle => _turnAngle;
        public float TurnSpeed => _turnSpeed;
    }
}