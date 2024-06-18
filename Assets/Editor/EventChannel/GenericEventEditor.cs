using EventSO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace EventSO
{
#if UNITY_EDITOR
    public abstract class GenericEventEditor<T> : Editor
    {
        SerializedProperty _testData;

        private void OnEnable()
        {
            _testData = serializedObject.FindProperty("_testData");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();
            GUI.enabled = Application.isPlaying;

            GenericEventChannelSO<T> e = target as GenericEventChannelSO<T>;

            if (Application.isPlaying)
                EditorGUILayout.PropertyField(_testData);

            if (GUILayout.Button("Raise"))
                e.Invoke(e._testData);

            serializedObject.ApplyModifiedProperties();

        }
    }
#endif
}



