using UnityEngine;

namespace Gob3AQ.GameElement.Clickable
{
    public delegate void OnHoverAction(bool enter);

    public interface IGameObjectHoverable
    {
        public void OnHover(bool enter);
    }

    public class GameElementClickable : MonoBehaviour, IGameObjectHoverable
    {
        private OnHoverAction _onHoverAction;

        public void OnHover(bool enter)
        {
            _onHoverAction?.Invoke(enter);
        }

        public void SetOnHoverAction(OnHoverAction onHoverAction)
        {
            _onHoverAction = onHoverAction;
        }
    }
}
