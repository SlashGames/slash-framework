using UnityEngine;
using System.Collections.Generic;
using ItemsListContexts;

namespace ItemsListContexts
{
	public class ClothingItem : EZData.Context
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
		
		#region Property Description
		private readonly EZData.Property<string> _privateDescriptionProperty = new EZData.Property<string>();
		public EZData.Property<string> DescriptionProperty { get { return _privateDescriptionProperty; } }
		public string Description
		{
		get    { return DescriptionProperty.GetValue();    }
		set    { DescriptionProperty.SetValue(value); }
		}
		#endregion
		
		#region Property Icon
		private readonly EZData.Property<string> _privateIconProperty = new EZData.Property<string>();
		public EZData.Property<string> IconProperty { get { return _privateIconProperty; } }
		public string Icon
		{
		get    { return IconProperty.GetValue();    }
		set    { IconProperty.SetValue(value); }
		}
		#endregion
		
		#region Property Strength
		private readonly EZData.Property<int> _privateStrengthProperty = new EZData.Property<int>();
		public EZData.Property<int> StrengthProperty { get { return _privateStrengthProperty; } }
		public int Strength
		{
		get    { return StrengthProperty.GetValue();    }
		set    { StrengthProperty.SetValue(value); }
		}
		#endregion

		#region Property Agility
		private readonly EZData.Property<int> _privateAgilityProperty = new EZData.Property<int>();
		public EZData.Property<int> AgilityProperty { get { return _privateAgilityProperty; } }
		public int Agility
		{
		get    { return AgilityProperty.GetValue();    }
		set    { AgilityProperty.SetValue(value); }
		}
		#endregion

		#region Property SpellPower
		private readonly EZData.Property<int> _privateSpellPowerProperty = new EZData.Property<int>();
		public EZData.Property<int> SpellPowerProperty { get { return _privateSpellPowerProperty; } }
		public int SpellPower
		{
		get    { return SpellPowerProperty.GetValue();    }
		set    { SpellPowerProperty.SetValue(value); }
		}
		#endregion
		
		#region Property MovementSpeed
		private readonly EZData.Property<int> _privateMovementSpeedProperty = new EZData.Property<int>();
		public EZData.Property<int> MovementSpeedProperty { get { return _privateMovementSpeedProperty; } }
		public int MovementSpeed
		{
		get    { return MovementSpeedProperty.GetValue();    }
		set    { MovementSpeedProperty.SetValue(value); }
		}
		#endregion

	}
	
	public class Character : EZData.Context
	{
		#region Collection Backpack
		private readonly EZData.Collection<ClothingItem> _privateBackpack =
			new EZData.Collection<ClothingItem>(true);
		public EZData.Collection<ClothingItem> Backpack { get { return _privateBackpack; } }
		#endregion
		
		public Character()
		{
			Load();
		}
		
		public void Reload()
		{
			Backpack.Clear();
			Load();
			Backpack.SelectItem(Random.Range(0, Backpack.ItemsCount));
		}
		
		public void Load()
		{
			Backpack.Add(new ClothingItem()
			{
				Name = "Boots of Haste",
				Description = "Increase movement speed, so your character can move faster.",
				Icon = "YellowButton",
				MovementSpeed = 15,
			});
			
			Backpack.Add(new ClothingItem()
			{
				Name = "Bracers of Strength",
				Description = "Increase strength, so your character attacks are dealing more damage.",
				Icon = "RedButton",
				Strength = 16,
			});
			
			Backpack.Add(new ClothingItem()
			{
				Name = "Bracers of Agility",
				Description = "Increase agility, so your character attacks are faster.",
				Icon = "GreenButton",
				Agility = 13,
			});
			
			Backpack.Add(new ClothingItem()
			{
				Name = "Shoulders of Spell Power",
				Description = "Increase spell power, so your character magic attacks are more effective.",
				Icon = "BlueButton",
				SpellPower = 14,
			});
		}
	}
}

public class ItemsList : MonoBehaviour
{
	public NguiRootContext View;
	public Character Context;
	
	void Awake()
	{
		Context = new Character();
		View.SetContext(Context);
	}
}
