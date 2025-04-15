using Godot;
using System;

public partial class MusicManager : AudioStreamPlayer
{
	public enum Song
	{
		Background1,
		Background2,
		Victory,
		Defeat
	}
	private static MusicManager instance;
	private static Song currentSong = Song.Background1;
	[Export] private AudioStream backgroundMusic1, backgroundMusic2, victoryMusic, defeatMusic;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		instance = this;
	}

	public static void SetBusVolume(string bus, float volume)
	{
		int busIndex = AudioServer.GetBusIndex(bus);
		AudioServer.SetBusVolumeDb(busIndex, volume);
	}

	public static void PlaySong(Song song)
	{
		if (currentSong == song)
			return;

		switch (song)
		{
			case Song.Background1:
				instance.Stream = instance.backgroundMusic1;
				break;
			case Song.Background2:
				instance.Stream = instance.backgroundMusic2;
				break;
			case Song.Victory:
				instance.Stream = instance.victoryMusic;
				break;
			case Song.Defeat:
				instance.Stream = instance.defeatMusic;
				break;
		}
		
		currentSong = song;
		instance.Play();
	}
}
