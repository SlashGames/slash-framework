using UnityEngine;
using System.Collections;

public class TBHoverChangeMaterial : MonoBehaviour
{
    public Material hoverMaterial;
    Material normalMaterial;

    void Start()
    {
        // remember our original material
        normalMaterial = renderer.sharedMaterial;
    }

    void OnFingerHover( FingerHoverEvent e )
    {
        if( e.Phase == FingerHoverPhase.Enter )
            renderer.sharedMaterial = hoverMaterial; // show hover-state material
        else
            renderer.sharedMaterial = normalMaterial; // restore original material
    }
}
