using TMPro;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace BombThrower
{
    [Flags]
    public enum BlockShowField
    {
        None = 0,
        Score = 1 << 0,
        Title = 1 << 1,
        Retry = 1 << 2,
        GotoTitle = 1 << 3,
        StageSelect = 1 << 4,
        ShowReslut = 1 << 5,
    }

    public class ReslutController : MonoBehaviour
    {
        [Header("リファレンス")]
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private InputActionAsset resultInput;
        [SerializeField] private Button retry;
        [SerializeField] private Button gotoTitle;
        [SerializeField] private Button stageSelect;
        [SerializeField] private Button showReslut;
        private InputAction confirm;

        [Header("設定")]
        [SerializeField] private string result = "Score:";
        [SerializeField] private string scoreformat = "{0:00000}";// 0埋め5桁
        [SerializeField] private string wonTitle = "勝つ";
        [SerializeField] private string lostTitle = "失う";
        [SerializeField] private float inputLockTime = 2f;

        [Header("値監視用")]
        [SerializeField] private float currentInputLockTime;
        [SerializeField] private string score;
        [SerializeField] private bool isWin;

        private void Start()
        {
            scoreText.gameObject.SetActive(false);
            titleText.gameObject.SetActive(false);
            retry.gameObject.SetActive(false);
            gotoTitle.gameObject.SetActive(false);
            stageSelect.gameObject.SetActive(false);
            showReslut.gameObject.SetActive(false);
        }
        private void OnEnable()
        {
            
        }
        public void Show(float score, bool isWin, BlockShowField field = BlockShowField.None, string format = "")
        {
            var f = format == "" ? scoreformat : format;
            this.score = result + string.Format(f, score);
            this.isWin = isWin;
            if (!field.HasFlag(BlockShowField.Score))
            {
                scoreText.gameObject.SetActive(true);
                scoreText.SetText(this.score);
            }
            if (!field.HasFlag(BlockShowField.Title))
            {
                var t = this.isWin ? wonTitle : lostTitle;
                titleText.gameObject.SetActive(true);
                titleText.SetText(t);
            }
            if (!field.HasFlag(BlockShowField.Retry))
                retry.gameObject.SetActive(true);
            if (!field.HasFlag(BlockShowField.GotoTitle))
                gotoTitle.gameObject.SetActive(true);
            if (!field.HasFlag(BlockShowField.StageSelect))
                stageSelect.gameObject.SetActive(true);
            if (!field.HasFlag(BlockShowField.ShowReslut))
                showReslut.gameObject.SetActive(true);
        }
    }
}
