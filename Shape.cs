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

    public Shape(Color shapeColor, Shapes shapeType, int x, int y)
    {
        ShapeColor = shapeColor;
        ShapeType = shapeType;

        X = x;
        Y = y;

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

    public void MoveShape()
    {
        Y++;
    }

}
