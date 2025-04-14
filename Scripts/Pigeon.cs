using Godot;
using System;

public partial class Pigeon : Node2D
{
	[Export] private AnimationPlayer animationPlayer;
	private Control winMenu;

	public void Fly(Control winMenu)
	{
		this.winMenu = winMenu;
		GD.Print(this.winMenu);
		animationPlayer.Play("fly");
	}

	public void ShowWinMenu()
	{
		winMenu.Visible = true;
		Engine.TimeScale = 0;
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
