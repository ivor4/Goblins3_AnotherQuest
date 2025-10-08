using UnityEngine;

namespace Gob3AQ.GameElement.Clickable
{
    public delegate void OnClickAction(bool enter);

    public class GameElementClickable : MonoBehaviour
    {
        private OnClickAction _onClickAction;

        public void SetOnClickAction(OnClickAction onClickAction)
        {
            _onClickAction = onClickAction;
        }

        private void OnMouseEnter()
        {
            _onClickAction?.Invoke(true);
        }

        private void OnMouseExit()
        {
            _onClickAction?.Invoke(false);
        }
    }
}
