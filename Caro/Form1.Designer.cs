using System.Drawing;
using System.Windows.Forms;

namespace Caro
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        private const int BOARD_SIZE = 3;
        private const int CELL_SIZE = 100;
        private Button[,] cells;
        private char[,] board;
        private char currentPlayer;
        private Label statusLabel;
        private Button restartButton;
        private ComboBox difficultyComboBox;
        private Label difficultyLabel;
        private int aiDepth = 10; // Mặc định khó nhất
        private bool gameOver = false;

      
        private void InitializeComponent()
        {
            this.Text = "Trò chơi Caro 3x3";
            this.Size = new Size(BOARD_SIZE * CELL_SIZE + 120, BOARD_SIZE * CELL_SIZE + 150);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Label hiển thị trạng thái
            statusLabel = new Label
            {
                Location = new Point(30, BOARD_SIZE * CELL_SIZE + 20),
                Size = new Size(BOARD_SIZE * CELL_SIZE, 30),
                Font = new Font("Arial", 12, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Text = "Lượt của X"
            };
            this.Controls.Add(statusLabel);

            // Nút khởi động lại
            restartButton = new Button
            {
                Location = new Point(BOARD_SIZE * CELL_SIZE / 2 - 50 + 30, BOARD_SIZE * CELL_SIZE + 60),
                Size = new Size(100, 30),
                Text = "Chơi lại",
                BackColor = Color.LightBlue
            };
            restartButton.Click += RestartButton_Click;
            this.Controls.Add(restartButton);

            // Combobox chọn độ khó
            difficultyLabel = new Label
            {
                Location = new Point(30, 10),
                Size = new Size(100, 20),
                Text = "Độ khó:"
            };
            this.Controls.Add(difficultyLabel);

            difficultyComboBox = new ComboBox
            {
                Location = new Point(100, 7),
                Size = new Size(150, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            difficultyComboBox.Items.Add("Dễ");
            difficultyComboBox.Items.Add("Trung bình");
            difficultyComboBox.Items.Add("Khó");
            difficultyComboBox.SelectedIndex = 2; // Mặc định khó nhất
            difficultyComboBox.SelectedIndexChanged += DifficultyComboBox_SelectedIndexChanged;
            this.Controls.Add(difficultyComboBox);
        }

    }
}

