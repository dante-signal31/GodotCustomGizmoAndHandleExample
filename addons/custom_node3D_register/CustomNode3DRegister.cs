#if TOOLS
using Godot;

namespace GodotCustomGizmoAndHandleExample.addons.custom_node3D_register;

[Tool]
public partial class CustomNode3DRegister : EditorPlugin
{
	public override void _EnterTree()
	{
		// Initialization of the plugin goes here.
		var script =
			GD.Load<Script>("res://nodes/CustomNode3D/CustomNode3D.cs");
		var texture = 
			GD.Load<Texture2D>("res://nodes/CustomNode3D/icon.svg");
		AddCustomType("CustomNode3D", "Node3D", script, texture);
	}

	public override void _ExitTree()
	{
		// Clean-up of the plugin goes here.
		RemoveCustomType("CustomNode3D");
	}
}
#endif
