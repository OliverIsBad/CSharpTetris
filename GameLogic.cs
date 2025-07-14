using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace CSharpTetris;

public static class GameLogic
{
    public static void LineClear(List<Shape> shapes, int cols, Label label, ref int score)
    {
        // Collect all global block positions
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

        // Count blocks per row
        var rowCounts = allBlocks
            .GroupBy(b => b.y)
            .ToDictionary(g => g.Key, g => g.Count());

        // Identify full rows
        var fullRows = rowCounts
            .Where(kv => kv.Value >= cols)
            .Select(kv => kv.Key)
            .OrderBy(y => y)
            .ToList();

        if (fullRows.Count == 0)
            return ;

        // Remove blocks that are in the full rows
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

        // Move down blocks above the cleared rows
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

        // Final adjustment (if needed)
        DropBlocks(shapes, fullRows);

        // Remove any shapes that have no blocks left
        shapes.RemoveAll(s => s.ShapeStructure.Count == 0);

        Soundmanager.PlayLineClear();
        score++;
        label.Text = "Score: " + score;
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

                // deleted lines under the block
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
