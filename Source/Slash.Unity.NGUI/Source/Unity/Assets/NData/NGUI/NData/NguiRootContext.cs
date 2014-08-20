using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("NGUI/NData/Root Context")]
public class NguiRootContext : NguiDataContext
{
	public EZData.MonoBehaviourContext defaultContext;
	
	void Awake()
	{
		if (defaultContext != null && _context == null)
			SetContext(defaultContext);
	}
	
	public void SetContext(EZData.IContext context)
	{
		_context = context;
	}
}
