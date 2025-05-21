using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Caro
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeGame();
        }
        private void DifficultyComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (difficultyComboBox.SelectedIndex)
            {
                case 0: 
                    aiDepth = 1;
                    break;
                case 1:
                    aiDepth = 3;
                    break;
                case 2: 
                    aiDepth = 10;
                    break;
            }
        }

        private void InitializeGame()
        {
            cells = new Button[BOARD_SIZE, BOARD_SIZE];
            board = new char[BOARD_SIZE, BOARD_SIZE];
            currentPlayer = 'X';
            gameOver = false;

            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    cells[i, j] = new Button
                    {
                        Location = new Point(j * CELL_SIZE + 30, i * CELL_SIZE + 40),
                        Size = new Size(CELL_SIZE, CELL_SIZE),
                        Font = new Font("Arial", 24, FontStyle.Bold),
                        Tag = new Point(i, j),
                        BackColor = Color.WhiteSmoke
                    };
                    cells[i, j].Click += Cell_Click;
                    this.Controls.Add(cells[i, j]);
                    board[i, j] = ' ';
                }
            }
            statusLabel.Text = "Lượt của X";
        }

        private void Cell_Click(object sender, EventArgs e)
        {
            if (gameOver) return;

            Button clickedButton = (Button)sender;
            Point position = (Point)clickedButton.Tag;
            int row = position.X;
            int col = position.Y;

            if (board[row, col] != ' ') return;

            board[row, col] = currentPlayer;
            clickedButton.Text = currentPlayer.ToString();
            clickedButton.Enabled = false;

            if (CheckWin(row, col, currentPlayer))
            {
                statusLabel.Text = $"Người chơi {currentPlayer} thắng!";
                gameOver = true;
                DisableAllCells();
                return;
            }

            if (CheckDraw())
            {
                statusLabel.Text = "Hòa!";
                gameOver = true;
                return;
            }

            currentPlayer = (currentPlayer == 'X') ? 'O' : 'X';
            statusLabel.Text = $"Lượt của {currentPlayer}";

            if (currentPlayer == 'O' && !gameOver)
            {
                Application.DoEvents();
                MakeAIMove();
            }
        }

        private void MakeAIMove()
        {
            int bestScore = int.MinValue;
            int bestRow = -1;
            int bestCol = -1;

            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    if (board[i, j] == ' ')
                    {
                        board[i, j] = 'O';
                        int score = Minimax(0, false, int.MinValue, int.MaxValue);
                        board[i, j] = ' ';

                        if (score > bestScore)
                        {
                            bestScore = score;
                            bestRow = i;
                            bestCol = j;
                        }
                    }
                }
            }

            if (bestRow != -1 && bestCol != -1)
            {
                board[bestRow, bestCol] = 'O';
                cells[bestRow, bestCol].Text = "O";
                cells[bestRow, bestCol].Enabled = false;

                if (CheckWin(bestRow, bestCol, 'O'))
                {
                    statusLabel.Text = "Máy thắng!";
                    gameOver = true;
                    DisableAllCells();
                    return;
                }

                if (CheckDraw())
                {
                    statusLabel.Text = "Hòa!";
                    gameOver = true;
                    return;
                }

                currentPlayer = 'X';
                statusLabel.Text = "Lượt của X";
            }
        }

        private int Minimax(int depth, bool isMaximizing, int alpha, int beta)
        {
            if (depth >= aiDepth || CheckGameOver())
            {
                return EvaluateBoard();
            }

            if (isMaximizing) 
            {
                int bestScore = int.MinValue;
                for (int i = 0; i < BOARD_SIZE; i++)
                {
                    for (int j = 0; j < BOARD_SIZE; j++)
                    {
                        if (board[i, j] == ' ')
                        {
                            board[i, j] = 'O';
                            int score = Minimax(depth + 1, false, alpha, beta);
                            board[i, j] = ' ';
                            bestScore = Math.Max(score, bestScore);
                            alpha = Math.Max(alpha, bestScore);
                            if (beta <= alpha)
                                return bestScore;
                        }
                    }
                }
                return bestScore;
            }
            else 
            {
                int bestScore = int.MaxValue;
                for (int i = 0; i < BOARD_SIZE; i++)
                {
                    for (int j = 0; j < BOARD_SIZE; j++)
                    {
                        if (board[i, j] == ' ')
                        {
                            board[i, j] = 'X';
                            int score = Minimax(depth + 1, true, alpha, beta);
                            board[i, j] = ' ';
                            bestScore = Math.Min(score, bestScore);
                            beta = Math.Min(beta, bestScore);
                            if (beta <= alpha)
                                return bestScore; 
                        }
                    }
                }
                return bestScore;
            }
        }

        private bool CheckGameOver()
        {
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    if (board[i, j] != ' ')
                    {
                        if (CheckWin(i, j, board[i, j]))
                            return true;
                    }
                }
            }

            return CheckDraw();
        }

        private int EvaluateBoard()
        {
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    if (board[i, j] == 'O' && CheckWin(i, j, 'O'))
                        return 10;
                    if (board[i, j] == 'X' && CheckWin(i, j, 'X'))
                        return -10;
                }
            }
            return 0; 
        }

        private bool CheckWin(int row, int col, char player)
        {
            bool win = true;
            for (int j = 0; j < BOARD_SIZE; j++)
            {
                if (board[row, j] != player)
                {
                    win = false;
                    break;
                }
            }
            if (win) return true;

            win = true;
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                if (board[i, col] != player)
                {
                    win = false;
                    break;
                }
            }
            if (win) return true;

            if (row == col)
            {
                win = true;
                for (int i = 0; i < BOARD_SIZE; i++)
                {
                    if (board[i, i] != player)
                    {
                        win = false;
                        break;
                    }
                }
                if (win) return true;
            }

            if (row + col == BOARD_SIZE - 1)
            {
                win = true;
                for (int i = 0; i < BOARD_SIZE; i++)
                {
                    if (board[i, BOARD_SIZE - 1 - i] != player)
                    {
                        win = false;
                        break;
                    }
                }
                if (win) return true;
            }

            return false;
        }

        private bool CheckDraw()
        {
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    if (board[i, j] == ' ')
                        return false;
                }
            }
            return true;
        }

        private void DisableAllCells()
        {
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    cells[i, j].Enabled = false;
                }
            }
        }

        private void RestartButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    this.Controls.Remove(cells[i, j]);
                }
            }

            InitializeGame();
        }
    }
}
