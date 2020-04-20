using Modifiers;
using System;
using System.Collections;
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

        private Dictionary<Modifier, StatusIcon> playerIcons;
        private Dictionary<Modifier, StatusIcon> enemyIcons;
        private Dictionary<RollBoundedBattleEffect, Sprite> iconSpriteMap;

        void Start()
        {
            playerIcons = new Dictionary<Modifier, StatusIcon>();
            enemyIcons = new Dictionary<Modifier, StatusIcon>();
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

        private void UpdateStatusIcons(BattleActor actor, Dictionary<Modifier, StatusIcon> icons,
            RectTransform iconParent)
        {
            // Check modifiers active for the actor - include both mods to be applied next roll
            // as well as currently active mods.
            IEnumerable<Modifier> statusMods = actor.Status().NextRollMods.Where(
                m => m.battleEffect != RollBoundedBattleEffect.NONE && m.isRollBounded).Union(
                actor.Status().Mods.GetRollBoundedBattleEffectModifiers());
            foreach (Modifier mod in statusMods)
            {
                if (!icons.ContainsKey(mod))
                {
                    InstantiateStatusIcon(mod, icons, iconParent);
                }
                else
                {
                    // Update sprite in case mod has changed it's effect
                    icons[mod].statusIcon.sprite = iconSpriteMap[mod.battleEffect];
                }
            }

            // Remove any deregistered mods or mods with no effect anymore
            foreach (Modifier mod in new List<Modifier>(icons.Keys))
            {
                if (mod.isDeregistered || mod.battleEffect == RollBoundedBattleEffect.NONE)
                {
                    Destroy(icons[mod].rectTransform.gameObject);
                    icons.Remove(mod);
                }
            }

            UpdateStatusIconsUI(icons);
        }

        private void UpdateStatusIconsUI(Dictionary<Modifier, StatusIcon> icons)
        {
            if (icons.Count == 0)
            {
                return;
            }

            RectTransform sampleTransform = statusIconPrefab.GetComponent<RectTransform>();
            float nextIconCenter = -1 * ((icons.Count - 1) / 2.0f) * 
                (iconMargin + sampleTransform.rect.width);
            
            foreach (Modifier mod in icons.Keys)
            {
                icons[mod].rectTransform.anchoredPosition = Vector2.right * nextIconCenter;
                nextIconCenter += (iconMargin + sampleTransform.rect.width);
            }
        }

        private void InstantiateStatusIcon(Modifier mod, Dictionary<Modifier, StatusIcon> icons,
            RectTransform parent)
        {
            GameObject icon = Instantiate(statusIconPrefab, parent);
            StatusIcon statusIcon = new StatusIcon()
            {
                rectTransform = icon.GetComponent<RectTransform>(),
                statusIcon = icon.GetComponent<Image>(),
            };
            statusIcon.statusIcon.sprite = iconSpriteMap[mod.battleEffect];
            icons.Add(mod, statusIcon);
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
