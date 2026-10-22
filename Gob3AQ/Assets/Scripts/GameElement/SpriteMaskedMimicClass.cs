using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.ItemMaster;
using UnityEngine;
using System.Collections.Generic;

namespace Gob3AQ.GameElement.SpriteMaskedMimic
{
    [System.Serializable]
    public struct SpriteMaskedMimicStruct
    {
        public Sprite srcSprite;
        public Sprite dstSprite;
    }

    [System.Serializable]
    public class SpriteMaskedMimicClass : MonoBehaviour
    {
        [SerializeField]
        private GameItem mimicTargetItem;

        [SerializeField]
        private SpriteMaskedMimicStruct[] spriteMappings;

        [SerializeField]
        private int sortingOrderOffset;

        private IReadOnlyDictionary<GameItem, GameElementClass> _levelItems;
        private GameObject mimicTarget;
        private SpriteRenderer mySpriteRenderer;
        private SpriteRenderer targetSpriteRenderer;
        private string previousSpriteName;
        private bool ready;

        private void Awake()
        {
            mySpriteRenderer = GetComponent<SpriteRenderer>();
            ready = false;
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            VARMAP_ItemMaster.REG_GAMESTATUS(_GameStatusChanged);
        }

        // Update is called once per frame
        private void Update()
        {
            if (ready)
            {
                transform.position = mimicTarget.transform.position;
                transform.localScale = mimicTarget.transform.localScale;
                transform.localEulerAngles = mimicTarget.transform.localEulerAngles;

                if(targetSpriteRenderer.sprite == null)
                {
                    mySpriteRenderer.sprite = null;
                    previousSpriteName = string.Empty;
                    return;
                }

                mySpriteRenderer.sortingLayerID = targetSpriteRenderer.sortingLayerID;
                mySpriteRenderer.sortingOrder = targetSpriteRenderer.sortingOrder + sortingOrderOffset;


                if (targetSpriteRenderer.sprite.name != previousSpriteName)
                {
                    bool found = false;

                    foreach (var mapping in spriteMappings)
                    {
                        if (targetSpriteRenderer.sprite.name == mapping.srcSprite.name)
                        {
                            mySpriteRenderer.sprite = mapping.dstSprite;
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        mySpriteRenderer.sprite = null;
                    }

                    previousSpriteName = targetSpriteRenderer.sprite.name;
                }
            }
        }

        private void OnDestroy()
        {
            VARMAP_ItemMaster.UNREG_GAMESTATUS(_GameStatusChanged);
        }

        private void AssignTarget()
        {
            VARMAP_ItemMaster.OBTAIN_SCENARIO_ITEMS(out _levelItems);

            if (_levelItems.TryGetValue(mimicTargetItem, out GameElementClass targetInstance))
            {
                mimicTarget = targetInstance.transform.parent.gameObject;
                targetSpriteRenderer = mimicTarget.GetComponent<SpriteRenderer>();

                ready = true;
            }
            else
            {
                Debug.LogError($"SpriteMaskedMimicClass: Target item {mimicTargetItem} not found in level items.");
            }
        }

        private void _GameStatusChanged(ChangedEventType evtype, in Game_Status oldval, in Game_Status newval)
        {
            _ = evtype;

            if (newval != oldval)
            {
                switch (newval)
                {
                    case Game_Status.GAME_STATUS_LOADING:
                        AssignTarget();
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
