using Godot;
using System;

public partial class GameManager : Node2D
{
	public static Node2D[,] objects = new Node2D[14, 9];

	public static Vector2I PositionToAtlasIndex(Vector2 position, TileMapLayer tileMap)
	{
		return tileMap.LocalToMap(tileMap.ToLocal(position));
	}
	
	public static Node2D GetObject(Vector2I tilePos)
	{
		if (tilePos.X < 0 || tilePos.X > objects.GetUpperBound(0) || tilePos.Y < 0 || tilePos.Y > objects.GetUpperBound(1))
			return null;

		return objects[tilePos.X, tilePos.Y];
	}
	public static string GetTileName(Vector2 position, TileMapLayer tileMap)
	{
		Vector2I tilePos = PositionToAtlasIndex(position, tileMap);

		TileData tileData = tileMap.GetCellTileData(tilePos);

		string tileName = (string)tileData?.GetCustomData("Name");

		return tileName;
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
