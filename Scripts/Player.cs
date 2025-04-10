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
		pushingObject = false,
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
	public void ObjectPushed(MoveableObject obj)
	{
		if (!isMoving)
			return;

		CallDeferred("ConnectMoveableObject", obj);
	}

	private void ConnectMoveableObject(MoveableObject moveableObject)
	{
		moveableObject.GetParent().RemoveChild(moveableObject);
		AddChild(moveableObject);
		moveableObject.Position = Velocity.Normalized() * tileSize;
		pushingObject = true;
		movingObject = moveableObject;
		moveableObject.isMoving = true;
	}

	public void MoveableObjectHitObstacle(MoveableObject obj, Node2D body)
	{
		if (!isMoving || !pushingObject)
			return;

		Position = originalPosition;
		StopMoving();
	}

	private void ReloadLevel()
	{
		GameManager.ReloadLevel();
	}
	
	public bool TiledObjectCollidedWithPlayer(TiledObject obj)
	{
		if (obj is Hippo hippo && !hippo.Closed)
		{
			CallDeferred("ReloadLevel");
			Engine.TimeScale = 0;
			return true;
		}

		return false;
	}

	public bool TiledObjectCollidedWithObject(TiledObject obj, TiledObject body)
	{
		if (obj is Hippo hippo && !hippo.Closed)
		{
			if (body is Melon melon)
			{
				hippo.Close();
			}

			body.Destroy();

			return true;
		}

		return false;
	}

	private void StopMoving()
	{
		Velocity = Vector2.Zero;
		isMoving = false;

		Vector2I tilePos = GameManager.PositionToAtlasIndex(Position, tileMap);

		Node2D currentObject = GameManager.GetObject(tilePos);

		if (currentObject != null && currentObject is not MoveableObject && currentObject is TiledObject tiledObject)
		{
			if (TiledObjectCollidedWithPlayer(tiledObject))
				return;
		}

		CallDeferred("DisconnectMoveableObject");
		moveCooldown.Start();
	}
	private void DisconnectMoveableObject()
	{
		if (pushingObject)
		{
			RemoveChild(movingObject);
			GetParent().AddChild(movingObject);
			Vector2I shift = (Vector2I)movingObject.Position.Normalized();

			movingObject.Position += Position;
			pushingObject = false;
			movingObject.isMoving = false;
			
			Vector2I tilePos = GameManager.PositionToAtlasIndex(movingObject.Position, tileMap);

			Node2D currentObject = GameManager.GetObject(tilePos);

			if (currentObject != null && currentObject is not MoveableObject && currentObject is TiledObject tiledObject)
			{
				if (TiledObjectCollidedWithObject(tiledObject, movingObject))
					return;
			}

			movingObject.ShiftPosition(shift);

			if (movingObject is Chocolate && GameManager.GetTileName(movingObject.GridPosition, tileMap) == "pigeon")
			{
				GD.Print("game won");
				Engine.TimeScale = 0;
				return;
			}
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		if (Input.IsActionPressed("reset"))
		{
			ReloadLevel();
			return;
		}

		if (Engine.TimeScale == 0)
			return;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Vector2.Zero;
		float horizontal = Input.GetAxis("left", "right");
		float vertical = Input.GetAxis("up", "down");

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


			if ((tileName != null && tileName == "wall") || (currentObject != null && currentObject is MoveableObject 
				&& ((nextTileName != null && nextTileName == "wall") || (nextObject != null && nextObject is MoveableObject))))
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

				if (Engine.TimeScale == 0)
					return;
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
