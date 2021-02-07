using System;
using System.Collections.Generic;
using System.Text;

namespace NARD_01
{
    class Rules
    {
        public Rules()
        {
           
        }

        // Naplneni matice 
        public void NaplnBoard(Board deska)
        {
            for (int i = 0; i < deska.hracideska.GetLength(0); i++)       // this ....... ukazuje na konkretni hraciDesku, protoze jich muze existovat vic
            {
                for (int j = 0; j < deska.hracideska.GetLength(1); j++)
                {
                    if (i <= 1)
                    {
                        deska.hracideska[i, j] = 1;
                    }
                    else if (i >= 5)
                    {
                        deska.hracideska[i, j] = 2;
                    }
                    else
                    {
                        deska.hracideska[i, j] = 0;
                    }
                }
            }
        }

        public bool KontrolaTahu(int[] anyMove, bool tahneBily, int cilovyKamen)
        {
            
            int d_X = Math.Abs(anyMove[0] - anyMove[3]);    // vzdalenost x-ovych souradnic, k pohybu dojde jen kdyz> posun o |1|; indexy se berou odtud: { TahOdkud[0], TahOdkud[1], 0, TahKam[0], TahKam[1], kamen };
            int d_Y = Math.Abs(anyMove[1] - anyMove[4]);    // vzdalenost x-ovych souradnic, k pohybu dojde jen kdyz> posun o |1|; indexy se berou odtud: { TahOdkud[0], TahOdkud[1], 0, TahKam[0], TahKam[1], kamen };

            if (d_X != 0 && d_Y != 0)                   // chyba = pokud aspon v jednom smeru neni rozdil vzdalenosti = 0
            {
                return false;
            }
            else if (d_X == 0 && d_Y != 1)              // chyba = pokud vzdalenost v ose "x" je 0, ale v ose "y" neni 1
            {
                return false;
            }
            else if (d_X != 1 && d_Y == 0)              // chyba = pokud vzdalenost v ose "y" je 0, ale v ose "x" neni 1
            {
                return false;
            }
            else if (anyMove[5] == 0)                   // chyba = pokud chci udelat pohyb na misto, kde uz stoji figurka; anyMove[5] == 0 >>>>> 0 znamena neobsazene policko
            {
                return false;
            }
            else if((tahneBily && anyMove[5] != 1) || (!tahneBily && anyMove[5] != 2))  // chyba = pokud ma tahnout Bily(Cerny) a netahnu bilou(cernou) figurkou
            {
                return false;
            }
            else
            {
                return cilovyKamen == 0;                // tam, kam se jde je prazdne policko (= hodnota 0 znamena prazdne policko)
            }
        }
    }
}
