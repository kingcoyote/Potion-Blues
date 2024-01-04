using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;
using System.Collections;

namespace PotionBlues.Definitions
{
    [CreateAssetMenu(menuName = "Potion Blues/Rarity")]
    public class RarityDefinition : ScriptableObject
    {
        public Color Color => _color;
        public float Weight => _weight;

        [SerializeField] private Color _color;
        [SerializeField] private float _weight;


#if UNITY_EDITOR
        private static IEnumerable GetRarity()
        {
            return UnityEditor.AssetDatabase.FindAssets("t:RarityDefinition")
                .Select(x => UnityEditor.AssetDatabase.GUIDToAssetPath(x))
                .Select(x => UnityEditor.AssetDatabase.LoadAssetAtPath<RarityDefinition>(x))
                .OrderByDescending(x => x.Weight)
                .Select(x => new ValueDropdownItem($"{x.name}", x));
        }
#endif
    }
}