using UnityEngine;

namespace PotionBlues.Definitions
{
    [CreateAssetMenu(menuName = "Potion Blues/Shop Object Category")]
    public class ShopObjectCategoryDefinition : ScriptableObject
    {
        public string Description => _description;
        public int Max => _max;

        [SerializeField, TextArea(3,10)] private string _description;
        [SerializeField] private int _max;
    }
}