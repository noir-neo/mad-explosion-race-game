using UnityEditor;
using UnityEngine;

namespace AIs
{
    [CustomEditor(typeof(CoursePath))]
    public class CoursePathEditor : Editor
    {
        private CoursePath _coursePath;

        private const string CalculatePath = "Calculate Path";

        void OnEnable()
        {
            _coursePath = target as CoursePath;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            serializedObject.Update();
            if(GUILayout.Button(CalculatePath))
            {
                Undo.RecordObject(_coursePath, CalculatePath);
                _coursePath.CalculatePath();
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
