using System;
using System.Windows.Forms;

namespace CSharpTetris;

public static class GameControl
{
    public static void HandleInput(Keys key, Shape shape)
    {
        switch (key)
        {
            case Keys.A:
                shape.X--;
                break;

            case Keys.D:
                shape.X++;
                break;

            case Keys.S:
                shape.Y++;
                break;

            case Keys.W:
                shape.RotateShape();
                break;
        }
    }
}
