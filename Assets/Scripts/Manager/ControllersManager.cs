using System.Collections.Generic;
using UnityEngine;

namespace WinterUniverse
{
    public class ControllersManager : MonoBehaviour
    {
        private PlayerController _player;
        //private List<NPCController> _controllers = new();

        public PlayerController Player => _player;

        public void Initialize()
        {
            _player = GameManager.StaticInstance.PrefabsManager.GetPlayer();
            _player.Initialize();
        }

        public void Enable()
        {
            _player.Enable();
        }

        public void Disable()
        {
            _player.Disable();
        }

        //public void ResetComponent()
        //{
        //    foreach (NPCController controller in _controllers)
        //    {
        //        controller.ResetComponent();
        //    }
        //}

        public void OnUpdate()
        {
            _player.OnUpdate();
            //foreach (NPCController controller in _controllers)
            //{
            //    controller.OnTick(deltaTime);
            //}
        }

        public void OnFixedUpdate()
        {
            _player.OnFixedUpdate();
            //foreach (NPCController controller in _controllers)
            //{
            //    controller.OnTick(deltaTime);
            //}
        }

        //public void AddController(NPCController controller)
        //{
        //    controller.transform.parent = transform;
        //    _controllers.Add(controller);
        //}

        //public void RemoveController(NPCController controller)
        //{
        //    if (_controllers.Contains(controller))
        //    {
        //        _controllers.Remove(controller);
        //    }
        //}
    }
}