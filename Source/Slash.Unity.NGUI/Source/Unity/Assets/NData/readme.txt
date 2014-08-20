NData boosts productivity and quality of NGUI based User Interface development in Unity3D with

Automatic data binding, following the popular MVVM pattern.
With NData you can build large scale, high quality user interfaces that are still stable, maintainable and robust.
More details on: http://tools.artofbytes.com/


Features

Two-way text bindings (including multi-bindings) with native .NET formatting capabilities.
Flexible bindings that allow visible and checked states of UI controls to be bound to properties in your code (also in two-way mode for check-boxes bound to boolean values).
Command bindings for buttons and text fields interaction that will trigger actions in your code.
Items source binding for connecting list control with attached item template prefab to collection of items in your code.
Code snippets for notifiable properties and collections for Visual Studio and Mono Develop.
Editor script for bindings validation.



Overview

MVVM (Model-View-ViewModel) pattern is one of the popular ways of separating UI from application logic. When powered by reflection based data bindings it allows writing an extremely clean code in both View, which is in case EZGUI controls hierarchy, and ViewModel which is basically an hierarchy of simple objects containing simple properties (like Boolean, int, string and Texture2D values) and collections of other simple objects. Simplicity of this objects means that they ideally don't have any logic and serve only like an hierarchical representation of user interface state. To enable interaction of user interface with application/game logic, objects in ViewModel can contain methods called commands, which are as simple as can be imagined -  void returning public methods without parameters. They don't have to be bound explicitly to UI controls like it used to be with regular events handlers. But rather this commands are invoked through command bindings. When command is invoked in order to perform some actions in Model (loading next game level, submitting score to the server, etc.), ViewModel invokes Model methods. If Model requires some data from UI as an arguments (like player name for score submission), this data is just taken from ViewModel, because it's already there thanks to the automatic data synchronization with two-way data bindings. When the action is finished, Model makes changes in ViewModel by changing appropriate properties. This changes made in a single property get automatically notified to all UI elements that rely on this property value.



NGUI 2 support

By default NData is configured to be used with NGUI 3.0.0 or higher, if you want to use it with NGUI 2.7.0, open the file Assets/smcs.rsp (or create an empty one if it's not there) and add the line:
-define:NGUI_2

Same line has to be added in the same way to the Assets/gmcs.rsp file.



Release notes


Version 1.0.19
==============
	NGUI 3 usage by default
	Examples upgrade to NGUI 3
	NGUI 3.0.8 f2 support


Version 1.0.18
==============
	Sprite binding compatibility fix
	
Projected binding improvement


Version 1.0.17
==============
	NguiFillAmount binding fix
	NGUI 3 support


Version 1.0.16
==============
	AOT compilation hot-fix


Version 1.0.15
==============
	MonoBehaviourContext support
	Default context support in views
	Content reposition on items removal in ItemsSourceBinding
	Master path behavior fix on template paths resolution
	Texture loader example
	DateTimeText example


Version 1.0.14
==============
	Separate transform fields binding
	Transform fields variant binding
	Stretch outside option support in texture binding
	More numeric sources supported in text bindings
	MasterPath support for prefixing binding paths
	Base sprite supported in fill amount binding

	Base binding class for common binding and multi-binding functionality
	Bind / Unbind sequence re-factoring for more straightforward bindings customization
	Numeric and text sources access re-factoring
	More universal recursive templates workaround
	
	Collection initial selected index state fix
	Selection load on bind in items source binding

	Inventory example
	Scale slider example
	Transform variant example
	Projected position example
	Health bar example
	MasterPath example
	VisibilityCheckboxes example
	Items list example


Version 1.0.13
==============
	Nested tables example
	Self-referenced item templates support
	Better context type detection in scene validation tool
	More correct tables repositioning
	Flash compatibility errors and warnings fixes
	Nested visibility bindings fix


Version 1.0.12
==============
New: 
	Persistent dependency properties (stored in PlayerPrefs)
	ProjectedPositionBinding to position UI elements over the objects in world space
	OnDropBinding
	Abstract numeric multi-binding
	Fade visibility binding
	NumericComparisonVisibilityBinding
	Persistent volume example
	Variable contexts example
	Staying alive example game

Improvements:
	Better non-template ItemsSourceBinding behavior (predefined items are not added/removed)
	Flexible format-string based sprite names support in SpriteBinding
	Localization support for format strings in bindings
	Binding to components, transforms and game objects support in Vector-based bindings
	Visibility control re-factored for custom visibility bindings support
	Multiple visibility bindings in single object support

Fixes:
	Demo font prefabs fix
	Texture binding zero scale fix
	Local scale binding GetValue fix
	Root bindings context access fix
	Unbind triggering on UI game object destroy
	Sample code cleanup
	Numeric binding warning fix


Version 1.0.11
==============
	Unity 3.x support restored


Version 1.0.10
==============
    Predefined list items support in item source bindings (to make list with predefined number of fully custom items, without item template)
    Vector2 and Texture2D properties supported in vector bindings in addition to Vector3
    Object references added in scene validation tool messages for quick navigation to errors
    Automatic sprites re-size to pixel perfect state in sprite binding
    More alignments supported in texture binding
    Some basic math transforms supported in position and scale bindings
    Localization key binding
    Context scope modifiers supported in bindings’ paths
    Audible binding distribution


Version 1.0.9
=============
    Core functionality source code is included to distribution


Version 1.0.8
=============
    UIGrid supported in items source binding


Version 1.0.7
=============
    LastItemSelected, FirstItemSelected properties support in collections
    AOT compilation issues fix
    Physics colliders affected by visibility binding
    Overridable sprite name creation in sprite binding
    Overridable value applier in TextBinding
    Initial UITable items’ visibility fix


Version 1.0.6
=============
    Quick start script cleanup
    Pre-filled collections supported in item source bindings


Version 1.0.5
=============
    Quick start utility
    Numeric bindings base classes
    Fill amount binding
    Slight slider binding re—factoring
    Base polling bindings classes for easier custom bindings creation
    Base Vector3 binding class
    Local scale, position and angles bindings


Version 1.0.4
=============
    IEnumerable support for collections to allow foreach for items
    Binding to collection items by index (allowing binding paths like: HighScores.Table.0.Name)
    Slight context access level modification for possible iOS issues
    Better handling of unresolved bindings
    Indexed bindings validation
    Sprite binding support
    Minor code cleanup
