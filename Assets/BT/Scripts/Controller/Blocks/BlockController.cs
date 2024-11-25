using UnityEngine;

namespace BombThrower
{
    public class BlockController : MonoBehaviour
    {
        [Header("�u���b�N�ݒ�")]
        [SerializeField] private string _blockTag;
        [SerializeField] private int _idx;

        // Check interval for position check (in seconds)
        private static readonly float CheckInterval = 0.5f;

        
        private BlockHolderController _parent;

        private void Start()
        {
            // ����I�Ȉʒu�`�F�b�N���J�n
            InvokeRepeating(nameof(CheckAltitudeAndDestroy), CheckInterval, CheckInterval);
        }

        private void DoDestroy()
        {
            if (GameSceneDirector.instance != null)
            {
                GameSceneDirector.instance.AddScore(_blockTag);
            }
            else
            {
                Debug.LogWarning("GameSceneDirector instance is not set. Cannot add score.");
            }

            _parent.DestroyBlock(_idx);
        }
        private void CheckAltitudeAndDestroy()
        {
            if (transform.position.y < GameSceneDirector.instance.DestroyHeight)
            {
                DoDestroy();
            }
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Terrain")
            {
                DoDestroy();
            }
        }
        private void OnDestroy()
        {
            // �K�v�Ȃ��Ȃ��������Ăяo�����L�����Z��
            CancelInvoke(nameof(CheckAltitudeAndDestroy));
        }

        public void SetParent(int idx, BlockHolderController parent)
        {
            _idx = idx;
            _parent = parent;
        }

        public void SetIdx(int idx)
        {
            _idx = idx;
        }
    }
}
