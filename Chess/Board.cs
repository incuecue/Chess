﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class Board
    {
        public string fen { get; private set; }
        Figure[,] figures;
        public Color moveColor { get; private set; }
        public int moveNumber { get; private set; }

        public Board(string fen)
        {
            this.fen = fen;
            figures = new Figure[8, 8];
            Init();
        }

        void Init()
        {
            //rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1
            // 0                                          1 2    3 4 5
            string [] parts = fen.Split();
            if(parts.Length!=6) return;
            InitFigures(parts[0]);
            moveColor = parts[1] == "b" ? Color.black : Color.white;
            moveNumber = int.Parse(parts[5]);
        }

        void InitFigures (string data)
        {
            for (int j = 9; j>=2; j--)
                data = data.Replace(j.ToString(), (j - 1).ToString()+"1");
            data = data.Replace("1",".");
            string[] lines = data.Split('/');
            for (int y=7; y>=0;y--)
                for (int x=0;x<8;x++)
                    figures[x,y]=(Figure) lines[7-y][x];
        }

        void GenerateFEN ()
        {
            return FenFigures() + " " + 
                (moveColor == Color.white ? "w" : "b") +
                " - - 0 " + moveNumber.ToString();

        }                         

        public Figure GetFigureAt(Square square)
        {
            if (square.onBoard())
                return figures[square.x, square.y];
            return Figure.none;
        }

        void SetFigureAt(Square square, Figure figure)
        {
            if (square.onBoard())
                figures[square.x, square.y] = figure;
        }

        public Board Move(FigureMoving fm)
        {
            Board next = new Board(fen);
            next.SetFigureAt(fm.from, Figure.none);
            next.SetFigureAt(fm.to, fm.promotion == Figure.none ? fm.figure : fm.promotion);
            if (moveColor == Color.black)
                next.moveNumber++;
            next.moveColor = moveColor.FlipColor();
            next.GenerateFEN();
            return next;
        }    
    }
}
