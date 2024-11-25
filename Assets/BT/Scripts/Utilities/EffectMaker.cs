using UnityEngine;

namespace BombThrower.Utilities
{
    public static class EffectMaker
    {
        public static GameObject MakeEffect(Transform target, GameObject effect, Vector3 padding = default)
        {
            if (effect == null)
            {
                Debug.LogError("エフェクトプレハブが空です");
            }
            return Object.Instantiate(effect, target.position + padding, Quaternion.identity, target);
        }

        public static GameObject MakeEffect(GameObject effect, Vector3 position)
        {
            return Object.Instantiate(effect, position, Quaternion.identity);
        }
    }
}