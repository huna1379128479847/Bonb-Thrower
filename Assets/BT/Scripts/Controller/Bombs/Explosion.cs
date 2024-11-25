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
        [SerializeField] private bool shouldFadeOut = false; // �t�F�[�h�A�E�g���s�����ǂ���

        public Vector3 makeColliderPosition = UnsetPosition;

        /// <summary>
        /// �����̈ʒu�ƃp�����[�^��ݒ肷�郁�\�b�h
        /// </summary>
        /// <param name="position">�����̒��S�ʒu</param>
        /// <param name="force">������</param>
        /// <param name="radius">�������a</param>
        /// <param name="upwards">�����̏�����ւ̗�</param>
        /// <param name="fadeOut">�t�F�[�h�A�E�g���s�����ǂ���</param>
        public void SetData(Vector3 position, float force = 20f, float radius = 5f, float upwards = 0f, bool fadeOut = false)
        {
            makeColliderPosition = position;
            m_force = Mathf.Max(m_force, force);
            m_radius = Mathf.Max(m_radius, radius);
            m_upwards = Mathf.Max(m_upwards, upwards);
            m_upwards = Mathf.Max(m_upwards, upwards);
            shouldFadeOut = fadeOut;

            // ParticleSystem �̃T�C�Y�� radius �ɍ��킹�Đݒ�
            AdjustParticleSystemSize(radius);
        }

        protected virtual void Start()
        {
            // �������m�F
            if (makeColliderPosition == UnsetPosition)
            {
                Debug.LogWarning("Explosion script requires makeColliderPosition to be set before starting.");
            }

            StartCoroutine(ExecuteExplosion());
        }

        private IEnumerator ExecuteExplosion()
        {
            var particleSystem = GetComponent<ParticleSystem>();

            // �����͈͓��� Rigidbody �� AddExplosionForce ��K�p
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
                // �t�F�[�h�A�E�g�Ək�������s
                yield return StartCoroutine(FadeOutAndShrink(particleSystem));
            }
            else
            {
                // �p�[�e�B�N���V�X�e���̊�����҂��Ă���I�u�W�F�N�g��j��
                yield return new WaitUntil(() => !IsParticleSystemAlive(particleSystem));
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// �p�[�e�B�N���V�X�e���������Ă��邩�𔻒肷�郁�\�b�h
        /// </summary>
        private bool IsParticleSystemAlive(ParticleSystem p)
        {
            if (p.main.loop)
            {
                // ���Ԃ�ݐ�
                time += Time.deltaTime;
                return time <= p.main.duration;
            }
            return p.IsAlive();
        }

        /// <summary>
        /// ParticleSystem �̃T�C�Y�� radius �Ɋ�Â��Đݒ�
        /// </summary>
        private void AdjustParticleSystemSize(float radius)
        {
            var particleSystem = GetComponent<ParticleSystem>();
            var mainModule = particleSystem.main;
            mainModule.startSize = radius * 2f; // ���a���璼�a��ݒ�
        }

        private float time = 0f;

        /// <summary>
        /// �t�F�[�h�A�E�g�Ək�����s���R���[�`��
        /// </summary>
        private IEnumerator FadeOutAndShrink(ParticleSystem p)
        {

            // �����T�C�Y�ƐF
            Vector3 initialScale = transform.localScale;
            Color initialColor = Color.white;

            // Renderer �̐F���擾
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null && renderer.material.HasProperty("_Color"))
            {
                initialColor = renderer.material.color;
            }

            float fadeDuration = 2f; // �t�F�[�h�A�E�g�̎���
            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                float t = elapsedTime / fadeDuration;

                // �k��
                transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, t);

                // �F�̃t�F�[�h�A�E�g
                if (renderer != null && renderer.material.HasProperty("_Color"))
                {
                    Color newColor = Color.Lerp(initialColor, new Color(initialColor.r, initialColor.g, initialColor.b, 0f), t);
                    renderer.material.color = newColor;
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // �ŏI�I�ɃT�C�Y�ƐF���[���ɐݒ�
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
