using System.Collections.Generic;
using UnityEngine;

namespace WinterUniverse
{
    public class ConfigsManager : MonoBehaviour
    {
        [SerializeField] private List<FactionConfig> _factions = new();
        [SerializeField] private List<WeaponConfig> _weapons = new();
        [SerializeField] private List<ProjectileConfig> _projectiles = new();
        [SerializeField] private List<ChassisConfig> _chassis = new();
        [SerializeField] private List<CabinConfig> _cabins = new();
        [SerializeField] private List<TruckConfig> _trucks = new();

        private List<VehiclePartConfig> _vehicleParts;

        public List<FactionConfig> Factions => _factions;

        public void Initialize()
        {
            _vehicleParts = new();
            foreach (ChassisConfig config in _chassis)
            {
                _vehicleParts.Add(config);
            }
            foreach (CabinConfig config in _cabins)
            {
                _vehicleParts.Add(config);
            }
            foreach (TruckConfig config in _trucks)
            {
                _vehicleParts.Add(config);
            }
        }

        public WeaponConfig GetWeapon(string id)
        {
            foreach (WeaponConfig config in _weapons)
            {
                if (config.ID == id)
                {
                    return config;
                }
            }
            return null;
        }

        public ProjectileConfig GetProjectile(string id)
        {
            foreach (ProjectileConfig config in _projectiles)
            {
                if (config.ID == id)
                {
                    return config;
                }
            }
            return null;
        }

        public VehiclePartConfig GetVehiclePart(string id)
        {
            foreach (VehiclePartConfig config in _vehicleParts)
            {
                if (config.ID == id)
                {
                    return config;
                }
            }
            return null;
        }

        public ChassisConfig GetChassis(string id)
        {
            foreach (ChassisConfig config in _chassis)
            {
                if (config.ID == id)
                {
                    return config;
                }
            }
            return null;
        }

        public CabinConfig GetCabin(string id)
        {
            foreach (CabinConfig config in _cabins)
            {
                if (config.ID == id)
                {
                    return config;
                }
            }
            return null;
        }

        public TruckConfig GetTruck(string id)
        {
            foreach (TruckConfig config in _trucks)
            {
                if (config.ID == id)
                {
                    return config;
                }
            }
            return null;
        }
    }
}