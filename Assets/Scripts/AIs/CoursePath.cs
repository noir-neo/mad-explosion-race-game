using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Debug = UnityEngine.Debug;

public class CoursePath : MonoBehaviour
{
    [SerializeField] private Vector3[] positions;

    [Conditional("UNITY_EDITOR")]
    public void CalculatePath()
    {
        positions = positions.Buffer(2, 1)
            .Where(p => p.Count == 2)
            .SelectMany(p => CalculatePath(p.First(), p.Last()))
            .Distinct()
            .ToArray();
    }

    private static IEnumerable<Vector3> CalculatePath(Vector3 sourcePosition, Vector3 targetPosition)
    {
        var path = new NavMeshPath();
        NavMesh.CalculatePath(sourcePosition, targetPosition, NavMesh.AllAreas, path);
        return path.corners;
    }

    private void OnDrawGizmosSelected()
    {
        foreach (var p in positions.Buffer(2, 1).Where(p => p.Count == 2))
        {
            Debug.DrawLine(p.First(), p.Last(), Color.green);
        }
    }
}
