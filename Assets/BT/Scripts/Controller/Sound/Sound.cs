using System.Collections;
using UnityEngine;

namespace BombThrower
{
    /// <summary>
    /// サウンドを再生し、再生が完了したらオブジェクトを破壊するコンポーネント
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class Sound : MonoBehaviour
    {
        private AudioSource _audioSource;

        [SerializeField]
        private bool _fadeOut = false; // フェードアウトを行うかどうか

        [SerializeField, Range(0f, 5f)]
        private float _fadeOutDuration = 1f; // フェードアウトの時間

        private void Awake()
        {
            // AudioSource コンポーネントを取得
            _audioSource = GetComponent<AudioSource>();
            if (_audioSource == null)
            {
                Debug.LogError("Sound: AudioSource component is missing.");
            }
        }

        private void Start()
        {
            if (_audioSource != null)
            {
                // AudioClip が設定されている場合のみ再生
                if (_audioSource.clip != null)
                {
                    _audioSource.Play();
                    StartCoroutine(HandleSoundLifecycle());
                    GameSceneDirector.instance.Play.AddListener(Play);
                    GameSceneDirector.instance.Pause.AddListener(Pause);
                }
                else
                {
                    Debug.LogWarning("Sound: No AudioClip assigned to AudioSource.");
                    Destroy(gameObject);
                }
            }
        }

        private void OnDestroy()
        {
            GameSceneDirector.instance.Pause.RemoveListener(Pause);
            GameSceneDirector.instance.Play.RemoveListener(Play);
        }
        public void Pause()
        {
            _audioSource?.Pause();
        }

        public void Play()
        {
            _audioSource?.Play();
        }

        /// <summary>
        /// サウンドの再生が完了したらオブジェクトを破壊するコルーチン
        /// </summary>
        private IEnumerator HandleSoundLifecycle()
        {
            // サウンドが再生中かどうかを監視
            yield return new WaitUntil(() => !_audioSource.isPlaying);

            if (_fadeOut)
            {
                yield return StartCoroutine(FadeOutAndDestroy());
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// フェードアウトしながらオブジェクトを破壊するコルーチン
        /// </summary>
        private IEnumerator FadeOutAndDestroy()
        {
            float elapsedTime = 0f;
            float startVolume = _audioSource.volume;

            while (elapsedTime < _fadeOutDuration)
            {
                elapsedTime += Time.deltaTime;
                _audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / _fadeOutDuration);
                yield return null;
            }

            _audioSource.volume = 0f;
            Destroy(gameObject);
        }
    }
}
