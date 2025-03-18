using Lean.Pool;
using System.Collections.Generic;
using UnityEngine;

namespace WinterUniverse
{
    public abstract class WeaponController : MonoBehaviour
    {
        [SerializeField] protected GameObject _testProjectileController;
        [SerializeField] protected WeaponConfig _config;
        [SerializeField] protected Transform _turnRoot;
        [SerializeField] protected Transform _aimRoot;
        [SerializeField] protected List<Transform> _shootPoints = new();
        [SerializeField] protected AudioSource _audioSource;

        [Header("Debug")]
        protected VehicleController _vehicle;
        [SerializeField] protected Vector3 _lookPoint;
        [SerializeField] protected Vector3 _lookDirection;
        [SerializeField] protected int _currentShootPointIndex;
        [SerializeField] protected float _fireTime;
        [SerializeField] protected float _turnAngle;
        [SerializeField] protected float _aimAngle;
        [SerializeField] protected float _xRot;
        [SerializeField] protected float _yRot;

        public WeaponConfig Config => _config;

        public virtual void Initialize(VehicleController vehicle)
        {
            _vehicle = vehicle;
        }

        public virtual bool CanFire()
        {
            return Time.time >= 60f / _config.FireRate + _fireTime;
        }

        public virtual void OnFire()
        {
            _fireTime = Time.time;
            for (int i = 0; i < _config.ProjectilePerShot; i++)
            {
                LeanPool.Spawn(_testProjectileController,
                    _shootPoints[_currentShootPointIndex].position,
                    Quaternion.Euler(_shootPoints[_currentShootPointIndex].eulerAngles + GetSpread())).
                    GetComponent<ProjectileController>().Launch(_vehicle, _config, _config.Projectile);
            }
            if (_currentShootPointIndex < _shootPoints.Count - 1)
            {
                _currentShootPointIndex++;
            }
            else
            {
                _currentShootPointIndex = 0;
            }
            _audioSource.pitch = Random.Range(0.9f, 1.1f);
            _audioSource.PlayOneShot(_config.FireClip);
        }

        public virtual void Aim(Vector3 position)
        {
            _lookPoint = position;
            _lookDirection = (_lookPoint - _turnRoot.position).normalized;
            _turnAngle = Vector3.SignedAngle(_turnRoot.forward, _lookDirection, _turnRoot.up);
            _aimAngle = Vector3.SignedAngle(_aimRoot.forward, _lookDirection, _aimRoot.right);
            if (Mathf.Abs(_turnAngle) > 5f)
            {
                _yRot = Mathf.MoveTowardsAngle(_yRot, _yRot + _turnAngle, _config.TurnSpeed * Time.fixedDeltaTime);
                _yRot = Mathf.Clamp(_yRot, -_config.MaxTurnAngle, _config.MaxTurnAngle);
                _turnRoot.localRotation = Quaternion.Euler(0f, _yRot, 0f);
            }
            if (Mathf.Abs(_aimAngle) > 5f)
            {
                _xRot = Mathf.MoveTowardsAngle(_xRot, _xRot + _aimAngle, _config.AimSpeed * Time.fixedDeltaTime);
                _xRot = Mathf.Clamp(_xRot, -_config.MaxAimAngle, _config.MinAimAngle);
                _aimRoot.localRotation = Quaternion.Euler(_xRot, 0f, 0f);
            }
        }

        protected Vector3 GetSpread()
        {
            return new(Random.Range(-_config.Spread, _config.Spread), Random.Range(-_config.Spread, _config.Spread), Random.Range(-_config.Spread, _config.Spread));
        }
    }
}