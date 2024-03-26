using GenericEventBus;
using Lean.Gui;
using PotionBlues.Definitions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

namespace PotionBlues.Shop {
    public class RunReviewPanelScript : MonoBehaviour
    {
        [SerializeField] private SceneManagerScript _scene;
        [SerializeField] private LeanWindow _window;
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _result;
        [SerializeField] private TextMeshProUGUI _reputation;
        [SerializeField] private ShopUpgradeUIPanelScript _unlockables;

        private GenericEventBus<IEvent, IEventNode> _bus;
        private RunData _completedRun;
        private float _availableReputation;
        

        public void Update()
        {
            _reputation.text = $"Reputation: {_availableReputation}";
        }

        public void PrepareBus()
        {
            _bus = PotionBlues.I().EventBus;

            _bus.SubscribeTo<RunEvent>(OnRunEvent);
            _bus.SubscribeTo<UpgradeEvent>(OnUpgradeEvent, 100);
        }

        private void OnDestroy()
        {
            _bus.UnsubscribeFrom<RunEvent>(OnRunEvent);
            _bus.UnsubscribeFrom<UpgradeEvent>(OnUpgradeEvent);
        }

        public void Show(List<UpgradeCardDefinition> unlockables)
        {
            _completedRun = PotionBlues.I().GameData.ActiveRun;
            _availableReputation = _completedRun.Reputation;

            _title.text = $"Run Complete";

            var result = new StringBuilder();
            var customers = _scene.PotionBlues.GameData.ActiveRun.CustomerTransactions;
            result.Append($"Total Customers: {customers.Count()}\n");
            result.Append($" - Satisfied: {customers.Where(c => c.Potion != null).Count()}\n");
            result.Append($" - Unsatisfied: {customers.Where(c => c.Potion == null).Count()}\n");
            result.Append($"Gold: {customers.Select(c => c.Gold).Sum()}g\n");
            result.Append($"Reputation: {customers.Select(c => c.Reputation).Sum()}\n");
            _result.text = result.ToString();

            _window.TurnOn();

            _unlockables.SetCards(unlockables.Select(card => new RunUpgradeCard(card)).ToList());
        }

        void OnRunEvent(ref RunEvent evt)
        { 
            
        }

        void OnUpgradeEvent(ref UpgradeEvent evt)
        {
            var upgrade = evt.Upgrade;
            switch (evt.Type)
            {
                case UpgradeEventType.Unlocked:
                    if (upgrade.ReputationCost > _availableReputation)
                    {
                        _bus.ConsumeCurrentEvent();
                        return;
                    }
                    _availableReputation -= upgrade.ReputationCost;
                    _unlockables.SetCards(_unlockables.GetCards().Where(card => card.Card != upgrade).ToList());
                    break;
            }
        }
    }
}
