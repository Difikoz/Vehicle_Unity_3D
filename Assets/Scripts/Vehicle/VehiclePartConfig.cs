using UnityEngine;

namespace WinterUniverse
{
    public abstract class VehiclePartConfig : BasicInfoConfig
    {
        [SerializeField] protected GameObject _model;
        [SerializeField] protected float _mass = 250f;
        [SerializeField] protected float _durability = 100f;
        [SerializeField] protected float _armor = 50f;

        public GameObject Model => _model;
        public float Mass => _mass;
        public float Durability => _durability;
        public float Armor => _armor;
    }
}