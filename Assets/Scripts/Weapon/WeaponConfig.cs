using System.Collections.Generic;
using UnityEngine;

namespace WinterUniverse
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Winter Universe/Weapon/New Weapon")]
    public class WeaponConfig : BasicInfoConfig
    {
        [SerializeField] private GameObject _model;
        [SerializeField] private float _damage = 10f;
        [SerializeField] private float _fireRate = 300f;
        [SerializeField] private float _range = 150f;
        [SerializeField] private float _force = 100f;
        [SerializeField] private float _spread = 5f;
        [SerializeField] private float _turnSpeed = 180f;
        [SerializeField] private float _maxTurnAngle = 90f;
        [SerializeField] private float _aimSpeed = 90f;
        [SerializeField] private float _minAimAngle = 45f;
        [SerializeField] private float _maxAimAngle = 45f;
        [SerializeField] private ProjectileConfig _projectile;
        [SerializeField] private int _projectilePerShot = 1;
        [SerializeField] private List<AudioClip> _fireClips = new();

        public GameObject Model => _model;
        public float Damage => _damage;
        public float FireRate => _fireRate;
        public float Range => _range;
        public float Force => _force;
        public float Spread => _spread;
        public float TurnSpeed => _turnSpeed;
        public float MaxTurnAngle => _maxTurnAngle;
        public float AimSpeed => _aimSpeed;
        public float MinAimAngle => _minAimAngle;
        public float MaxAimAngle => _maxAimAngle;
        public ProjectileConfig Projectile => _projectile;
        public int ProjectilePerShot => _projectilePerShot;
        public AudioClip FireClip => _fireClips[Random.Range(0, _fireClips.Count)];
    }
}