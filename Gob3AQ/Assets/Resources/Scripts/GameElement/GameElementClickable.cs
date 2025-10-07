using UnityEngine;

namespace Gob3AQ.GameElement.Clickable
{
    public delegate void OnClickAction();

    public class GameElementClickable : MonoBehaviour
    {
        private OnClickAction _onClickAction;

        public void SetOnClickAction(OnClickAction onClickAction)
        {
            _onClickAction = onClickAction;
        }

        private void OnMouseUp()
        {
            _onClickAction?.Invoke();
        }
    }
}
