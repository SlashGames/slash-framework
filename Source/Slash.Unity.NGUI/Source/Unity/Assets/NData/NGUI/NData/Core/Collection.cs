using System.Collections.Generic;
using System.Linq;

namespace EZData
{
    public delegate void ItemInsertDelegate(int position, Context insertedItem);
    public delegate void ItemRemoveDelegate(int position);
    public delegate void ItemsClearDelegate();
    public delegate void SelectionChangeDelegate();
    public delegate void ItemsReorderDelegate(int oldPosition, int newPosition);

    public abstract class Collection : Context
    {
		public event ItemInsertDelegate OnItemInsert;
        public event ItemRemoveDelegate OnItemRemove;
        public event ItemsClearDelegate OnItemsClear;
        public event SelectionChangeDelegate OnSelectionChange;
        public event ItemsReorderDelegate OnItemsReorder;
        
        protected void InvokeOnItemInsert(int position, Context insertedItem)
        {
            if (OnItemInsert != null)
                OnItemInsert(position, insertedItem);
        }

        protected void InvokeOnItemRemove(int position)
        {
            if (OnItemRemove != null)
                OnItemRemove(position);
        }

        protected void InvokeOnItemsClear()
        {
            if (OnItemsClear != null)
                OnItemsClear();
        }

        protected void InvokeOnSelectionChange()
        {
            if (OnSelectionChange != null)
                OnSelectionChange();
        }

        protected void InvokeOnItemsReorder(int from, int to)
        {
            if (OnItemsReorder != null)
                OnItemsReorder(from, to);
        }

        public abstract int ItemsCount { get; protected set; }

        public abstract int SelectedIndex { get; set; }

        public abstract bool FirstItemSelected { get; protected set; }

        public abstract bool LastItemSelected { get; protected set; }

        public abstract Context GetBaseItem(int index);

        public abstract void SelectItem(int index);

        public abstract void MoveItem(int from, int to);

        public abstract void Remove(int index);

        public abstract IEnumerable<Context> BaseItems { get; }

        public abstract VariableContext GetItemPlaceholder(int collectionItemIndex);
    }


