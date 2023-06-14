using System;
using System.Windows.Forms;
using System.Drawing;

namespace Maze
{
    public class Labirint
    {
        // позиция главного персонажа
        public int CharacterPositionX { get; set; }
        public int CharacterPositionY { get; set; }
        int _totalMedals;
        int _totalEnemies;

        int height; // высота лабиринта (количество строк)
        int width; // ширина лабиринта (количество столбцов в каждой строке)

        //int _count = 3;

        public MazeObject[,] objects;

        public PictureBox[,] images;

        public static Random r = new Random();

        public Form parent;

        public Labirint(Form parent, int height, int width)
        {
            this.width = width;
            this.height = height;
            this.parent = parent;

            objects = new MazeObject[height, width];
            images = new PictureBox[height, width];

            CharacterPositionX = 0;
            CharacterPositionY = 2;

            _totalMedals = 0;


            Generate();
        }

        public void Generate()
        {
            for (int rows = 0; rows < height; rows++)
            {
                for (int columns = 0; columns < width; columns++)
                {
                    MazeObject.MazeObjectType current = MazeObject.MazeObjectType.HALL;

                    // в 1 случае из 5 - ставим стену
                    if (r.Next(5) == 0)
                    {
                        current = MazeObject.MazeObjectType.WALL;
                    }

                    // в 1 случае из 250 - кладём денежку
                    if (columns != 0 && rows != 0 && columns != height - 1 && rows != width - 1)
                    {
                        if (r.Next(250) == 0)
                        {
                            current = MazeObject.MazeObjectType.MEDAL;
                            _totalMedals++;
                        }
                    }

                    // в 1 случае из 250 - размещаем жизнь
                    if (r.Next(250) == 0)
                    {
                        current = MazeObject.MazeObjectType.HEALTH;
                    }

                    //в 1 случае из 250 - размещаем врага
                    if (columns != 0 && rows != 0 && columns != height - 1 && rows != width - 1)
                    {
                        if (r.Next(200) == 0)
                        {
                            current = MazeObject.MazeObjectType.ENEMY;
                            _totalEnemies++;
                        }
                    }

                    // стены по периметру обязательны
                    if (columns == 0 || rows == 0 || columns == height - 1 || rows == width - 1)
                    {
                        current = MazeObject.MazeObjectType.WALL;
                    }

                    // наш персонажик
                    if (columns == CharacterPositionX && rows == CharacterPositionY)
                    {
                        current = MazeObject.MazeObjectType.CHAR;
                    }

                    // есть выход, и соседняя ячейка справа всегда свободна
                    if (columns == CharacterPositionX + 1 && rows == CharacterPositionY || columns == width - 1 && rows == height - 3)
                    {
                        current = MazeObject.MazeObjectType.HALL;
                    }

                    objects[rows, columns] = new MazeObject(current);
                    images[rows, columns] = new PictureBox();
                    images[rows, columns].Location = new Point(columns * objects[rows, columns].width, rows * objects[rows, columns].height);
                    images[rows, columns].Parent = parent;
                    images[rows, columns].Width = objects[rows, columns].width;
                    images[rows, columns].Height = objects[rows, columns].height;
                    images[rows, columns].BackgroundImage = objects[rows, columns].texture;
                }
            }
        }
        public int GetTotalMedals() { return _totalMedals; }
        public int GetTotalEnemies() { return _totalEnemies; }
    }
}
