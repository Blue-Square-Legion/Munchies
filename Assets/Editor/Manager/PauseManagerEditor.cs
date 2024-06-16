using EventSO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace EventSO
{
#if UNITY_EDITOR
    [CustomEditor(typeof(PauseManagerSO), editorForChildClasses: true)]
    public class PauseManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            PauseManagerSO e = target as PauseManagerSO;

            if (!e.IsPaused && GUILayout.Button("Pause"))
            {
                e.Pause();
            }

            if (e.IsPaused && GUILayout.Button("Resume"))
            {
                e.Resume();
            }
        }
    }
#endif
}



