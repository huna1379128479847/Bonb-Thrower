using BombThrower.Utilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace BombThrower
{
    public class GameSceneDirector : SingletonBehavior<GameSceneDirector>
    {
        [Header("インプットシステム")]
        [SerializeField] private InputActionAsset _inGameAsset;
        private InputAction _pauseAndCont;

        [Header("スポーン")]
        [SerializeField] private float _spawnInterval = 20f;
        [SerializeField] private float _destoroyHeight = 10f;

        [Header("難易度管理")]
        [SerializeField] protected int addScore = 2;

        [Header("内部管理")]
        [SerializeField] private int _holderCount = 0;
        [SerializeField] private float _nextPatternSpawnTime;

        [Header("デバッグ用")]
        [SerializeField][Tooltip("ゲームのカウントダウン停止")] private bool d_timefrize;

        private ScoreController _scoreController;
        private TimeController _timeController;

        public UnityEvent Pause;
        public UnityEvent Play;

        public float DestroyHeight => _destoroyHeight;

        private void Start()
        {
            _scoreController = GetController<ScoreController>();
            _timeController = GetController<TimeController>();
#if !UNITY_EDITOR
            d_timefrize = false;
#endif
            _timeController.ResetTime();
            _scoreController.SetScore(0);
            _nextPatternSpawnTime = _timeController.GetTime() - _spawnInterval;
        }

        private void OnEnable()
        {
            if (_inGameAsset != null)
            {
                var map = _inGameAsset.actionMaps[0];
                _pauseAndCont = map.actions[1];

                _pauseAndCont.Enable();
                _pauseAndCont.performed += OnTabClick;
            }
        }
        private void OnDisable()
        {
            if (_inGameAsset != null)
            {
                _pauseAndCont.performed -= OnTabClick;
                _pauseAndCont.Disable();
            }
        }

        private TController GetController<TController>() where TController : MonoBehaviour
        {
            var controller = gameObject.GetComponent<TController>();
            if (controller == null)
            {
                Debug.LogWarning($"GameSceneDirector: No attached component of type {typeof(TController).Name} found.");
            }
            return controller;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (Time.timeScale == 0)
                {
                    GamePlay();
                }
                else
                {
                    GamePause();
                }
            }
        }
        public virtual void AddScore(string blocktag)
        {
            _scoreController.AddScore(2);
        }

        private void FixedUpdate()
        {
            if (d_timefrize) return;

            ManageTime();
            if (_timeController.GetTime() <= _nextPatternSpawnTime)
            {
                TrySpawnBlockHolderOnFloor();
            }
        }

        private void ManageTime()
        {
            var remainingTime = _timeController.GetTime();
            if (remainingTime <= 0)
            {
                SceneChanger.score = _scoreController.GetScore();
                CursorHelper.UnlockCursor();
                SceneChanger.instance.LoadLevel("Result");
            }
            else
            {
                _timeController.SubTime(Time.fixedDeltaTime);
            }
        }

        private void TrySpawnBlockHolderOnFloor()
        {
            HolderSummonController.instance.TryNewSummonSets();
            _nextPatternSpawnTime -= _spawnInterval;
        }


        public void AddBlockHolder()
        {
            _holderCount++;
        }

        public void RemoveBlockHolder()
        {
            _holderCount--;
            if (_holderCount < 3)
            {
                TrySpawnBlockHolderOnFloor();
            }
        }

        private void OnTabClick(InputAction.CallbackContext _)
        {
            if (Time.timeScale == 0)
                GamePlay();
            else
                GamePause();
        }
        
        public void GamePause()
        {
            Time.timeScale = 0;
            Pause?.Invoke();
            CursorHelper.UnlockCursor();
        }

        public void GamePlay()
        {
            Time.timeScale = 1f;
            Play?.Invoke();
            CursorHelper.LockCursor();
        }
    }
}
