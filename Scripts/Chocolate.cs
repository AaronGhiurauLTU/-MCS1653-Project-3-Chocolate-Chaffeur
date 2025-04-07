using Godot;
using System;

public partial class Chocolate : MoveableObject
{
	[Export] private float meltSpeed = 0.03f,
		coolSpeed = 0.03f;
	private float color = 1;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
	}
	private void ReloadLevel()
	{
		GameManager.ReloadLevel();
	}
	public override void Destroy()
	{
		base.Destroy();
		CallDeferred("ReloadLevel");
		Engine.TimeScale = 0;
	}
	public override void _Process(double delta)
	{
		string currentTile = GameManager.GetTileName(GridPosition, tileMap);

		if (!isMoving)
		{
			if (currentTile == "heater")
			{
				color = Mathf.MoveToward(color, 0, meltSpeed);
			}
			else
			{
				color = Mathf.MoveToward(color, 1, coolSpeed);
			}
			
			Modulate = new Color(color, color, color);

			if (color <= 0)
			{
				Destroy();
			}
		}
	}

}
