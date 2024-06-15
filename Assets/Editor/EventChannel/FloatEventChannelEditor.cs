using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EventSO.DebugEditor
{
    [CustomEditor(typeof(FloatEventChannel), editorForChildClasses: true)]
    public class FloatEventChannelEditor : GenericEventEditor<float>
    {
    }

    [CustomEditor(typeof(IntEventChannel), editorForChildClasses: true)]
    public class IntEventChannelEditor : GenericEventEditor<int>
    {
    }
}
