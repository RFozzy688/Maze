using System;
using System.Drawing;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Timer = System.Windows.Forms.Timer;

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
        private static DateTime timeStart;
        private static Timer timer;
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
            ClientSize = new Size(columns * pictureSize/* + 150*/, rows * pictureSize + 22);

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

            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1.SuspendLayout();

            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3,
            this.toolStripStatusLabel4});
            this.statusStrip1.Location = new System.Drawing.Point(0, 375);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(641, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            this.statusStrip1.BackColor = Color.FromArgb(255, 92, 118, 137);
            this.statusStrip1.ForeColor = Color.White;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(48, 17);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(51, 17);
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(49, 17);
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(41, 17);

            this.toolStripStatusLabel1.Text = "Health: 100%";
            this.toolStripStatusLabel2.Text = "Medals: 0";
            this.toolStripStatusLabel3.Text = "00:00:00";
            this.toolStripStatusLabel4.Text = "Steps: 0";

            this.Controls.Add(this.statusStrip1);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
        }
        public void StartGame()
        {
            l = new Labirint(this, rows, columns);
            _char = new Character();

            StartTimer();
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
            ShowCurrentSteps();

            if (IsCheckCountMedals(_char, l)) { WinGame(); }
        }
        private void WinGame()
        {
            StopTimer();

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
            //_medalsLbl.Text = "Medals: " + _char.CurrentMedals.ToString();
            this.toolStripStatusLabel2.Text = String.Format("Medals: {0}", _char.CurrentMedals.ToString());
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
            //_healthLbl.Text = String.Format("Health: {0}%", _char.Health.ToString());
            this.toolStripStatusLabel1.Text = String.Format("Health: {0}%", _char.Health.ToString());
        }
        private void GameOver()
        {
            if (_char.Health == 0)
            {
                StopTimer();

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
        private void SetSteps()
        {
            _char.Steps++;
        }
        private void ShowCurrentSteps()
        {
            SetSteps();

            this.toolStripStatusLabel4.Text = String.Format("Steps: {0}", _char.Steps.ToString());
        }
        public void StartTimer()
        {
            timeStart = DateTime.Now;
            timer = new Timer();

            timer.Interval = 1000;
            timer.Enabled = true;
            timer.Tick += new EventHandler(ShowTimer);
        }
        public void StopTimer()
        {
            timer.Stop();
            timer.Dispose();
        }
        private void ShowTimer(object sender, EventArgs e)
        {
            TimeSpan interval = DateTime.Now - timeStart;

            this.toolStripStatusLabel3.Text = String.Format("{0}", interval.ToString(@"hh\:mm\:ss"));
        }
    }
}
