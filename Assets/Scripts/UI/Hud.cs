using UnityEngine;

namespace FCS
{
    public class Hud : Singleton<Hud>
    {
        [SerializeField]
        private Canvas _canvas;
        [SerializeField]
        private RectTransform _guiHolder;

        public RectTransform GuiHolder { get { return _guiHolder; } }
    }
}