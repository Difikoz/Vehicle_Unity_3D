using System.Collections.Generic;
using UnityEngine;

namespace WinterUniverse
{
    public class Cabin : VehiclePart
    {
        [SerializeField] private CabinConfig _config;
        [SerializeField] private List<WeaponSlot> _weaponSlots = new();
        [SerializeField] private AudioSource _engineSource;

        public CabinConfig Config => _config;
        public List<WeaponSlot> WeaponSlots => _weaponSlots;
        public AudioSource EngineSource => _engineSource;

        protected override VehiclePartConfig GetConfig()
        {
            return _config;
        }
    }
}