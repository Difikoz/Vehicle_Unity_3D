using UnityEngine;

namespace WinterUniverse
{
    [RequireComponent(typeof(Rigidbody))]
    public class VehicleController : MonoBehaviour
    {
        [SerializeField] private VehicleConfig _config;
        [SerializeField] private Transform _centerOfMass;
        [SerializeField] private WheelController[] _wheels;
        public float GasInput;
        public float TurnInput;
        public bool BrakeInput;

        private Rigidbody _rb;
        private WeaponSlot[] _weaponSlots;
        private float _gasTorque;
        private float _brakeTorque;
        private float _turnAngle;
        private float _forwardVelocity;
        private float _rightVelocity;
        private float _currentSpeed;

        private void Start()
        {
            Initialize();
        }

        private void FixedUpdate()
        {
            OnFixedUpdate();
        }

        public void Initialize()
        {
            _rb = GetComponent<Rigidbody>();
            _weaponSlots = GetComponentsInChildren<WeaponSlot>();
            _rb.centerOfMass = _centerOfMass.localPosition;
        }

        public void OnFixedUpdate()
        {
            ClampInputs();
            HandleGasInput();
            HandleTurnInput();
            HandleGravity();
        }

        private void ClampInputs()
        {
            GasInput = Mathf.Clamp(GasInput, -1f, 1f);
            TurnInput = Mathf.Clamp(TurnInput, -1f, 1f);
        }

        private void HandleGasInput()
        {
            _gasTorque = 0f;
            _brakeTorque = 0f;
            if (GasInput > 0f)
            {
                if (_forwardVelocity >= 0f)
                {
                    if (_forwardVelocity < _config.MaxForwardSpeed)
                    {
                        _gasTorque = GasInput * _config.AccelerationForce * GetAccelerationMultiplier();
                    }
                }
                else
                {
                    _gasTorque = GasInput * _config.DecelerationForce * GetDecelerationMultiplier();
                }
            }
            else if (GasInput < 0f)
            {
                if (_forwardVelocity <= 0f)
                {
                    if (_forwardVelocity > -_config.MaxBackwardSpeed)
                    {
                        _gasTorque = GasInput * _config.AccelerationForce * GetAccelerationMultiplier();
                    }
                }
                else
                {
                    _gasTorque = GasInput * _config.DecelerationForce * GetDecelerationMultiplier();
                }
            }
            else
            {
                _brakeTorque = _config.DecelerationForce * GetDecelerationMultiplier();
            }
            if (BrakeInput)
            {
                _brakeTorque = _config.BrakeForce;
            }
            foreach (WheelController controller in _wheels)
            {
                controller.Collider.motorTorque = controller.Type != WheelType.Turn ? _gasTorque : 0f;
                controller.Collider.brakeTorque = _brakeTorque;
            }
            _forwardVelocity = Vector3.Dot(_rb.linearVelocity, transform.forward);
            _rightVelocity = Vector3.Dot(_rb.linearVelocity, transform.right);
            _currentSpeed = _forwardVelocity;
            Debug.Log($"gas torque : {_gasTorque} / brake torque : {_brakeTorque} / speed : {_currentSpeed}");
        }

        private void HandleTurnInput()
        {
            _turnAngle = Mathf.MoveTowardsAngle(_turnAngle, TurnInput * _config.TurnAngle, _config.TurnSpeed * GetTurnMultiplier() * Time.fixedDeltaTime);
            foreach (WheelController controller in _wheels)
            {
                if (controller.Type != WheelType.Gas)
                {
                    controller.Collider.steerAngle = _turnAngle;
                }
                controller.Collider.GetWorldPose(out Vector3 pos, out Quaternion rot);
                controller.Mesh.transform.SetPositionAndRotation(pos, rot);
            }
        }

        private void HandleGravity()
        {

        }

        private float GetAccelerationMultiplier()
        {
            if (_currentSpeed >= 0f)
            {
                return _config.AccelerationCurve.Evaluate(Mathf.InverseLerp(_config.MaxForwardSpeed, 0f, _currentSpeed));
            }
            else
            {
                return _config.AccelerationCurve.Evaluate(Mathf.InverseLerp(-_config.MaxBackwardSpeed, 0f, _currentSpeed));
            }
        }

        private float GetDecelerationMultiplier()
        {
            if (_currentSpeed > 1f)
            {
                return _config.DecelerationCurve.Evaluate(Mathf.InverseLerp(_config.MaxForwardSpeed, 0f, _currentSpeed));
            }
            else if (_currentSpeed < -1f)
            {
                return _config.DecelerationCurve.Evaluate(Mathf.InverseLerp(-_config.MaxBackwardSpeed, 0f, _currentSpeed));
            }
            else
            {
                return 1f;
            }
        }

        private float GetTurnMultiplier()
        {
            if (_currentSpeed > 1f)
            {
                return _config.TurnCurve.Evaluate(Mathf.InverseLerp(_config.MaxForwardSpeed, 0f, _currentSpeed));
            }
            else if (_currentSpeed < -1f)
            {
                return _config.TurnCurve.Evaluate(Mathf.InverseLerp(-_config.MaxBackwardSpeed, 0f, _currentSpeed));
            }
            else
            {
                return 1f;
            }
        }

        public void HandleWeaponSlots(Vector3 lookDirection, bool isFiring)
        {
            foreach (WeaponSlot slot in _weaponSlots)
            {
                if (slot.Weapon != null)
                {
                    slot.Weapon.LookDirection = lookDirection;
                    slot.Weapon.FireInput = isFiring;
                }
            }
        }
    }
}