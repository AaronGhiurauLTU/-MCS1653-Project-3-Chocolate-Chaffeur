using Godot;
using System;

public partial class TiledObject : AnimatedSprite2D
{
	protected TileMapLayer tileMap;
	protected Player player;

	protected Vector2I gridPosition;
	public Vector2I GridPosition { get { return gridPosition; } }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = GetParent().GetNode<Player>("Player");
		tileMap = player.GetParent().GetNode<Node2D>("Static").GetNode<Node2D>("TileMaps").GetNode<TileMapLayer>("TileMap");

		gridPosition = GameManager.PositionToAtlasIndex(Position, tileMap);
		GameManager.objects[gridPosition.X, gridPosition.Y] = this;
	}

	public void Destroy()
	{
		GameManager.objects[gridPosition.X, gridPosition.Y] = null;
		QueueFree();
	}
}
