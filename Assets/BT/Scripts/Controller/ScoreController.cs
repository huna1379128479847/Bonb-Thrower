using UnityEngine;
using TMPro;
using BombThrower.Utilities;

namespace BombThrower
{
    public class ScoreController : MonoBehaviour
    {
        [SerializeField] private TMP_Text score;

        private int _currentScore;

        // Start is called before the first frame update
        void Start()
        {
            if (score == null)
            {
                Debug.LogError("ScoreController: score TMP_Text is not assigned in the inspector.");
                return;
            }
            SetText();
        }

        private void SetText()
        {
            score.SetText($"Score: {_currentScore:000}");
        }

        public void AddScore(int amount)
        {
            _currentScore += amount;
            SetText();
        }

        public void RemoveScore(int amount)
        {
            _currentScore = (_currentScore - amount).ClampToZeroOrPositive();
            SetText();
        }

        public void SetScore(int amount)
        {
            _currentScore = amount.ClampToZeroOrPositive();
            SetText();
        }

        public int GetScore()
        {
            return _currentScore;
        }
    }
}
