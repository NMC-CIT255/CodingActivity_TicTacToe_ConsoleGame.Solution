using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace CodingActivity_TicTacToe_ConsoleGame
{
    public class ConsoleView
    {
        #region ENUMS

        public enum ViewState
        {
            Active,
            PlayerTimedOut, // TODO Track player time on task
            PlayerUsedMaxAttempts
        }

        #endregion

        #region FIELDS

        private const int TOP_LEFT_ROW = 3;
        private const int TOP_LEFT_COLUMN = 3;
        private const int MAX_NUM_WINDOW_ROWS = 30;
        private const int MAX_NUM_WINDOW_COLUMNS = 60;

        private Gameboard _gameboard;

        private ViewState _currentViewStat;

        #endregion

        #region PROPERTIES
        public ViewState CurrentViewState
        {
            get { return _currentViewStat; }
            set { _currentViewStat = value; }
        }

        #endregion

        #region CONSTRUCTORS

        public ConsoleView(Gameboard gameboard)
        {
            _gameboard = gameboard;
            _currentViewStat = ViewState.Active;

            DisplaySetup();
        }

        #endregion

        #region METHODS

        public void DisplaySetup()
        {
            Console.SetWindowSize(MAX_NUM_WINDOW_COLUMNS, MAX_NUM_WINDOW_ROWS);
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.DarkBlue;

        }

        public void DisplayWelcomeScreen()
        {
            Console.Clear();

            Console.SetCursorPosition(3, 3);
            Console.WriteLine("\t    Welcome to Tic-tac-toc");
            Console.WriteLine("\t   Laughing Leaf Productions\n\n");
            Console.WriteLine("\tPress the ENTER key to continue.");
            Console.ReadLine();
        }

        public void DisplayGameArea()
        {
            Console.Clear();

            DisplayGameHeader();
            DisplayGameboard();
            DisplayMessageBox();
            DisplayGameStatus();
        }

        private void DisplayGameHeader()
        {
            Console.SetCursorPosition(TOP_LEFT_COLUMN, TOP_LEFT_ROW);
            Console.Write("\t\t      Current Game Board");
        }

        private void DisplayGameStatus()
        {
            Console.SetCursorPosition(TOP_LEFT_COLUMN, TOP_LEFT_ROW + 12);

            switch (_gameboard.CurrentGameState)
            {
                case Gameboard.GameState.NewGame:
                    //
                    // The new game status should not be an necessary option here
                    //
                    break;
                case Gameboard.GameState.PlayerXTurn:
                    DisplayMessage("It is currently Player X's turn.", "", "");
                    break;
                case Gameboard.GameState.PlayerOTurn:
                    DisplayMessage("It is currently Player O's turn.", "", "");
                    break;
                case Gameboard.GameState.PlayerXWin:
                    DisplayMessage("Player X Wins!!!", "", "");
                    break;
                case Gameboard.GameState.PlayerOWin:
                    DisplayMessage("Player O Wins!!!", "", "");
                    break;
                case Gameboard.GameState.CatsGame:
                    DisplayMessage("Cat Game!!!", "", "");
                    break;
                default:
                    break;
            }
        }

        public void DisplayMessage(string messageLine1, string messageLine2, string messageLine3)
        {
            //
            // Display new message
            //
            Console.SetCursorPosition(8, 22);
            Console.Write(messageLine1);
            Console.SetCursorPosition(8, 23);
            Console.Write(messageLine2);
            Console.SetCursorPosition(8, 24);
            Console.Write(messageLine3);
        }

        public void DisplayMessageBox()
        {
            Console.SetCursorPosition(0, TOP_LEFT_ROW + 17);
            Console.WriteLine("     ***************************************************");
            for (int rows = 0; rows < 5; rows++)
            {
                Console.WriteLine("     *                                                 *");
            }
            Console.WriteLine("     ***************************************************");
        }

        private void DisplayGameboard()
        {

            Console.SetCursorPosition(TOP_LEFT_COLUMN, TOP_LEFT_ROW + 2);

            Console.Write("\n\t\t        |---+---+---|\n");

            for (int i = 0; i < 3; i++)
            {
                Console.Write("\t\t        | ");

                for (int j = 0; j < 3; j++)
                {
                    if (_gameboard.PositionState[i, j] == Gameboard.PlayerPiece.None)
                    {
                        Console.Write(" " + " | ");
                    }
                    else
                    {
                        Console.Write(_gameboard.PositionState[i, j] + " | ");
                    }

                }

                Console.Write("\n\t\t        |---+---+---|\n");
            }

        }

        private void DisplayPositionPropmt(string coordinateType)
        {
            //
            // Clear line
            //
            Console.SetCursorPosition(TOP_LEFT_COLUMN, TOP_LEFT_ROW + 14);
            Console.Write(new String(' ', Console.BufferWidth));
            //
            // Write prompt
            //
            Console.SetCursorPosition(TOP_LEFT_COLUMN, TOP_LEFT_ROW + 14);
            Console.Write("Enter " + coordinateType + " number: ");
        }

        public void DisplayMaxAttemptsMessage()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 10);
            Console.WriteLine(" It appears that you are having difficulty entering your");
            Console.WriteLine(" choice. Please refer to the instructions and play again.");
            Console.WriteLine();
            Console.WriteLine("           Press the (Enter) key to Exit.");

            Console.ReadLine();
        }

        public GameboardPosition GetPlayerPositionChoice(GameboardPosition gameboardPosition)
        {
            //
            // Initialize gameboardPosition with -1 values
            //
            gameboardPosition.Row = -1;
            gameboardPosition.Column = -1;

            //
            // Get row number from player
            //
            gameboardPosition.Row = PlayerCoordinateChoice("Row");

            //
            // Player successfully entered row number, get column number
            //
            if (CurrentViewState == ViewState.Active)
            {
                gameboardPosition.Column = PlayerCoordinateChoice("Column");
            }

            return gameboardPosition;

        }

        private int PlayerCoordinateChoice(string coordinateType)
        {
            int tempCoordinate = -1;
            int numOfPlayerAttempts = 1;
            int maxNumOfPlayerAttempts = 4;

            while ((numOfPlayerAttempts <= maxNumOfPlayerAttempts))
            {
                DisplayPositionPropmt(coordinateType);

                if (int.TryParse(Console.ReadLine(), out tempCoordinate))
                {
                    //
                    // Player response within range
                    //
                    if (tempCoordinate >= 1 && tempCoordinate <= _gameboard.MaxNumOfRowsColumns)
                    {
                        return tempCoordinate;
                    }
                    //
                    // Player response out of range
                    //
                    else
                    {
                        DisplayMessage(coordinateType + " numbers are limited to (1,2,3)", "", "");
                    }
                }
                //
                // Player response cannot be parsed as integer
                //
                else
                {
                    DisplayMessage(coordinateType + " numbers are limited to (1,2,3)", "", "");
                }

                //
                // Increment the number of player attempts
                //
                numOfPlayerAttempts++;
            }

            //
            // Player used maximum number of attempts, set view state and return
            //
            CurrentViewState = ViewState.PlayerUsedMaxAttempts;
            return tempCoordinate;
        }

        #endregion
    }
}
