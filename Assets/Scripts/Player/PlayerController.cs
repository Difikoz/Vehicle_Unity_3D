using UnityEngine;

namespace WinterUniverse
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private VehicleController _vehicle;

        private PlayerInputActions _inputActions;

        private void Awake()
        {
            _inputActions = new();
        }

        private void OnEnable()
        {
            _inputActions.Enable();
        }

        private void OnDisable()
        {
            _inputActions.Disable();
        }

        private void Update()
        {
            _vehicle.GasInput = _inputActions.Vehicle.Gas.ReadValue<float>();
            _vehicle.TurnInput = _inputActions.Vehicle.Turn.ReadValue<float>();
        }
    }
}