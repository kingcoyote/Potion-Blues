using UnityEngine;
using UnityEngine.Events;

namespace PotionBlues.Prototypes.Autodispense
{
    public class SelectHandlerScript : MonoBehaviour
    {
        public UnityEvent OnSelect = new();
        public UnityEvent OnRelease = new();

        public void Select()
        {
            OnSelect.Invoke();
        }

        public void Release()
        {
            OnRelease.Invoke();
        }
    }
}