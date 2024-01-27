using System;
using System.Linq;
using System.Collections.Generic;
using GenericEventBus;
using UnityEngine;
using TMPro;
using Lean.Gui;
using Sirenix.OdinInspector;
using PotionBlues.Definitions;

namespace PotionBlues.Shop {
    public class DayPreviewPanelScript : MonoBehaviour
    {
        [BoxGroup("Upgrades")] private List<UpgradeCardDefinition> _upgradeCards;
        [BoxGroup("Upgrades")] private List<UpgradeCardDefinition> _merchantCards;

        [SerializeField, BoxGroup("Day Preview Panel")] private SceneManagerScript _scene;
        [SerializeField, BoxGroup("Day Preview Panel")] public LeanWindow Window;

        [SerializeField, BoxGroup("Run Status")] private TextMeshProUGUI _dayNumber;
        [SerializeField, BoxGroup("Run Status")] private TextMeshProUGUI _gold;
        [SerializeField, BoxGroup("Run Status")] private TextMeshProUGUI _reputation;

        [SerializeField, BoxGroup("Upgrade Panels")] private ShopUpgradeUIPanelScript _door;
        [SerializeField, BoxGroup("Upgrade Panels")] private ShopUpgradeUIPanelScript _counter;
        [SerializeField, BoxGroup("Upgrade Panels")] private ShopUpgradeUIPanelScript _cauldrons;
        [SerializeField, BoxGroup("Upgrade Panels")] private ShopUpgradeUIPanelScript _ingredients;
        [SerializeField, BoxGroup("Upgrade Panels")] private ShopUpgradeUIPanelScript _staff;
        [SerializeField, BoxGroup("Upgrade Panels")] private ShopUpgradeUIPanelScript _upgrades;
        [SerializeField, BoxGroup("Upgrade Panels")] private ShopUpgradeUIPanelScript _merchant;
        [SerializeField, BoxGroup("Upgrade Panels")] private ShopUpgradeUIPanelScript _daily;

        private GenericEventBus<IEvent, IEventNode> _bus;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        public void Show(List<UpgradeCardDefinition> upgrades, List<UpgradeCardDefinition> merchant)
        {
            _upgradeCards = upgrades;
            _merchantCards = merchant;
            RedrawUpgradePanels();
            Window.TurnOn();
        }

        public List<UpgradeCardDefinition> GetUpgrades()
        {
            return _upgradeCards;
        }

        public void BuyCard(UpgradeCardDefinition card)
        {
            _merchantCards.Remove(card);
        }

        public void PrepareBus()
        {
            _bus = PotionBlues.I().EventBus;
            _bus.SubscribeTo<UpgradeEvent>(OnUpgradeEvent);
        }

        private void OnDestroy()
        {
            _bus.UnsubscribeFrom<UpgradeEvent>(OnUpgradeEvent);
        }

        // Update is called once per frame
        void Update()
        {
            _dayNumber.text = $"Day: {_scene.PotionBlues.GameData.ActiveRun.Day}";
            _gold.text = $"Gold: {_scene.PotionBlues.GameData.ActiveRun.Gold}";
            _reputation.text = $"Rep: {_scene.PotionBlues.GameData.ActiveRun.Reputation}";
        }

        void RedrawUpgradePanels()
        {
            var categories = new List<string>() { "Door", "Counter", "Cauldron", "Ingredient" }
                .Select(x => ShopObjectCategoryDefinition.Load(x))
                .ToDictionary(x => x.name);

            _door.SetCards(GetShopUpgrades(_upgradeCards, categories["Door"]).ToList<UpgradeCardDefinition>());
            _counter.SetCards(GetShopUpgrades(_upgradeCards, categories["Counter"]).ToList<UpgradeCardDefinition>());
            _cauldrons.SetCards(GetShopUpgrades(_upgradeCards, categories["Cauldron"]).ToList<UpgradeCardDefinition>());
            _ingredients.SetCards(GetShopUpgrades(_upgradeCards, categories["Ingredient"]).ToList<UpgradeCardDefinition>());

            _door.GetComponent<LimitedSelectionScript>().SetMaximumSelection(categories["Door"].Max);
            _counter.GetComponent<LimitedSelectionScript>().SetMaximumSelection(categories["Counter"].Max);
            _cauldrons.GetComponent<LimitedSelectionScript>().SetMaximumSelection(categories["Cauldron"].Max);
            _ingredients.GetComponent<LimitedSelectionScript>().SetMaximumSelection(categories["Ingredient"].Max);

            _upgrades.SetCards(GetShopAttributeUpgrades(_upgradeCards).ToList<UpgradeCardDefinition>());
            _merchant.SetCards(_merchantCards);
        }

        void OnUpgradeEvent(ref UpgradeEvent evt)
        {
            switch (evt.Type)
            {
                case UpgradeEventType.Purchased:
                    BuyCard(evt.Upgrade);
                    break;
            }

            RedrawUpgradePanels();
        }

        public List<ShopAttributeUpgradeCardDefintion> GetShopAttributeUpgrades(List<UpgradeCardDefinition> upgrades)
        {
            return GetCardsOfType<ShopAttributeUpgradeCardDefintion>(upgrades)
                .ToList();
        }

        private List<ShopObjectDefinition> GetShopObjects(List<UpgradeCardDefinition> upgrades, ShopObjectCategoryDefinition category)
        {
            return GetShopUpgrades(upgrades, category)
                .Select(card => card.ShopObject)
                .ToList();
        }

        private List<ShopObjectUpgradeCardDefinition> GetShopUpgrades(List<UpgradeCardDefinition> upgrades, ShopObjectCategoryDefinition category)
        {
            return GetCardsOfType<ShopObjectUpgradeCardDefinition>(upgrades)
                .Where(card => card.ShopObject.Category == category)
                .ToList();
        }

        private IEnumerable<TValue> GetCardsOfType<TValue>(List<UpgradeCardDefinition> upgrades) where TValue : UpgradeCardDefinition
        {
            return upgrades
                .Where(card => card.GetType() == typeof(TValue))
                .Select(card => (TValue)card);
        }
    }
}
