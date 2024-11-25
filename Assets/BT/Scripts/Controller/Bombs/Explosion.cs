using System.Collections;
using UnityEngine;

namespace BombThrower
{
    [RequireComponent(typeof(ParticleSystem))]
    public class Explosion : MonoBehaviour
    {
        private static readonly Vector3 UnsetPosition = new Vector3(float.MinValue, float.MinValue, float.MinValue);

        [SerializeField] private float m_force = 20f;
        [SerializeField] private float m_radius = 5f;
        [SerializeField] private float m_upwards = 0f;
        [SerializeField] private bool shouldFadeOut = false; // フェードアウトを行うかどうか

        public Vector3 makeColliderPosition = UnsetPosition;

        /// <summary>
        /// 爆発の位置とパラメータを設定するメソッド
        /// </summary>
        /// <param name="position">爆発の中心位置</param>
        /// <param name="force">爆発力</param>
        /// <param name="radius">爆発半径</param>
        /// <param name="upwards">爆発の上方向への力</param>
        /// <param name="fadeOut">フェードアウトを行うかどうか</param>
        public void SetData(Vector3 position, float force = 20f, float radius = 5f, float upwards = 0f, bool fadeOut = false)
        {
            makeColliderPosition = position;
            m_force = Mathf.Max(m_force, force);
            m_radius = Mathf.Max(m_radius, radius);
            m_upwards = Mathf.Max(m_upwards, upwards);
            m_upwards = Mathf.Max(m_upwards, upwards);
            shouldFadeOut = fadeOut;

            // ParticleSystem のサイズを radius に合わせて設定
            AdjustParticleSystemSize(radius);
        }

        protected virtual void Start()
        {
            // 初期化確認
            if (makeColliderPosition == UnsetPosition)
            {
                Debug.LogWarning("Explosion script requires makeColliderPosition to be set before starting.");
            }

            StartCoroutine(ExecuteExplosion());
        }

        private IEnumerator ExecuteExplosion()
        {
            var particleSystem = GetComponent<ParticleSystem>();

            // 爆発範囲内の Rigidbody に AddExplosionForce を適用
            Collider[] hitColliders = Physics.OverlapSphere(makeColliderPosition, m_radius);
            foreach (var collider in hitColliders)
            {
                var rb = collider.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(m_force, makeColliderPosition, m_radius, m_upwards, ForceMode.Impulse);
                }
            }

            if (shouldFadeOut)
            {
                // フェードアウトと縮小を実行
                yield return StartCoroutine(FadeOutAndShrink(particleSystem));
            }
            else
            {
                // パーティクルシステムの完了を待ってからオブジェクトを破壊
                yield return new WaitUntil(() => !IsParticleSystemAlive(particleSystem));
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// パーティクルシステムが生きているかを判定するメソッド
        /// </summary>
        private bool IsParticleSystemAlive(ParticleSystem p)
        {
            if (p.main.loop)
            {
                // 時間を累積
                time += Time.deltaTime;
                return time <= p.main.duration;
            }
            return p.IsAlive();
        }

        /// <summary>
        /// ParticleSystem のサイズを radius に基づいて設定
        /// </summary>
        private void AdjustParticleSystemSize(float radius)
        {
            var particleSystem = GetComponent<ParticleSystem>();
            var mainModule = particleSystem.main;
            mainModule.startSize = radius * 2f; // 半径から直径を設定
        }

        private float time = 0f;

        /// <summary>
        /// フェードアウトと縮小を行うコルーチン
        /// </summary>
        private IEnumerator FadeOutAndShrink(ParticleSystem p)
        {

            // 初期サイズと色
            Vector3 initialScale = transform.localScale;
            Color initialColor = Color.white;

            // Renderer の色を取得
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null && renderer.material.HasProperty("_Color"))
            {
                initialColor = renderer.material.color;
            }

            float fadeDuration = 2f; // フェードアウトの時間
            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                float t = elapsedTime / fadeDuration;

                // 縮小
                transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, t);

                // 色のフェードアウト
                if (renderer != null && renderer.material.HasProperty("_Color"))
                {
                    Color newColor = Color.Lerp(initialColor, new Color(initialColor.r, initialColor.g, initialColor.b, 0f), t);
                    renderer.material.color = newColor;
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // 最終的にサイズと色をゼロに設定
            transform.localScale = Vector3.zero;
            if (renderer != null && renderer.material.HasProperty("_Color"))
            {
                Color finalColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);
                renderer.material.color = finalColor;
            }

            Destroy(gameObject);
        }
    }
}
