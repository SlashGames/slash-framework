using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class NguiMultiBinding : NguiBaseBinding
{
	public List<string> Paths;
	public override IList<string> ReferencedPaths { get { return Paths; } }
}
