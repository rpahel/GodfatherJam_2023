using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CPlayerHoldReleaseManager : MonoBehaviour
{
    [SerializeField]
    private bool isBase;

    private CPlayerInputs inputs;
    private Rigidbody2D rb2d;

    private void Awake()
    {
        TryGetComponent(out inputs);
        TryGetComponent(out rb2d);
    }

    public Transform GrabCharacter(Transform newParent = null)
    {
        if (isBase)
        {
            if(newParent)
                transform.SetParent(newParent);

            if (inputs)
                inputs.ToggleControls(false);

            if (rb2d)
                rb2d.velocity = Vector2.zero;

            return transform;
        }

        if (transform.parent.TryGetComponent(out CPlayerHoldReleaseManager manager))
        {
            // ptet
            //if (rb2d)
            //  rb2d.velocity = Vector2.zero;
            // ici aussi ?

            return manager.GrabCharacter(newParent);
        }

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
