using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gob3AQ.CameraMaster
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
