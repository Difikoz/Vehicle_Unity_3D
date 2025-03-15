using UnityEngine;

namespace WinterUniverse
{
    [RequireComponent(typeof(Rigidbody))]
    public class VehicleController : MonoBehaviour
    {
        [SerializeField] private VehicleConfig _config;
        [SerializeField] private WheelController[] _wheels;
        public float GasInput;
        public float TurnInput;

        private Rigidbody _rb;
        private WeaponSlot[] _weaponSlots;
        private float _gasInput;
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
            _gasInput = 0f;
            if (GasInput > 0f)
            {
                if (_forwardVelocity >= 0f)
                {
                    if (_forwardVelocity < _config.MaxForwardSpeed)
                    {
                        _gasInput = GasInput * _config.AccelerationForce;
                    }
                }
                else
                {
                    _gasInput = GasInput * _config.DecelerationForce;
                }
            }
            else if (GasInput < 0f)
            {
                if (_forwardVelocity <= 0f)
                {
                    if (_forwardVelocity > -_config.MaxBackwardSpeed)
                    {
                        _gasInput = GasInput * _config.AccelerationForce;
                    }
                }
                else
                {
                    _gasInput = GasInput * _config.DecelerationForce;
                }
            }
            if (_gasInput != 0f)
            {
                foreach (WheelController controller in _wheels)
                {
                    if (controller.Type != WheelType.Turn)
                    {
                        controller.Collider.motorTorque += _gasInput;
                    }
                }
            }
            _forwardVelocity = Vector3.Dot(_rb.linearVelocity, transform.forward);
            _rightVelocity = Vector3.Dot(_rb.linearVelocity, transform.right);
            _currentSpeed = Mathf.Abs(_forwardVelocity);
        }

        private void HandleTurnInput()
        {
            _turnAngle = Mathf.MoveTowardsAngle(_turnAngle, TurnInput * _config.TurnAngle, _config.TurnSpeed * Time.fixedDeltaTime);
            foreach (WheelController controller in _wheels)
            {
                if (controller.Type != WheelType.Gas)
                {
                    controller.Collider.GetWorldPose(out Vector3 pos, out Quaternion rot);
                    controller.Mesh.transform.position = pos;
                    controller.Mesh.transform.rotation = rot;
                    controller.Collider.steerAngle = _turnAngle;
                }
            }
        }

        private void HandleGravity()
        {

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