using BombThrower.Utilities;
using TMPro;
using UnityEngine;

namespace BombThrower
{
    public class TimeController : MonoBehaviour
    {
        [SerializeField] private TMP_Text _timerText;
        [SerializeField] private float _currentTime;
        private const float DefaultTime = 60f;

        private void Start()
        {
            if (_timerText == null)
            {
                Debug.LogError("TimeController: Timer TMP_Text is not assigned in the inspector.");
                return;
            }
            SetText();
        }

        private void SetText()
        {
            _timerText.SetText($"{_currentTime:000.0}");
        }

        public void AddTime(float time)
        {
            _currentTime += time;
            SetText();
        }

        public void SubTime(float time)
        {
            _currentTime -= time;
            _currentTime = _currentTime.ClampToZeroOrPositive();
            SetText();
        }

        public void SetTime(float time)
        {
            _currentTime = time.ClampToZeroOrPositive();
            SetText();
        }

        public void ResetTime()
        {
            _currentTime = DefaultTime;
            SetText();
        }

        public float GetTime()
        {
            return _currentTime;
        }
    }
}
