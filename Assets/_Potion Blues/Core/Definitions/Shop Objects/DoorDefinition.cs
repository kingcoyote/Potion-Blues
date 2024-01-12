using UnityEngine;
using Sirenix.OdinInspector;

namespace PotionBlues.Definitions
{
    [CreateAssetMenu(menuName = "Potion Blues/Objects/Door")]
    public class DoorDefinition : ShopObjectDefinition
    {
        public float CustomerFrequency => _customerFrequency;
        public float CustomerTipping => _customerTipping;
        public float CustomerPatience => _customerPatience;
        public float ReputationBonus => _reputationBonus;

        [BoxGroup("Door"), SerializeField] private float _customerFrequency;
        [BoxGroup("Door"), SerializeField] private float _customerTipping;
        [BoxGroup("Door"), SerializeField] private float _customerPatience;
        [BoxGroup("Door"), SerializeField] private float _reputationBonus;
    }
}
  