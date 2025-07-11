namespace CSharpTetris;

using System;
using System.Windows.Forms;

public class GameForm : Form
{
    private Button button;
    private const int blockSize = 20;
    private const int rows = 20;
    private const int cols = 10;
    private const int offsetX = 50;
    private const int offsetY = 50;

    private Shape currentShape;

    public GameForm()
    {
        this.Text = "Tetris";
        this.Size = new System.Drawing.Size(335, 520);

        this.DoubleBuffered = true;
    }

    private void DrawShape(Graphics g, Shape shape)
    {
        using SolidBrush brush = new SolidBrush(shape.ShapeColor);

        foreach (var (x, y) in shape.ShapeStructure)
        {
            int drawX = offsetX + (shape.X + x) * blockSize;
            int drawY = offsetY + (shape.Y + y) * blockSize;

            g.FillRectangle(brush, drawX, drawY, blockSize, blockSize);
            g.DrawRectangle(Pens.Black, drawX, drawY, blockSize, blockSize);
        }
    }
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        Graphics g = e.Graphics;
        Pen gridPen = new Pen(Color.Gray, 1);



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

        Button testButton = new Button
        {
            Text = "Reset",
            Size = new Size(80, 30),
            Location = new Point(100, 10)
        };

        testButton.Click += TestButton_Click;

        this.Controls.Add(testButton);
   
        gridPen.Dispose();
    }

    private void TestButton_Click(object sender, EventArgs e)
    {
        
        MessageBox.Show("Button wurde geklickt!");
        currentShape = ShapeFactory.GenerateRandomShape(0, 0);
        using Graphics g = this.CreateGraphics();

        g.Clear(this.BackColor);

        DrawShape(g, currentShape);

    }
}
