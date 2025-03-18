using UnityEngine;

namespace WinterUniverse
{
    [CreateAssetMenu(fileName = "Projectile", menuName = "Winter Universe/Projectile/New Projectile")]
    public class ProjectileConfig : BasicInfoConfig
    {
        [SerializeField] private GameObject _model;
        [SerializeField] private float _splashRadius = 1f;
        [SerializeField] private int _pierceCount = 1;

        public GameObject Model => _model;
        public float SplashRadius => _splashRadius;
        public int PierceCount => _pierceCount;
    }
}