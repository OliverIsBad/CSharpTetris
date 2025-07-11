using System;
namespace CSharpTetris;

public static class ShapeFactory
{
    public static Shape GenerateRandomShape(int x, int y)
    {
        var colors = new[] { Color.Red, Color.Blue,
                             Color.Green, Color.Yellow,
                             Color.Purple };

        Random rnd = new Random();
        int shapeNum = rnd.Next(1, 5);
        Color randomColor = colors[rnd.Next(colors.Length)];

        switch (shapeNum)
        {
            case 1:
                return new Shape(randomColor, Shapes.I, x, y);

            case 2:
                return new Shape(randomColor, Shapes.O, x, y);

            case 3:
                return new Shape(randomColor, Shapes.T, x, y);

            case 4:
                return new Shape(randomColor, Shapes.S, x, y);

            case 5:
                return new Shape(randomColor, Shapes.Z, x, y);

            case 6:
                return new Shape(randomColor, Shapes.J, x, y);

            case 7:
                return new Shape(randomColor, Shapes.L, x, y);

            default:
                return new Shape(randomColor, Shapes.L, x, y);
        }
    }
}
