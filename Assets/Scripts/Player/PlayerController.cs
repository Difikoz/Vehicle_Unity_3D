using UnityEngine;

namespace WinterUniverse
{
    public class PlayerController : MonoBehaviour
    {
        private PlayerInputActions _inputActions;
        private VehicleController _vehicle;

        public VehicleController Vehicle => _vehicle;

        public void Initialize()
        {
            _inputActions = new();
            _vehicle = GameManager.StaticInstance.PrefabsManager.GetVehicle();
            _vehicle.Initialize();
        }

        public void Enable()
        {
            _inputActions.Enable();
        }

        public void Disable()
        {
            _inputActions.Disable();
        }

        public void OnUpdate()
        {
            _vehicle.GasInput = _inputActions.Vehicle.Gas.ReadValue<float>();
            _vehicle.TurnInput = _inputActions.Vehicle.Turn.ReadValue<float>();
            _vehicle.BrakeInput = _inputActions.Vehicle.Brake.IsPressed();
        }

        public void OnFixedUpdate()
        {
            _vehicle.OnFixedUpdate();
        }
    }
}