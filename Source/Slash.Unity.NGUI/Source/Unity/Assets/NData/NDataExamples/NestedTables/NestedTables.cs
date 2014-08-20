using UnityEngine;

public class NestedNode : EZData.Context
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
	
	#region Collection SubNodes
	private readonly EZData.Collection<NestedNode> _privateSubNodes = new EZData.Collection<NestedNode>(false);
	public EZData.Collection<NestedNode> SubNodes { get { return _privateSubNodes; } }
	#endregion
}

public class NestedTablesContext : EZData.Context
{
	#region Collection Nodes
	private readonly EZData.Collection<NestedNode> _privateNodes = new EZData.Collection<NestedNode>(false);
	public EZData.Collection<NestedNode> Nodes { get { return _privateNodes; } }
	#endregion
	
	public void Populate()
	{
		Nodes.Add(new NestedNode { Name = "Chapter I" });
		Nodes.Add(new NestedNode { Name = "Chapter II" });
		Nodes.Add(new NestedNode { Name = "Chapter III" });
		foreach(var n1 in Nodes.Items)
		{
			n1.SubNodes.Add(new NestedNode { Name = "Section A" });
			n1.SubNodes.Add(new NestedNode { Name = "Section B" });
			n1.SubNodes.Add(new NestedNode { Name = "Section C" });
			foreach(var n2 in n1.SubNodes.Items)
			{
				n2.SubNodes.Add(new NestedNode { Name = "Item 1" });
				n2.SubNodes.Add(new NestedNode { Name = "Item 2" });
				n2.SubNodes.Add(new NestedNode { Name = "Item 3" });
			}
		}
	}
}

public class NestedTables : MonoBehaviour
{
	public NguiRootContext View;
	public NestedTablesContext Context;
	
	void Awake()
	{
		Context = new NestedTablesContext();
		View.SetContext(Context);
	}
}
