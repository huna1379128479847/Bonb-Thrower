using UnityEngine;
using BombThrower.Utilities;

namespace BombThrower
{
    public class Grenade : MonoBehaviour
    {
        private void Start()
        {
            var dic = GrenadeSoundHolder.ResourceHelper.GetResources();
            var sound = CollectionsHelper.RandomPick(dic);
            EffectMaker.MakeEffect(transform, sound);
        }
    }
}
