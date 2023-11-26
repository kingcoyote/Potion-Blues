using UnityEngine;
using UnityEngine.InputSystem;

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
        if (_context.phase != InputActionPhase.Performed || _context.phase != InputActionPhase.Canceled)
            return;

        Debug.Log(_mainCamera.ScreenToWorldPoint(_playerInput.actions["cursor"].ReadValue<Vector2>()));
    }
}
