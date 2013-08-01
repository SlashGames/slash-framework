// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FPSDisplay.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine;

/// <summary>
///   Displays the frames per second (fps) to check performance.
/// </summary>
public class FPSDisplay : MonoBehaviour
{
    #region Fields

    /// <summary>
    ///   UI element to show fps.
    /// </summary>
    public GUIText Text;

    /// <summary>
    ///   Time frame the fps should be updated (in s).
    /// </summary>
    public float TimeFrame = 1.0f;

    /// <summary>
    ///   Elapsed time since last computation (in s).
    /// </summary>
    private float elapsedTime;

    /// <summary>
    ///   Frame count since last computation.
    /// </summary>
    private int frameCount;

    #endregion

    #region Methods

    private void Update()
    {
        ++this.frameCount;
        this.elapsedTime += Time.deltaTime;

        if (this.elapsedTime > this.TimeFrame)
        {
            if (this.Text != null)
            {
                // Compute fps.
                float fps = this.frameCount / this.elapsedTime;
                this.Text.text = string.Format("{0:F2} fps ({1} s)", fps, this.TimeFrame);
            }

            // Reset stats.
            this.frameCount = 0;
            this.elapsedTime = 0.0f;
        }
    }

    #endregion
}