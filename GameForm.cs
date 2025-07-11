namespace CSharpTetris;

using System;
using System.Windows.Forms;

public class GameForm : Form
{
    private Button button;
     
    public GameForm()
    {
        this.Text = "Tetris";
        this.Size = new System.Drawing.Size(335, 520);

        this.DoubleBuffered = true;
    }
protected override void OnPaint(PaintEventArgs e)
{
    base.OnPaint(e);

    Graphics g = e.Graphics;
    Pen gridPen = new Pen(Color.Gray, 1);

    int blockSize = 20;
    int rows = 20;
    int cols = 10;
    int offsetX = 50;
    int offsetY = 50;

    // Vertikale Linien
    for (int x = 0; x <= cols; x++)
    {
        int xPos = offsetX + x * blockSize;
        g.DrawLine(gridPen, xPos, offsetY, xPos, offsetY + rows * blockSize);
    }

    // Horizontale Linien
    for (int y = 0; y <= rows; y++)
    {
        int yPos = offsetY + y * blockSize;
        g.DrawLine(gridPen, offsetX, yPos, offsetX + cols * blockSize, yPos);
    }

    gridPen.Dispose();
}
}
