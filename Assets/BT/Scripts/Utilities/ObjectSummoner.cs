using System.Linq;
using UnityEngine;

namespace BombThrower.Utilities
{
    public static class ObjectSummoner
    {
        public static GameObject SummonBlockHolder(GameObject holder, Vector3 position)
        {
            if (holder == null)
            {
                Debug.LogWarning("ObjectSummoner: Holder is null. Cannot summon block holder.");
                return null;
            }
            return GameObject.Instantiate(holder, position, Quaternion.identity);
        }

        public static GameObject SummonAnchorPoint(GameObject holder, GameObject anchorPoint)
        {
            if (holder == null || anchorPoint == null)
            {
                Debug.LogWarning("ObjectSummoner: Holder or AnchorPoint is null. Cannot summon object.");
                return null;
            }
            return SummonBlockHolder(holder, anchorPoint.transform.position);
        }

        public static GameObject SummonBlockHolderOnFloor(GameObject holder, GameObject floor)
        {
            if (holder == null || floor == null)
            {
                Debug.LogWarning("ObjectSummoner: Holder or Floor is null. Cannot summon object.");
                return null;
            }

            // Find all tagged "Anchor" children
            var holderList = floor.GetComponentsInChildren<Transform>().ToList()
                                  .Where(child => child.CompareTag("Anchor"))
                                  .Select(child => child.gameObject)
                                  .ToList();

            if (holderList.Count == 0)
            {
                Debug.LogWarning("ObjectSummoner: No 'Anchor' tagged objects found in floor.");
                var renderer = floor.GetComponent<Renderer>();
                if (renderer != null)
                {
                    return SummonBlockHolder(holder, renderer.bounds.center);
                }
                else
                {
                    Debug.LogWarning($"ObjectSummoner: Floor '{floor.name}' has no Renderer component. Cannot determine center for fallback spawn.");
                    return null;
                }
            }

            // Summon at a random anchor point
            return SummonAnchorPoint(holder, CollectionsHelper.RandomPick(holderList));
        }
    }
}
