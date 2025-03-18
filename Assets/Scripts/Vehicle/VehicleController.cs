using Lean.Pool;
using UnityEngine;

namespace WinterUniverse
{
    [RequireComponent(typeof(Rigidbody))]
    public class VehicleController : MonoBehaviour
    {
        [Header("Test Configs")]
        public bool CreateTestVehicle;
        public ChassisConfig TestChassisConfig;
        public CabinConfig TestCabinConfig;
        public TruckConfig TestTruckConfig;
        [Header("Defaukt Configs")]
        public ChassisConfig DefaultChassisConfig;
        public CabinConfig DefaultCabinConfig;
        public TruckConfig DefaultTruckConfig;
        [Header("Inputs")]
        public float GasInput;
        public float TurnInput;
        public bool BrakeInput;

        private Rigidbody _rb;
        private Chassis _chassis;
        private Cabin _cabin;
        private Truck _truck;
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
            if (CreateTestVehicle)
            {
                CreateTestVehicle = false;
                CreateVehicle(TestChassisConfig, TestCabinConfig, TestTruckConfig);
            }
        }

        public void Initialize()
        {
            _rb = GetComponent<Rigidbody>();
            CreateVehicle(DefaultChassisConfig, DefaultCabinConfig, DefaultTruckConfig);
        }

        public void CreateVehicle(ChassisConfig chassis, CabinConfig cabin, TruckConfig truck)
        {
            DeleteVehicle();
            _chassis = LeanPool.Spawn(chassis.Model, transform).GetComponent<Chassis>();
            _cabin = LeanPool.Spawn(cabin.Model, _chassis.CabinRoot).GetComponent<Cabin>();
            _truck = LeanPool.Spawn(truck.Model, _chassis.TruckRoot).GetComponent<Truck>();
            _rb.centerOfMass = transform.InverseTransformPoint(_chassis.CenterOfMass.position) + transform.InverseTransformPoint(_cabin.CenterOfMass.position) + transform.InverseTransformPoint(_truck.CenterOfMass.position);
            _rb.mass = _chassis.Config.Mass + _cabin.Config.Mass + _truck.Config.Mass;
            _weaponSlots = GetComponentsInChildren<WeaponSlot>();
        }

        public void DeleteVehicle()
        {
            if (_truck != null)
            {
                LeanPool.Despawn(_truck.gameObject);
                _truck = null;
            }
            if (_cabin != null)
            {
                LeanPool.Despawn(_cabin.gameObject);
                _cabin = null;
            }
            if (_chassis != null)
            {
                LeanPool.Despawn(_chassis.gameObject);
                _chassis = null;
            }
            _weaponSlots = null;
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
                    if (_forwardVelocity < _cabin.Config.MaxForwardSpeed)
                    {
                        _gasTorque = GasInput * _cabin.Config.AccelerationForce * GetAccelerationMultiplier();
                    }
                }
                else
                {
                    _gasTorque = GasInput * _cabin.Config.DecelerationForce * GetDecelerationMultiplier();
                }
            }
            else if (GasInput < 0f)
            {
                if (_forwardVelocity <= 0f)
                {
                    if (_forwardVelocity > -_cabin.Config.MaxBackwardSpeed)
                    {
                        _gasTorque = GasInput * _cabin.Config.AccelerationForce * GetAccelerationMultiplier();
                    }
                }
                else
                {
                    _gasTorque = GasInput * _cabin.Config.DecelerationForce * GetDecelerationMultiplier();
                }
            }
            else
            {
                _brakeTorque = _cabin.Config.DecelerationForce * GetDecelerationMultiplier();
            }
            if (BrakeInput)
            {
                _brakeTorque = _chassis.Config.BrakeForce;
            }
            foreach (WheelController controller in _chassis.Wheels)
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
            _turnAngle = Mathf.MoveTowardsAngle(_turnAngle, TurnInput * _chassis.Config.TurnAngle, _chassis.Config.TurnSpeed * GetTurnMultiplier() * Time.fixedDeltaTime);
            foreach (WheelController controller in _chassis.Wheels)
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
                return _cabin.Config.AccelerationCurve.Evaluate(Mathf.InverseLerp(_cabin.Config.MaxForwardSpeed, 0f, _currentSpeed));
            }
            else
            {
                return _cabin.Config.AccelerationCurve.Evaluate(Mathf.InverseLerp(-_cabin.Config.MaxBackwardSpeed, 0f, _currentSpeed));
            }
        }

        private float GetDecelerationMultiplier()
        {
            if (_currentSpeed > 1f)
            {
                return _cabin.Config.DecelerationCurve.Evaluate(Mathf.InverseLerp(_cabin.Config.MaxForwardSpeed, 0f, _currentSpeed));
            }
            else if (_currentSpeed < -1f)
            {
                return _cabin.Config.DecelerationCurve.Evaluate(Mathf.InverseLerp(-_cabin.Config.MaxBackwardSpeed, 0f, _currentSpeed));
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
                return _chassis.Config.TurnCurve.Evaluate(Mathf.InverseLerp(_cabin.Config.MaxForwardSpeed, 0f, _currentSpeed));
            }
            else if (_currentSpeed < -1f)
            {
                return _chassis.Config.TurnCurve.Evaluate(Mathf.InverseLerp(-_cabin.Config.MaxBackwardSpeed, 0f, _currentSpeed));
            }
            else
            {
                return 1f;
            }
        }

        public void HandleWeaponSlots(Vector3 lookDirection, bool isFiring)
        {
            if (_weaponSlots == null)
            {
                return;
            }
            foreach (WeaponSlot slot in _weaponSlots)
            {
                if (slot.Weapon != null)
                {
                    slot.Weapon.LookDirection = lookDirection;
                    slot.Weapon.FireInput = isFiring;
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (_rb != null)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(transform.TransformPoint(_rb.centerOfMass), 0.25f);
            }
        }
    }
}