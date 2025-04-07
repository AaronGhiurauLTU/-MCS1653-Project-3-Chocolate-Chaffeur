using Godot;
using System;

public partial class MoveableObject : TiledObject
{
	[Signal] public delegate void PushedEventHandler(MoveableObject obj);
	[Signal] public delegate void HitObstacleEventHandler(MoveableObject obj, Node2D body);

	protected void OnBodyEntered(Node2D body)
	{
		if (body == player)
		{
			EmitSignal(SignalName.Pushed, this);
		}
		else
		{
			GD.Print(body.Name);
			EmitSignal(SignalName.HitObstacle, this, body);
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = GetParent().GetNode<Player>("Player");
		tileMap = player.GetParent().GetNode<Node2D>("Static").GetNode<Node2D>("TileMaps").GetNode<TileMapLayer>("TileMap");

		gridPosition = GameManager.PositionToAtlasIndex(Position, tileMap);
		GameManager.objects[gridPosition.X, gridPosition.Y] = this;

		this.Pushed += player.ObjectPushed;
		this.HitObstacle += player.MoveableObjectHitObstacle;
	}
	
	public void ShiftPosition(Vector2I shift)
	{
		GameManager.objects[gridPosition.X, gridPosition.Y] = null;
		
		gridPosition += shift;
		GameManager.objects[gridPosition.X, gridPosition.Y] = this;
	}
}
