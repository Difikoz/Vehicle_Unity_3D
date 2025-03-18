using UnityEngine;

namespace WinterUniverse
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private float _raycastDistance = 1000f;
        [SerializeField] private float _followSpeed = 100f;
        [SerializeField] private Transform _rotationRoot;
        [SerializeField] private float _rotateSpeed = 45f;
        [SerializeField] private float _minAngle = 45f;
        [SerializeField] private float _maxAngle = 45f;
        [SerializeField] private Transform _collisionRoot;
        [SerializeField] private float _collisionRadius = 0.25f;
        [SerializeField] private float _collisionAvoidanceSpeed = 10f;

        private PlayerInputActions _inputActions;
        private PlayerController _player;
        private Camera _camera;
        private Vector2 _lookInput;
        private RaycastHit _cameraHit;
        private float _xRot;
        private Vector3 _collisionCurrentOffset;
        private float _collisionDefaultOffset;
        private float _collisionRequiredOffset;
        private RaycastHit _collisionHit;

        public void Initialize()
        {
            _camera = GetComponentInChildren<Camera>();
            _inputActions = new();
            _player = GameManager.StaticInstance.ControllersManager.Player;
        }

        public void Enable()
        {
            _inputActions.Enable();
            _inputActions.Camera.LockTarget.performed += ctx => OnLockTargetPerfomed();
            transform.position = _player.Vehicle.transform.position;
            _xRot = _rotationRoot.localEulerAngles.x;
            _collisionDefaultOffset = _collisionRoot.localPosition.z;
        }

        public void Disable()
        {
            _inputActions.Camera.LockTarget.performed -= ctx => OnLockTargetPerfomed();
            _inputActions.Disable();
        }

        public void OnLateUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, _player.Vehicle.transform.position, _followSpeed * Time.deltaTime);
            HandleFreeLook();
            HandleCollision();
            HandleLookPoint();
            _player.Vehicle.FireInput = _inputActions.Camera.Fire.IsPressed();
            // set crosshair position
        }

        private void OnLockTargetPerfomed()
        {
            if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out _cameraHit, _raycastDistance, GameManager.StaticInstance.LayersManager.VehicleMask))
            {
                VehicleController vehicle = _cameraHit.transform.GetComponentInParent<VehicleController>();
                if (vehicle != null)
                {
                    _player.Vehicle.SetTarget(vehicle);
                    return;
                }
            }
            _player.Vehicle.ResetTarget();
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
            if (Physics.SphereCast(_rotationRoot.position, _collisionRadius, direction, out _collisionHit, Mathf.Abs(_collisionRequiredOffset), GameManager.StaticInstance.LayersManager.ObstacleMask))
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

        private void HandleLookPoint()
        {
            if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out _cameraHit, _raycastDistance, GameManager.StaticInstance.LayersManager.DetectableMask))
            {
                _player.Vehicle.LookPoint = _cameraHit.point;
            }
            else
            {
                _player.Vehicle.LookPoint = _camera.transform.position + _camera.transform.forward * _raycastDistance;
            }
        }
    }
}