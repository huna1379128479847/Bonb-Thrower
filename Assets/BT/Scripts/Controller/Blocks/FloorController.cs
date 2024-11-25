using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BombThrower
{
    public enum MovePattern
    {
        Static,
        Straight,
        Curve,
    }

    public enum Loop
    {
        Once,
        PingPong
    }

    public class FloorController : MonoBehaviour
    {
        [Header("床の状態")]
        [SerializeField] private float moveSpeed = 5f; // 移動速度
        [SerializeField] private MovePattern movePattern = MovePattern.Straight; // 移動パターン
        [SerializeField] private Loop loopMode = Loop.Once; // ループモード
        [SerializeField] private GameObject init;

        [SerializeField] private bool isMoving = false; // 移動中かどうか
        private bool isReversing = false; // 往復中かどうか
        private List<GameObject> destinations = new List<GameObject>(); // 目的地リスト
        private int currentDestinationIndex = 0; // 現在の目的地インデックス


        /// <summary>
        /// 初期化メソッド
        /// </summary>
        /// <param name="destinationParent">移動先の親オブジェクト（子にAnchorを持つ）</param>
        /// <param name="pattern">移動パターン</param>
        /// <param name="loop">ループモード</param>
        /// <param name="isStart">開始時に移動を開始するかどうか</param>
        /// <returns>自身のTransform</returns>
        public Transform Initialize(GameObject destinationParent, MovePattern pattern, Loop loop = Loop.Once, bool isStart = true)
        {
            List<GameObject> destinations = new List<GameObject>();

            destinations = GetDestinations(init);


            if (destinations == null || destinations.Count < 2)
            {
                Debug.LogError("FloorController: Initialize requires at least two destinations.");
                movePattern = MovePattern.Static;
                return transform;
            }

            this.destinations = destinations;
            movePattern = pattern;
            loopMode = loop;
            isReversing = false;
            currentDestinationIndex = 1; // 最初の目的地に設定
            transform.position = destinations[0].transform.position; // 初期位置を最初の目的地に設定
            if (isStart)
            {
                StartMoving();
            }
            return transform;
        }

        /// <summary>
        /// 指定された親オブジェクトからタグが "Anchor" の子オブジェクトを取得
        /// </summary>
        /// <param name="parent">親オブジェクト</param>
        /// <returns>目的地オブジェクトのリスト</returns>
        private List<GameObject> GetDestinations(GameObject parent)
        {
            List<GameObject> list = new List<GameObject>();
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                var child = parent.transform.GetChild(i);
                if (child.CompareTag("Anchor"))
                {
                    list.Add(child.gameObject);
                }
            }
            return list;
        }

        private void Start()
        {
            if (HolderSummonController.instance != null)
            {
                HolderSummonController.instance.SetFloor(gameObject);
            }
            else
            {
                Debug.LogWarning("FloorController: HolderSummonController instance is not set.");
            }
            if (init != null)
            {
                destinations = GetDestinations(init);
                isReversing = false;
                currentDestinationIndex = 1; // 最初の目的地に設定
                transform.position = destinations[0].transform.position; // 初期位置を最初の目的地に設定
                StartMoving();
            }
        }

        /// <summary>
        /// 移動を開始するメソッド
        /// </summary>
        public void StartMoving()
        {
            isMoving = true;
            StartCoroutine(MoveRoutine());
        }

        /// <summary>
        /// 移動を一時停止するメソッド
        /// </summary>
        public void PauseMoving()
        {
            isMoving = false;
        }

        /// <summary>
        /// 移動を再開するメソッド
        /// </summary>
        public void ResumeMoving()
        {
            if (!isMoving)
            {
                isMoving = true;
                StartCoroutine(MoveRoutine());
            }
        }

        private void Update()
        {
            if (movePattern == MovePattern.Static) return;
            // 他の入力処理や状態管理をここに追加
        }

        /// <summary>
        /// 移動を管理するコルーチン
        /// </summary>
        /// <returns></returns>
        private IEnumerator MoveRoutine()
        {
            while (isMoving && destinations.Count > 1)
            {
                if (currentDestinationIndex < 0 || currentDestinationIndex >= destinations.Count)
                {
                    Debug.LogError("FloorController: Current destination index is out of bounds.");
                    isMoving = false;
                    yield break;
                }

                Transform targetTransform = destinations[currentDestinationIndex].transform;
                Vector3 targetPosition = targetTransform.position;

                switch (movePattern)
                {
                    case MovePattern.Straight:
                        while (Vector3.Distance(transform.position, targetPosition) > 0.1f && isMoving)
                        {
                            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                            yield return null;
                        }
                        break;

                    case MovePattern.Curve:
                        if (currentDestinationIndex + 1 < destinations.Count)
                        {
                            Vector3 p0 = transform.position;
                            Vector3 p1 = (p0 + targetPosition) / 2 + Vector3.up * 5f; // 制御点（適宜調整）
                            Vector3 p2 = targetPosition;

                            float t = 0f;
                            while (t < 1f && isMoving)
                            {
                                transform.position = CalculateBezierPoint(t, p0, p1, p2);
                                t += moveSpeed * Time.deltaTime / Vector3.Distance(p0, p2);
                                yield return null;
                            }
                        }
                        else
                        {
                            // 制御点がない場合は直線移動
                            while (Vector3.Distance(transform.position, targetPosition) > 0.1f && isMoving)
                            {
                                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                                yield return null;
                            }
                        }
                        break;
                }

                // 目的地に到達した場合
                if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
                {
                    UpdateDestinationIndex();
                }

                yield return null;
            }
        }

        /// <summary>
        /// ベジェ曲線を計算するメソッド
        /// </summary>
        /// <param name="t">パラメータ（0～1）</param>
        /// <param name="p0">開始点</param>
        /// <param name="p1">制御点</param>
        /// <param name="p2">終了点</param>
        /// <returns>計算されたベジェ曲線上の点</returns>
        private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;

            Vector3 point = uu * p0; // (1-t)^2 * P0
            point += 2 * u * t * p1; // 2*(1-t)*t * P1
            point += tt * p2; // t^2 * P2

            return point;
        }

        /// <summary>
        /// 次の目的地のインデックスを更新するメソッド
        /// </summary>
        private void UpdateDestinationIndex()
        {
            if (loopMode == Loop.Once)
            {
                // 一方向ループモード
                currentDestinationIndex++;
                if (currentDestinationIndex >= destinations.Count)
                {
                    isMoving = false; // 移動停止
                }
            }
            else if (loopMode == Loop.PingPong)
            {
                // 往復ループモード
                if (!isReversing)
                {
                    currentDestinationIndex++;
                    if (currentDestinationIndex >= destinations.Count)
                    {
                        currentDestinationIndex = destinations.Count - 2; // 最後の目的地から一つ前に戻る
                        isReversing = true; // 逆方向へ切り替え
                    }
                }
                else
                {
                    currentDestinationIndex--;
                    if (currentDestinationIndex < 0)
                    {
                        currentDestinationIndex = 1; // 最初の目的地に戻る
                        isReversing = false; // 順方向へ切り替え
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (destinations != null && destinations.Count > 0)
            {
                Gizmos.color = Color.red;
                foreach (var dest in destinations)
                {
                    if (dest != null)
                    {
                        Gizmos.DrawSphere(dest.transform.position, 0.5f);
                    }
                }

                // 現在の目的地を強調表示
                if (currentDestinationIndex >= 0 && currentDestinationIndex < destinations.Count)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawSphere(destinations[currentDestinationIndex].transform.position, 1f);
                }
            }
        }
    }
}
