using UnityEngine;
using BombThrower.Utilities;

namespace BombThrower
{
    public class Magic : MonoBehaviour
    {
        private void Start()
        {
            var d = DarkMagicSoundHolder.ResourceHelper.GetResources();
            var s = CollectionsHelper.RandomPick(d);
            EffectMaker.MakeEffect(transform, s);
        }
    }
}
