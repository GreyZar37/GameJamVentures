using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    private PlayerInput input;
    public PlayerInput Input
    {
        get
        {
            if(input == null)
            {
                input = new PlayerInput();
                input.Enable();
            }
            return input;
        }
    }

    private void OnDestroy()
    {
        Input.Dispose();
    }
}