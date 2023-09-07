using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CPlayerHoldReleaseManager : MonoBehaviour
{
    [SerializeField]
    private bool isBase;

    private CPlayerInputs inputs;

    private void Awake()
    {
        TryGetComponent(out inputs);
    }

    public Transform GrabCharacter(Transform newParent = null)
    {
        if (isBase)
        {
            if(newParent)
                transform.SetParent(newParent);

            if (inputs)
                inputs.ToggleControls(false);

            return transform;
        }

        if (transform.parent.TryGetComponent(out CPlayerHoldReleaseManager manager))
            return manager.GrabCharacter(newParent);

        return null;
    }

    public void ReleaseCharacter()
    {
        if (isBase)
        {
            transform.SetParent(null);

            if (inputs)
                inputs.ToggleControls(true);
        }

        if (transform.parent && transform.parent.TryGetComponent(out CPlayerHoldReleaseManager manager))
            manager.ReleaseCharacter();
    }
}
