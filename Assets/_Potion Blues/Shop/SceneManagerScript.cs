using System;
using GenericEventBus;
using Lean.Gui;
using PotionBlues.Definitions;
using PotionBlues.Events;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.VisualScripting;

namespace PotionBlues.Shop {
    public class SceneManagerScript : MonoBehaviour
    {
        [BoxGroup("Run Settings"), MinValue(0), MaxValue(1800)] public float DayLength = 180;
        [BoxGroup("Run Settings"), MinValue(0), MaxValue("DayLength")] public float DayTimeRemaining;

        [BoxGroup("Shop Object Containers"), SerializeField] private ShopObjectContainerScript Doors;
        [BoxGroup("Shop Object Containers"), SerializeField] private ShopObjectContainerScript Counters;
        [BoxGroup("Shop Object Containers"), SerializeField] private ShopObjectContainerScript Cauldrons;
        [BoxGroup("Shop Object Containers"), SerializeField] private ShopObjectContainerScript Ingredients;

        [BoxGroup("UI Panels"), SerializeField] private LeanWindow _topMenu;
        [BoxGroup("UI Panels"), SerializeField] private DayPreviewPanelScript _dayPreviewPanel;
        [BoxGroup("UI Panels"), SerializeField] private LeanWindow _dayPanel;
        [BoxGroup("UI Panels"), SerializeField] private LeanWindow _dayReviewPanel;
        [BoxGroup("UI Panels"), SerializeField] private RunReviewPanelScript _runReviewPanel;

        public PotionBlues PotionBlues => _pb;

        private PotionBlues _pb;
        private GenericEventBus<IEvent, IEventNode> _bus;

        // Start is called before the first frame update
        void Start()
        {
            _pb = PotionBlues.I();

            _bus = _pb.EventBus;
            _bus.SubscribeTo<DoorEvent>(OnDoorEvent, 100);
            _bus.SubscribeTo<CounterEvent>(OnCounterEvent, 100);
            _bus.SubscribeTo<CauldronEvent>(OnCauldronEvent, 100);
            _bus.SubscribeTo<IngredientEvent>(OnIngredientEvent, 100);
            _bus.SubscribeTo<RunEvent>(OnRunEvent);
            _bus.SubscribeTo<UpgradeEvent>(OnUpgradeEvent, 100);

            _dayPreviewPanel.GetComponent<DayPreviewPanelScript>().PrepareBus();
            _dayPanel.GetComponent<DayPanelScript>().PrepareBus();
            _dayReviewPanel.GetComponent<DayReviewPanelScript>().PrepareBus();
            _runReviewPanel.GetComponent<RunReviewPanelScript>().PrepareBus();

            ClearShopObjects();

            Time.timeScale = 0;
        }

        private void OnDestroy()
        {
            _bus.UnsubscribeFrom<DoorEvent>(OnDoorEvent);
            _bus.UnsubscribeFrom<CounterEvent>(OnCounterEvent);
            _bus.UnsubscribeFrom<CauldronEvent>(OnCauldronEvent);
            _bus.UnsubscribeFrom<IngredientEvent>(OnIngredientEvent);
            _bus.UnsubscribeFrom<RunEvent>(OnRunEvent);
            _bus.UnsubscribeFrom<UpgradeEvent>(OnUpgradeEvent);
        }

        // Update is called once per frame
        void Update()
        {
            DayTimeRemaining -= Time.deltaTime;
            if (DayTimeRemaining < 10)
            {
                _bus.Raise(new RunEvent(RunEventType.ShopClosed));
            }
            if (DayTimeRemaining < 0 && Time.timeScale > 0)
            {
                _bus.Raise(new RunEvent(RunEventType.DayEnded));
                Time.timeScale = 0;
            }
        }

        public void Play()
        {
            if (_pb.GameData.ActiveRun != null && _pb.GameData.ActiveRun.Day > 0 && _pb.GameData.ActiveRun.Day <= _pb.GameData.ActiveRun.RunDuration)
            {
                Debug.Log("Resuming existing run");
                _pb.EventBus.Raise(new RunEvent(RunEventType.DayPreview));
            }
            else
            {
                Debug.Log("Starting new run");
                _pb.StartNewRun();
                _pb.EventBus.Raise(new RunEvent(RunEventType.Created));
            }
        }

        public void StartDay()
        {
            InstantiateShopObjects();
            Time.timeScale = 1;
            DayTimeRemaining = DayLength;
            _bus.Raise(new RunEvent(RunEventType.DayStarted));
        }

        public void EndDay()
        {
            _bus.Raise(new RunEvent(RunEventType.DayPreview));
        }

        public void ReviewRun()
        {
            _bus.Raise(new RunEvent(RunEventType.RunReview));
        }

        public void EndRun()
        {
            _bus.Raise(new RunEvent(RunEventType.Ended));
        }

        [HorizontalGroup("Shop Object Containers/Objects"), Button("Instantiate")]
        public void InstantiateShopObjects()
        {
            var categories = PotionBlues.I().ShopObjectCategories;

            var activeRun = PotionBlues.I().GameData.ActiveRun;

            Doors.ResetShopObjects(activeRun.GetShopObjects(categories["Door"]));
            Counters.ResetShopObjects(activeRun.GetShopObjects(categories["Counter"]));
            Cauldrons.ResetShopObjects(activeRun.GetShopObjects(categories["Cauldron"]));
            Ingredients.ResetShopObjects(activeRun.GetShopObjects(categories["Ingredient"]));
        }

