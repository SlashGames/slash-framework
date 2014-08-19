//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// All children added to the game object with this script will be repositioned to be on a grid of specified dimensions.
/// If you want the cells to automatically set their scale based on the dimensions of their content, take a look at UITable.
/// </summary>

[AddComponentMenu("NGUI/Interaction/Grid")]
public class UIGrid : UIWidgetContainer
{
	public delegate void OnReposition ();

	public enum Arrangement
	{
		Horizontal,
		Vertical,
	}

	public enum Sorting
	{
		None,
		Alphabetic,
		Horizontal,
		Vertical,
		Custom,
	}

	/// <summary>
	/// Type of arrangement -- vertical or horizontal.
	/// </summary>

	public Arrangement arrangement = Arrangement.Horizontal;

	/// <summary>
	/// How to sort the grid's elements.
	/// </summary>

	public Sorting sorting = Sorting.None;

	/// <summary>
	/// Final pivot point for the grid's content.
	/// </summary>

	public UIWidget.Pivot pivot = UIWidget.Pivot.TopLeft;

	/// <summary>
	/// Maximum children per line.
	/// If the arrangement is horizontal, this denotes the number of columns.
	/// If the arrangement is vertical, this stands for the number of rows.
	/// </summary>

	public int maxPerLine = 0;

	/// <summary>
	/// The width of each of the cells.
	/// </summary>

	public float cellWidth = 200f;

	/// <summary>
	/// The height of each of the cells.
	/// </summary>

	public float cellHeight = 200f;

	/// <summary>
	/// Whether the grid will smoothly animate its children into the correct place.
	/// </summary>

	public bool animateSmoothly = false;

	/// <summary>
	/// Whether to ignore the disabled children or to treat them as being present.
	/// </summary>

	public bool hideInactive = true;

	/// <summary>
	/// Whether the parent container will be notified of the grid's changes.
	/// </summary>

	public bool keepWithinPanel = false;

	/// <summary>
	/// Callback triggered when the grid repositions its contents.
	/// </summary>

	public OnReposition onReposition;

	// Use the 'sorting' property instead
	[HideInInspector][SerializeField] bool sorted = false;

	protected bool mReposition = false;
	protected UIPanel mPanel;
	protected bool mInitDone = false;

	/// <summary>
	/// Reposition the children on the next Update().
	/// </summary>

	public bool repositionNow { set { if (value) { mReposition = true; enabled = true; } } }

	protected virtual void Init ()
	{
		mInitDone = true;
		mPanel = NGUITools.FindInParents<UIPanel>(gameObject);
	}

	protected virtual void Start ()
	{
		if (!mInitDone) Init();
		bool smooth = animateSmoothly;
		animateSmoothly = false;
		Reposition();
		animateSmoothly = smooth;
		enabled = false;
	}

	protected virtual void Update ()
	{
		if (mReposition) Reposition();
		enabled = false;
	}

	static public int SortByName (Transform a, Transform b) { return string.Compare(a.name, b.name); }
	static public int SortHorizontal (Transform a, Transform b) { return a.localPosition.x.CompareTo(b.localPosition.x); }
	static public int SortVertical (Transform a, Transform b) { return b.localPosition.y.CompareTo(a.localPosition.y); }

	/// <summary>
	/// Want your own custom sorting logic? Override this function.
	/// </summary>

	protected virtual void Sort (BetterList<Transform> list) { list.Sort(SortByName); }

	/// <summary>
	/// Recalculate the position of all elements within the grid, sorting them alphabetically if necessary.
	/// </summary>

	[ContextMenu("Execute")]
	public virtual void Reposition ()
	{
		if (Application.isPlaying && !mInitDone && NGUITools.GetActive(this))
		{
			mReposition = true;
			return;
		}

		if (!mInitDone) Init();

		mReposition = false;
		Transform myTrans = transform;

		int x = 0;
		int y = 0;
		int maxX = 0;
		int maxY = 0;

		if (sorting != Sorting.None || sorted)
		{
			BetterList<Transform> list = new BetterList<Transform>();

			for (int i = 0; i < myTrans.childCount; ++i)
			{
				Transform t = myTrans.GetChild(i);
				if (t && (!hideInactive || NGUITools.GetActive(t.gameObject))) list.Add(t);
			}

			if (sorting == Sorting.Alphabetic) list.Sort(SortByName);
			else if (sorting == Sorting.Horizontal) list.Sort(SortHorizontal);
			else if (sorting == Sorting.Vertical) list.Sort(SortVertical);
			else Sort(list);

			for (int i = 0, imax = list.size; i < imax; ++i)
			{
				Transform t = list[i];

				if (!NGUITools.GetActive(t.gameObject) && hideInactive) continue;

				float depth = t.localPosition.z;
				Vector3 pos = (arrangement == Arrangement.Horizontal) ?
					new Vector3(cellWidth * x, -cellHeight * y, depth) :
					new Vector3(cellWidth * y, -cellHeight * x, depth);

				if (animateSmoothly && Application.isPlaying)
				{
					SpringPosition.Begin(t.gameObject, pos, 15f).updateScrollView = true;
				}
				else t.localPosition = pos;

				maxX = Mathf.Max(maxX, x);
				maxY = Mathf.Max(maxY, y);

				if (++x >= maxPerLine && maxPerLine > 0)
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

				if (!NGUITools.GetActive(t.gameObject) && hideInactive) continue;

				float depth = t.localPosition.z;
				Vector3 pos = (arrangement == Arrangement.Horizontal) ?
					new Vector3(cellWidth * x, -cellHeight * y, depth) :
					new Vector3(cellWidth * y, -cellHeight * x, depth);

				if (animateSmoothly && Application.isPlaying)
				{
					SpringPosition.Begin(t.gameObject, pos, 15f).updateScrollView = true;
				}
				else t.localPosition = pos;

				maxX = Mathf.Max(maxX, x);
				maxY = Mathf.Max(maxY, y);

				if (++x >= maxPerLine && maxPerLine > 0)
				{
					x = 0;
					++y;
				}
			}
		}

		// Apply the origin offset
		if (pivot != UIWidget.Pivot.TopLeft)
		{
			Vector2 po = NGUIMath.GetPivotOffset(pivot);

			float fx, fy;

			if (arrangement == Arrangement.Horizontal)
			{
				fx = Mathf.Lerp(0f, maxX * cellWidth, po.x);
				fy = Mathf.Lerp(-maxY * cellHeight, 0f, po.y);
			}
			else
			{
				fx = Mathf.Lerp(0f, maxY * cellWidth, po.x);
				fy = Mathf.Lerp(-maxX * cellHeight, 0f, po.y);
			}

			for (int i = 0; i < myTrans.childCount; ++i)
			{
				Transform t = myTrans.GetChild(i);

				if (!NGUITools.GetActive(t.gameObject) && hideInactive) continue;

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

		if (keepWithinPanel && mPanel != null)
			mPanel.ConstrainTargetToBounds(myTrans, true);

		if (onReposition != null)
			onReposition();
	}
}
