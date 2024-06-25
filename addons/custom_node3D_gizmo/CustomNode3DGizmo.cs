using Godot;
using GodotCustomGizmoAndHandleExample.nodes.CustomNode3D;

namespace GodotCustomGizmoAndHandleExample.addons.custom_node3D_gizmo;

public partial class CustomNode3DGizmo : EditorNode3DGizmoPlugin
{
    private const int MainPointHandleId = 0;
	private const int SecondaryPointHandleId = 1;

	private EditorUndoRedoManager _undoRedo;
	
	public CustomNode3DGizmo(EditorUndoRedoManager undoRedo)
	{
		CreateMaterial("NodeMainPointMaterial", new Color(1,0,0));
		CreateMaterial("NodeSecondaryPointMaterial", new Color(0, 1, 0));
		CreateHandleMaterial("HandleMainPointMaterial");
		CreateHandleMaterial("HandleSecondaryPointMaterial");
		var handleMainPointMaterial = GetMaterial("HandleMainPointMaterial");
		var handleSecondaryPointMaterial = GetMaterial("HandleSecondaryPointMaterial");
		handleMainPointMaterial.AlbedoColor = new Color(1, 0, 0);
		handleSecondaryPointMaterial.AlbedoColor = new Color(0, 1, 0);
		_undoRedo = undoRedo;
	}

	public override void _Redraw(EditorNode3DGizmo gizmo)
	{
		base._Redraw(gizmo);
		gizmo.Clear();
		var customNode3D = (CustomNode3D) gizmo.GetNode3D();
		
		// Draw NodeMainPoint.
		var lines = new[]
		{
			new Vector3(0, 0, 0),
			customNode3D.NodeMainPoint,
		};
		var handles = new[]
		{
			customNode3D.NodeMainPoint,
		};
		gizmo.AddLines(lines, GetMaterial("NodeMainPointMaterial", gizmo));
		gizmo.AddHandles(handles, 
			GetMaterial("HandleMainPointMaterial", gizmo),
			new[] {MainPointHandleId});
		
		// Draw NodeSecondaryPoint.
		var linesSecondary = new[]
		{
			new Vector3(0, 0, 0),
			customNode3D.NodeSecondaryPoint,
		};
		var handlesSecondary = new[]
		{
			customNode3D.NodeSecondaryPoint,
		};
		gizmo.AddLines(linesSecondary, 
			GetMaterial("NodeSecondaryPointMaterial", gizmo));
		gizmo.AddHandles(handlesSecondary, 
			GetMaterial("HandleSecondaryPointMaterial", gizmo),
			new[] {SecondaryPointHandleId});
}

	public override string _GetGizmoName()
	{
		return "CustomNode3D Gizmo";
	}

	public override bool _HasGizmo(Node3D forNode3D)
	{
		return forNode3D is CustomNode3D;
	}

	public override void _CommitHandle(EditorNode3DGizmo gizmo, int handleId, 
		bool secondary, Variant restore, bool cancel)
	{
		var customNode3D = (CustomNode3D) gizmo.GetNode3D();
		_undoRedo.CreateAction("Change CustomNode3D");
		switch (handleId)
		{
			case MainPointHandleId:
				_undoRedo.AddDoProperty(customNode3D, 
					CustomNode3D.PropertyName.NodeMainPoint, 
					customNode3D.NodeMainPoint);
				_undoRedo.AddUndoProperty(customNode3D, 
					CustomNode3D.PropertyName.NodeMainPoint, restore);
				break;
			case SecondaryPointHandleId:
				_undoRedo.AddDoProperty(customNode3D, 
					CustomNode3D.PropertyName.NodeSecondaryPoint, 
					customNode3D.NodeSecondaryPoint);
				_undoRedo.AddUndoProperty(customNode3D, 
					CustomNode3D.PropertyName.NodeSecondaryPoint, restore);
				break;
		}
		
		if (cancel)
		{
			switch (handleId)
			{
				case MainPointHandleId:
					customNode3D.NodeMainPoint = (Vector3) restore;
					break;
				case SecondaryPointHandleId:
					customNode3D.NodeSecondaryPoint = (Vector3) restore;
					break;
			}
		}
		_undoRedo.CommitAction();
		// customNode3D.UpdateGizmos();
	}

	public override string _GetHandleName(EditorNode3DGizmo gizmo, int handleId, 
		bool secondary)
	{
		switch (handleId)	
		{
			case MainPointHandleId:
				return nameof(CustomNode3D.NodeMainPoint);
			case SecondaryPointHandleId:
				return nameof(CustomNode3D.NodeSecondaryPoint);
			default:
				return "Unknown handle";
		}
	}

	public override Variant _GetHandleValue(EditorNode3DGizmo gizmo, int handleId, 
		bool secondary)
	{
		var customNode3D = (CustomNode3D) gizmo.GetNode3D();
		switch (handleId)
		{
			case MainPointHandleId:
				return customNode3D.NodeMainPoint;
			case SecondaryPointHandleId:
				return customNode3D.NodeSecondaryPoint;
			default:
				return Vector3.Zero;
		}
	}

	public override void _SetHandle(EditorNode3DGizmo gizmo, int handleId, 
		bool secondary, Camera3D camera, Vector2 screenPos)
	{
		var customNode3D = (CustomNode3D)gizmo.GetNode3D();
		switch (handleId)
		{ 
			case MainPointHandleId:
				customNode3D.NodeMainPoint = camera.ProjectPosition(screenPos, 
					GetZDepth(camera, customNode3D.GlobalPosition + 
					                  customNode3D.NodeMainPoint));
				break;
			case SecondaryPointHandleId:
				customNode3D.NodeSecondaryPoint = camera.ProjectPosition(screenPos, 
					GetZDepth(camera, customNode3D.GlobalPosition + 
					                  customNode3D.NodeSecondaryPoint));
				break;
		}
		// customNode3D.UpdateGizmos();
	}
	
	private float GetZDepth(Camera3D camera, Vector3 position)
	{
		Vector3 cameraPosition = camera.GlobalPosition;
		// Remember Camera3D looks towards its -Z local axis.
		Vector3 cameraForwardVector = -camera.GlobalTransform.Basis.Z;
		
		Vector3 vectorToPosition = position - cameraPosition;
		float zDepth = vectorToPosition.Dot(cameraForwardVector);
		return zDepth;
	}
}