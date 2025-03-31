using Godot;
using System;

public partial class Chocolate : AnimatedSprite2D
{
	[Export] private Player player;
	[Signal] public delegate void ChocolatePushedEventHandler();
	[Signal] public delegate void ChocolateHitObstacleEventHandler();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
	private void OnBodyEntered(Node2D body)
	{
		if (body == player)
		{
			EmitSignal(SignalName.ChocolatePushed);
		}
		else
		{
			GD.Print(body.Name);
			EmitSignal(SignalName.ChocolateHitObstacle);
		}

		// GetParent().RemoveChild(this);

		// body.AddChild(this);
		// GD.Print("chocolate touched");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
