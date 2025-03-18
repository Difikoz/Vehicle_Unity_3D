using System.Collections.Generic;
using UnityEngine;

namespace WinterUniverse
{
    public class Chassis : MonoBehaviour
    {
        [SerializeField] private ChassisConfig _config;
        [SerializeField] private Transform _centerOfMass;
        [SerializeField] private Transform _cabinRoot;
        [SerializeField] private Transform _truckRoot;
        [SerializeField] private List<WheelController> _wheels = new();

        public ChassisConfig Config => _config;
        public Transform CenterOfMass => _centerOfMass;
        public Transform CabinRoot => _cabinRoot;
        public Transform TruckRoot => _truckRoot;
        public List<WheelController> Wheels => _wheels;
    }
}