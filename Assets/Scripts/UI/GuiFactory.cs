using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FCS
{
    public class GuiFactory : Singleton<GuiFactory>
    {
        [SerializeField]
        private RectTransform _guiHolder;

        [SerializeField] private DefaultAttackAbility _defaultAttack;
        [SerializeField] private Explosion _explosion;
        [SerializeField] private FireBall _fireBall;
        [SerializeField] private ForcePush _forcePush;
        [SerializeField] private InvisibilityAbility _invisibility;
        [SerializeField] private LifeDrain _lifeDrain;
        [SerializeField] private Shield _shield;
        [SerializeField] private Swap _swap;
        [SerializeField] private TeleportAbility _teleport;
        [SerializeField] private HookAbility _hook;

        [Serializable]
        public struct AbilitiesToImage
        {
            public Abilities Type;
            public Sprite Sprite;
        }

        [SerializeField] private List<AbilitiesToImage> _spriteStorage;

        private List<Ability> _allAbilities;

        public BattleHud BattleHud;

        protected void Awake()
        {
            _allAbilities = new List<Ability>
            {
                _defaultAttack,
                _explosion,
                _fireBall,
                _forcePush,
                _invisibility,
                _lifeDrain,
                _shield,
                _swap,
                _teleport,
                _hook
            };
        }


        public void Add(RectTransform gui)
        {
            gui.SetParent(_guiHolder, false);
        }

        public Ability Instantiate(Abilities abilityType)
        {
            var ability = _allAbilities.First(element => element.AbilityType == abilityType);
            return Instantiate(ability);
        }

        public Sprite GetSprite(Abilities abilityType)
        {
            return _spriteStorage.First(element => element.Type == abilityType).Sprite;
        }
    }
}