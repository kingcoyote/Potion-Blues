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

        [BoxGroup("Door"), SerializeField, Tooltip("Customers per second that will walk through this door")]
        private float _customerFrequency;
        [BoxGroup("Door"), SerializeField, Tooltip("Percent of potion value that customers will pay, default is 1")] 
        private float _customerTipping;
        [BoxGroup("Door"), SerializeField, Tooltip("Number of seconds a customer will wait at the counter before leaving unsatisfied")] 
        private float _customerPatience;
        [BoxGroup("Door"), SerializeField, Tooltip("Modifier to reputation gain/loss when this customer leaves")] 
        private float _reputationBonus;
    }
}
  