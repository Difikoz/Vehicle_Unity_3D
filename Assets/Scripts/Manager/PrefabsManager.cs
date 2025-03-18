using Lean.Pool;
using UnityEngine;

namespace WinterUniverse
{
    public class PrefabsManager : MonoBehaviour
    {
        [SerializeField] private Transform _testSpawnPoint;
        [SerializeField] private GameObject _playerController;
        [SerializeField] private GameObject _npcController;
        [SerializeField] private GameObject _vehicleController;
        [SerializeField] private GameObject _projectileController;

        public PlayerController GetPlayer()
        {
            return LeanPool.Spawn(_playerController).GetComponent<PlayerController>();
        }

        //public NPCController GetNPC()
        //{
        //    return LeanPool.Spawn(_npcController).GetComponent<NPCController>();
        //}

        public VehicleController GetVehicle()
        {
            return LeanPool.Spawn(_vehicleController, _testSpawnPoint.position, _testSpawnPoint.rotation).GetComponent<VehicleController>();
        }

        public VehicleController GetVehicle(Vector3 position, Quaternion rotation)
        {
            return LeanPool.Spawn(_vehicleController, position, rotation).GetComponent<VehicleController>();
        }

        public ProjectileController GetProjectile(Vector3 position, Quaternion rotation)
        {
            return LeanPool.Spawn(_projectileController, position, rotation).GetComponent<ProjectileController>();
        }

        public void DespawnObject(GameObject go, float delay = 0f)
        {
            LeanPool.Despawn(go, delay);
        }
    }
}