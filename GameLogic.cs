using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace CSharpTetris;

public static class GameLogic
{
    public static void LineClear(List<Shape> shapes, int cols)
    {
        // Alle globalen Blockpositionen erfassen
        var allBlocks = new List<(int x, int y, Shape owner)>();

        foreach (var shape in shapes)
        {
            foreach (var (x, y) in shape.ShapeStructure)
            {
                int globalX = shape.X + x;
                int globalY = shape.Y + y;
                allBlocks.Add((globalX, globalY, shape));
            }
        }

        // Zeilen zählen
        var rowCounts = allBlocks
            .GroupBy(b => b.y)
            .ToDictionary(g => g.Key, g => g.Count());

        var fullRows = rowCounts
            .Where(kv => kv.Value >= cols)
            .Select(kv => kv.Key)
            .OrderBy(y => y)
            .ToList();

        if (fullRows.Count == 0)
            return;

        // Entferne Blöcke in den vollen Reihen
        foreach (var shape in shapes)
        {
            shape.ShapeStructure = shape.ShapeStructure
                .Where(p =>
                {
                    int absY = shape.Y + p.y;
                    return !fullRows.Contains(absY);
                })
                .ToList();
        }

        // Schiebe Blöcke oberhalb nach unten
        foreach (var shape in shapes)
        {
            var newStruct = new List<(int x, int y)>();

            foreach (var (x, y) in shape.ShapeStructure)
            {
                int absY = shape.Y + y;
                int shift = fullRows.Count(rowY => rowY < absY);
                newStruct.Add((x, y + shift));
            }

            shape.ShapeStructure = newStruct;
        }

        DropBlocks(shapes, fullRows);

        shapes.RemoveAll(s => s.ShapeStructure.Count == 0);
    }

    private static void DropBlocks(List<Shape> shapes, List<int> fullRows)
    {
        if (fullRows.Count == 0) return;

        fullRows.Sort();

        foreach (var shape in shapes)
        {
            var newStructure = new List<(int x, int y)>();

            foreach (var (x, y) in shape.ShapeStructure)
            {
                int absY = shape.Y + y;

                // Wie viele gelöschte Reihen liegen unterhalb dieses Blocks?
                int shift = fullRows.Count(row => row > absY);

                newStructure.Add((x, y + shift));
            }

            shape.ShapeStructure = newStructure;
        }
    }

    public static bool HasGameEnded(List<Shape> fallenShapes)
    {
        foreach (var shape in fallenShapes)
        {
            foreach (var (x, y) in shape.ShapeStructure)
            {
                int blockY = shape.Y + y;

                if (blockY <= 1)
                {
                    return true;
                }
            }
        }
        return false;
    }

}
