using UnityEngine;

namespace WinterUniverse
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private VehicleController _vehicle;
        [SerializeField] private float _followSpeed = 100f;
        [SerializeField] private Transform _rotationRoot;
        [SerializeField] private float _rotateSpeed = 45f;
        [SerializeField] private float _minAngle = 45f;
        [SerializeField] private float _maxAngle = 45f;
        [SerializeField] private Transform _collisionRoot;
        [SerializeField] private float _collisionRadius = 0.25f;
        [SerializeField] private float _collisionAvoidanceSpeed = 10f;

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
            _xRot = _rotationRoot.localEulerAngles.x;
            _collisionDefaultOffset = _collisionRoot.localPosition.z;
        }

        public void OnLateUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, _vehicle.transform.position, _followSpeed * Time.deltaTime);
            HandleFreeLook();
            HandleCollision();
            _vehicle.HandleWeaponSlots(_camera.transform.forward, _inputActions.Camera.Fire.IsPressed());
        }

        private void OnLockTargetPerfomed()
        {
            if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out _cameraHit, 1000f))// add layer mask!
            {
                VehicleController vehicle = _cameraHit.transform.GetComponentInParent<VehicleController>();
                if (vehicle != null)
                {
                    // set target
                }
            }
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
            if (Physics.SphereCast(_rotationRoot.position, _collisionRadius, direction, out _collisionHit, Mathf.Abs(_collisionRequiredOffset)))// add layer mask!
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