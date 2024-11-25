using System;
using UnityEngine;
using BombThrower.Resource;

namespace BombThrower
{
    public static class FloorObjectHolder
    {
        public static GameObject Mini_Floor_1;
        public static GameObject Floor_1;


        public static ResourceHelper<GameObject> ResourceHelper;
        static FloorObjectHolder()
        {
            Type type = typeof(FloorObjectHolder);
            ResourceHelper = new ResourceHelper<GameObject>(type);
            ResourcesLoader.ResourcesSupport(type, "Floor");
        }
    }
}