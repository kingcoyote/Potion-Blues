using Lean.Gui;
using PotionBlues.Definitions;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace PotionBlues.Shop
{
    public class ShopUpgradeUIPanelScript : MonoBehaviour
    {
        public int MaxSelectable = 1;
        public ShopObjectCardScript ShopObjectPrefab;

        [SerializeField] private RectTransform _layoutGroup;
        private List<RunUpgradeCard> _cards;

        public List<RunUpgradeCard> GetCards()
        {
            return _cards;
        }
        public void SetCards(List<RunUpgradeCard> cards)
        {
            _cards = cards;
            Redraw();
        }

        [Button]
        private void Redraw()
        {
            var existingCards = _layoutGroup.GetComponentsInChildren<ShopObjectCardScript>()
                .Select(card => card.UpgradeCard);
            var newCards = _cards.Except(existingCards).ToList();
            var removedCards = existingCards.Except(_cards).ToList();
            var removedCardObjects = _layoutGroup.GetComponentsInChildren<ShopObjectCardScript>()
                .Where(card => removedCards.Contains(card.UpgradeCard));

            foreach (var card in removedCardObjects)
            {
                if (Application.isPlaying) Destroy(card.gameObject);
                else                       DestroyImmediate(card.gameObject);
            }

            foreach (var upgradeCard in newCards)
            {
                var upgradeCardObject = Instantiate(ShopObjectPrefab, _layoutGroup);
                upgradeCardObject.name = $"Upgrade [{upgradeCard.Card.name}]";
                upgradeCardObject.UpgradeCard = upgradeCard;
                upgradeCardObject.transform.SetAsFirstSibling();

                var toggle = upgradeCardObject.GetComponent<LeanToggle>();

                if (toggle != null)
                {
                    toggle.Set(upgradeCard.IsSelected);
                    toggle.OnOn.AddListener(UpdateToggles);
                    toggle.OnOff.AddListener(UpdateToggles);
                }
            }

            UpdateToggles();
        }

        public void UpdateToggles()
        {
            var toggleChildren = GetComponentsInChildren<LeanToggle>()
                .Where(x => x.gameObject.IsDestroyed() == false);

            var onToggles = toggleChildren.Where(x => x.On);
            var offToggles = toggleChildren.Where(x => !x.On);

            var canSelectMore = onToggles.Count() < MaxSelectable;

            foreach (var offToggle in offToggles)
            {
                offToggle.GetComponent<ShopObjectCardScript>().SetInteractable(canSelectMore);
            }
        }
    }
}