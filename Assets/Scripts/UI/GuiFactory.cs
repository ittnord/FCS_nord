using UnityEngine;

namespace FCS
{
    public class GuiFactory : Singleton<GuiFactory>
    {
        [SerializeField]
        private RectTransform _guiHolder;


        public BattleHud BattleHud;


        public void Add(RectTransform gui)
        {
            gui.SetParent(_guiHolder, false);
        }
    }
}