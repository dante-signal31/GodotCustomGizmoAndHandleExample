using Godot;

namespace GodotCustomGizmoAndHandleExample.addons.custom_node3D_gizmo;

[Tool]
public partial class CustomNode3DGizmoRegister : EditorPlugin
{
    private CustomNode3DGizmo _gizmoPlugin;
    
    public override void _EnterTree()
    {
        base._EnterTree();
        _gizmoPlugin = (CustomNode3DGizmo) 
            GD.Load<CSharpScript>("res://addons/custom_node3D_gizmo/CustomNode3DGizmo.cs").New(GetUndoRedo());
        AddNode3DGizmoPlugin(_gizmoPlugin);
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        RemoveNode3DGizmoPlugin(_gizmoPlugin);
    }
}