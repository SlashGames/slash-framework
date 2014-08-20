using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("NGUI/NData/PopupList Binding")]
public class NguiPopupListSourceBinding : NguiItemsSourceBinding
{
	/*
	private UIPopupList _uiPopupList = null;
	public override void Awake()
	{
		base.Awake();
		_uiPopupList = GetComponent<UIPopupList>();
		if (_uiPopupList != null)
		{
			_uiPopupList.onChange.Add(new EventDelegate(OnSelectionChange));
		}
	}
	
	protected override void Bind()
	{
		base.Bind();
		
		if (_uiPopupList == null)
			return;
		OnItemsClear();
		if (_collection == null)
			return;
		for (var i = 0; i < _collection.ItemsCount; ++i)
		{
			_uiPopupList.items.Add(GetItemDisplayValue(i));
		}
		_uiPopupList.value = GetItemDisplayValue(_collection.SelectedIndex);
	}
		
	protected override void OnItemInsert(int position, EZData.Context item)
	{
		base.OnItemInsert(position, item);
		_uiPopupList.items.Insert(position, GetDisplayValueProperty(item).GetValue());
		_uiPopupList.value = GetItemDisplayValue(_collection.SelectedIndex);
	}
	
	protected override void OnItemRemove(int position)
	{
		if (_collection == null || _uiPopupList == null)
			return;
		_displayValuesCache.Remove(_collection.GetBaseItem(position));
		base.OnItemRemove(position);
		_uiPopupList.items.RemoveAt(position);
		if (_uiPopupList.items.Count == 0)
			_uiPopupList.value = string.Empty;
		else
			_uiPopupList.value = GetItemDisplayValue(_collection.SelectedIndex);
	}
	
	protected override void OnItemsClear()
	{
		_displayValuesCache.Clear();
		_uiPopupList.items.Clear();
		_uiPopupList.value = string.Empty;
	}
	
	private EZData.Property<string> GetDisplayValueProperty(EZData.Context item)
	{
		if (item == null)
			return null;
		
		EZData.Property<string> property = null;
		if (_displayValuesCache.TryGetValue(item, out property))
			return property;
		property = item.FindProperty<string>(DisplayValuePath, this);
		if (property != null)
			_displayValuesCache.Add(item, property);
		return property;
	}
	
	private string GetItemDisplayValue(int index)
	{
		if (_collection == null)
			return string.Empty;
		var property = GetDisplayValueProperty(_collection.GetBaseItem(index));
		if (property == null)
			return string.Empty;
		return property.GetValue();
	}
	
	public void OnSelectionChange()
	{
		var selectedItem = _uiPopupList.value;
		if (_collection != null && !_isCollectionSelecting)
		{
			_isCollectionSelecting = true;
			for (var i = 0; i < _collection.ItemsCount; ++i)
			{
				if (GetItemDisplayValue(i) == selectedItem)
				{
					_collection.SelectItem(i);
					break;
				}
			}
			_isCollectionSelecting = false;
		}
	}
	
	protected override void OnCollectionSelectionChange()
	{
		if (_uiPopupList == null || _collection == null)
			return;
		
		var selectedValue = GetItemDisplayValue(_collection.SelectedIndex);
		_uiPopupList.value = selectedValue;
	}
	*/
	
	public string DisplayValuePath;
	
	private readonly Dictionary<EZData.Context, EZData.Property<string>> _displayValuesCache = new Dictionary<EZData.Context, EZData.Property<string>>();
	
	private UIPopupList _uiPopupList = null;
	
#if NGUI_2
	private GameObject _nativeEventReceiver;
	private string _nativeFunctionName;
#endif
	
	private void AssignValue(string value)
	{
#if NGUI_2
		_uiPopupList.selection = value;
#else
		_uiPopupList.value = value;
#endif
	}
	
	public override void Awake()
	{
		base.Awake();
		_uiPopupList = GetComponent<UIPopupList>();
		if (_uiPopupList != null)
		{
#if NGUI_2
			_nativeEventReceiver = _uiPopupList.eventReceiver;
			_nativeFunctionName = _uiPopupList.functionName;
			
			_uiPopupList.eventReceiver = gameObject;
			_uiPopupList.functionName = "OnSelectionChange";
#else
			_uiPopupList.onChange.Add(new EventDelegate(OnSelectionChange));
#endif
		}
	}
	
	protected override void Bind()
	{
		base.Bind();
		
		if (_uiPopupList == null)
			return;
		OnItemsClear();
		if (_collection == null)
			return;
		for (var i = 0; i < _collection.ItemsCount; ++i)
		{
			_uiPopupList.items.Add(GetItemDisplayValue(i));
		}
		AssignValue(GetItemDisplayValue(_collection.SelectedIndex));
	}
		
	protected override void OnItemInsert(int position, EZData.Context item)
	{
		base.OnItemInsert(position, item);
		_uiPopupList.items.Insert(position, GetDisplayValueProperty(item).GetValue());
		AssignValue(GetItemDisplayValue(_collection.SelectedIndex));
	}
	
	protected override void OnItemRemove(int position)
	{
		if (_collection == null || _uiPopupList == null)
			return;
		_displayValuesCache.Remove(_collection.GetBaseItem(position));
		base.OnItemRemove(position);
		_uiPopupList.items.RemoveAt(position);
		if (_uiPopupList.items.Count == 0)
			AssignValue(string.Empty);
		else
			AssignValue(GetItemDisplayValue(_collection.SelectedIndex));
	}
	
	protected override void OnItemsClear()
	{
		_displayValuesCache.Clear();
		_uiPopupList.items.Clear();
		AssignValue(string.Empty);
	}
	
	private EZData.Property<string> GetDisplayValueProperty(EZData.Context item)
	{
		if (item == null)
			return null;
		
		EZData.Property<string> property = null;
		if (_displayValuesCache.TryGetValue(item, out property))
			return property;
		property = item.FindProperty<string>(DisplayValuePath, this);
		if (property != null)
			_displayValuesCache.Add(item, property);
		return property;
	}
	
	private string GetItemDisplayValue(int index)
	{
		if (_collection == null)
			return string.Empty;
		var property = GetDisplayValueProperty(_collection.GetBaseItem(index));
		if (property == null)
			return string.Empty;
		return property.GetValue();
	}
	
#if !NGUI_2
	public void OnSelectionChange()
	{
		OnSelectionChange(_uiPopupList.value);
	}
#endif
	
	public void OnSelectionChange(string selectedItem)
	{
		if (_collection != null && !_isCollectionSelecting)
		{
			_isCollectionSelecting = true;
			for (var i = 0; i < _collection.ItemsCount; ++i)
			{
				if (GetItemDisplayValue(i) == selectedItem)
				{
					_collection.SelectItem(i);
					break;
				}
			}
			_isCollectionSelecting = false;
		}
#if NGUI_2
		if (_nativeEventReceiver != null)
		{
			if (_nativeEventReceiver != gameObject || _nativeFunctionName != "OnSelectionChange")
			{
				_nativeEventReceiver.SendMessage(_nativeFunctionName, selectedItem, SendMessageOptions.DontRequireReceiver);
			}
		}
#endif
	}
	
	protected override void OnCollectionSelectionChange()
	{
		if (_uiPopupList == null || _collection == null)
			return;
		
		var selectedValue = GetItemDisplayValue(_collection.SelectedIndex);
		AssignValue(selectedValue);
	}
}
