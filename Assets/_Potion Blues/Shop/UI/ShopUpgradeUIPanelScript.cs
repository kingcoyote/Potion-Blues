using PotionBlues.Definitions;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PotionBlues.Shop
{
    public class ShopUpgradeUIPanelScript : MonoBehaviour
    {
        public int MaxSelectable = 1;
        public ShopObjectCardScript ShopObjectPrefab;

        [SerializeField] private RectTransform _layoutGroup;
        private List<RunUpgradeCard> _cards;

        public void SetCards(List<RunUpgradeCard> cards)
        {
            _cards = cards;
            Redraw();
        }

        [Button]
        private void Redraw()
        {
            for (int i = _layoutGroup.childCount - 1; i >= 0; i--)
            {
                if (Application.isPlaying) Destroy(_layoutGroup.GetChild(i).gameObject);
                else                       DestroyImmediate(_layoutGroup.GetChild(i).gameObject);
            }

            foreach (var upgradeCard in _cards)
            {
                var upgradeCardObject = Instantiate(ShopObjectPrefab, _layoutGroup);
                upgradeCardObject.name = $"Upgrade [{upgradeCard.Card.name}]";
                upgradeCardObject.UpgradeCard = upgradeCard.Card;
                upgradeCardObject.transform.SetAsFirstSibling();
            }
        }
    }
}