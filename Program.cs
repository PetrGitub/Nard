using System;

namespace _07_B___poleCELL_piece_STRING
{
    class Program
    {
        static void Main(string[] args)
        {
            Board hraciDeska = new Board(8);

            /* metodu START spustim na hraci desce => vykresli se pole 8x8 ze samych 8 */
            Console.Write(hraciDeska.ToString());

            while (true) /*'while' mi zajisti, ze muzu tahat figurkami dokud mne to bude bavit; 'for' by to omezil konkretni hodnotou(poctem tahu)*/
            {
                hraciDeska.Move();
                Console.Clear();
                Console.Write(hraciDeska.ToString());
            }   ////// Komntar
        }
    }
}
