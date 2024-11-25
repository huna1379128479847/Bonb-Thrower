using UnityEngine;
using BombThrower.Resource;
using System;

namespace BombThrower
{
    public static class MoveObjectHolder
    {
        public static GameObject HarfCicle;

        public static ResourceHelper<GameObject> ResourceHelper;
        static MoveObjectHolder()
        {
            Type type = typeof(MoveObjectHolder);
            ResourceHelper = new ResourceHelper<GameObject>(type);
            ResourcesLoader.ResourcesSupport(type, "MovePoint");

        }
    }
}