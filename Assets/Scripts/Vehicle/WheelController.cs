using UnityEngine;

namespace WinterUniverse
{
    [System.Serializable]
    public class WheelController
    {
        [SerializeField] private WheelType _type;
        [SerializeField] private GameObject _mesh;
        [SerializeField] private WheelCollider _collider;

        public WheelType Type => _type;
        public GameObject Mesh => _mesh;
        public WheelCollider Collider => _collider;
    }
}