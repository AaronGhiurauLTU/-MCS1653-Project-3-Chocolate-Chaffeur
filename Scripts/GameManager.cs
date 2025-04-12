using Godot;
using System;

public partial class GameManager : Node
{
	[Export] private int level = 1;
	public static int currentLevel;
	public static GameManager instance;
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
	public static string GetTileName(Vector2I tilePos, TileMapLayer tileMap)
	{
		TileData tileData = tileMap.GetCellTileData(tilePos);

		string tileName = (string)tileData?.GetCustomData("Name");

		return tileName;
	}
	public static string GetTileName(Vector2 position, TileMapLayer tileMap)
	{
		Vector2I tilePos = PositionToAtlasIndex(position, tileMap);

		TileData tileData = tileMap.GetCellTileData(tilePos);

		string tileName = (string)tileData?.GetCustomData("Name");

		return tileName;
	}

	public static void LoadLevel(int level)
	{
		currentLevel = level;
		Engine.TimeScale = 1;
		objects = new Node2D[14, 9];
		switch (level)
		{
			case 1:
				instance.GetTree().ChangeSceneToFile("res://Scenes/level.tscn");
				break;
		}
	}

	public static void ReloadLevel()
	{
		LoadLevel(currentLevel);
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Engine.TimeScale = 1;
		currentLevel = level;
		instance = this;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
