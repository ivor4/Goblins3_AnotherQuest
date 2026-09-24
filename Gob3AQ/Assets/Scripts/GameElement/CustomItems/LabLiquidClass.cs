using Gob3AQ.ItemMaster;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.ItemMaster;
using UnityEngine;
using System.Collections.Generic;

namespace Gob3AQ.GameElement.Extension.LabLiquidExt
{
    public class LabLiquidClass : MonoBehaviour, IGameElementExtension
    {
        private GameObject body;
        private SpriteRenderer spriteRenderer;
        private LabLiquidConf liquidConf;
        private float liquidLevelSource;
        private float liquidLevelTarget;
        private float liquidLevelActual;
        private Color liquidColorSource;
        private Color liquidColorTarget;
        private Color liquidColorActual;
        private ulong startTimestamp;

        private static readonly IReadOnlyDictionary<LabLiquid, Color> colorDict = new Dictionary<LabLiquid, Color>()
        {
            {LabLiquid.LAB_LIQUID_FLOORWASHER, new(0.5f, 0.1f, 0.2f, 1f) },
            {LabLiquid.LAB_LIQUID_DETERGENT, Color.gray },
            {LabLiquid.LAB_LIQUID_INSECTICIDE, Color.green },
            {LabLiquid.LAB_LIQUID_VARNISH, Color.lightYellow },
            {LabLiquid.LAB_LIQUID_RUST, Color.red }
        };

        private static readonly Color colorGray = new (0.5f, 0.5f, 0.5f, 1.0f);

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
            spriteRenderer = body.GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            liquidLevelSource = 0f;
            liquidLevelActual = 0f;
            liquidLevelTarget = 0f;

            liquidColorSource = colorGray;
            liquidColorActual = colorGray;
            liquidColorTarget = colorGray;

            body.transform.localScale = new Vector3(body.transform.localScale.x, 0, 1);
            ItemMasterClass.AddItemExtension(ItemExtensionFunction.ITEM_EXTENSION_FN_FILL_LAB_LIQUID, this);
        }

        private void Update()
        {
            if(liquidLevelActual < liquidLevelTarget)
            {
                ulong actualTimestamp = VARMAP_ItemMaster.GET_ELAPSED_TIME_MS();
                ulong delta = actualTimestamp - startTimestamp;

                float factor = 1.05f + (0f - 1.05f) * Mathf.Exp(-(float)delta/(1500f/4f));
                factor = Mathf.Clamp(factor, 0.0f, 1.0f);

                liquidLevelActual = Mathf.Lerp(liquidLevelSource, liquidLevelTarget, factor);
                liquidColorActual = Color.Lerp(liquidColorSource, liquidColorTarget, factor);

                RefreshLiquid();
            }
        }

        private void FillLiquidFunction(in NumberPack numberPack)
        {
            /* Mix breakdwon */
            liquidConf = new LabLiquidConf(in numberPack);

            liquidLevelSource = liquidLevelActual;
            liquidLevelTarget = liquidConf.nTotal;

            liquidColorSource = liquidColorActual;

            if (liquidConf.nTotal > 0)
            {
                float mixR = (colorDict[LabLiquid.LAB_LIQUID_FLOORWASHER].r * liquidConf.nFloorwasher) + (colorDict[LabLiquid.LAB_LIQUID_DETERGENT].r * liquidConf.nDetergent) +
                    (colorDict[LabLiquid.LAB_LIQUID_INSECTICIDE].r * liquidConf.nInsecticide) + (colorDict[LabLiquid.LAB_LIQUID_VARNISH].r * liquidConf.nVarnish);
                float mixG = (colorDict[LabLiquid.LAB_LIQUID_FLOORWASHER].g * liquidConf.nFloorwasher) + (colorDict[LabLiquid.LAB_LIQUID_DETERGENT].g * liquidConf.nDetergent) +
                    (colorDict[LabLiquid.LAB_LIQUID_INSECTICIDE].g * liquidConf.nInsecticide) + (colorDict[LabLiquid.LAB_LIQUID_VARNISH].g * liquidConf.nVarnish);
                float mixB = (colorDict[LabLiquid.LAB_LIQUID_FLOORWASHER].b * liquidConf.nFloorwasher) + (colorDict[LabLiquid.LAB_LIQUID_DETERGENT].b * liquidConf.nDetergent) +
                    (colorDict[LabLiquid.LAB_LIQUID_INSECTICIDE].b * liquidConf.nInsecticide) + (colorDict[LabLiquid.LAB_LIQUID_VARNISH].b * liquidConf.nVarnish);

                liquidColorTarget = new Color(mixR / liquidConf.nTotal, mixG / liquidConf.nTotal, mixB / liquidConf.nTotal, 1f);
            }
            else
            {
                liquidColorTarget = colorGray;
            }


            /* If immediate */
            if (numberPack.bool1)
            {
                liquidLevelActual = liquidLevelTarget;
                liquidLevelSource = liquidLevelActual;

                liquidColorActual = liquidColorTarget;
                liquidColorSource = liquidColorActual;

                RefreshLiquid();
            }

            startTimestamp = VARMAP_ItemMaster.GET_ELAPSED_TIME_MS();
        }

        private void RefreshLiquid()
        {
            body.transform.localScale = new Vector3(body.transform.localScale.x, liquidLevelActual * 0.54f, 1f);
            spriteRenderer.color = liquidColorActual;
        }

    }
}
