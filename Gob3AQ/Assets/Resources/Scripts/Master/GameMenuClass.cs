using UnityEngine;
using Gob3AQ.VARMAP.GameMenu;

namespace Gob3AQ.GameMenu
{
    public class GameMenuClass : MonoBehaviour
    {
        private static GameMenuClass _singleton;
        private static GameObject _itemMenu;
        private static SpriteRenderer _itemMenuspr;
        private static bool _prevItemMenuOpened;

        void Awake()
        {
            if(_singleton)
            {
                Destroy(gameObject);
            }
            else
            {
                _singleton = this;

                Transform child = transform.Find("ItemMenu");
                _itemMenu = child.gameObject;
                _itemMenuspr = _itemMenu.GetComponent<SpriteRenderer>();
            }
        }

        void Start()
        {
            _prevItemMenuOpened = false;
        }

        void Update()
        {
            bool itemMenuOpened = VARMAP_GameMenu.GET_ITEM_MENU_ACTIVE();

            if(itemMenuOpened && !_prevItemMenuOpened)
            {
                _itemMenuspr.enabled = true;
            }
            else if(!itemMenuOpened && _prevItemMenuOpened)
            {
                _itemMenuspr.enabled = false;
            }


            _prevItemMenuOpened = itemMenuOpened;
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