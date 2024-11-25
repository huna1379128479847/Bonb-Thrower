using UnityEngine;
using BombThrower.Resource;
using System;

namespace BombThrower
{
    public static class BlockHoldersObjectHolder
    {
        public static GameObject Set1;
        public static GameObject Set2;
        public static GameObject Set3;
        public static GameObject Set4;
        public static GameObject Set5;
        public static GameObject Set6;

        public static ResourceHelper<GameObject> ResourceHelper;
        static BlockHoldersObjectHolder()
        {
            Type type = typeof(BlockHoldersObjectHolder);
            ResourceHelper = new ResourceHelper<GameObject>(type);
            ResourcesLoader.ResourcesSupport(type, "BlockHolders");
            
        }
    }
}