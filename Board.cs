using System;
using System.Collections.Generic;
using System.Text;

namespace _07_B___poleCELL_piece_STRING
{
    class Board
    {
        public int Size { get; private set; }
        public Cell[,] Grid;
        //public Piece Piece { get; set; }
        public string zprava;

        public Board(int s)
        {
            Size = s;
            Grid = new Cell[Size, Size]; // 'NEW' dokud jsem ho tady nemel, hazelo mi to chybu

            makeGrid();
            //MakeCoords();
        }


        public void makeGrid()
        {
            for (int i = 0; i < Size; i++)   /*Obvykle jsou RADKY souřadnice Y a je to vnější smyčka     =>   radky = y; sloupce = x*/
            {
                for (int j = 0; j < Size; j++)
                {
                    Coordinates coord = new Coordinates(i, j);
                    Grid[i, j] = new Cell(coord);

                    if (i == 0 || i == 1)
                    {
                        Grid[i, j].pcs = new Piece(coord, Piece.ShapePiece.WhiteStone);
                    }
                    else if (i >= Size - 2)
                    {
                        Grid[i, j].pcs = new Piece(coord, Piece.ShapePiece.BlackStone);
                    }
                    else
                    {
                        Grid[i, j].pcs = new Piece(coord, Piece.ShapePiece.Empty);
                    }
                }
            }
        }


        public override string ToString()
        {
            string returnStrg = "\n     A  B  C  D  E  F  G  H\n\n";
            string smallSpace = " ";
            string Space = "   ";

            for (int y = Size - 1; y >= 0; y--)
            {
                returnStrg = returnStrg + smallSpace;
                returnStrg += (y + 1).ToString();       /* cisluju radku od horniho, pro ten plati y=7 => 7 + 1 = 8 */
                returnStrg += Space;

                for (int x = 0; x < Size; x++)
                {
                    if (Grid[y, x].pcs != null)         /* pokud je na policku(=Cell) nejaka figurka(=pcs) => probehne cyklus plneni do => [1. a 2.radek] */
                    {                                   /* pokud se dojde ke 3. az 6. radku, tento cyklus je preskocen a do policek(=Cell) se vlozi '0' => radek 81 */
                        returnStrg = returnStrg + Grid[y, x].pcs.ToString();
                        if (y >= Size - 2)
                        {
                            returnStrg = returnStrg + "  ";/* radky, kde jsou 'x'; overeni, ze to tak skutecne je=> zmenit velikost mezery a uvidim, na kterych radcich se to zmeni*/
                        }
                        else if (y == 0 || y == 1)
                        {
                            returnStrg = returnStrg + "  "; /* radky, kde jsou 'o' */
                        }
                        else
                        {
                            returnStrg = returnStrg + "  ";
                        }
                    }
                    else
                    {
                        returnStrg += '.'.ToString() + "  ";
                    }
                }
                returnStrg += "\n\n";   /* mezera mezi radky */
            }
            return returnStrg;
        }


        public int[] EnterCoordsPiece()     /*metoda pro zadávání souřadnic figurky, kterou chci posunout*/
        {
            int chyba = 0;
            int[] result = new int[2];
            int b;

            while (chyba > -1)
            {
                chyba = -1;

                Console.WriteLine("Zadej souřadnice figurky, kterou se má táhnout(ve tvaru A1): ");
                string zadani = Console.ReadLine();                     /* nactu napsany text do 'zadani' */

                if (zadani.Length == 2 && (zadani[1] < '9'))
                {
                    for (int i = 0; i < 2; i++)
                    {
                        b = (int)zadani[1 - i];
                        if (!(b < 49 || (56 < b && b < 65) || (72 < b && b < 97) || b > 104 || (zadani[0] > 48 && zadani[0] < 57)))
                        {
                            if (b >= 65 && b <= 72)
                            {
                                result[i] = b - 65;
                            }
                            else if (b >= 49 && b <= 56)
                            {
                                result[i] = b - 49;
                            }
                            else if (b >= 97 && b <= 104)
                            {
                                result[i] = b - 97;
                            }
                        }
                        else
                        {
                            Console.Write("\tChyba - zadal jsi hodnoty mimo hraci pole.\n");
                            chyba += 1;
                        }
                    }
                }
                else
                {
                    Console.Write("\tChyba - zadal jsi vice nez 2 hodnoty nebo nesprávné souřadnice.\n");
                    chyba += 1;
                }
            }
            return result;
        }

        public void Move()
        {
            int[] Pole1;
            Pole1 = EnterCoordsPiece();
            int[] Pole2;
            Pole2 = EnterCoordsPiece();          /*uložení načtených souřadnic do přechodných Polí*/

            Grid[Pole2[0], Pole2[1]].pcs = Grid[Pole1[0], Pole1[1]].pcs; /*souradnice figurky z Pole1 vlozim do souradnic fig. z Pole2 => radek 10 Cell urcuje, ze jde o figurky*/
            Grid[Pole2[0], Pole2[1]].pcs.NewCoords(Pole2[0], Pole2[1]); /*figurka, ktera je na novych souradnicich ma stale sve puvodni souradnice => vkladam do ni nove souradnice a to souradnice bunky, na kterou prisla*/
            Grid[Pole1[0], Pole1[1]].pcs = null; /*premazu souradnice odkud vysla figurka na nulu(tzn - tam ted uz nic nestoji)*/
        }
    }
}
