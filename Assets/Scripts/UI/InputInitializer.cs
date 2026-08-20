using UnityEngine;

public class InputInitializer : MonoBehaviour
{
    private void Awake()
    {
        InputManager.Load();
    }
}