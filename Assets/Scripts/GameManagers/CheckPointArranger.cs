using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AIs;

namespace GameManagers
{
    public class CheckPointArranger : MonoBehaviour
    {
        [SerializeField] private CoursePath coursePath;
        [SerializeField] private int minCheckPointCount;
        [SerializeField] private Vector3 colliderSize;
        [SerializeField] private List<CheckPoint> checkPoints;

        void OnValidate()
        {
            minCheckPointCount = Mathf.Clamp(minCheckPointCount, 3, int.MaxValue);
        }

        public void GenerateCheckPoints()
        {
            foreach (var checkPoint in checkPoints)
            {
                DestroyImmediate(checkPoint.gameObject);
            }
            var chunkItemCount = coursePath.Corners.Count() / minCheckPointCount;
            checkPoints = coursePath.Corners
                .Select((v, i) => new { Index = i, Vector3 = v })
                .GroupBy(x => x.Index / chunkItemCount, x => x.Vector3)
                .Select((list, i) =>
                {
                    var gameObject = new GameObject($"CheckPoint{i + 1}");
                    gameObject.transform.parent = this.transform;
                    gameObject.transform.position = list.First();
                    gameObject.transform.LookAt(list.ElementAt(1));
                    var collider = gameObject.AddComponent<BoxCollider>();
                    collider.isTrigger = true;
                    collider.size = colliderSize;
                    return gameObject.AddComponent<CheckPoint>();
                })
                .ToList();
        }
    }
}