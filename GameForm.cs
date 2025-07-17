namespace CSharpTetris;

using System;
using System.Windows.Forms;

public class GameForm : Form
{
    private Button testButton;
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
        this.BackColor = System.Drawing.Color.FromArgb(36, 32, 35);

        this.DoubleBuffered = true;

        this.KeyPreview = true;
        this.KeyDown += GameForm_KeyDown;

        scoreLabel = new Label();
        scoreLabel.Text = "Score: 0";
        scoreLabel.ForeColor = System.Drawing.Color.White;
        scoreLabel.Location = new Point(10, 10);
        scoreLabel.AutoSize = true;

        testButton = new Button
        {
            Text = "Reset",
            ForeColor = System.Drawing.Color.White,
            Size = new Size(80, 30),
            Location = new Point(100, 10)
        };

        testButton.Click += TestButton_Click;

        this.Controls.Add(testButton);
        this.Controls.Add(scoreLabel);

        gameTimer = new Timer();

        gameTimer.Interval = 500;
        gameTimer.Tick += GameLoop;
        gameTimer.Start();

    }

    private void GameLoop(object sender, EventArgs e)
    {
        if (currentShape != null && !gameover)
        {
            currentShape.MoveShape();

            bool result = OnCollision();
            GameData.Instance.Score = score;
            SaveManager.Save();
        }
        if (GameLogic.HasGameEnded(fallenShapes))
        {
            gameover = true;
            Console.WriteLine("Game Over");
            gameTimer.Stop();
            this.Hide();

            using (GameOverForm gameOverForm = new GameOverForm())
            {
                gameOverForm.ShowDialog();
            }

            this.Close();
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

    private bool OnCollision()
    {
        if (GameLogic.CheckCollisionGround(currentShape) || GameLogic.WouldCollide(currentShape, fallenShapes, rows))
        {
            Shape fallenShape = currentShape;
            Soundmanager.PlayFallenShape();
            fallenShapes.Add(fallenShape);
            GameData.Instance.fallenShapes.Add(fallenShape);
            SaveManager.Save();

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

        if (GameLogic.CheckCollisionGround(currentShape) || GameLogic.CheckCollision(currentShape, fallenShapes))
        {
            currentShape.X = oldX;
            currentShape.Y = oldY;

            if (e.KeyCode == Keys.S)
            {
                OnCollision();
            }
        }

        Invalidate();
    }
}
