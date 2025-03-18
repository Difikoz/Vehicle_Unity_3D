using UnityEngine;

namespace WinterUniverse
{
    [CreateAssetMenu(fileName = "Truck", menuName = "Winter Universe/Vehicle/New Truck")]
    public class TruckConfig : VehiclePartConfig
    {
        [SerializeField] private float _maxWeight = 1000f;
        [SerializeField] private int _maxSize = 100;

        public float MaxWeight => _maxWeight;
        public int MaxSize => _maxSize;
    }
}