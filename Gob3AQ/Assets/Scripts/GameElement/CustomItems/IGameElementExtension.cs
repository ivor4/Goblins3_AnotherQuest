using Gob3AQ.VARMAP.Types;
using UnityEngine;

namespace Gob3AQ.GameElement.Extension
{

    public interface IGameElementExtension
    {
        public void OnSpawn();
        public void OnDespawn();
        public void OnExtensionDestroy();
        public void OnReceiveExtFn(ItemExtensionFunction extFn);
    }
}
