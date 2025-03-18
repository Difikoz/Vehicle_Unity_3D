using Lean.Pool;
using UnityEngine;

namespace WinterUniverse
{
    public class WeaponSlot : MonoBehaviour
    {
        public WeaponConfig TestWeapon;

        private VehicleController _vehicle;
        private WeaponController _weapon;

        public WeaponController Weapon => _weapon;

        public void Initialize(VehicleController vehicle)
        {
            _vehicle = vehicle;
            CreateWeapon(TestWeapon);
        }

        public void CreateWeapon(WeaponConfig config)
        {
            DeleteWeapon();
            _weapon = LeanPool.Spawn(config.Model, transform).GetComponent<WeaponController>();
            _weapon.Initialize(_vehicle);
        }

        public void DeleteWeapon()
        {
            if (_weapon != null)
            {
                LeanPool.Despawn(_weapon.gameObject);
                _weapon = null;
            }
        }

        public void OnFixedUpdate()
        {
            if (_weapon == null)
            {
                return;
            }
            HandleAiming();
            HandleFiring();
        }

        private void HandleAiming()
        {
            _weapon.Aim(_vehicle.LookPoint);
        }

        private void HandleFiring()
        {
            if (_vehicle.FireInput && _weapon.CanFire())
            {
                _weapon.OnFire();
            }
        }
    }
}