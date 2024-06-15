using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EventSO
{
#if UNITY_EDITOR
    [CustomEditor(typeof(EventChannelSO), editorForChildClasses: true)]
    public class EventEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            EventChannelSO e = target as EventChannelSO;
            if (GUILayout.Button("Raise"))
            {
                e.Invoke();
            }
        }
    }
#endif
}