using System;
using System.Collections;
using System.Drawing;
using System.Globalization;

namespace CSharpTetris;

public class Shape
{
    public Color ShapeColor { get; }
    public Shapes ShapeType { get; }
    public int X { get; set; }
    public int Y { get; set; }

    public List<(int x, int y)> ShapeStructure { get; set; }

    public int Rotation { get; set; }

    public Shape(Color shapeColor, Shapes shapeType, int x, int y, int rotation = 0)
    {
        ShapeColor = shapeColor;
        ShapeType = shapeType;

        X = x;
        Y = y;

        Rotation = rotation;

        switch (shapeType)
        {
            case Shapes.I:
                ShapeStructure = new List<(int x, int y)>
                { (0, 0), (1, 0), (2, 0), (3, 0) };
                break;

            case Shapes.J:
                ShapeStructure = new List<(int x, int y)>
                { (0, 0), (0, 1), (1, 1), (2, 1)};
                break;

            case Shapes.L:
                ShapeStructure = new List<(int x, int y)>
                { (0, 1), (1, 1), (2, 1), (2, 0)};
                break;

            case Shapes.O:
                ShapeStructure = new List<(int x, int y)>
                { (0, 0), (0, 1), (1, 1), (1, 0)};
                break;

            case Shapes.S:
                ShapeStructure = new List<(int x, int y)>
                { (0, 1), (1, 1), (1, 0), (2, 0)};
                break;

            case Shapes.T:
                ShapeStructure = new List<(int x, int y)>
                { (0, 1), (1, 1), (1, 0), (2, 1)};
                break;

            case Shapes.Z:
                ShapeStructure = new List<(int x, int y)>
                { (0, 0), (1, 0), (1, 1), (2, 1)};
                break;
        }
    }

    public bool IsOutOfBounds(int bounds)
    {
        foreach (var (x, _) in ShapeStructure)
        {
            int absX = X + x;

            if (absX < 0 || absX >= bounds)
            {
                return true;
            }
        }
        return false;
    }

    public void RotateShape()
    {
        // New List with rotated Coordinates
        List<(int x, int y)> rotated = new List<(int x, int y)>();

        foreach (var (x, y) in ShapeStructure)
        {
            // 90 degrees turn around (0,0)
            rotated.Add((y, -x));
        }

        // Find smallest x and y values after turn
        int minX = rotated.Min(p => p.x);
        int minY = rotated.Min(p => p.y);

        // Move all points so that the shape is in the correct position
        for (int i = 0; i < rotated.Count; i++)
        {
            rotated[i] = (rotated[i].x - minX, rotated[i].y - minY);
        }

        ShapeStructure = rotated;
        Rotation = (Rotation + 90) % 360;
    }

    public void MoveShape()
    {
        Y++;
    }

}
