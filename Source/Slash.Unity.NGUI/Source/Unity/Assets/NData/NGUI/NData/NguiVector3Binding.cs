using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public abstract class NguiVector3Binding : NguiBinding
{
	private EZData.Property<GameObject> _gameObjectProperty;
	private EZData.Property<Component> _componentProperty;
	private EZData.Property<Transform> _transformProperty;
	private EZData.Property<Vector3> _v3property;
	private EZData.Property<Vector2> _v2property;
	private EZData.Property<Texture2D> _t2property;
	
	private bool _ignoreChanges = false;
	
	protected override void Unbind()
	{
		base.Unbind();
		
		if (_gameObjectProperty != null)
			_gameObjectProperty.OnChange -= OnChange;
		if (_componentProperty != null)
			_componentProperty.OnChange -= OnChange;
		if (_transformProperty != null)
			_transformProperty.OnChange -= OnChange;
		if (_v3property != null)
			_v3property.OnChange -= OnChange;
		if (_v2property != null)
			_v2property.OnChange -= OnChange;
		if (_t2property != null)
			_t2property.OnChange -= OnChange;
		
		_gameObjectProperty = null;
		_componentProperty = null;
		_transformProperty = null;
		_v3property = null;
		_v2property = null;
		_t2property = null;
	}
	
	private void Bind(NguiDataContext context)
	{
		_v3property = context.FindProperty<Vector3>(Path, this);
		if (_v3property != null)
		{
			_v3property.OnChange += OnChange;
			return;
		}
		
		_v2property = context.FindProperty<Vector2>(Path, this);
		if (_v2property != null)
		{
			_v2property.OnChange += OnChange;
			return;
		}
		
		_t2property = context.FindProperty<Texture2D>(Path, this);
		if (_t2property != null)
		{
			_t2property.OnChange += OnChange;
			return;
		}
		
		_transformProperty = context.FindProperty<Transform>(Path, this);
		if (_transformProperty != null)
		{
			_transformProperty.OnChange += OnChange;
			return;
		}
		
		_componentProperty = context.FindProperty<Component>(Path, this);
		if (_componentProperty != null)
		{
			_componentProperty.OnChange += OnChange;
			return;
		}

		_gameObjectProperty = context.FindProperty<GameObject>(Path, this);
		if (_gameObjectProperty != null)
		{
			_gameObjectProperty.OnChange += OnChange;
			return;
		}
	}
	
	protected override void Bind()
	{
		base.Bind();
			
		var context = GetContext(Path);
		if (context == null)
		{
			Debug.LogWarning("NguiVector3Binding.UpdateBinding - context is null");
			return;
		}
		
		Bind(context);
	}
	
	protected void SetVector3Value(Vector3 val)
	{
		_ignoreChanges = true;
		if (_v3property != null)
			_v3property.SetValue(val);
		if (_v2property != null)
			_v2property.SetValue(new Vector2(val.x, val.y));
		if (_transformProperty != null && _transformProperty.GetValue() != null)
			_transformProperty.GetValue().position = val;
		if (_componentProperty != null && _componentProperty.GetValue() != null)
			_componentProperty.GetValue().transform.position = val;
		if (_gameObjectProperty != null && _gameObjectProperty.GetValue() != null)
			_gameObjectProperty.GetValue().transform.position = val;
		_ignoreChanges = false;
	}
	
	protected Vector3 GetVector3Value()
	{	
		if (_v3property != null)
		{
			return _v3property.GetValue();
		}
		
		if (_v2property != null)
		{
			var v2 = _v2property.GetValue();
			return new Vector3(v2.x, v2.y, 0);
		}
		
		if (_t2property != null)
		{
			var t2 = _t2property.GetValue();
			return new Vector3(t2.width, t2.height, 0);
		}
		
		if (_transformProperty != null)
		{
			var t = _transformProperty.GetValue();
			if (t != null)
				return t.position;
		}
		
		if (_componentProperty != null)
		{
			var c = _componentProperty.GetValue();
			if (c != null)
				return c.transform.position;
		}
		
		if (_gameObjectProperty != null)
		{
			var go = _gameObjectProperty.GetValue();
			if (go != null)
				return go.transform.position;
		}
		
		return Vector3.zero;
	}
	
	protected abstract void ApplyNewValue(Vector3 val);
	
	protected override void OnChange()
	{
		base.OnChange();
		
		if (_ignoreChanges)
			return;
		
		ApplyNewValue(GetVector3Value());
	}
}
