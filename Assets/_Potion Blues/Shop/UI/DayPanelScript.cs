using GenericEventBus;
using Lean.Gui;
using PotionBlues;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PotionBlues.Shop {
    public class DayPanelScript : MonoBehaviour
    {
        [SerializeField] private Slider _dayProgress;
        [SerializeField] private SceneManagerScript _scene;
        [SerializeField] private LeanWindow _window;

        [SerializeField] private TextMeshProUGUI _dayNumber;
        [SerializeField] private TextMeshProUGUI _gold;

        private GenericEventBus<IEvent, IEventNode> _bus;

        // Start is called before the first frame update
        void Start()
        {
            _bus = PotionBlues.I().EventBus;
            _dayProgress.minValue = 0;
            _dayProgress.maxValue = _scene.DayLength;
        }

        private void OnDestroy()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            _dayProgress.value = _scene.DayTimeRemaining;
            _dayNumber.text = $"Day {_scene.PotionBlues.GameData.ActiveRun.Day}";
            _gold.text = $"{_scene.PotionBlues.GameData.ActiveRun.Gold}g";
        }
    }
}
