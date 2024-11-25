using BombThrower.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace BombThrower
{
    public class CameraController : MonoBehaviour
    {
        [Header("カメラ設定")]
        [SerializeField] private InputActionAsset _cameraActionAsset;
        [SerializeField] private Camera _camera;
        [SerializeField] private float _xSensitivity = 0.3f; // マウスのX軸感度
        [SerializeField] private float _ySensitivity = 0.7f; // マウスのY軸感度

        private InputAction _cameraRollAction; // 視野の回転
        private InputAction _cameraPinchAction; // カメラのピンチ、ピンチアウト
        private InputAction _cameraZoomSwitch;

        [Header("ズーム関連")]
        [SerializeField] private float _zoomSpeed = 1f;
        [SerializeField] private float _normalFOV = 60f;
        [SerializeField][Range(0f, 180f)] private float _zoomedFOV = 30f;
        [SerializeField][Range(0f, 50f)] private float _zoomedMinFOV = 5f;
        [SerializeField][Range(1, 100)] private int _zoomLevels = 10;
        [SerializeField] private Image _crossCursor;

        private Vector3 _currentCameraAngle;
        private bool _isPinched;
        private int _currentZoomLevel = 0;

        public bool IsPinched => _isPinched;
        private float ZoomValuePerLevel => (_zoomedFOV - _zoomedMinFOV) / _zoomLevels;

        private void OnEnable()
        {
            if (_cameraActionAsset != null)
            {
                var actionMap = _cameraActionAsset.actionMaps[0];
                _cameraRollAction = actionMap.actions[0];
                _cameraPinchAction = actionMap.actions[1];
                _cameraZoomSwitch = actionMap.actions[2];

                _cameraRollAction.Enable();
                _cameraRollAction.performed += OnScroll;

                _cameraPinchAction.Enable();
                _cameraPinchAction.performed += OnMouseScroll;

                _cameraZoomSwitch.Enable();
                _cameraZoomSwitch.performed += OnZoomToggle;
                CursorHelper.LockCursor();
            }
            else
            {
                Debug.LogError("CameraActionAsset is null.");
            }

            if (_camera != null)
            {
                _currentCameraAngle = _camera.transform.rotation.eulerAngles;
            }
            else
            {
                Debug.LogError("CameraController: No camera assigned.");
            }
        }

        private void OnDisable()
        {
            if (_cameraRollAction != null)
            {
                _cameraRollAction.performed -= OnScroll;
                _cameraRollAction.Disable();
            }

            if (_cameraPinchAction != null)
            {
                _cameraPinchAction.performed -= OnMouseScroll;
                _cameraPinchAction.Disable();
            }

            if (_cameraZoomSwitch != null)
            {
                _cameraZoomSwitch.performed -= OnZoomToggle;
                _cameraZoomSwitch.Disable();
            }
            CursorHelper.UnlockCursor();
        }

        private void OnScroll(InputAction.CallbackContext context)
        {
            float t = Time.deltaTime;
            Vector2 scrollValue = context.ReadValue<Vector2>();

            Vector3 v = new Vector3();
            v.x = scrollValue.y * _ySensitivity * t * -1;
            v.y = scrollValue.x * _xSensitivity * t;

            _currentCameraAngle += v;

            // カメラの回転を適用
            _camera.transform.rotation = Quaternion.Euler(_currentCameraAngle);
        }

        private void OnMouseScroll(InputAction.CallbackContext context)
        {
            if (!_isPinched)
                return;

            float y = context.ReadValue<Vector2>().y;

            if (y > 0)
            {
                _currentZoomLevel++;
            }
            else if (y < 0)
            {
                _currentZoomLevel--;
            }

            _currentZoomLevel = Mathf.Clamp(_currentZoomLevel, 0, _zoomLevels);
        }

        private void OnZoomToggle(InputAction.CallbackContext context)
        {
            _isPinched = !_isPinched;
            if (!_isPinched)
                _currentZoomLevel = 0;
        }

        public void Update()
        {
            UpdateZoomUI();
        }
        private void UpdateZoomUI(bool skippingLeap = false)
        {
            _crossCursor.gameObject.SetActive(_isPinched);
            if (_isPinched)
            {
                float targetFOV = _zoomedFOV - _currentZoomLevel * ZoomValuePerLevel;
                targetFOV = Mathf.Clamp(targetFOV, _zoomedMinFOV, _normalFOV);
                if (!skippingLeap)
                    _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, targetFOV, _zoomSpeed * Time.deltaTime);
                else
                    _camera.fieldOfView = targetFOV;
            }
            else
            {
                if (!skippingLeap)
                    _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, _normalFOV, _zoomSpeed * Time.deltaTime * 0.5f);
                else
                    _camera.fieldOfView = _normalFOV;
            }
        }
    }
}
