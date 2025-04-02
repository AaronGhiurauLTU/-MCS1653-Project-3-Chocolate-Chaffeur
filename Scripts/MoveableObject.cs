using Godot;
using System;

public partial class MoveableObject : Node2D
{
	[Export] protected TileMapLayer tileMap;
	[Export] private Player player;

	[Signal] public delegate void PushedEventHandler(MoveableObject obj);
	[Signal] public delegate void HitObstacleEventHandler(MoveableObject obj, Node2D body);
	protected Vector2I gridPosition;
	public Vector2I GridPosition { get { return gridPosition; } }
	private void OnBodyEntered(Node2D body)
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
		gridPosition = GameManager.PositionToAtlasIndex(Position, tileMap);
		GameManager.objects[gridPosition.X, gridPosition.Y] = this;

		this.Pushed += player.ChocolatePushed;
		this.HitObstacle += player.ChocolateHitObstacle;
	}
	
	public void ShiftPosition(Vector2I shift)
	{
		GameManager.objects[gridPosition.X, gridPosition.Y] = null;
		
		gridPosition += shift;
		GameManager.objects[gridPosition.X, gridPosition.Y] = this;
	}
}
