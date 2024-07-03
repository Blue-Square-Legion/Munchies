using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EventSO.DebugEditor
{
    [CustomEditor(typeof(MusicDataEventChannel), editorForChildClasses: true)]
    public class MusicDataEventChannelEditor : GenericEventEditor<MusicDataFormat>
    {
    }
}
