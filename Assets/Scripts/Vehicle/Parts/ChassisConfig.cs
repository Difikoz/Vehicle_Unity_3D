using UnityEngine;

namespace WinterUniverse
{
    [CreateAssetMenu(fileName = "Chassis", menuName = "Winter Universe/Vehicle/New Chassis")]
    public class ChassisConfig : VehiclePartConfig
    {
        [SerializeField] private float _brakeForce = 10000f;
        [SerializeField] private float _turnAngle = 45f;
        [SerializeField] private float _turnSpeed = 90f;
        [SerializeField] private AnimationCurve _turnCurve;

        public float BrakeForce => _brakeForce;
        public float TurnAngle => _turnAngle;
        public float TurnSpeed => _turnSpeed;
        public AnimationCurve TurnCurve => _turnCurve;
    }
}