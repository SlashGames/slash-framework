using UnityEngine;

[System.Serializable]
[AddComponentMenu("NGUI/NData/Projected Position Binding")]
public class NguiProjectedPositionBinding : NguiVector3Binding
{
	public Camera targetCamera;
	public bool pixelPerfect = true;
	
	private Vector3 _wsPosition;
	private Transform _anchor;
	private UICamera _nguiCamera;
	
	public override void Start()
	{
		base.Start();
		_nguiCamera = NguiUtils.GetComponentInParents<UICamera>(gameObject);
		var anchor = NguiUtils.GetComponentInParents<UIAnchor>(gameObject);
		if (anchor == null)
		{
			Debug.LogWarning("There have to be an UIAnchor upper in the hierarchy");
			return;
		}
		if (anchor.side != UIAnchor.Side.BottomLeft &&
			anchor.side != UIAnchor.Side.BottomRight &&
			anchor.side != UIAnchor.Side.TopLeft &&
			anchor.side != UIAnchor.Side.TopRight)
		{
			Debug.LogWarning("Parent UIAnchor have to be a corner one (BottomLeft, BottomRight, TopLeft or TopRight)");
		}
		_anchor = anchor.transform;
	}
	
	protected override void ApplyNewValue(Vector3 value)
	{
		_wsPosition = value;
		LateUpdate();
	}
	
	void LateUpdate()
	{
		var c = targetCamera;
		if (c == null)
			c = Camera.main;
		if (c == null)
			return;
		
		_wsPosition = GetVector3Value();
		
		if (_nguiCamera != null)
		{
			var ss = c.WorldToViewportPoint(_wsPosition);
			ss = _nguiCamera.camera.ViewportToWorldPoint(ss);
			if (pixelPerfect)
			{
				ss.x = (int)ss.x;
				ss.y = (int)ss.y;
			}
			ss.z = 0;
			gameObject.transform.localPosition = ss;
		}
		
		if (_anchor != null)
		{
			var ss = c.WorldToViewportPoint(_wsPosition);
			ss.x = -2 * ss.x * _anchor.localPosition.x;
			ss.y = -2 * ss.y * _anchor.localPosition.y;
			if (pixelPerfect)
			{
				ss.x = (int)ss.x;
				ss.y = (int)ss.y;
			}
			ss.z = 0;
			gameObject.transform.localPosition = ss;
		}
	}
}
