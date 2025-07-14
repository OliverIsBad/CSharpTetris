using System;
using System.Collections.Generic;
using System.IO;
using NAudio.Wave;

namespace CSharpTetris;

public static class Soundmanager
{
    // Liste zur Lebensverlängerung, verhindert sofortiges Dispose
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

        // Sobald Playback vorbei ist: Ressourcen freigeben
        outputDevice.PlaybackStopped += (s, e) =>
        {
            outputDevice.Dispose();
            audioFile.Dispose();
            activeSounds.RemoveAll(entry => entry.output == outputDevice);
        };

        // Hält Instanzen am Leben
        activeSounds.Add((outputDevice, audioFile));
    }

    public static void PlayGameStart()
    {
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        string soundPath = Path.Combine(baseDir, "assets", "sounds", "game-start.mp3");
        PlaySound(soundPath);
    }
}
