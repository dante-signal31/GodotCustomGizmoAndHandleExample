using Godot;

namespace GodotCustomGizmoAndHandleExample.nodes.CustomNode3D;

// It must be tool to make _HasGizmo comparison work at CustomNode3DGizmo.cs.
// https://github.com/godotengine/godot/issues/82869
[Tool]
public partial class CustomNode3D : Node3D
{
    [ExportCategory("CONFIGURATION:")] 
    private Vector3 _nodeMainPoint;

    [Export] public Vector3 NodeMainPoint
    {
        get => _nodeMainPoint;
        set
        {
            if (_nodeMainPoint != value)
            {
                _nodeMainPoint = value;
                UpdateGizmos();
            }
        }
    }
    
    private Vector3 _nodeSecondaryPoint;
    [Export] public Vector3 NodeSecondaryPoint
    {
        get => _nodeSecondaryPoint;
        set
        {
            if (_nodeSecondaryPoint != value)
            {
                _nodeSecondaryPoint = value;
                UpdateGizmos();
            }
        }
    }
}