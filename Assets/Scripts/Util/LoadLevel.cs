using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Util
{
    public class LoadLevel : MonoBehaviour
    {
        public void OpenLevel(string name)
        {
            SceneManager.LoadScene(name);
        }

        public void OpenLevel(int level)
        {
            SceneManager.LoadScene(level);
        }

        public void Quit()
        {
            Application.Quit();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

    }
}

