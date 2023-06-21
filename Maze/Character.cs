using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Maze
{
    internal class Character
    {
        public int Health { get; set; }
        public int CurrentMedals { get; set; }
        public int Steps { get; set; }
        public Character() 
        {
            Health = 100;
            CurrentMedals = 0;
            Steps = 0;
        }
        public void MoveRight(Labirint l)
        {
            // проверка на то, свободна ли ячейка справа
            if (l.objects[l.CharacterPositionY, l.CharacterPositionX + 1].type ==
                MazeObject.MazeObjectType.HALL) // проверяем ячейку правее на 1 позицию, является ли она коридором
            {
                l.objects[l.CharacterPositionY, l.CharacterPositionX].type =
                    MazeObject.MazeObjectType.HALL;

                l.objects[l.CharacterPositionY, l.CharacterPositionX].texture = MazeObject.images[0]; // hall
                l.images[l.CharacterPositionY, l.CharacterPositionX].BackgroundImage =
                    l.objects[l.CharacterPositionY, l.CharacterPositionX].texture;

                l.CharacterPositionX++;

                l.objects[l.CharacterPositionY, l.CharacterPositionX].type =
                    MazeObject.MazeObjectType.CHAR;

                l.objects[l.CharacterPositionY, l.CharacterPositionX].texture = MazeObject.images[4]; // character
                l.images[l.CharacterPositionY, l.CharacterPositionX].BackgroundImage =
                    l.objects[l.CharacterPositionY, l.CharacterPositionX].texture;
            }
        }
        public void MoveLeft(Labirint l)
        {
            if (l.CharacterPositionX == 0) { return; }

            if (l.objects[l.CharacterPositionY, l.CharacterPositionX - 1].type ==
                   MazeObject.MazeObjectType.HALL) // проверяем ячейку левее на 1 позицию, является ли она коридором
            {
                l.objects[l.CharacterPositionY, l.CharacterPositionX].type =
                    MazeObject.MazeObjectType.HALL;

                l.objects[l.CharacterPositionY, l.CharacterPositionX].texture = MazeObject.images[0]; // hall
                l.images[l.CharacterPositionY, l.CharacterPositionX].BackgroundImage =
                    l.objects[l.CharacterPositionY, l.CharacterPositionX].texture;

                l.CharacterPositionX--;

                l.objects[l.CharacterPositionY, l.CharacterPositionX].type =
                    MazeObject.MazeObjectType.CHAR;

                l.objects[l.CharacterPositionY, l.CharacterPositionX].texture = MazeObject.images[4]; // character
                l.images[l.CharacterPositionY, l.CharacterPositionX].BackgroundImage =
                    l.objects[l.CharacterPositionY, l.CharacterPositionX].texture;
            }
        }
        public void MoveUp(Labirint l)
        {
            // проверка на то, свободна ли ячейка сверху
            if (l.objects[l.CharacterPositionY - 1, l.CharacterPositionX].type ==
                MazeObject.MazeObjectType.HALL) // проверяем ячейку сверху на 1 позицию, является ли она коридором
            {
                l.objects[l.CharacterPositionY, l.CharacterPositionX].type =
                    MazeObject.MazeObjectType.HALL;

                l.objects[l.CharacterPositionY, l.CharacterPositionX].texture = MazeObject.images[0]; // hall
                l.images[l.CharacterPositionY, l.CharacterPositionX].BackgroundImage =
                    l.objects[l.CharacterPositionY, l.CharacterPositionX].texture;

                l.CharacterPositionY--;

                l.objects[l.CharacterPositionY, l.CharacterPositionX].type =
                    MazeObject.MazeObjectType.CHAR;

                l.objects[l.CharacterPositionY, l.CharacterPositionX].texture = MazeObject.images[4]; // character
                l.images[l.CharacterPositionY, l.CharacterPositionX].BackgroundImage =
                    l.objects[l.CharacterPositionY, l.CharacterPositionX].texture;
            }
        }
        public void MoveDown(Labirint l)
        {
            // проверка на то, свободна ли ячейка снизу
            if (l.objects[l.CharacterPositionY + 1, l.CharacterPositionX].type ==
                MazeObject.MazeObjectType.HALL) // проверяем ячейку снизу на 1 позицию, является ли она коридором
            {
                l.objects[l.CharacterPositionY, l.CharacterPositionX].type =
                    MazeObject.MazeObjectType.HALL;

                l.objects[l.CharacterPositionY, l.CharacterPositionX].texture = MazeObject.images[0]; // hall
                l.images[l.CharacterPositionY, l.CharacterPositionX].BackgroundImage =
                    l.objects[l.CharacterPositionY, l.CharacterPositionX].texture;

                l.CharacterPositionY++;

                l.objects[l.CharacterPositionY, l.CharacterPositionX].type =
                    MazeObject.MazeObjectType.CHAR;

                l.objects[l.CharacterPositionY, l.CharacterPositionX].texture = MazeObject.images[4]; // character
                l.images[l.CharacterPositionY, l.CharacterPositionX].BackgroundImage =
                    l.objects[l.CharacterPositionY, l.CharacterPositionX].texture;
            }
        }
        public void PickUpMedal(Labirint l, Keys keys)
        {
            switch (keys)
            {
                case Keys.Left:
                    if (l.CharacterPositionX == 0) { break; }

                    if (l.objects[l.CharacterPositionY, l.CharacterPositionX - 1].type == 
                        MazeObject.MazeObjectType.MEDAL)
                    {
                        l.objects[l.CharacterPositionY, l.CharacterPositionX - 1].type =
                            MazeObject.MazeObjectType.HALL;
                        CurrentMedals++;
                    }
                    break; 
                case Keys.Right:
                    if (l.objects[l.CharacterPositionY, l.CharacterPositionX + 1].type ==
                        MazeObject.MazeObjectType.MEDAL)
                    {
                        l.objects[l.CharacterPositionY, l.CharacterPositionX + 1].type =
                            MazeObject.MazeObjectType.HALL;
                        CurrentMedals++;
                    }
                    break; 
                case Keys.Up:
                    if (l.objects[l.CharacterPositionY - 1, l.CharacterPositionX].type ==
                        MazeObject.MazeObjectType.MEDAL)
                    {
                        l.objects[l.CharacterPositionY - 1, l.CharacterPositionX].type =
                            MazeObject.MazeObjectType.HALL;
                        CurrentMedals++;
                    }
                    break;
                case Keys.Down:
                    if (l.objects[l.CharacterPositionY + 1, l.CharacterPositionX].type ==
                        MazeObject.MazeObjectType.MEDAL)
                    {
                        l.objects[l.CharacterPositionY + 1, l.CharacterPositionX].type =
                            MazeObject.MazeObjectType.HALL;
                        CurrentMedals++;
                    }
                    break;
            }
        }
        public void PickUpHealth(Labirint l, Keys keys)
        {
            if (Health < 100)
            {
                switch (keys)
                {
                    case Keys.Left:
                        if (l.CharacterPositionX == 0) { break; }

                        if (l.objects[l.CharacterPositionY, l.CharacterPositionX - 1].type ==
                            MazeObject.MazeObjectType.HEALTH)
                        {
                            l.objects[l.CharacterPositionY, l.CharacterPositionX - 1].type =
                                MazeObject.MazeObjectType.HALL;
                            Health += 5;
                        }
                        break;
                    case Keys.Right:
                        if (l.objects[l.CharacterPositionY, l.CharacterPositionX + 1].type ==
                            MazeObject.MazeObjectType.HEALTH)
                        {
                            l.objects[l.CharacterPositionY, l.CharacterPositionX + 1].type =
                                MazeObject.MazeObjectType.HALL;
                            Health += 5;
                        }
                        break;
                    case Keys.Up:
                        if (l.objects[l.CharacterPositionY - 1, l.CharacterPositionX].type ==
                            MazeObject.MazeObjectType.HEALTH)
                        {
                            l.objects[l.CharacterPositionY - 1, l.CharacterPositionX].type =
                                MazeObject.MazeObjectType.HALL;
                            Health += 5;
                        }
                        break;
                    case Keys.Down:
                        if (l.objects[l.CharacterPositionY + 1, l.CharacterPositionX].type ==
                            MazeObject.MazeObjectType.HEALTH)
                        {
                            l.objects[l.CharacterPositionY + 1, l.CharacterPositionX].type =
                                MazeObject.MazeObjectType.HALL;
                            Health += 5;
                        }
                        break;
                }
            }
        }
    }
}
