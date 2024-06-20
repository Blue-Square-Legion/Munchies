using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Util
{
    public class Stringify : MonoBehaviour
    {
        public string Prefix = "";
        public string Suffix = "";

        public UnityEvent<string> OnTextChange;

        public void Convert(object obj)
        {
            OnTextChange.Invoke($"{Prefix}{obj.ToString()}{Suffix}");
        }

        public void Convert(int i) { Trigger(i.ToString()); }
        public void Convert(float i) { Trigger(i.ToString()); }

        private void Trigger(string text)
        {
            OnTextChange.Invoke($"{Prefix}{text}{Suffix}");
        }
    }
}

