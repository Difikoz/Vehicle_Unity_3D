using UnityEngine;

namespace WinterUniverse
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private VehicleController _vehicle;
        [SerializeField] private float _raycastDistance = 1000f;
        [SerializeField] private float _followSpeed = 100f;
        [SerializeField] private Transform _rotationRoot;
        [SerializeField] private float _rotateSpeed = 45f;
        [SerializeField] private float _minAngle = 45f;
        [SerializeField] private float _maxAngle = 45f;
        [SerializeField] private Transform _collisionRoot;
        [SerializeField] private float _collisionRadius = 0.25f;
        [SerializeField] private float _collisionAvoidanceSpeed = 10f;
        [SerializeField] private LayerMask _obstacleMask;
        [SerializeField] private LayerMask _vehicleMask;
        [SerializeField] private LayerMask _detectableMask;

        private PlayerInputActions _inputActions;
        private Camera _camera;
        private Vector2 _lookInput;
        private RaycastHit _cameraHit;
        private float _xRot;
        private Vector3 _collisionCurrentOffset;
        private float _collisionDefaultOffset;
        private float _collisionRequiredOffset;
        private RaycastHit _collisionHit;

        private void Start()
        {
            Initialize();
        }

        private void LateUpdate()
        {
            OnLateUpdate();
        }

        public void Initialize()
        {
            _camera = GetComponentInChildren<Camera>();
            _inputActions = new();
            _inputActions.Enable();
            _inputActions.Camera.LockTarget.performed += ctx => OnLockTargetPerfomed();
            transform.position = _vehicle.transform.position;
            _xRot = _rotationRoot.localEulerAngles.x;
            _collisionDefaultOffset = _collisionRoot.localPosition.z;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void OnLateUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, _vehicle.transform.position, _followSpeed * Time.deltaTime);
            HandleFreeLook();
            HandleCollision();
            if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out _cameraHit, _raycastDistance, _detectableMask))
            {
                _vehicle.LookPoint = _cameraHit.point;
            }
            else
            {
                _vehicle.LookPoint = _camera.transform.position + _camera.transform.forward * _raycastDistance;
            }
            _vehicle.FireInput = _inputActions.Camera.Fire.IsPressed();
        }

        private void OnLockTargetPerfomed()
        {
            if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out _cameraHit, _raycastDistance, _vehicleMask))
            {
                VehicleController vehicle = _cameraHit.transform.GetComponentInParent<VehicleController>();
                if (vehicle != null)
                {
                    _vehicle.SetTarget(vehicle);
                    return;
                }
            }
            _vehicle.ResetTarget();
        }

        private void HandleFreeLook()
        {
            _lookInput = _inputActions.Camera.Look.ReadValue<Vector2>();
            if (_lookInput.x != 0f)
            {
                transform.Rotate(Vector3.up * _lookInput.x * _rotateSpeed * Time.deltaTime);
            }
            if (_lookInput.y != 0f)
            {
                _xRot = Mathf.Clamp(_xRot - (_lookInput.y * _rotateSpeed * Time.deltaTime), -_maxAngle, _minAngle);
                _rotationRoot.localRotation = Quaternion.Euler(_xRot, 0f, 0f);
            }
        }

        private void HandleCollision()
        {
            _collisionRequiredOffset = _collisionDefaultOffset;
            Vector3 direction = (_collisionRoot.position - _rotationRoot.position).normalized;
            if (Physics.SphereCast(_rotationRoot.position, _collisionRadius, direction, out _collisionHit, Mathf.Abs(_collisionRequiredOffset), _obstacleMask))
            {
                _collisionRequiredOffset = -(Vector3.Distance(_rotationRoot.position, _collisionHit.point) - _collisionRadius);
            }
            if (Mathf.Abs(_collisionRequiredOffset) < _collisionRadius)
            {
                _collisionRequiredOffset = -_collisionRadius;
            }
            _collisionCurrentOffset.z = Mathf.Lerp(_collisionRoot.localPosition.z, _collisionRequiredOffset, _collisionAvoidanceSpeed * Time.deltaTime);
            _collisionRoot.localPosition = _collisionCurrentOffset;
        }
    }
}