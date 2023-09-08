using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CPlayerInputs : MonoBehaviour
{
    private CPlayerMovements cPlayerMovements;
    private bool isControlsEnabled;

    private void Awake()
    {
        if (!TryGetComponent(out PlayerInput playerInput))
            Debug.LogError("ON S'EN FOUT : No PlayerInput found in " + name + " !");
            
        if (!TryGetComponent(out cPlayerMovements))
            Debug.LogError("No CPlayerMovements found in " + name + " !");

        // TODO : A supp
        isControlsEnabled = true;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!isControlsEnabled)
            return;

        if (!cPlayerMovements)
            return;

        if (context.started)
            cPlayerMovements.Jump();
    }

    public void HorizontalMovement(InputAction.CallbackContext context)
    {
        if (!isControlsEnabled)
            return;

        if (!cPlayerMovements)
            return;

        cPlayerMovements.HorizontalMovement(context.ReadValue<float>());
    }

    public bool ToggleControls(bool isEnabled)
    {
        isControlsEnabled = isEnabled;
        return isControlsEnabled;
    }
}