using UnityEngine;
using BombThrower.Resource;
using System;

namespace BombThrower
{
    public static class GrenadeSoundHolder
    {
        public static GameObject Grenade3Short;
        public static GameObject Grenade3;
        public static GameObject Grenade6Short;
        public static GameObject Grenade1Short;

        public static ResourceHelper<GameObject> ResourceHelper;
        static GrenadeSoundHolder()
        {
            Type type = typeof(GrenadeSoundHolder);
            ResourceHelper = new ResourceHelper<GameObject>(type);
            ResourcesLoader.ResourcesSupport(type, "Sound");
        }
    }

    public static class DarkMagicSoundHolder
    {
        public static GameObject hit10;

        public static ResourceHelper<GameObject> ResourceHelper;
        static DarkMagicSoundHolder()
        {
            Type type = typeof(DarkMagicSoundHolder);
            ResourceHelper = new ResourceHelper<GameObject>(type);
            ResourcesLoader.ResourcesSupport(type, "Sound");
        }
    }
}