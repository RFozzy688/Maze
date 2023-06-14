using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timer = System.Windows.Forms.Timer;

namespace Maze
{
    public class Enemy
    {
        public event Collision EventCharCollision;
        public Timer MoveTimer { get; set; }
        public int Damage { get; set; }
        int _positionX;
        int _positionY;
        int _direction;
        int _amountSteps;
        Random _random;
        readonly Labirint l;
        public Enemy(int y, int x, Labirint l)
        {
            _positionX = x;
            _positionY = y;
            this.l = l;
            Damage = 20;

            _random = new Random();
            _direction = 0;
            _amountSteps = 0;

            MoveTimer = new Timer();
            MoveTimer.Tick += new EventHandler(MoveEnemy);
            MoveTimer.Interval = 150;
            MoveTimer.Start();
        }
        public int GetPosX() { return _positionX; }
        public int GetPosY() { return _positionY; }
        public bool GetPos(int i, int j)
        {
            return (_positionY == i && _positionX == j);
        }
        public bool CharCollision()
        {
            if (l.objects[_positionY, _positionX + 1].type == MazeObject.MazeObjectType.CHAR ||
                l.objects[_positionY, _positionX - 1].type == MazeObject.MazeObjectType.CHAR ||
                l.objects[_positionY - 1, _positionX].type == MazeObject.MazeObjectType.CHAR ||
                l.objects[_positionY + 1, _positionX].type == MazeObject.MazeObjectType.CHAR)
            {
                if (EventCharCollision != null)
                {
                    
                    EventCharCollision(_positionY, _positionX);

                    MoveTimer.Enabled = false;
                    MoveTimer.Stop();

                    return true;
                }
            }

            return false;
        }
        public void MoveRight()
        {
            if (_positionX == l.objects.GetLength(1) - 2) { return; }

            // проверка на то, свободна ли ячейка справа
            if (l.objects[_positionY, _positionX + 1].type ==
                MazeObject.MazeObjectType.HALL) // проверяем ячейку правее на 1 позицию, является ли она коридором
            {
                l.objects[_positionY, _positionX].type = MazeObject.MazeObjectType.HALL;

                l.objects[_positionY, _positionX].texture = MazeObject.images[0]; // hall
                l.images[_positionY, _positionX].BackgroundImage =
                    l.objects[_positionY, _positionX].texture;

                _positionX++;

                l.objects[_positionY, _positionX].type = MazeObject.MazeObjectType.ENEMY;

                l.objects[_positionY, _positionX].texture = MazeObject.images[3]; // enemy
                l.images[_positionY, _positionX].BackgroundImage =
                    l.objects[_positionY, _positionX].texture;
            }
            else
                _direction = _random.Next(0, 4);
        }
        public void MoveLeft()
        {
            if (_positionX - 1 == 0) { return; }

            if (l.objects[_positionY, _positionX - 1].type ==
                   MazeObject.MazeObjectType.HALL) // проверяем ячейку левее на 1 позицию, является ли она коридором
            {
                l.objects[_positionY, _positionX].type = MazeObject.MazeObjectType.HALL;

                l.objects[_positionY, _positionX].texture = MazeObject.images[0]; // hall
                l.images[_positionY, _positionX].BackgroundImage =
                    l.objects[_positionY, _positionX].texture;

                _positionX--;

                l.objects[_positionY, _positionX].type = MazeObject.MazeObjectType.ENEMY;

                l.objects[_positionY, _positionX].texture = MazeObject.images[3]; // enemy
                l.images[_positionY, _positionX].BackgroundImage =
                    l.objects[_positionY, _positionX].texture;
            }
            else
                _direction = _random.Next(0, 4);
        }
        public void MoveUp()
        {
            // проверка на то, свободна ли ячейка сверху
            if (l.objects[_positionY - 1, _positionX].type ==
                MazeObject.MazeObjectType.HALL) // проверяем ячейку сверху на 1 позицию, является ли она коридором
            {
                l.objects[_positionY, _positionX].type = MazeObject.MazeObjectType.HALL;

                l.objects[_positionY, _positionX].texture = MazeObject.images[0]; // hall
                l.images[_positionY, _positionX].BackgroundImage =
                    l.objects[_positionY, _positionX].texture;

                _positionY--;

                l.objects[_positionY, _positionX].type = MazeObject.MazeObjectType.ENEMY;

                l.objects[_positionY, _positionX].texture = MazeObject.images[3]; // enemy
                l.images[_positionY, _positionX].BackgroundImage =
                    l.objects[_positionY, _positionX].texture;
            }
            else
                _direction = _random.Next(0, 4);
        }
        public void MoveDown()
        {
            // проверка на то, свободна ли ячейка снизу
            if (l.objects[_positionY + 1, _positionX].type ==
                MazeObject.MazeObjectType.HALL) // проверяем ячейку снизу на 1 позицию, является ли она коридором
            {
                l.objects[_positionY, _positionX].type = MazeObject.MazeObjectType.HALL;

                l.objects[_positionY, _positionX].texture = MazeObject.images[0]; // hall
                l.images[_positionY, _positionX].BackgroundImage =
                    l.objects[_positionY, _positionX].texture;

                _positionY++;

                l.objects[_positionY, _positionX].type = MazeObject.MazeObjectType.ENEMY;

                l.objects[_positionY, _positionX].texture = MazeObject.images[3]; // enemy
                l.images[_positionY, _positionX].BackgroundImage =
                    l.objects[_positionY, _positionX].texture;
            }
            else
                _direction = _random.Next(0, 4);
        }
        private void MoveEnemy(object sender, EventArgs e)
        {
            if (!CharCollision())
            {
                if (_amountSteps == 0)
                {
                    _direction = _random.Next(0, 4);
                    _amountSteps = _random.Next(1, 10);
                }

                switch (_direction)
                {
                    case 0:
                        MoveRight();
                        break;
                    case 1:
                        MoveLeft();
                        break;
                    case 2:
                        MoveUp();
                        break;
                    case 3:
                        MoveDown();
                        break;
                    default:
                        break;
                }

                _amountSteps--;
            }
        }
    }
}
