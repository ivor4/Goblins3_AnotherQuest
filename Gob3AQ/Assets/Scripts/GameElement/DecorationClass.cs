using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.ItemMaster;
using UnityEngine;
using System.Collections;
using Gob3AQ.ResourceAtlas;
using Gob3AQ.ResourceSprites;

[System.Serializable]
public class DecorationClass : MonoBehaviour
{
    [SerializeField]
    private GameSprite sprite;

    private SpriteRenderer mySpriteRenderer;

    private void Awake()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        StartCoroutine(Load_Coroutine());
    }

    private IEnumerator Load_Coroutine()
    {
        bool sprites_loaded = false;

        while(!sprites_loaded)
        {
            VARMAP_ItemMaster.IS_MODULE_LOADED(GameModules.MODULE_GameMaster, out sprites_loaded);
            yield return ResourceAtlasClass.WaitForNextFrame;
        }

        mySpriteRenderer.sprite = ResourceSpritesClass.GetSprite(sprite);
        UpdateSortingOrder();
    }

    private void UpdateSortingOrder()
    {
        /* Set sorting order based on its actual Y */
        mySpriteRenderer.sortingOrder = -(int)(mySpriteRenderer.bounds.min.y * 1000);
    }
}
