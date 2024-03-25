using GenericEventBus;
using Lean.Gui;
using PotionBlues.Definitions;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace PotionBlues.Shop {
    public class RunReviewPanelScript : MonoBehaviour
    {
        [SerializeField] private SceneManagerScript _scene;
        [SerializeField] private LeanWindow _window;
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _result;
        [SerializeField] private ShopUpgradeUIPanelScript _unlockables;

        private GenericEventBus<IEvent, IEventNode> _bus;

        public void PrepareBus()
        {
            _bus = PotionBlues.I().EventBus;

            _bus.SubscribeTo<RunEvent>(OnRunEvent);
            _bus.SubscribeTo<UpgradeEvent>(OnUpgradeEvent);
        }

        private void OnDestroy()
        {
            _bus.UnsubscribeFrom<RunEvent>(OnRunEvent);
            _bus.UnsubscribeFrom<UpgradeEvent>(OnUpgradeEvent);
        }

        public void Show(List<UpgradeCardDefinition> unlockables)
        {
            _title.text = $"Run Complete";
            _result.text = "Did stuff.\nClick Next Day";

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
                    _unlockables.SetCards(_unlockables.GetCards().Where(card => card.Card != upgrade).ToList());
                    break;
            }
        }
    }
}
