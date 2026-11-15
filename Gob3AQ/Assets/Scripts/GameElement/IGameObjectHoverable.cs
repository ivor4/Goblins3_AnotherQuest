using Gob3AQ.VARMAP.Types;
using UnityEngine;

namespace Gob3AQ.GameElement.Clickable
{
    public interface IGameObjectHoverable
    {
        public ref readonly LevelElemInfo GetHoverableLevelElemInfo();
    }

    public class ItemMenuHoverable : IGameObjectHoverable
    {
        private LevelElemInfo hoverInfo;
        
        public void SetHoverInfo(in LevelElemInfo info)
        {
            hoverInfo = info;
        }
        public ref readonly LevelElemInfo GetHoverableLevelElemInfo()
        {
            return ref hoverInfo;
        }
    }
}
