using System;
using System.Collections.Generic;
using System.Linq;          //  <-  tento řádek a řádek pod ním bylo třeba přidat (jinak třeba neznal "XmlDocument")
using System.Xml;
using System.Text;



namespace NARD_01
{
    class Files
    {
        public Files()      // <- Konstruktor
        { 
        
        }





        public bool SaveGame( string fileName, Board board )        // metoda pro uložení hry  ( musím zadat jméno souboru, do kterého hru uložím a název šachovnice )
        {
            if (fileName == null)           // není zadán název souboru, do kterého uložím hru => vrátí chybu
                return false;

            XmlDocument document = new XmlDocument();                                               // <- instance XmlDocument
            XmlDeclaration declaration = document.CreateXmlDeclaration( "1.0", "utf-8", null );     // HLAVIČKA  <- vytvořeno pomocí  "metody CreateXmlDeclaration()"  na instanci  "XmlDocument" (= document)
            document.AppendChild( declaration );                                                    // <- vrátí se mi uzel, který do dokumentu (=document) přidám pomocí  "metody AppendChild()"
            XmlElement root = document.CreateElement( "Nard" );                                     // vytvoření kořenového elementu




			XmlElement players = document.CreateElement("Players");
			players.SetAttribute("Player1", "0");
			players.SetAttribute("Player2", "0");

			root.AppendChild(players);

			XmlElement moves = document.CreateElement("Moves");
			moves.SetAttribute("Pointer", (board.IndexSeznamu).ToString());
			foreach (int[] tahZeSeznamu in board.SeznamTahu)
			{
				XmlElement oneMove = document.CreateElement("OneMove");

				oneMove.SetAttribute("FromX", tahZeSeznamu[0].ToString());
				oneMove.SetAttribute("FromY", tahZeSeznamu[1].ToString());
				oneMove.SetAttribute("ToX", tahZeSeznamu[4].ToString());
				oneMove.SetAttribute("ToY", tahZeSeznamu[5].ToString());

				moves.AppendChild(oneMove);
			}
			root.AppendChild(moves);

			document.AppendChild(root);
			try
			{
				document.Save(fileName);
			}
			catch
			{
				return false;
			}

			return true;
		}





		public bool LoadGame(string fileName, out Board newBoard, out int playerOnMove, out int player1settings, out int player2settings)
		{
			newBoard = null;
			player1settings = -1;
			player2settings = -1;

			playerOnMove = 1;
			if (fileName == null)
				return false;

			int pointer = 0;
			List<int[]> loadedMoves = new List<int[]>();
			try
			{
				XmlDocument document = new XmlDocument();
				document.Load(fileName);

				XmlNode root = document.DocumentElement;

				foreach (XmlNode level1node in root)
				{
					switch (level1node.Name)
					{
						case "Players":
							XmlElement player = (XmlElement)level1node;
							player1settings = int.Parse(player.GetAttribute("Player1"));
							player2settings = int.Parse(player.GetAttribute("Player2"));
							break;
						case "Moves":
							XmlElement moves = (XmlElement)level1node;
							pointer = int.Parse(moves.GetAttribute("Pointer"));
							foreach (XmlNode level2node in level1node)
							{
								XmlElement oneMove = (XmlElement)level2node;
								int fromX = int.Parse(oneMove.GetAttribute("FromX"));
								int fromY = int.Parse(oneMove.GetAttribute("FromY"));
								int toX = int.Parse(oneMove.GetAttribute("ToX"));
								int toY = int.Parse(oneMove.GetAttribute("ToY"));
								if (ValidateMove(fromX, fromY, toX, toY))
									loadedMoves.Add(new int[] { fromX, fromY, toX, toY });
								else
									return false;
							}
							break;
					}
				}
			}


			catch
			{
				return false;
			}

			if (player1settings < 0 || player1settings > 4 || player2settings < 0 || player2settings > 4)
				return false;
			if (pointer > loadedMoves.Count)
				return false;

			newBoard = new Board();
			Rules rules = new Rules();
			rules.NaplnBoard(newBoard);
			foreach (int[] move in loadedMoves)
			{
				bool validMoveFound = false;
				foreach (int[] validMove in rules.GenerujPlatneTahy(newBoard, playerOnMove))
				{
					if (move[0] == validMove[0] && move[1] == validMove[1] && move[2] == validMove[4] && move[3] == validMove[5])
					{
						newBoard.VykonejTah(validMove);
						playerOnMove *= -1;
						validMoveFound = true;
						break;
					}
				}
				if (!validMoveFound)
					return false;
			}
			while (newBoard.IndexSeznamu > pointer)
			{
				newBoard.TahZpet();
				playerOnMove *= -1;
			}
			return true;
		}


		/// <summary>
		/// OVĚŘENÍ - jestli nezadávám TAH mimo souřadnice
		/// </summary>
		/// <param name="fromX"></param>
		/// <param name="fromY"></param>
		/// <param name="toX"></param>
		/// <param name="toY"></param>
		/// <returns></returns>
		private bool ValidateMove(int fromX, int fromY, int toX, int toY)
		{
			if (fromX < 0 || fromX > 6 || fromY < 0 || fromY > 7)
				return false;

			if (toX < 0 || toX > 6 || toY < 0 || toY > 7)
				return false;

			return true;
		}
	}
}
