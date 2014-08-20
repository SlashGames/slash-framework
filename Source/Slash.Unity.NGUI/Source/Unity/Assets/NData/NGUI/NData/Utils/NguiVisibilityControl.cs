using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public interface IVisibilityBinding
{
	bool Visible { get; }
	bool ObjectVisible { get; }
	bool ComponentVisible { get; }
	void InvalidateParent();
}

public class NguiVisibilityControl
{
	private bool _parentIsValid;
	private IVisibilityBinding _parentVisibility;
	private bool _selfVisible;
	private List<IVisibilityBinding> _siblings;
	private GameObject _gameObject;
	
	private bool SiblingsVisible
	{
		get
		{
			if (_siblings == null)
				return true;
			foreach(var s in _siblings)
				if (!s.ComponentVisible)
					return false;
			return true;
		}
	}
	
	public bool ComponentVisible
	{
		get
		{
			return _selfVisible;
		}
	}
	
	public bool ObjectVisible
	{
		get
		{
			return ComponentVisible && SiblingsVisible;
		}
	}
	
	public bool Visible
	{
		get 
		{
			if (!_parentIsValid)
			{
				UpdateParentVisibility();
			}
			if (_parentVisibility != null)
				return ObjectVisible && _parentVisibility.Visible;
			return ObjectVisible;
		}	
	}
	
	public void Awake(GameObject gameObject)
	{
		_gameObject = gameObject;
		
		UpdateParentVisibility();
		
		var siblings = NguiUtils.GetComponentsAs<IVisibilityBinding>(_gameObject);
		if (siblings.Length > 1)
		{
			_siblings = new List<IVisibilityBinding>();
			for (var i = 0; i < siblings.Length; ++i)
				if (siblings[i] != this)
					_siblings.Add(siblings[i]);
		}
	}
	
	public void InvalidateParent()
	{
		_parentIsValid = false;
	}
	
	private void UpdateParentVisibility()
	{
		_parentVisibility = _gameObject.transform.parent != null
			? NguiUtils.GetComponentInParentsAs<IVisibilityBinding>(_gameObject.transform.parent.gameObject)
			: null;
		_parentIsValid = true;
	}
	
	public void ApplyNewValue(bool newValue)
	{
		_selfVisible = newValue;
		NguiUtils.SetVisible(_gameObject, Visible);
	}
}
