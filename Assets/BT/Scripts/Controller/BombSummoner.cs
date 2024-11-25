using BombThrower.Utilities;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BombThrower
{
    [RequireComponent(typeof(CameraController))]
    public class BombSummoner : MonoBehaviour
    {
        [Header("インプット")]
        [SerializeField] private InputActionAsset _inGameAsset;
        private InputAction _fireAction;

        [Header("ボム設定")]
        [SerializeField] private GameObject _bombPrefab;
        [SerializeField] private float _addPower = 1000f;
        [SerializeField] private float _fallDelay = 0f;
        [SerializeField] private Vector3 _spawnPositionOffset = Vector3.zero;
        [SerializeField] private Vector3 _spawnRotationOffset = Vector3.zero;
        [SerializeField] private float _fireInterval = 0.5f;

        private Camera _camera;
        private CameraController _controller;
        private EntityManager _entityManager;

        [Header("内部管理")]
        [SerializeField] private float _currentTime = 0f;

        [Header("デバッグ")]
        [SerializeField] private bool d_through_coolTime;

        private void OnEnable()
        {
            _controller = GetComponent<CameraController>();
            _camera = Camera.main;

            if (_inGameAsset != null)
            {
                var map = _inGameAsset.actionMaps[0];
                _fireAction = map.actions[0];

                _fireAction.Enable();
                _fireAction.performed += OnClickFire;
            }
            else
            {
                Debug.LogError("InGameAsset is null. Please assign an InputActionAsset.");
            }

            if (_camera == null)
            {
                Debug.LogError("Camera not found. Please assign a main camera.");
            }

            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        }

        private void OnDisable()
        {
            if (_fireAction != null)
            {
                _fireAction.performed -= OnClickFire;
                _fireAction.Disable();
            }
        }

        private void Update()
        {
            _currentTime = Mathf.Max(_currentTime - Time.deltaTime, 0);
        }

        private void OnClickFire(InputAction.CallbackContext ctx)
        {
            if (d_through_coolTime || _currentTime == 0)
            {
                var g = Instantiate(_bombPrefab, transform.position, transform.rotation);
                var r = g.GetComponent<Rigidbody>();
                r.AddForce(_addPower * transform.forward);
                if (_fallDelay > 0)
                {
                    r.useGravity = false;
                }
                _currentTime = _fireInterval;
            }
        }
    }
}
