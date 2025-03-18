using System.Collections.Generic;
using UnityEngine;

namespace WinterUniverse
{
    public class Truck : VehiclePart
    {
        [SerializeField] private CabinConfig _config;
        [SerializeField] private List<WeaponSlot> _weaponSlots = new();

        public CabinConfig Config => _config;
        public List<WeaponSlot> WeaponSlots => _weaponSlots;

        protected override VehiclePartConfig GetConfig()
        {
            return _config;
        }
    }
}