using UnityEditor;
using UnityEngine;
using MVerse.VARMAP.EnemyMaster;
using MVerse.VARMAP.Types;
using MVerse.FixedConfig;
using System.Collections.Generic;

namespace MVerse.Boss.BossMaster
{
    public class BossMasterClass : MonoBehaviour
    {
        private static BossMasterClass _singleton;

        private void Awake()
        {
            if(_singleton != this)
            {
                _singleton = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {

            
        }


        private void Update()
        {

        }

        private void OnDestroy()
        {
            if(_singleton == this)
            {
                _singleton = null;
            }
        }


    }
}