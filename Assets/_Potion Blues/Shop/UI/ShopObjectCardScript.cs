using Lean.Gui;
using PotionBlues.Definitions;
using TMPro;
using PotionBlues;
using UnityEngine;

namespace PotionBlues.Shop
{
    public class ShopObjectCardScript : MonoBehaviour
    {
        public RunUpgradeCard UpgradeCard;

        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private LeanTooltipData _tooltip;
        [SerializeField] private TextMeshProUGUI _gold;
        [SerializeField] private TextMeshProUGUI _reputation;
        [SerializeField] private LeanButton _enable;

        public void Start()
        {
            _title.text = UpgradeCard.Card.name;
            _tooltip.Text = UpgradeCard.Card.Description;

            if (_gold != null) _gold.text = $"{UpgradeCard.Card.GoldCost}g";
            if (_reputation != null) _reputation.text = $"{UpgradeCard.Card.ReputationCost} rep";
        }

        public void SetInteractable(bool interactable)
        {
            _enable.interactable = interactable;
        }

        public void Unlock()
        {
            PotionBlues.I().EventBus.Raise(new UpgradeEvent(UpgradeEventType.Unlocked, UpgradeCard.Card));
        }
        public void Buy()
        {
            PotionBlues.I().EventBus.Raise(new UpgradeEvent(UpgradeEventType.Purchased, UpgradeCard.Card));
        }

        public void Sell()
        {
            PotionBlues.I().EventBus.Raise(new UpgradeEvent(UpgradeEventType.Sold, UpgradeCard.Card));
        }

        public void Select()
        {
            Debug.Log($"Selecting upgrade card {UpgradeCard}");
            UpgradeCard.IsSelected = true;
        }

        public void Deselect()
        {
            UpgradeCard.IsSelected = false;
        }

        public void Toggle()
        {
            UpgradeCard.IsSelected = !UpgradeCard.IsSelected;
        }
    }
}