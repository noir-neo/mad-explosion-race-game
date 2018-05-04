using UnityEditor;
using UnityEngine;

namespace GameManagers
{
    [CustomEditor(typeof(CheckPointArranger))]
    public class CheckPointArrangerEditor : Editor
    {
        private CheckPointArranger checkPointArranger;

        private const string GenerateCheckPoints = "Generate CheckPoints";

        void OnEnable()
        {
            checkPointArranger = target as CheckPointArranger;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            serializedObject.Update();
            if(GUILayout.Button(GenerateCheckPoints))
            {
                Undo.RecordObject(checkPointArranger, GenerateCheckPoints);
                checkPointArranger.GenerateCheckPoints();
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
