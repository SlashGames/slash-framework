// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UIGridWithOffset.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.NGUIExt.Interaction
{
    using UnityEngine;

    public class UIGridWithOffset : UIGrid
    {
        #region Fields

        /// <summary>
        ///   Number of empty cells before the first filled grid cell.
        /// </summary>
        public int CellOffset;

        #endregion

        #region Public Methods and Operators

        [ContextMenu("Execute")]
        public override void Reposition()
        {
            // Basically taken from UIGrid.Reposition.
            if (Application.isPlaying && !this.mInitDone && NGUITools.GetActive(this))
            {
                this.mReposition = true;
                return;
            }

            if (!this.mInitDone)
            {
                this.Init();
            }

            this.mReposition = false;
            Transform myTrans = this.transform;

            int x = this.CellOffset % this.maxPerLine;
            int y = this.CellOffset / this.maxPerLine;
            int maxX = 0;
            int maxY = 0;

            if (this.sorting != Sorting.None)
            {
                BetterList<Transform> list = new BetterList<Transform>();

                for (int i = 0; i < myTrans.childCount; ++i)
                {
                    Transform t = myTrans.GetChild(i);
                    if (t && (!this.hideInactive || NGUITools.GetActive(t.gameObject)))
                    {
                        list.Add(t);
                    }
                }

                if (this.sorting == Sorting.Alphabetic)
                {
                    list.Sort(SortByName);
                }
                else if (this.sorting == Sorting.Horizontal)
                {
                    list.Sort(SortHorizontal);
                }
                else if (this.sorting == Sorting.Vertical)
                {
                    list.Sort(SortVertical);
                }
                else
                {
                    this.Sort(list);
                }

                for (int i = 0, imax = list.size; i < imax; ++i)
                {
                    Transform t = list[i];

                    if (!NGUITools.GetActive(t.gameObject) && this.hideInactive)
                    {
                        continue;
                    }

                    float depth = t.localPosition.z;
                    Vector3 pos = (this.arrangement == Arrangement.Horizontal)
                                      ? new Vector3(this.cellWidth * x, -this.cellHeight * y, depth)
                                      : new Vector3(this.cellWidth * y, -this.cellHeight * x, depth);

                    if (this.animateSmoothly && Application.isPlaying)
                    {
                        SpringPosition.Begin(t.gameObject, pos, 15f).updateScrollView = true;
                    }
                    else
                    {
                        t.localPosition = pos;
                    }

                    maxX = Mathf.Max(maxX, x);
                    maxY = Mathf.Max(maxY, y);

                    if (++x >= this.maxPerLine && this.maxPerLine > 0)
                    {
                        x = 0;
                        ++y;
                    }
                }
            }
            else
            {
                for (int i = 0; i < myTrans.childCount; ++i)
                {
                    Transform t = myTrans.GetChild(i);

                    if (!NGUITools.GetActive(t.gameObject) && this.hideInactive)
                    {
                        continue;
                    }

                    float depth = t.localPosition.z;
                    Vector3 pos = (this.arrangement == Arrangement.Horizontal)
                                      ? new Vector3(this.cellWidth * x, -this.cellHeight * y, depth)
                                      : new Vector3(this.cellWidth * y, -this.cellHeight * x, depth);

                    if (this.animateSmoothly && Application.isPlaying)
                    {
                        SpringPosition.Begin(t.gameObject, pos, 15f).updateScrollView = true;
                    }
                    else
                    {
                        t.localPosition = pos;
                    }

                    maxX = Mathf.Max(maxX, x);
                    maxY = Mathf.Max(maxY, y);

                    if (++x >= this.maxPerLine && this.maxPerLine > 0)
                    {
                        x = 0;
                        ++y;
                    }
                }
            }

            // Apply the origin offset
            if (this.pivot != UIWidget.Pivot.TopLeft)
            {
                Vector2 po = NGUIMath.GetPivotOffset(this.pivot);

                float fx, fy;

                if (this.arrangement == Arrangement.Horizontal)
                {
                    fx = Mathf.Lerp(0f, maxX * this.cellWidth, po.x);
                    fy = Mathf.Lerp(-maxY * this.cellHeight, 0f, po.y);
                }
                else
                {
                    fx = Mathf.Lerp(0f, maxY * this.cellWidth, po.x);
                    fy = Mathf.Lerp(-maxX * this.cellHeight, 0f, po.y);
                }

                for (int i = 0; i < myTrans.childCount; ++i)
                {
                    Transform t = myTrans.GetChild(i);

                    if (!NGUITools.GetActive(t.gameObject) && this.hideInactive)
                    {
                        continue;
                    }

                    SpringPosition sp = t.GetComponent<SpringPosition>();

                    if (sp != null)
                    {
                        sp.target.x -= fx;
                        sp.target.y -= fy;
                    }
                    else
                    {
                        Vector3 pos = t.localPosition;
                        pos.x -= fx;
                        pos.y -= fy;
                        t.localPosition = pos;
                    }
                }
            }

            if (this.keepWithinPanel && this.mPanel != null)
            {
                this.mPanel.ConstrainTargetToBounds(myTrans, true);
            }

            if (this.onReposition != null)
            {
                this.onReposition();
            }
        }

        #endregion
    }
}