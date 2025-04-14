using Godot;
using System;

public partial class CameraEffects : Camera2D
{
	//The starting range of possible offsets using random values
	[Export] private float RANDOM_SHAKE_STRENGTH = 30.0f,
	//Multiplier for lerping the shake strength to zero
		SHAKE_DECAY_RATE = 5.0f,
		shake_strength = 0.0f;

	[Export] private RandomNumberGenerator rand;

	public override void _Ready()
	{
		base._Ready();
		rand = new RandomNumberGenerator();
		rand.Randomize();
	}

	public void apply_shake(float strength)
	{
		shake_strength = strength;
	}
	//https://forum.godotengine.org/t/how-to-use-lerp-functions-in-c/15038
	float Lerp(float firstFloat, float secondFloat, float by)
	{
		return firstFloat * (1 - by) + secondFloat * by;
	}

	public override void _Process(double delta)
	{
		//Fade out the intensity over time
		shake_strength = Lerp(shake_strength, 0, (float)(SHAKE_DECAY_RATE * delta));

		//Shake by adjusting camera.offset so we can move the camera around the level via it's position
		this.Offset = get_random_offset();

	}

	private Vector2 get_random_offset()
	{
		return new Vector2(
			rand.RandfRange(-shake_strength, shake_strength),
			rand.RandfRange(-shake_strength, shake_strength)
		);
	}
}