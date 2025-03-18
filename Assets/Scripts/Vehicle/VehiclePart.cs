using UnityEngine;

namespace WinterUniverse
{
    public abstract class VehiclePart : MonoBehaviour
    {
        [SerializeField] protected Transform _centerOfMass;
        [SerializeField] protected Transform _middlePoint;
        protected VehicleController _vehicle;
        protected float _armorCurrent;
        protected float _armorMax;

        public Transform CenterOfMass => _centerOfMass;
        public Transform MiddlePoint => _middlePoint;
        public VehicleController Vehicle => _vehicle;
        public float ArmorPercent => _armorCurrent / _armorMax;

        public void RepairArmor()
        {
            _armorMax = GetConfig().Armor;
            _armorCurrent = _armorMax;
        }

        public void TakeDamage(float damage, VehicleController source)
        {
            if (_armorCurrent >= damage)
            {
                _armorCurrent -= damage;
            }
            else
            {
                _armorCurrent = 0f;
            }
            damage *= Mathf.InverseLerp(_armorMax, 0f, _armorCurrent);
            if (source != null)
            {
                // do something
            }
            if (damage > 0f)
            {
                _vehicle.DamageVehicle(damage);
            }
        }

        protected abstract VehiclePartConfig GetConfig();
    }
}