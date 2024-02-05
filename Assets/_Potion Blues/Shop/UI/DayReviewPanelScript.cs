using GenericEventBus;
using Lean.Gui;
using PotionBlues;
using System.Collections;
using System.Collections.Generic;
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
            _title.text = $"Day {_scene.PotionBlues.GameData.ActiveRun.Day - 1} Complete";
            _result.text = "Did stuff.\nClick Next Day";
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
