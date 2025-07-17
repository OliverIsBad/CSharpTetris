using System;
using System.Text.Json;

namespace CSharpTetris;

public static class SaveManager
{
    private static string savePath = "gameSave.json";

    public static void Save()
    {
        string json = JsonSerializer.Serialize(GameData.Instance);
        File.WriteAllText(savePath, json);
    }

    public static void Load()
    {
        if (!File.Exists(savePath)) return;
        string json = File.ReadAllText(savePath);
        GameData loadedData = JsonSerializer.Deserialize<GameData>(json)!;
        GameData.Load(loadedData);

    }
}