        [HorizontalGroup("Shop Object Containers/Objects"), Button("Clear")]
        public void ClearShopObjects()
        {
            Doors.ClearShopObjects();
            Counters.ClearShopObjects();
            Cauldrons.ClearShopObjects();
            Ingredients.ClearShopObjects();
        }

        public void OnDoorEvent(ref DoorEvent evt, IEventNode target, IEventNode source)
        {
            evt.Attributes = AdjustAttributes(evt.Attributes);

            switch (evt.Type)
            {
                case DoorEventType.CustomerLeave:
                    if (evt.Potion != null)
                    {
                        PotionBlues.I().GameData.ActiveRun.Gold += (int)(evt.Potion.Attributes.TryGet("Potion Value"));
                    }
                    break;
            }
        }

        public void OnCounterEvent(ref CounterEvent evt, IEventNode target, IEventNode source)
        {
            evt.Attributes = AdjustAttributes(evt.Attributes);
        }

        public void OnCauldronEvent(ref CauldronEvent evt, IEventNode target, IEventNode source)
        {
            evt.Attributes = AdjustAttributes(evt.Attributes);
        }

        public void OnIngredientEvent(ref IngredientEvent evt, IEventNode target, IEventNode source)
        {
            evt.Attributes = AdjustAttributes(evt.Attributes);
        }

        private List<ShopAttributeValue> AdjustAttributes(List<ShopAttributeValue> attributes)
        {
            var upgradeModifiers = GetAttributeModifiers();

            return attributes.Stack(upgradeModifiers);
        }

        /**
         * Process the current run's shop attribute upgrades to return a flattened List<ShopAttributeValue>
         */
        private List<ShopAttributeValue> GetAttributeModifiers()
        {
            return PotionBlues.I().GameData.ActiveRun
                .GetShopAttributeUpgrades()
                .SelectMany(card => ((ShopAttributeUpgradeCardDefintion)card.Card).AttributeValues)
                .GroupBy(av => av.Attribute)
                .Select(group => new ShopAttributeValue(
                    group.Key, 
                    group.Select(av => av.Value).Aggregate((a, b) => group.Key.Stack(a, b))
                ))
                .ToList();
        }

        private void OnRunEvent(ref RunEvent evt)
        {
            switch (evt.Type)
            {
                case RunEventType.Created:
                case RunEventType.DayPreview:
                    Time.timeScale = 0;
                    _pb.GameData.ActiveRun.MerchantCards = _pb.GameData.Upgrades
                       .Except(_pb.GameData.ActiveRun.Upgrades.Select(card => card.Card))
                       .OrderBy(x => Guid.NewGuid())
                       .Take(5)
                       .Select(card => new RunUpgradeCard(card))
                       .ToList();
                    _dayPreviewPanel.Show();
                    break;
                case RunEventType.DayStarted:
                    _dayPanel.TurnOn();
                    break;
                case RunEventType.DayEnded:
                    Time.timeScale = 0;
                    _dayReviewPanel.TurnOn();
                    _pb.GameData.ActiveRun.Day += 1;
                    _pb.Save();
                    break;
                case RunEventType.RunReview:
                    Time.timeScale = 0;
                    var unlockables = _pb.Upgrades.Except(_pb.GameData.Upgrades)
                       .OrderBy(x => Guid.NewGuid())
                       .Take(5)
                       .ToList();
                    _runReviewPanel.Show(unlockables);
                    break;
                case RunEventType.Ended:
                    Time.timeScale = 0;
                    _topMenu.TurnOn();
                    ClearShopObjects();
                    _pb.GameData.RunHistory.Add(_pb.GameData.ActiveRun);
                    _pb.GameData.ActiveRun = null;
                    break;
            }
        }

        private void OnUpgradeEvent(ref UpgradeEvent evt)
        {
            var upgrade = evt.Upgrade;
            switch (evt.Type)
            {
                case UpgradeEventType.Purchased:
                    if (upgrade.GoldCost > _pb.GameData.ActiveRun.Gold)
                    {
                        _bus.ConsumeCurrentEvent();
                        return;
                    }
                    _pb.GameData.ActiveRun.Gold -= upgrade.GoldCost;
                    var upgradeCard = new RunUpgradeCard(upgrade);
                    _pb.GameData.ActiveRun.Upgrades.Add(upgradeCard);
                    _pb.GameData.ActiveRun.MerchantCards = _pb.GameData.ActiveRun.MerchantCards.Where(card => card.Card != upgrade).ToList();
                    break;
                case UpgradeEventType.Unlocked:
                    if (upgrade.ReputationCost > _pb.GameData.ActiveRun.Reputation)
                    {
                        _bus.ConsumeCurrentEvent();
                        return;
                    }
                    _pb.GameData.ActiveRun.Reputation -= upgrade.ReputationCost;
                    _pb.GameData.Upgrades.Add(upgrade);
                    break;
            }
        }
    }
}
