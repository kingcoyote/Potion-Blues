using System;
using GenericEventBus;
using PotionBlues.Definitions;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace PotionBlues.Shop
{
    public class ShopUpgradeUIPanelScript : MonoBehaviour
    {
        [OnValueChanged("Redraw", includeChildren:true)]
        public RunData RunData;
        public ShopObjectCardScript ShopObjectPrefab;
        public ShopUpgradeType Type;
        [ShowIf("@Type == ShopUpgradeType.Object")]
        [ValueDropdown("@ShopObjectCategoryDefinition.GetCategories()")]
        public ShopObjectCategoryDefinition Category;
        [ShowIf("@Type == ShopUpgradeType.Merchant")]
        public int AvailableMerchantCards = 5;

        [SerializeField] private RectTransform _layoutGroup;
        private GenericEventBus<IEvent, IEventNode> _bus;
        private PotionBlues _pb;
        private List<UpgradeCardDefinition> _cards;

        // Start is called before the first frame update
        void Start()
        {
            _pb = PotionBlues.I();
            _bus = _pb.EventBus;
            RunData = _pb.GameData.ActiveRun;

            _bus.SubscribeTo<RunEvent>(OnRunEvent);
            _bus.SubscribeTo<UpgradeEvent>(OnUpgradeEvent);

            _cards = GetUpgradeCards();
            Redraw();
        }

        private void OnDestroy()
        {
            _bus.UnsubscribeFrom<RunEvent>(OnRunEvent);
            _bus.UnsubscribeFrom<UpgradeEvent>(OnUpgradeEvent);
        }

        // Update is called once per frame
        void Update()
        {

        }

        [Button]
        public void Redraw()
        {
            for (int i = _layoutGroup.childCount - 1; i >= 0; i--)
            {
                if (Application.isPlaying) Destroy(_layoutGroup.GetChild(i).gameObject);
                else                       DestroyImmediate(_layoutGroup.GetChild(i).gameObject);
            }

            foreach (var upgradeCard in _cards)
            {
                var upgradeCardObject = Instantiate(ShopObjectPrefab, _layoutGroup);
                upgradeCardObject.name = $"Upgrade [{upgradeCard.name}]";
                upgradeCardObject.UpgradeCard = upgradeCard;
            }
        }

        private List<UpgradeCardDefinition> GetUpgradeCards()
        {
            switch (Type)
            {
                case ShopUpgradeType.Object:
                    return RunData.GetShopUpgrades(Category).ToList<UpgradeCardDefinition>();
                case ShopUpgradeType.Attribute:
                    return RunData.GetShopAttributeUpgrades().ToList<UpgradeCardDefinition>();
                case ShopUpgradeType.Daily:
                    throw new NotImplementedException();
                case ShopUpgradeType.Merchant:
                    return _pb.GetMerchantCards(AvailableMerchantCards).ToList();
            }

            throw new NotImplementedException();
        }

        private void OnRunEvent(ref RunEvent evt)
        {
            switch (evt.Type)
            {
                case RunEventType.DayPreview:
                    _cards = GetUpgradeCards();
                    Redraw();
                    break;
            }
        }

        private void OnUpgradeEvent(ref UpgradeEvent evt)
        {
            switch (evt.Type)
            {
                case UpgradeEventType.Purchased:
                    if (Type == ShopUpgradeType.Merchant)
                    {
                        _cards.Remove(evt.Upgrade);
                    } else
                    {
                        _cards = GetUpgradeCards();
                    }
                    Redraw();
                    break;
            }
        }
    }

    public enum ShopUpgradeType
    {
        Object,
        Attribute,
        Daily,
        Merchant
    }
}