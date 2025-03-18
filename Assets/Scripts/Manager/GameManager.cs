using UnityEngine;

namespace WinterUniverse
{
    public class GameManager : Singleton<GameManager>
    {
        private InputMode _inputMode;
        private AudioManager _audioManager;
        private CameraManager _cameraManager;
        private ConfigsManager _configsManager;
        private ControllersManager _controllersManager;
        private LayersManager _layersManager;
        private PrefabsManager _prefabsManager;
        private UIManager _uiManager;

        public InputMode InputMode => _inputMode;
        public AudioManager AudioManager => _audioManager;
        public CameraManager CameraManager => _cameraManager;
        public ConfigsManager ConfigsManager => _configsManager;
        public ControllersManager ControllersManager => _controllersManager;
        public LayersManager LayersManager => _layersManager;
        public PrefabsManager PrefabsManager => _prefabsManager;
        public UIManager UIManager => _uiManager;

        protected override void Awake()
        {
            base.Awake();
            GetComponents();
            InitializeComponents();
        }

        private void OnEnable()
        {
            EnableComponents();
        }

        private void OnDisable()
        {
            DisableComponents();
        }

        private void GetComponents()
        {
            _audioManager = GetComponentInChildren<AudioManager>();
            _cameraManager = GetComponentInChildren<CameraManager>();
            _configsManager = GetComponentInChildren<ConfigsManager>();
            _controllersManager = GetComponentInChildren<ControllersManager>();
            _layersManager = GetComponentInChildren<LayersManager>();
            _prefabsManager = GetComponentInChildren<PrefabsManager>();
            _uiManager = GetComponentInChildren<UIManager>();
        }

        private void InitializeComponents()
        {
            _configsManager.Initialize();
            _audioManager.Initialize();
            _controllersManager.Initialize();
            _cameraManager.Initialize();
            _uiManager.Initialize();
        }

        private void EnableComponents()
        {
            _controllersManager.Enable();
            _cameraManager.Enable();
            _uiManager.Enable();
        }

        private void DisableComponents()
        {
            _uiManager.Disable();
            _cameraManager.Disable();
            _controllersManager.Disable();
        }

        private void Update()
        {
            _controllersManager.OnUpdate();
        }

        private void FixedUpdate()
        {
            _controllersManager.OnFixedUpdate();
        }

        private void LateUpdate()
        {
            _cameraManager.OnLateUpdate();
        }

        public void SetInputMode(InputMode mode)
        {
            _inputMode = mode;
            switch (_inputMode)
            {
                case InputMode.Game:
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    break;
                case InputMode.UI:
                    Cursor.lockState = CursorLockMode.Confined;
                    Cursor.visible = true;
                    break;
            }
        }
    }
}