using GenericEventBus;
using Lean.Gui;
using PotionBlues;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace PotionBlues.Shop {
    public class DayPreviewPanelScript : MonoBehaviour
    {
        [SerializeField] private SceneManagerScript _scene;
        [SerializeField] private LeanWindow _window;
        [SerializeField] private TextMeshProUGUI _dayNumber;
        [SerializeField] private TextMeshProUGUI _gold;
        [SerializeField] private TextMeshProUGUI _reputation;

        private GenericEventBus<IEvent, IEventNode> _bus;

        // Start is called before the first frame update
        void Start()
        {
            _bus = PotionBlues.I().EventBus;
        }

        private void OnDestroy()
        {
           
        }

        // Update is called once per frame
        void Update()
        {
            _dayNumber.text = $"Day: {_scene.PotionBlues.GameData.ActiveRun.Day}";
            _gold.text = $"Gold: {_scene.PotionBlues.GameData.ActiveRun.Gold}";
            _reputation.text = $"Rep: {_scene.PotionBlues.GameData.ActiveRun.Reputation}";
        }
    }
}
