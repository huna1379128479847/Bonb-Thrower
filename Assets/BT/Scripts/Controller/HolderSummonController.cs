using System;
using System.Collections.Generic;
using UnityEngine;
using BombThrower.Utilities;

namespace BombThrower
{
    public class HolderSummonController : SingletonBehavior<HolderSummonController>
    {
        [Header("インスペクター監視用")]
        [SerializeField] private List<GameObject> _floorlist = new List<GameObject>();
        [SerializeField] private List<GameObject> _summonPoint = new List<GameObject>();

        public void SetFloor(GameObject floor) => _floorlist.Add(floor);

        public void SetPoint(GameObject point) => _summonPoint.Add(point);

        public GameObject SummonHolderOnNewFloor(GameObject holder, GameObject floor, Vector3 floorPosition, GameObject anchor = null, MovePattern pattern = MovePattern.Static, Loop loop = Loop.Once, bool isStart = false)
        {
            var newFloor = CreateFloor(floor, floorPosition);
            if (newFloor == null) return null;

            ObjectSummoner.SummonBlockHolderOnFloor(holder, newFloor);
            ConfigureFloor(newFloor, anchor, pattern, loop, isStart);

            return newFloor;
        }

        public (bool Success, string Message) TryNewSummonSets()
        {
            var holderFloorPair = GetHolderFloorPair();
            if (holderFloorPair == null)
                return (false, "No valid holder and floor pair found.");

            if (_summonPoint.Count == 0)
                return (false, "No summon points available.");

            var summonPoint = GetSummonPoint();
            if (summonPoint == null)
                return (false, "Failed to get a valid summon point.");

            var spawnedFloor = SummonHolderOnNewFloor(holderFloorPair.Item1, holderFloorPair.Item2, summonPoint.transform.position);
            return (spawnedFloor != null, spawnedFloor != null ? "Success" : "Failed to summon floor.");
        }

        private GameObject CreateFloor(GameObject floorPrefab, Vector3 position)
        {
            return ObjectSummoner.SummonBlockHolder(floorPrefab, position);
        }

        private void ConfigureFloor(GameObject floor, GameObject anchor, MovePattern pattern, Loop loop, bool isStart)
        {
            var floorController = floor.GetComponent<FloorController>();
            if (floorController != null)
            {
                floorController.Initialize(anchor, pattern, loop, isStart);
            }
            else
            {
                Debug.LogWarning("ConfigureFloor: Floor does not have a FloorController component.");
            }
        }

        private Tuple<GameObject, GameObject> GetHolderFloorPair()
        {
            return new Tuple<GameObject, GameObject>(
                GetHolder(),
                GetFloor()
            );
        }

        private GameObject GetSummonPoint()
        {
            return CollectionsHelper.RandomPick(_summonPoint);
        }

        private GameObject GetHolder()
        {
            var holderResources = BlockHoldersObjectHolder.ResourceHelper.GetResources();
            if (holderResources == null || holderResources.Count == 0)
            {
                Debug.LogWarning("GetHolder: No block holders available.");
                return null;
            }
            return CollectionsHelper.RandomPick(holderResources);
        }

        private GameObject GetFloor()
        {
            var floorResources = FloorObjectHolder.ResourceHelper.GetResources();
            if (floorResources == null || floorResources.Count == 0)
            {
                Debug.LogWarning("GetFloor: No floors available.");
                return null;
            }
            return CollectionsHelper.RandomPick(floorResources);
        }
    }
}
