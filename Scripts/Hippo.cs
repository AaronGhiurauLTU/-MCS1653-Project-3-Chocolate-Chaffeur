using Godot;
using System;

public partial class Hippo : TiledObject
{
	private bool closed = false;
	public bool Closed { get { return closed; } }
	public void Close()
	{
		Play("close");
		closed = true;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
