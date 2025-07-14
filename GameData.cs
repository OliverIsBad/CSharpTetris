using System;

namespace CSharpTetris;

public class GameData
{

    private static GameData? _instance;
    private static readonly object _lock = new();

    public static GameData Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new GameData();
                }
                return _instance;
            }
        }
        private set
        {
            _instance = value;
        }
    }

    public int Score { get; set; }
    public List<Shape> fallenShapes { get; set; }

    private GameData() { }

    public static void Load(GameData data)
    {
        Instance = data;
    }
}
