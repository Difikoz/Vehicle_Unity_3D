using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WinterUniverse
{
    [RequireComponent(typeof(Rigidbody))]
    public class ProjectileController : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private LayerMask _damageableMask;

        private VehicleController _vehicle;
        private WeaponConfig _weaponConfig;
        private ProjectileConfig _projectileConfig;
        private GameObject _model;
        private int _pierceCount;
        private List<VehiclePart> _damagedParts = new();

        public void Launch(VehicleController vehicle, WeaponConfig weaponConfig, ProjectileConfig projectileConfig)
        {
            _vehicle = vehicle;
            _weaponConfig = weaponConfig;
            _projectileConfig = projectileConfig;
            _model = LeanPool.Spawn(_projectileConfig.Model, transform);
            StartCoroutine(DespawnTimer());
            _rb.linearVelocity = transform.forward * _weaponConfig.Force;
        }

        private IEnumerator DespawnTimer()
        {
            yield return new WaitForSeconds(_weaponConfig.Range / _weaponConfig.Force * 2f);
            Despawn();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out VehiclePart vehiclePart))
            {
                vehiclePart.TakeDamage(_weaponConfig.Damage, _vehicle);
                _pierceCount++;
                if (_pierceCount == _projectileConfig.PierceCount)
                {
                    Despawn();
                }
            }
            else
            {
                Despawn();
            }
        }

        private void Despawn()
        {
            if (_projectileConfig.SplashRadius > 0f)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, _projectileConfig.SplashRadius, _damageableMask);
                foreach (Collider collider in colliders)
                {
                    if (collider.TryGetComponent(out VehiclePart vehiclePart) && !_damagedParts.Contains(vehiclePart))
                    {
                        _damagedParts.Add(vehiclePart);
                        vehiclePart.TakeDamage(_weaponConfig.Damage * Mathf.InverseLerp(_projectileConfig.SplashRadius, 0f, Vector3.Distance(collider.ClosestPoint(transform.position), transform.position)), _vehicle);
                    }
                }
            }
            _pierceCount = 0;
            _rb.linearVelocity = Vector3.zero;
            _damagedParts.Clear();
            LeanPool.Despawn(_model);
            LeanPool.Despawn(gameObject);
        }
    }
}