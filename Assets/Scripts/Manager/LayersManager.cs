using UnityEngine;

namespace WinterUniverse
{
    public class LayersManager : MonoBehaviour
    {
        [SerializeField] private LayerMask _obstacleMask;
        [SerializeField] private LayerMask _vehicleMask;
        [SerializeField] private LayerMask _detectableMask;

        public LayerMask ObstacleMask => _obstacleMask;
        public LayerMask VehicleMask => _vehicleMask;
        public LayerMask DetectableMask => _detectableMask;
    }
}