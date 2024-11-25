using System;
using UnityEngine;
using BombThrower.Resource;

namespace BombThrower
{
    public static class MagicObjectHolder
    {

        public static GameObject DarkMagic;

        public static GameObject ReverseMagic;

        public static ResourceHelper<GameObject> ResourceHelper;
        static MagicObjectHolder()
        {
            Type type = typeof(MagicObjectHolder);
            ResourceHelper = new ResourceHelper<GameObject>(type);
            ResourcesLoader.ResourcesSupport(type, "Effect");
        }
    }

    public static class ExplotsionObjectHolder
    {
        public static GameObject Explosion;

        public static GameObject BigExplosionEffect;

        public static ResourceHelper<GameObject> ResourceHelper;
        static ExplotsionObjectHolder()
        {
            Type type = typeof(ExplotsionObjectHolder);
            ResourceHelper = new ResourceHelper<GameObject>(type);
            ResourcesLoader.ResourcesSupport(type, "Effect");
        }
    }
}