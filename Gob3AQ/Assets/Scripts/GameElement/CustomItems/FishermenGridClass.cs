using Gob3AQ.ItemMaster;
using Gob3AQ.VARMAP.Types;
using UnityEngine;

namespace Gob3AQ.GameElement.Extension.FishermanGrid
{

    public class FishermenGridClass : MonoBehaviour, IGameElementExtension
    {
        private const int X_SIZE = 6;
        private const int Y_SIZE = 18;

        private struct GridElement
        {
            public bool selectable;
            public bool painted;
            public int x;
            public int y;
        }
        private GridElement[,] grid;

        public void OnDespawn()
        {
            
        }

        public void OnExtensionDestroy()
        {
            ItemMasterClass.RemoveItemExtension(ItemExtensionFunction.ITEM_EXTENSION_FN_PAINT_FISHERMEN_GRID);
        }

        public void OnReceiveExtFn(ItemExtensionFunction extFn, in NumberPack numberPack)
        {
            
        }

        public void OnSpawn()
        {
            
        }

        private void Awake()
        {
            grid = new GridElement[X_SIZE, Y_SIZE];
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            ItemMasterClass.AddItemExtension(ItemExtensionFunction.ITEM_EXTENSION_FN_PAINT_FISHERMEN_GRID, this);

            for(int x=0; x < X_SIZE; x++)
            {
                for(int y=0; y < Y_SIZE; y++)
                {
                    bool selectable = (x + y) % 2 == 0;
                    selectable &= !(x <= 1 && y >= 11);
                    selectable &= !(x <= 2 && y >= 16);
                    grid[x, y] = new GridElement { selectable = selectable, painted = false, x = x, y = y };
                }
            }
        }
    }
}
