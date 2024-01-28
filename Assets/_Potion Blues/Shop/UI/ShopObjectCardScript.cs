using Lean.Gui;
using PotionBlues.Definitions;
using TMPro;
using PotionBlues;
using UnityEngine;

namespace PotionBlues.Shop
{
    public class ShopObjectCardScript : MonoBehaviour
    {
        public UpgradeCardDefinition UpgradeCard;

        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private LeanTooltipData _tooltip;
        [SerializeField] private TextMeshProUGUI _gold;
        [SerializeField] private LeanButton _enable;

        public void Start()
        {
            _title.text = UpgradeCard.name;
            _tooltip.Text = UpgradeCard.Description;

            if (_gold != null) _gold.text = $"{UpgradeCard.GoldCost}g";
        }

        public void SetInteractable(bool interactable)
        {
            _enable.interactable = interactable;
        }

        public void Buy()
        {
            PotionBlues.I().EventBus.Raise(new UpgradeEvent(UpgradeEventType.Purchased, UpgradeCard));
        }

        public void Sell()
        {
            PotionBlues.I().EventBus.Raise(new UpgradeEvent(UpgradeEventType.Sold, UpgradeCard));
        }
    }
}