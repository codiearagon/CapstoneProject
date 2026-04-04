using UnityEngine;

public class CharacterInput : MonoBehaviour
{
    private PlayerInput _input;

    private void Awake()
    {
        _input = new PlayerInput();
    }

    private void OnEnable()
    {
        _input.Player.Enable();
    }

    private void OnDisable()
    {
        _input.Player.Disable();
    }
}
