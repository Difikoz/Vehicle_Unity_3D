using UnityEngine;

namespace WinterUniverse
{
    public class WeaponController : MonoBehaviour
    {
        public Vector3 LookDirection;
        public bool FireInput;

        public void OnFixedUpdate()
        {
            RotateToLookDirection();
            HandleFireInput();
        }

        private void RotateToLookDirection()
        {

        }

        private void HandleFireInput()
        {

        }
    }
}