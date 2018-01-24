namespace Slash.Unity.Common.UI.Layouting
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    ///     Does layouting for the child rectangles, so they don't overlap.
    /// </summary>
    public class AvoidVerticalOverlapLayoutGroup : LayoutGroup
    {
        /// <summary>
        ///     Minimum space between each child rectangle (in pixels).
        /// </summary>
        [Tooltip("Minimum space between each child rectangle (in pixels)")]
        public float Space;

        /// <inheritdoc />
        public override void CalculateLayoutInputVertical()
        {
            float totalMin = 0;
            float totalPreferred = 0;
            foreach (var rectChild in this.rectChildren)
            {
                totalMin += rectChild.sizeDelta.y;
                totalPreferred += rectChild.sizeDelta.y + this.Space;
            }

            this.SetLayoutInputForAxis(totalMin, totalPreferred, totalPreferred, 1);
        }

        /// <inheritdoc />
        public override void SetLayoutHorizontal()
        {
            this.DoLayout(0);
        }

        /// <inheritdoc />
        [ContextMenu("Do Layout")]
        public override void SetLayoutVertical()
        {
            this.DoLayout(1);
        }

        private void DoLayout(int axis)
        {
            if (this.rectChildren.Count == 0)
            {
                return;
            }

            // Collect child rectangles.
            var layoutRectangles = new List<AvoidOverlapping.LayoutRectangle>();
            foreach (var rectChild in this.rectChildren)
            {
                var autoLayoutRectangle = rectChild.GetComponent<AvoidOverlappingRectangle>();
                if (autoLayoutRectangle == null)
                {
                    continue;
                }

                layoutRectangles.Add(new AvoidOverlapping.LayoutRectangle
                {
                    Data = rectChild,
                    Position = autoLayoutRectangle.PreferredPosition,
                    Size = rectChild.sizeDelta
                });
            }

            // Do auto layout for children.
            AvoidOverlapping.DoVerticalLayout(layoutRectangles, this.Space);

            // Update rect transforms of layout rectangles.
            foreach (var layoutRectangle in layoutRectangles)
            {
                var rectChild = (RectTransform) layoutRectangle.Data;
                rectChild.anchoredPosition = layoutRectangle.Position;
            }
        }
    }
}