using BombThrower.Utilities;
using UnityEngine;

namespace BombThrower
{
    public class NormalBomb : MonoBehaviour
    {
        [Header("ボム設定")]
        [SerializeField] protected float Radius { get; private set; } = 5f;
        [SerializeField] protected float Force { get; private set; } = 20f;
        [SerializeField] protected float Up { get; private set; } = 0f;

        public void SetData(float radius, float force, float up)
        {
            Radius = radius;
            Force = force;
            Up = up;
        }

        protected virtual void Update()
        {
            if (IsBelowDestroyHeight())
            {
                Destroy(gameObject);
            }
        }

        private bool IsBelowDestroyHeight()
        {
            return transform.position.y < GameSceneDirector.instance.DestroyHeight;
        }

        protected virtual void OnCollisionEnter(Collision _)
        {
            var explosionEffect = CreateExplosionEffect();

            if (explosionEffect != null)
            {
                explosionEffect.SetData(transform.position, Force, Radius, Up);
            }
            else
            {
                Debug.LogWarning("Explosion effect could not be created. Make sure EffectMaker and EffectObjectHolder are set up correctly.");
            }

            Destroy(gameObject);
        }

        protected virtual Explosion CreateExplosionEffect()
        {
            if (NumberProcess.Chance(0.2f))
            {
                return EffectMaker.MakeEffect(CollectionsHelper.RandomPick(MagicObjectHolder.ResourceHelper.GetResources()), transform.position)
                                  ?.GetComponent<Explosion>();
            }
            else if (NumberProcess.Chance(0.4f))
            {
                return EffectMaker.MakeEffect(ExplotsionObjectHolder.BigExplosionEffect, transform.position)
                                  ?.GetComponent<Explosion>();
            }
            else
            {
                return EffectMaker.MakeEffect(ExplotsionObjectHolder.Explosion, transform.position)
                                  ?.GetComponent<Explosion>();
            }
        }
    }
}
