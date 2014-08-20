using UnityEngine;

public class ShopItem : EZData.Context
{
    #region Property Name
    private readonly EZData.Property<string> _privateNameProperty = new EZData.Property<string>();
    public EZData.Property<string> NameProperty { get { return _privateNameProperty; } }
    public string Name
    {
    get    { return NameProperty.GetValue();    }
    set    { NameProperty.SetValue(value); }
    }
    #endregion
    
    #region Property Price
    private readonly EZData.Property<int> _privatePriceProperty = new EZData.Property<int>();
    public EZData.Property<int> PriceProperty { get { return _privatePriceProperty; } }
    public int Price
    {
    get    { return PriceProperty.GetValue();    }
    set    { PriceProperty.SetValue(value); }
    }
    #endregion
}

public class ShopContext : EZData.Context
{
	public ShopItem StaticFeaturedItem { get; set; }
	
	#region DynamicFeaturedItem
	public readonly EZData.VariableContext<ShopItem> DynamicFeaturedItemEzVariableContext =
		new EZData.VariableContext<ShopItem>(null);
	public ShopItem DynamicFeaturedItem
	{
	    get { return DynamicFeaturedItemEzVariableContext.Value; }
	    set { DynamicFeaturedItemEzVariableContext.Value = value; }
	}
	#endregion
	
	private ShopItem [] _items;
    
    public ShopContext()
    {
        _items = new []
        {
            new ShopItem { Name = "Boots of Speed", Price = 450 },
            new ShopItem { Name = "Power Treads", Price = 1400 },
            new ShopItem { Name = "Phase Boots", Price = 1350 },
            new ShopItem { Name = "Tranquil Boots", Price = 975 },
            new ShopItem { Name = "Boots of Travel", Price = 2450 },
            new ShopItem { Name = "Arcane Boots", Price = 1450 },
        };
        
		StaticFeaturedItem = _items[0];
		DynamicFeaturedItem = _items[0];
	}
    
    public void FeatureRandomItem()
    {
		ShopItem randomItem = null;
		while (randomItem == null || randomItem == StaticFeaturedItem)
		{
			randomItem = _items[Random.Range(0, _items.Length - 1)];
		}
		
		// Both sub-contexts are assigned here,
		// but only dynamic one will trigger the UI update.
        StaticFeaturedItem = randomItem;
		DynamicFeaturedItem = randomItem;
    }
}

public class VariableContexts : MonoBehaviour
{
	public NguiRootContext View;
	public ShopContext Context;
	
	void Awake()
	{
		Context = new ShopContext();
		View.SetContext(Context);
	}
}
