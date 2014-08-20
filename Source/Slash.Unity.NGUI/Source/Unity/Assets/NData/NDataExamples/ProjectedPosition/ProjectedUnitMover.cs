using UnityEngine;
using System.Collections;

public class ProjectedUnitMover : MonoBehaviour
{
	private Vector3 _spinAxis;
	private Vector3 _rotationAxis;
		
	void Start()
	{
		_spinAxis = Random.onUnitSphere;	
		_rotationAxis = Random.onUnitSphere;
		transform.position = Random.insideUnitSphere * 4;
	}
	
	void Update()
	{
		var rotation = Matrix4x4.TRS(
			Vector3.zero,
			Quaternion.AngleAxis(Time.deltaTime * 50, _rotationAxis),
			Vector3.one);
		transform.position = rotation.MultiplyPoint(transform.position);
		transform.Rotate(_spinAxis, Time.deltaTime * 50);
	}
}
