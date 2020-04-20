using Modifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Battle
{
    public class StatusUI : MonoBehaviour
    {
        public GameObject statusIconPrefab;
        public RectTransform playerStatusIconParent;
        public RectTransform enemyStatusIconParent;
        public float iconMargin;
        public StatusIconSprite[] iconSprites;

        private Dictionary<RollBoundedBattleEffect, StatusIcon> playerIcons;
        private Dictionary<RollBoundedBattleEffect, StatusIcon> enemyIcons;
        private Dictionary<RollBoundedBattleEffect, Sprite> iconSpriteMap;

        void Start()
        {
            playerIcons = new Dictionary<RollBoundedBattleEffect, StatusIcon>();
            enemyIcons = new Dictionary<RollBoundedBattleEffect, StatusIcon>();
            iconSpriteMap = new Dictionary<RollBoundedBattleEffect, Sprite>();
            foreach (StatusIconSprite iconSprite in iconSprites)
            {
                iconSpriteMap.Add(iconSprite.effect, iconSprite.sprite);
            }
        }

        public void UpdateStatusIcons()
        {
            UpdateStatusIcons(BattleActor.PLAYER, playerIcons, playerStatusIconParent);
            UpdateStatusIcons(BattleActor.ENEMY, enemyIcons, enemyStatusIconParent);
        }

        private void UpdateStatusIcons(BattleActor actor, 
            Dictionary<RollBoundedBattleEffect, StatusIcon> icons,
            RectTransform iconParent)
        {
            // Check modifiers active for the actor - include both mods to be applied next roll
            // as well as currently active mods.
            IEnumerable<Modifier> statusMods = 
                actor.Status().GetNextRollRollBoundedBattleEffectModifiers().Union(
                actor.Status().GetActiveRollBoundedBattleEffectModifiers());

            List<RollBoundedBattleEffect> foundEffects = new List<RollBoundedBattleEffect>();
            foreach (Modifier mod in statusMods)
            {
                if (mod.battleEffect != RollBoundedBattleEffect.NONE)
                {
                    foundEffects.Add(mod.battleEffect);
                    if (!icons.ContainsKey(mod.battleEffect))
                    {
                        InstantiateStatusIcon(mod.battleEffect, icons, iconParent);
                    }
                }
            }

            foreach (RollBoundedBattleEffect removed in icons.Keys.Except(foundEffects).ToList())
            {
                // Remove any icons that are no longer present
                Destroy(icons[removed].rectTransform.gameObject);
                icons.Remove(removed);
            }

            UpdateStatusIconsUI(icons);
        }

        private void UpdateStatusIconsUI(Dictionary<RollBoundedBattleEffect, StatusIcon> icons)
        {
            if (icons.Count == 0)
            {
                return;
            }

            RectTransform sampleTransform = statusIconPrefab.GetComponent<RectTransform>();
            float nextIconCenter = -1 * ((icons.Count - 1) / 2.0f) * 
                (iconMargin + sampleTransform.rect.width);
            
            foreach (RollBoundedBattleEffect effect in icons.Keys)
            {
                icons[effect].rectTransform.anchoredPosition = Vector2.right * nextIconCenter;
                nextIconCenter += (iconMargin + sampleTransform.rect.width);
            }
        }

        private void InstantiateStatusIcon(RollBoundedBattleEffect effect,
            Dictionary<RollBoundedBattleEffect, StatusIcon> icons,
            RectTransform parent)
        {
            GameObject icon = Instantiate(statusIconPrefab, parent);
            StatusIcon statusIcon = new StatusIcon()
            {
                rectTransform = icon.GetComponent<RectTransform>(),
                statusIcon = icon.GetComponent<Image>(),
            };
            statusIcon.statusIcon.sprite = iconSpriteMap[effect];
            icons.Add(effect, statusIcon);
        }

        private class StatusIcon
        {
            public RectTransform rectTransform;
            public Image statusIcon;
        }
    }

    [Serializable]
    public class StatusIconSprite
    {
        public RollBoundedBattleEffect effect;
        public Sprite sprite;
    }
}
