using UnityEngine;
using System.Collections;

[System.Serializable]
[AddComponentMenu("NGUI/NData/Master Path")]
public class NguiMasterPath : MonoBehaviour
{
	public string MasterPath = "";
	
	public string GetFullPath()
	{
		var parent = NguiUtils.GetComponentInParentsExcluding<NguiMasterPath>(gameObject);
		var parentMasterPath = (parent == null) ? string.Empty : parent.GetFullPath();
		var fullPath = (string.IsNullOrEmpty(parentMasterPath)) ?
			MasterPath : (parentMasterPath + "." + MasterPath);
		return fullPath;
	}
}
