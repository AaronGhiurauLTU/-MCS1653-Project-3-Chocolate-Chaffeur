using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export] private int tileSize = 160;
	[Export] private float Speed = 300.0f;
	[Export] private AnimatedSprite2D animatedSprite;
	[Export] Timer moveCooldown;
	private bool isMoving = false;
	private bool canMove = true;
	private Vector2 targetPosition, previousRawDirection, previousDirection;
	private void OnMoveCooldownTimeout()
	{
		canMove = true;
		moveCooldown.Stop();
	}
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		//GD.Print(canMove);
		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Vector2.Zero;
		float horizontal = Input.GetAxis("ui_left", "ui_right");
		float vertical = Input.GetAxis("ui_up", "ui_down");

		Vector2 rawDirection = new (horizontal, vertical);
		Vector2 directionDifference = rawDirection - previousRawDirection;
		if (directionDifference.Length() > 0)
			GD.Print(horizontal +  " " + directionDifference + " " + (horizontal != 0 && ((directionDifference.X == 0 && vertical == 0) || directionDifference.X != 0 )));

		if (horizontal != 0 && ((directionDifference.X == 0 && vertical == 0) || directionDifference.X != 0 ))
		{
			direction.X = horizontal;
		}
		else if (vertical != 0 && ((directionDifference.Y == 0 && horizontal == 0) || directionDifference.Y != 0 ))
		{
			direction.Y = vertical;
		}
		else if (rawDirection != Vector2.Zero)
		{
			direction = previousDirection;
		}

		previousRawDirection = rawDirection;
		previousDirection = direction;

		if (direction != Vector2.Zero && !isMoving && canMove)
		{
			targetPosition = Position + (direction * tileSize);
			velocity = direction * Speed;
			isMoving = true;
			canMove = false;

			if (direction.X > 0)
			{
				animatedSprite.FlipH = true;
			}
			else if (direction.X < 0)
			{
				animatedSprite.FlipH = false;
			}
		}
		else if (isMoving)
		{
			if ((Position - targetPosition).Length() < 7)
			{
				Position = targetPosition;
				velocity = Vector2.Zero;
				isMoving = false;
				moveCooldown.Start();
			}
		}
		else 
		{
			velocity = Vector2.Zero;
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
