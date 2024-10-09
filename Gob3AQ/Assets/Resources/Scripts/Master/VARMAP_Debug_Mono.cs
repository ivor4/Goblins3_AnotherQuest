using UnityEngine;
using Gob3AQ.VARMAP.Debug;
using Gob3AQ.VARMAP.Variable;
using System.Collections.Generic;
using Gob3AQ.VARMAP.Enum;

[System.Serializable]
public class VARMAP_Debug_Mono : MonoBehaviour
{
    [System.Serializable]
    public struct varmapElem
    {
        public VARMAP_Variable_ID ID;
        public List<string> list;
    }

    [SerializeField]
    public List<varmapElem> DEBUGVALUES;

    private VARMAP_Variable_Indexable[] refer;


    void Awake()
    {
#if UNITY_EDITOR
        DEBUGVALUES = new List<varmapElem>();
#endif
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
#if UNITY_EDITOR
        refer = VARMAP_Debug_Master.GetRef();
        

        for(int i=0;i<refer.Length - 1;i++)
        {
            varmapElem newitem = new varmapElem();
            newitem.ID = refer[i + 1].GetID();
            newitem.list = new List<string>();
            DEBUGVALUES.Add(newitem);
        }
#else
        Destroy(gameObject);
#endif
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR

        for(int i=0;i< DEBUGVALUES.Count;i++)
        {
            string[] values = refer[i+1].GetDebugValues();

            DEBUGVALUES[i].list.Clear();

            for (int e=0;e<values.Length;e++)
            {
                DEBUGVALUES[i].list.Add(values[e]);    
            }
        }
#endif
    }
}
