using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BombThrower
{
    public class BlockHolderController : MonoBehaviour
    {
        [SerializeField] private List<BlockController> _blocks = new List<BlockController>();

        private void Start()
        {
            GameSceneDirector.instance.AddBlockHolder();
            _blocks = GetChildBlocks();
            StartCoroutine(WaitBlockClear());
        }

        private List<BlockController> GetChildBlocks()
        {
            return GetChildren(gameObject);
        }

        private List<BlockController> GetChildren(GameObject obj)
        {
            List<BlockController> result = new List<BlockController>();

            // 子要素が存在する場合に再帰的に取得
            foreach (Transform child in obj.transform)
            {
                var blockController = child.GetComponent<BlockController>();
                if (blockController != null)
                {
                    blockController.SetParent(result.Count, this); // インデックスを設定
                    result.Add(blockController); // 子オブジェクトを追加
                    result.AddRange(GetChildren(child.gameObject)); // 再帰的に子要素も追加
                }
                else
                {
                    Debug.LogWarning($"GetChildren: '{child.gameObject.name}' does not have a BlockController component.");
                }
            }

            return result;
        }

        public void DestroyBlock(int idx)
        {
            if (_blocks.Count > idx && _blocks[idx] != null)
            {
                Destroy(_blocks[idx].gameObject);
                _blocks.RemoveAt(idx);
                UpdateBlockIndices(); // インデックスを更新
            }
        }

        private void UpdateBlockIndices()
        {
            for (int i = 0; i < _blocks.Count; i++)
            {
                _blocks[i].SetIdx(i);
            }
        }

        private IEnumerator WaitBlockClear()
        {
            yield return new WaitUntil(() => _blocks.Count == 0);
            GameSceneDirector.instance.RemoveBlockHolder();
            Destroy(gameObject);
        }
    }
}
