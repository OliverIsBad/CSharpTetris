namespace CSharpTetris;

using System;
using System.Windows.Forms;

public class GameForm : Form
{
    private Button button;
    private Label scoreLabel;

    private const int blockSize = 20;
    private const int rows = 20;
    private const int cols = 10;
    private const int offsetX = 50;
    private const int offsetY = 50;

    private Timer gameTimer;

    private List<Shape> fallenShapes = new List<Shape>();
    private Shape currentShape;

    private bool gameover = false;

    private int score = 0;

    public GameForm()
    {
        this.Text = "Tetris";
        this.Size = new System.Drawing.Size(335, 520);

        this.DoubleBuffered = true;

        this.KeyPreview = true;
        this.KeyDown += GameForm_KeyDown;

        scoreLabel = new Label();
        scoreLabel.Text = "Score: 0";
        scoreLabel.Location = new Point(10, 10);
        scoreLabel.AutoSize = true;

        Button testButton = new Button
        {
            Text = "Reset",
            Size = new Size(80, 30),
            Location = new Point(100, 10)
        };

        testButton.Click += TestButton_Click;

        this.Controls.Add(testButton);
        this.Controls.Add(scoreLabel);

        gameTimer = new Timer();
        //gameTimer.Interval = 500;

        gameTimer.Interval = 500;
        gameTimer.Tick += GameLoop;
        gameTimer.Start();
    }

    private void GameLoop(object sender, EventArgs e)
    {
        if (currentShape != null && !gameover)
        {
            currentShape.MoveShape();
            //currentShape.RotateShape();

            bool result = OnCollision();

            Console.WriteLine("Collision: " + result);
        }
        if (GameLogic.HasGameEnded(fallenShapes))
        {
            gameover = true;
            Console.WriteLine("Game Over");
        }

        Invalidate();
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

    private void DisposeShape(Graphics g, Shape shape)
    {
        using SolidBrush brush = new SolidBrush(this.BackColor);

        foreach (var (x, y) in shape.ShapeStructure)
        {
            int drawX = offsetX + (shape.X + x) * blockSize;
            int drawY = offsetY + (shape.Y + y) * blockSize;

            g.FillRectangle(brush, drawX, drawY, blockSize, blockSize);
            g.DrawRectangle(Pens.Black, drawX, drawY, blockSize, blockSize);
        }
    }

    private bool CheckCollisionGround()
    {
        foreach (var (dx, dy) in currentShape.ShapeStructure)
        {
            int blockY = currentShape.Y + dy;

            if (blockY >= 19)
            {
                return true;
            }
        }

        return false;
    }

    private bool CheckCollision()
    {
        foreach ((int dx, int dy) in currentShape.ShapeStructure)
        {
            int absX = currentShape.X + dx;
            int absY = currentShape.Y + dy;

            foreach (Shape shape in fallenShapes)
            {
                foreach ((int sx, int sy) in shape.ShapeStructure)
                {
                    int absSX = shape.X + sx;
                    int absSY = shape.Y + sy;

                    if (absX == absSX && absY == absSY)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private bool WouldCollide()
    {
        foreach ((int dx, int dy) in currentShape.ShapeStructure)
        {
            int absX = currentShape.X + dx;
            int absY = currentShape.Y + dy + 1; // ← wir simulieren den nächsten Schritt

            // Kollision mit Boden
            if (absY >= rows)
            {
                return true;
            }

            foreach (Shape shape in fallenShapes)
            {
                foreach ((int sx, int sy) in shape.ShapeStructure)
                {
                    int absSX = shape.X + sx;
                    int absSY = shape.Y + sy;

                    if (absX == absSX && absY == absSY)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private bool OnCollision()
    {
        if (CheckCollisionGround() || WouldCollide())
        {
            Shape fallenShape = currentShape;
            fallenShapes.Add(fallenShape);

            GameLogic.LineClear(fallenShapes, cols, scoreLabel, ref score);

            SpawnNewShape();

            return true;
        }
        return false;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        Graphics g = e.Graphics;
        using Pen gridPen = new Pen(Color.Gray, 1);

        // Col lines
        for (int x = 0; x <= cols; x++)
        {
            int xPos = offsetX + x * blockSize;
            g.DrawLine(gridPen, xPos, offsetY, xPos, offsetY + rows * blockSize);
        }

        // Row lines
        for (int y = 0; y <= rows; y++)
        {
            int yPos = offsetY + y * blockSize;
            g.DrawLine(gridPen, offsetX, yPos, offsetX + cols * blockSize, yPos);
        }

        if (currentShape != null)
        {
            DrawShape(g, currentShape);
        }

        foreach (Shape shape in fallenShapes)
        {
            DrawShape(g, shape);
        }
    }

    private void SpawnNewShape()
    {
        int startPoint = 1;

        if (currentShape != null && currentShape.ShapeStructure.Count > 3)
        {
            startPoint = (currentShape.ShapeStructure[3].x == 3) ? 5 : 7;
        }

        Random rnd = new Random();
        int x = rnd.Next(1, startPoint);

        currentShape = ShapeFactory.GenerateRandomShape(x, 0);
        Invalidate();
    }

    private void TestButton_Click(object sender, EventArgs e)
    {
        Soundmanager.PlayGameStart();
        SpawnNewShape();
    }

    private void GameForm_KeyDown(object sender, KeyEventArgs e)
    {
        if (currentShape == null) return;

        int oldX = currentShape.X;
        int oldY = currentShape.Y;

        GameControl.HandleInput(e.KeyCode, currentShape);

        if (currentShape.IsOutOfBounds(cols))
        {
            currentShape.X = oldX;
            currentShape.Y = oldY;
        }

        Invalidate();
    }
}
