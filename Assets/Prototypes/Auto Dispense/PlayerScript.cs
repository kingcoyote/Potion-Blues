using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PotionBlues.Prototypes.Autodispense
{
    public class PlayerScript : MonoBehaviour
    {
        private Camera _mainCamera;
        private PlayerInput _playerInput;

        // Start is called before the first frame update
        void Start()
        {
            _mainCamera = Camera.main;
            _playerInput = GetComponent<PlayerInput>();
        }

        public void OnSelect(InputAction.CallbackContext _context)
        {
            var cursorPos = _playerInput.actions["cursor"].ReadValue<Vector2>();
            var targets = Physics2D.GetRayIntersectionAll(_mainCamera.ScreenPointToRay(cursorPos))
                .Select(t => t.collider.gameObject.GetComponent<SelectHandlerScript>())
                .Where(t => t != null);

            if (targets.Count() == 0) return;

            switch (_context.phase)
            {
                case InputActionPhase.Started:
                    foreach (var target in targets)
                    {
                        target.Select();
                    }
                    break;
                case InputActionPhase.Performed:
                    break;
                case InputActionPhase.Canceled:
                    foreach (var target in targets)
                    {
                        target.Release();
                    }
                    break;
            }

            // if the player clicked down on a cauldron and the cauldron has a potion ready
            // spawn potion, bind to cursor
            // if the player clicked down on an ingredient
            // spawn ingredient, bind to cursor

            // if the player released click and something is bound
            // activate the bound ingredients drop logic
        }
    }
}