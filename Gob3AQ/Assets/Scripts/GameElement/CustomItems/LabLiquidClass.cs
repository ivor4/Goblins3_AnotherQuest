using Gob3AQ.ItemMaster;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.ItemMaster;
using UnityEngine;

namespace Gob3AQ.GameElement.Extension.LabLiquid
{
    public class LabLiquidClass : MonoBehaviour, IGameElementExtension
    {
        private GameObject body;
        private SpriteRenderer spriteRenderer;
        private LabLiquidConf liquidConf;
        private float liquidLevelTarget;
        private float liquidLevelActual;
        private ulong prevTimestamp;

        void IGameElementExtension.OnDespawn()
        {
            
        }

        void IGameElementExtension.OnExtensionDestroy()
        {
            ItemMasterClass.RemoveItemExtension(ItemExtensionFunction.ITEM_EXTENSION_FN_FILL_LAB_LIQUID);
        }

        void IGameElementExtension.OnReceiveExtFn(ItemExtensionFunction extFn, in NumberPack numberPack)
        {
            switch(extFn)
            {
                case ItemExtensionFunction.ITEM_EXTENSION_FN_FILL_LAB_LIQUID:
                    FillLiquidFunction(in numberPack);
                    break;
            }
        }

        void IGameElementExtension.OnSpawn()
        {
            
        }

        private void Awake()
        {
            body = transform.Find("Body").gameObject;
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            liquidLevelActual = 0f;
            liquidLevelTarget = 0f;

            body.transform.localScale = new Vector3(body.transform.localScale.x, 0, 1);
            ItemMasterClass.AddItemExtension(ItemExtensionFunction.ITEM_EXTENSION_FN_FILL_LAB_LIQUID, this);
        }

        private void Update()
        {
            if(liquidLevelActual < liquidLevelTarget)
            {
                ulong actualTimestamp = VARMAP_ItemMaster.GET_ELAPSED_TIME_MS();
                ulong delta = actualTimestamp - prevTimestamp;

                liquidLevelActual = Mathf.Clamp(liquidLevelActual + (delta * 1f), 0f, liquidLevelTarget);

                RefreshLiquid();

                prevTimestamp = actualTimestamp;
            }
        }

        private void FillLiquidFunction(in NumberPack numberPack)
        {
            /* Mix breakdwon */
            liquidConf = new LabLiquidConf(in numberPack);

            Debug.Log($"Unpacking update  npack.long1 {numberPack.long1} and nFloorw {liquidConf.nFloorwasher} and nTotal {liquidConf.nTotal}");

            liquidLevelTarget = liquidConf.nTotal;

            /* If immediate */
            if (numberPack.bool1)
            {
                liquidLevelActual = liquidLevelTarget;
                RefreshLiquid();
            }

            prevTimestamp = VARMAP_ItemMaster.GET_ELAPSED_TIME_MS();
        }

        private void RefreshLiquid()
        {
            body.transform.localScale = new Vector3(body.transform.localScale.x, liquidLevelActual, 1f);
        }

    }
}
