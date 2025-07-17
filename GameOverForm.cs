using System;

namespace CSharpTetris;

public class GameOverForm : Form
{
    public GameOverForm()
    {
        this.Text = "currently Empty";
        this.Size = new System.Drawing.Size(400, 300);

        Label gameOverTextLabel = new Label();
        gameOverTextLabel.Text = "Game Over";
        gameOverTextLabel.Location = new System.Drawing.Point(80, 80);

        Button restartButton = new Button()
        {
            Text = "Restart",
            Size = new System.Drawing.Size(100, 40),
            Location = new System.Drawing.Point(100, 80)
        };

        restartButton.Click += RestartButton_Click;
        this.Controls.Add(restartButton);
    }

    private void RestartButton_Click(object sender, EventArgs e)
    {
        Application.Restart();
        Environment.Exit(0);
    }
}
