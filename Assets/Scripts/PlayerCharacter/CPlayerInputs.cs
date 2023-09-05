using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CPlayerInputs : MonoBehaviour
{
    private CPlayerMovements cPlayerMovements;

    private void Awake()
    {
        if(!TryGetComponent(out cPlayerMovements))
            Debug.LogError("No CPlayerMovements found in " + name + " !");
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!cPlayerMovements)
            return;

        if (context.started)
            cPlayerMovements.Jump();
    }

    public void HorizontalMovement(InputAction.CallbackContext context)
    {
        if (!cPlayerMovements)
            return;

        cPlayerMovements.HorizontalMovement(context.ReadValue<float>());
    }
}
