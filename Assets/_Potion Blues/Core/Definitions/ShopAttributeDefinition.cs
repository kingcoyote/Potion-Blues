using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System;
using System.Linq;

namespace PotionBlues.Definitions
{
    [CreateAssetMenu(menuName = "Potion Blues/Shop Attribute")]
    public class ShopAttributeDefinition : ScriptableObject
    {
        public string Description => _description;
        public Sprite Icon => _icon;
        public StackType StackingType => _stackingType;
        public float Identity { get
            {
                switch (_stackingType)
                {
                    case StackType.Multiply: 
                        return 1;
                    case StackType.Add:
                        return 0;
                    case StackType.Overwrite:
                        return 0;
                    case StackType.Ignore: 
                        return 0;
                }

                return 0;
            } 
        }

        [SerializeField, TextArea(3, 10)] private string _description;
        [SerializeField, PreviewField] private Sprite _icon;
        [SerializeField] private StackType _stackingType;

        private Func<float, float, float> _stack
        {
            get
            {

                Func<float, float, float> aggr;
                switch (StackingType)
                {
                    default:
                    case StackType.Multiply:
                        aggr = (a, b) => a * b;
                        break;
                    case StackType.Add:
                        aggr = (a, b) => a + b;
                        break;
                    case StackType.Overwrite:
                        aggr = (a, b) => b;
                        break;
                    case StackType.Ignore:
                        aggr = (a, b) => a;
                        break;

                }

                return aggr;

            }
        }

        public float Stack(List<ShopAttributeValue> values)
        {
            return Stack(values.Select(v => v.Value).ToArray());
        }

        public float Stack(params float[] values)
        {
            return values.Aggregate(_stack);
        }
    }

    public enum StackType
    {
        Multiply, // stack by multiplying attribute values together, e.g. 1.1 stacked with 1.1 = 1.21.
        Add, // stack by adding attribute values together, e.g. 3 stacked with 1 = 4
        Overwrite, // stack by replacing initial value with subsequent value, e.g. 3 stacked with 1 = 1
        Ignore // stack by ignoring subsequent value and keeping initial value, e.g. 3 stacked with 1 = 3
    }
}