using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export] private int tileSize = 160;
	[Export] private float Speed = 300.0f;
	[Export] private AnimatedSprite2D animatedSprite;
	[Export] private Timer moveCooldown;
	[Export] private TileMapLayer tileMap;

	private bool isMoving = false,
		pushingChocolate = false,
		canMove = true;
	private MoveableObject movingObject;
	private Vector2 originalPosition, targetPosition, previousRawDirection, previousDirection;
	public override void _Ready()
	{

	}
	private void OnMoveCooldownTimeout()
	{
		canMove = true;
		moveCooldown.Stop();
	}
	public void ChocolatePushed(MoveableObject obj)
	{
		if (!isMoving)
			return;

		CallDeferred("ConnectChocolate", obj);
	}

	private void ConnectChocolate(MoveableObject chocolate)
	{
		chocolate.GetParent().RemoveChild(chocolate);
		AddChild(chocolate);
		chocolate.Position = Velocity.Normalized() * tileSize;
		pushingChocolate = true;
		movingObject = chocolate;
	}

	public void ChocolateHitObstacle(MoveableObject obj, Node2D body)
	{
		if (!isMoving || !pushingChocolate)
			return;

		Position = originalPosition;
		StopMoving();
	}

	private void StopMoving()
	{
		Velocity = Vector2.Zero;
		isMoving = false;

		CallDeferred("DisconnectChocolate", movingObject);
		moveCooldown.Start();
	}
	private void DisconnectChocolate(MoveableObject chocolate)
	{
		if (pushingChocolate)
		{
			RemoveChild(chocolate);
			GetParent().AddChild(chocolate);
			chocolate.ShiftPosition((Vector2I)chocolate.Position.Normalized());
			chocolate.Position = Position + chocolate.Position;
			pushingChocolate = false;
			
			if (GameManager.GetTileName(chocolate.GridPosition, tileMap) == "pigeon")
			{
				GD.Print("game won");
			}
		}
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

		Vector2 rawDirection = new(horizontal, vertical);
		Vector2 directionDifference = rawDirection - previousRawDirection;

		if (horizontal != 0 && ((directionDifference.X == 0 && vertical == 0) || directionDifference.X != 0))
		{
			direction.X = horizontal;
		}
		else if (vertical != 0 && ((directionDifference.Y == 0 && horizontal == 0) || directionDifference.Y != 0))
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

			Vector2I tilePos = GameManager.PositionToAtlasIndex(targetPosition, tileMap);

			string tileName = GameManager.GetTileName(targetPosition, tileMap);


			Vector2I nextPos = tilePos + (Vector2I)direction;

			string nextTileName = GameManager.GetTileName(nextPos, tileMap);

			Node2D currentObject = GameManager.GetObject(tilePos);
			Node2D nextObject = GameManager.GetObject(nextPos);


			if ((tileName != null && tileName == "wall") || (currentObject != null && ((nextTileName != null && nextTileName == "wall") || nextObject != null)))
			{
				return;
			}

			originalPosition = Position;

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
				StopMoving();
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