    public class Collection<T> : Collection
        where T : Context
    {
        private readonly bool _autoSelect;
		
		public Collection() : this(false) { }
		public Collection(bool autoSelect)
		{
			_autoSelect = autoSelect;
			SelectedIndex = -1;
		}

        public override Context GetBaseItem(int index)
        {
#if UNITY_FLASH
			return ((object)GetItem(index)) as Context; // forcing explicit cast in AS3
#else
			return GetItem(index);
#endif
        }

        public T GetItem(int index)
        {
            if (index < 0 || index >= _items.Count)
                return null;

            return _items[index];
        }
        
        public override void SelectItem(int index)
        {
            if (index < 0 || index >= _items.Count)
                SetSelectedItem(-1, null);
            else
                SetSelectedItem(index, _items[index]);
        }

        public override void MoveItem(int from, int to)
        {
            if (from < 0 || from >= _items.Count || to < 0 || to >= _items.Count)
                return;
            
            if (from == to)
                return;
            
            var temp = GetItem(from);
            _items.RemoveAt(from);
            _items.Insert(to, temp);
            UpdateAuxProperties();

            InvokeOnItemsReorder(from, to);
        }

        public override IEnumerable<Context> BaseItems
        {
            get
			{
				var baseItems = new List<Context>();
				foreach(var i in _items)
				{
					baseItems.Add(i);
				}
				return baseItems;
			}
        }

        public IEnumerable<T> Items
        {
            get { return _items; }
        }


        public void Add(T item)
        {
            Insert(_items.Count, item);
        }

        public void Remove(T item)
        {
            for (var i = 0; i < _items.Count; i++)
            {
                if (_items[i] != item)
                    continue;

                if (SelectedItem == _items[i])
                    SelectItem(-1);

                Remove(i);
                break;
            }
        }
        
        public void Insert(int position, T item)
        {
            if (position < 0 || position >= _items.Count)
                position = _items.Count;

            // Do not change the order of actions here, if you think that you need inserted item to be in the
            // collection when you're inside of OnItemInsert, think again. Inserted item is available as
            // an argument of a notifiation, you can use that. And changing the order here for convenience
            // in your tool, may cause logical bugs in another tools that rely on this library.
            // General pattern with this notifications is based on two rules (that you can only rely on):
            //   1 - notification happens before collection is affected
            //   2 - affected item is somehow available inside the notification routine
            
            InvokeOnItemInsert(position, item);
            _items.Insert(position, item);
            UpdateAuxProperties();

            if (_autoSelect && _items.Count == 1)
                SelectItem(0);
        }

        public override void Remove(int index)
        {
            if (index < 0 || index >= _items.Count)
                return;

            // Do not change the order of actions here, if you think that item shouldn't already exist in
            // collection when you're inside of OnItemRemove, think again. Removed item may be still needed there.
            // And changing the order here for convenience in your tool, may (and will) cause logical bugs in another
            // tools that rely on this library.
            // General pattern with this notifications is based on two rules (that you can only rely on):
            //   1 - notification happens before collection is affected
            //   2 - affected item is somehow available inside the notification routine
            
            InvokeOnItemRemove(index);
            _items.RemoveAt(index);
            UpdateAuxProperties();

            if (_autoSelect && _items.Count > 0)
            {
                SelectItem(System.Math.Max(0, System.Math.Min(SelectedIndex, _items.Count - 1)));
            }
            else
            {
                SelectItem(SelectedIndex);
            }
        }

        public void Clear()
        {
            // Do not change the order of actions here, if you think that collection should be already empty
            // when you're inside of OnItemsClear, think again. Changing the order here for convenience
            // in your tool, may cause logical bugs in another tools that rely on this library.
            // General pattern with this notifications is based on two rules (that you can only rely on):
            //   1 - notification happens before collection is affected
            //   2 - affected item is somehow available inside the notification routine
            
            InvokeOnItemsClear();
            SelectItem(-1);
            _items.Clear();
            UpdateAuxProperties();
        }

        public int Count { get { return ItemsCount; } }

        private readonly ReadonlyProperty<int> _selectedIndexProperty =
			new ReadonlyProperty<int>();
        public ReadonlyProperty<int> SelectedIndexProperty { get { return _selectedIndexProperty; } }
        public override int SelectedIndex
        {
            get
            {
                return SelectedIndexProperty.GetValue();
            }
            set
            {
                SelectedIndexProperty.InternalSetValue(value);
            }
        }
		
		#region SelectedItem
		public readonly EZData.VariableContext<T> SelectedItemEzVariableContext =
			new EZData.VariableContext<T>(null);
		public T SelectedItem
		{
		    get { return SelectedItemEzVariableContext.Value; }
		    set { SelectedItemEzVariableContext.Value = value; }
		}
		#endregion

        private readonly ReadonlyProperty<int> _itemsCountProperty =
			new ReadonlyProperty<int>();
        public ReadonlyProperty<int> ItemsCountProperty { get { return _itemsCountProperty; } }
        public override int ItemsCount
        {
            get
            {
                return ItemsCountProperty.GetValue();
            }
            protected set
            {
                ItemsCountProperty.InternalSetValue(value);
            }
        }

        private readonly ReadonlyProperty<bool> _hasItemsProperty =
			new ReadonlyProperty<bool>();
        public ReadonlyProperty<bool> HasItemsProperty { get { return _hasItemsProperty; } }
        public bool HasItems
        {
            get
            {
                return HasItemsProperty.GetValue();
            }
            set
            {
                HasItemsProperty.InternalSetValue(value);
            }
        }

        private readonly ReadonlyProperty<bool> _hasSelectionProperty =
			new ReadonlyProperty<bool>();
        public ReadonlyProperty<bool> HasSelectionProperty { get { return _hasSelectionProperty; } }
        public bool HasSelection
        {
            get
            {
                return HasSelectionProperty.GetValue();
            }
            set
            {
                HasSelectionProperty.InternalSetValue(value);
            }
        }

        private readonly ReadonlyProperty<bool> _firstItemSelectedProperty =
			new ReadonlyProperty<bool>();
        public ReadonlyProperty<bool> FirstItemSelectedProperty { get { return _firstItemSelectedProperty; } }
        public override bool FirstItemSelected
        {
            get
            {
                return FirstItemSelectedProperty.GetValue();
            }
            protected set
            {
                FirstItemSelectedProperty.InternalSetValue(value);
            }
        }

        private readonly ReadonlyProperty<bool> _lastItemSelectedProperty =
			new ReadonlyProperty<bool>();
        public ReadonlyProperty<bool> LastItemSelectedProperty { get { return _lastItemSelectedProperty; } }
        public override bool LastItemSelected
        {
            get
            {
                return LastItemSelectedProperty.GetValue();
            }
            protected set
            {
                LastItemSelectedProperty.InternalSetValue(value);
            }
        }

        protected override void AddBindingDependency(IBinding binding)
        {
            if (binding == null)
                return;

            if (_dependencies.Contains(binding))
                return;
            _dependencies.Add(binding);
        }

        private void SetSelectedItem(int index, T item)
        {
            SelectedIndex = index;
            SelectedItem = item;
            HasSelection = (index >= 0);
            FirstItemSelected = (index == 0) && (ItemsCount > 0);
            LastItemSelected = (index == ItemsCount - 1) && (ItemsCount > 0);

            var temp = new List<IBinding>(_dependencies);
            _dependencies.Clear();
            foreach (var binding in temp)
            {
               	try
				{
                	binding.OnContextChange();
				}
				catch(UnityEngine.MissingReferenceException)
				{
				}
            }

            InvokeOnSelectionChange();
        }

        private void UpdateAuxProperties()
        {
            ItemsCount = _items.Count;
            HasItems = _items.Count > 0;
            HasSelection = (SelectedIndex >= 0);
            FirstItemSelected = (SelectedIndex == 0) && (ItemsCount > 0);
            LastItemSelected = (SelectedIndex == ItemsCount - 1) && (ItemsCount > 0);

            UpdatePlaceholders();
        }

        private void UpdatePlaceholders()
        {
            foreach (var p in _indexedPlaceholders)
            {
                p.Value.Value = (p.Key >= _items.Count) ? null : _items[p.Key];
            }
        }

        public override VariableContext GetItemPlaceholder(int collectionItemIndex)
        {
            VariableContext<Context> placeholder;
            if (_indexedPlaceholders.TryGetValue(collectionItemIndex, out placeholder))
                return placeholder;

            placeholder = new VariableContext<Context>((collectionItemIndex >= _items.Count) ? null : _items[collectionItemIndex]);
            _indexedPlaceholders.Add(collectionItemIndex, placeholder);
            return placeholder;
        }

        private readonly Dictionary<int, VariableContext<Context>> _indexedPlaceholders = new Dictionary<int, VariableContext<Context>>();

        private readonly List<T> _items = new List<T>();
        private readonly List<IBinding> _dependencies = new List<IBinding>();
    }
}
