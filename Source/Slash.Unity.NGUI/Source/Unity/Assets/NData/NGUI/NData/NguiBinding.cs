using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum NguiBindingInitialValue
{
	TakeFromModel = 0,
	TakeFromView,
}

public enum NguiBindingDirection
{
	TwoWay = 0,
	ModelToViewOnly,
	ViewToModelOnly,
}

public static class NguiBindingDirectionExtensions
{
    public static bool NeedsViewTracking(this NguiBindingDirection val)
    {
        return val == NguiBindingDirection.TwoWay ||
				val == NguiBindingDirection.ViewToModelOnly;
    }
	
	public static bool NeedsViewUpdate(this NguiBindingDirection val)
	{
		return val == NguiBindingDirection.TwoWay ||
				val == NguiBindingDirection.ModelToViewOnly;
	}
}

[System.Serializable]
public class NguiBinding : NguiBaseBinding
{
	public string Path;
	public override IList<string> ReferencedPaths { get { return new List<string> { Path }; } }
}
