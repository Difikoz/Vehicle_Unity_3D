using System.Collections.Generic;
using UnityEngine;

namespace WinterUniverse
{
    public class Truck : MonoBehaviour
    {
        [SerializeField] private CabinConfig _config;
        [SerializeField] private Transform _centerOfMass;
        [SerializeField] private List<WeaponSlot> _weaponSlots = new();

        public CabinConfig Config => _config;
        public Transform CenterOfMass => _centerOfMass;
        public List<WeaponSlot> WeaponSlots => _weaponSlots;
    }
}