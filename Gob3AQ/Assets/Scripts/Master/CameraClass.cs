using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MVerse.CameraMaster
{
    public class CameraClass : MonoBehaviour
    {
        private Camera _camera;
        private void Awake()
        {
            _camera = GetComponent<Camera>();
        }

        // Start is called before the first frame update
        private void Start()
        {

        }

        // Update is called once per frame
        private void Update()
        {

        }
    }
}
