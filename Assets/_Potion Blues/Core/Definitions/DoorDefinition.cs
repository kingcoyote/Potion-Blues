using UnityEngine;
using Sirenix.OdinInspector;

namespace PotionBlues.Definitions
{
    public class DoorDefinition : ShopObjectDefinition
    {
        public float CustomerRate => _customerRate;

        [BoxGroup("Door"), SerializeField] private float _customerRate;
    }
}