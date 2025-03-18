using UnityEngine;

namespace WinterUniverse
{
    public abstract class VehiclePartConfig : BasicInfoConfig
    {
        [SerializeField] protected GameObject _model;
        [SerializeField] protected float _mass;

        public GameObject Model => _model;
        public float Mass => _mass;
    }
}