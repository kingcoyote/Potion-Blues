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

        public void Show()
        {
            RedrawUpgradePanels();
            Window.TurnOn();
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
            _dayNumber.text = $"Day: {_scene.PotionBlues.GameData.ActiveRun.Day} / {_scene.PotionBlues.GameData.ActiveRun.RunDuration}";
            _gold.text = $"Gold: {_scene.PotionBlues.GameData.ActiveRun.Gold}";
            _reputation.text = $"Rep: {_scene.PotionBlues.GameData.ActiveRun.Reputation}";
        }

        void RedrawUpgradePanels()
        {
            var categories = new List<string>() { "Door", "Counter", "Cauldron", "Ingredient" }
                .Select(x => ShopObjectCategoryDefinition.Load(x))
                .ToDictionary(x => x.name);

            _door.SetCards(GetShopUpgrades(categories["Door"]));
            _counter.SetCards(GetShopUpgrades(categories["Counter"]));
            _cauldrons.SetCards(GetShopUpgrades(categories["Cauldron"]));
            _ingredients.SetCards(GetShopUpgrades(categories["Ingredient"]));

            _door.MaxSelectable = categories["Door"].Max;
            _counter.MaxSelectable = categories["Counter"].Max;
            _cauldrons.MaxSelectable = categories["Cauldron"].Max;
            _ingredients.MaxSelectable = categories["Ingredient"].Max;

            _upgrades.SetCards(GetShopAttributeUpgrades());
            _merchant.SetCards(PotionBlues.I().GameData.ActiveRun.MerchantCards);
        }

        void OnUpgradeEvent(ref UpgradeEvent evt)
        {
            RedrawUpgradePanels();
        }

        public List<RunUpgradeCard> GetShopAttributeUpgrades()
        {
            return GetCardsOfType<ShopAttributeUpgradeCardDefintion>()
                .ToList();
        }

        private List<ShopObjectDefinition> GetShopObjects(ShopObjectCategoryDefinition category)
        {
            return GetShopUpgrades(category)
                .Select(card => ((ShopObjectUpgradeCardDefinition)card.Card).ShopObject)
                .ToList();
        }

        private List<RunUpgradeCard> GetShopUpgrades(ShopObjectCategoryDefinition category)
        {
            return GetCardsOfType<ShopObjectUpgradeCardDefinition>()
                .Where(card => ((ShopObjectUpgradeCardDefinition)card.Card).ShopObject.Category == category)
                .ToList();
        }

        private IEnumerable<RunUpgradeCard> GetCardsOfType<TValue>() where TValue : UpgradeCardDefinition
        {
            return PotionBlues.I().GameData.ActiveRun.Upgrades
                .Where(card => card.Card.GetType() == typeof(TValue)); ;
        }
    }
}
