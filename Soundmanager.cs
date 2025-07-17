using System;
using System.Collections.Generic;
using System.IO;
using NAudio.Wave;

namespace CSharpTetris;

public static class Soundmanager
{
    private static readonly List<(WaveOutEvent output, AudioFileReader reader)> activeSounds = new();

    public static void PlaySound(string path)
    {
        if (!File.Exists(path))
        {
            Console.WriteLine("File not found: " + path);
            return;
        }

        var audioFile = new AudioFileReader(path);
        var outputDevice = new WaveOutEvent();
        outputDevice.Init(audioFile);
        outputDevice.Play();

        outputDevice.PlaybackStopped += (s, e) =>
        {
            outputDevice.Dispose();
            audioFile.Dispose();
            activeSounds.RemoveAll(entry => entry.output == outputDevice);
        };

        activeSounds.Add((outputDevice, audioFile));
    }

    public static void PlayGameStart()
    {
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        string soundPath = Path.Combine(baseDir, "assets", "sounds", "game-start.mp3");
        PlaySound(soundPath);
    }

    public static void PlayBlockMove()
    {
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        string soundPath = Path.Combine(baseDir, "assets", "sounds", "block-move.wav");
        PlaySound(soundPath);
    }

    public static void PlayLineClear()
    {
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        string soundPath = Path.Combine(baseDir, "assets", "sounds", "line-clear.wav");
        PlaySound(soundPath);
    }

    public static void PlayGameOver()
    {
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        string soundPath = Path.Combine(baseDir, "assets", "sounds", "game-over.mp3");
    }

    public static void PlayFallenShape()
    {
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        string soundPath = Path.Combine(baseDir, "assets", "sounds", "fallen-block.wav");
    }
}
