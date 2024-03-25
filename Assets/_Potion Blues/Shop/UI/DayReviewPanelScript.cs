using GenericEventBus;
using Lean.Gui;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

namespace PotionBlues.Shop {
    public class DayReviewPanelScript : MonoBehaviour
    {
        [SerializeField] private SceneManagerScript _scene;
        [SerializeField] private LeanWindow _window;
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _result;
        [SerializeField] private LeanButton _nextDay;
        [SerializeField] private LeanButton _endRun;

        private GenericEventBus<IEvent, IEventNode> _bus;

        // Start is called before the first frame update
        void Start()
        {
            UpdateButtons();
        }

        public void PrepareBus()
        {
            _bus = PotionBlues.I().EventBus;

            _bus.SubscribeTo<RunEvent>(OnRunEvent);
        }

        private void OnDestroy()
        {
            _bus.UnsubscribeFrom<RunEvent>(OnRunEvent);
        }

        // Update is called once per frame
        void Update()
        {
            var dayNumber = _scene.PotionBlues.GameData.ActiveRun.Day - 1;
            _title.text = $"Day {dayNumber} Complete";
            var result = new StringBuilder();
            var customers = _scene.PotionBlues.GameData.ActiveRun.CustomerTransactions.Where(t => t.Day == dayNumber);
            result.Append($"Total Customers: {customers.Count()}\n");
            result.Append($" - Satisfied: {customers.Where(c => c.Potion != null).Count()}\n");
            result.Append($" - Unsatisfied: {customers.Where(c => c.Potion == null).Count()}\n");
            result.Append($"Gold: {customers.Select(c => c.Gold).Sum()}g\n");
            result.Append($"Reputation: {customers.Select(c => c.Reputation).Sum()}\n");
            _result.text = result.ToString();
        }

        void OnRunEvent(ref RunEvent evt)
        { 
            UpdateButtons();
        }

        void UpdateButtons()
        {
            var activeRun = PotionBlues.I().GameData.ActiveRun;
            if (activeRun == null) return;
            var _isRunEnded = activeRun.Day > activeRun.RunDuration;

            _nextDay.gameObject.SetActive(!_isRunEnded);
            _endRun.gameObject.SetActive(_isRunEnded);
        }
    }
}
