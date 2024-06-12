using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Util
{
    public class Billboard : MonoBehaviour
    {
        private Transform target;

        private void Awake()
        {
            target = Camera.main.transform;
        }

        private void LateUpdate()
        {
            //transform.LookAt(mainCamera.transform);
            transform.rotation = target.rotation;
        }
    }
}

