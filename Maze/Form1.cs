using System;
using System.Drawing;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Maze
{
    public delegate void Collision(int i, int j);
    public partial class Form1 : Form
    {
        // размеры лабиринта в ячейках 16х16 пикселей
        int columns = 40;
        int rows = 40;

        int pictureSize = 16; // ширина и высота одной ячейки

        Labirint l; // ссылка на логику всего происходящего в лабиринте
        Character _char; // ссылка на персонажа
        List<Enemy> _enemyList;
        Label _healthLbl;
        Label _medalsLbl;
        public Form1()
        {
            InitializeComponent();
            Options();
            StartGame();
        }
        public void Options()
        {
            Text = "Maze";
            BackColor = Color.FromArgb(255, 92, 118, 137);

            // размеры клиентской области формы (того участка, на котором размещаются ЭУ)
            ClientSize = new Size(columns * pictureSize + 150, rows * pictureSize);

            StartPosition = FormStartPosition.CenterScreen;

            _healthLbl = new Label();
            _healthLbl.Size = new Size(150, 20);
            _healthLbl.Location = new Point(columns * pictureSize + 10, 16);
            _healthLbl.Text = "Health: 100%";
            _healthLbl.ForeColor = Color.White;
            _healthLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            _healthLbl.Parent = this;

            _medalsLbl = new Label();
            _medalsLbl.Size = new Size(150, 20);
            _medalsLbl.Location = new Point(columns * pictureSize + 10, 40);
            _medalsLbl.Text = "Medals: 0";
            _medalsLbl.ForeColor = Color.White;
            _medalsLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            _medalsLbl.Parent = this;
        }
        public void StartGame()
        {
            l = new Labirint(this, rows, columns);
            _char = new Character();

            FillArrayEnemies();
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Right:
                    _char.PickUpMedal(l, e.KeyCode);
                    _char.PickUpHealth(l, e.KeyCode);
                    _char.MoveRight(l);
                    break;
                case Keys.Left:
                    _char.PickUpMedal(l, e.KeyCode);
                    _char.PickUpHealth(l, e.KeyCode);
                    _char.MoveLeft(l);
                    break;
                case Keys.Up:
                    _char.PickUpMedal(l, e.KeyCode);
                    _char.PickUpHealth(l, e.KeyCode);
                    _char.MoveUp(l);
                    break;
                case Keys.Down:
                    _char.PickUpMedal(l, e.KeyCode);
                    _char.PickUpHealth(l, e.KeyCode);
                    _char.MoveDown(l);
                    break;
            }

            if (IsFoundWayOut()) { WinGame(); }

            ShowCurrentMedals();
            ShowCurrentHealth();

            if (IsCheckCountMedals(_char, l)) { WinGame(); }
        }
        private void WinGame()
        {
            DialogResult res = MessageBox.Show("Ура Вы выиграли!!!\nПродолжить?", "Поздравляем", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Information);
            if (res == DialogResult.Yes)
            {
                Controls.Clear();
                Options();
                StartGame();
            }
            else { this.Close(); }
        }
        private bool IsFoundWayOut()
        {
            if (l.CharacterPositionX == (rows - 1) && l.CharacterPositionY == (columns - 3))
            {
                return true;
            }
            else { return false; }
        }
        private void ShowCurrentMedals()
        {
            _medalsLbl.Text = "Medals: " + _char.CurrentMedals.ToString();
        }
        private bool IsCheckCountMedals(Character character, Labirint l)
        {
            if (l.GetTotalMedals() > 0)
            {
                if (l.GetTotalMedals() == character.CurrentMedals)
                    return true;
                else { return false; }
            }
            else { return false; }
        }
        private void FillArrayEnemies()
        {
            _enemyList = new List<Enemy>(l.GetTotalEnemies());

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (l.objects[i, j].type == MazeObject.MazeObjectType.ENEMY)
                    {
                        _enemyList.Add(new Enemy(i, j, l));
                        _enemyList[_enemyList.Count - 1].EventCharCollision += CharCollision;
                    }
                }
            }
        }
        public void CharCollision(int i, int j)
        {
            int index = _enemyList.FindIndex(a => a.GetPos(i, j));

            if (index >= 0)
            {
                SetHealth(_enemyList[index].Damage);
                _enemyList.RemoveAt(index);

                ShowCurrentHealth();
                GameOver();

                l.objects[i, j].type = MazeObject.MazeObjectType.HALL;
                l.objects[i, j].texture = MazeObject.images[0];
                l.images[i, j].BackgroundImage = MazeObject.images[0];
            } 
        }
        private void SetHealth(int damage)
        {
            _char.Health -= damage;

            if (_char.Health < 0)
            {
                _char.Health = 0;
            }
        }
        private void ShowCurrentHealth()
        {
            _healthLbl.Text = String.Format("Health: {0}%", _char.Health.ToString());
        }
        private void GameOver()
        {
            if (_char.Health == 0)
            {
                DialogResult res = MessageBox.Show("Увы Вы проиграли!!!\nПродолжить?", "Maze",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Information);
                if (res == DialogResult.Yes)
                {
                    Controls.Clear();
                    Options();
                    StartGame();
                }
                else { this.Close(); }
            }
        }
    }
}
