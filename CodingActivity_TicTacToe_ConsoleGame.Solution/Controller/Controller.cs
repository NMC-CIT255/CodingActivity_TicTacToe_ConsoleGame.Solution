using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingActivity_TicTacToe_ConsoleGame
{
    public class GameController
    {
        #region FIELDS

        private bool _playingGame;
        private bool _playingRound;

        private static Gameboard _gameboard = new Gameboard();
        private static ConsoleView _gameView = new ConsoleView(_gameboard);

        #endregion

        #region PROPERTIES



        #endregion

        #region CONSTRUCTORS

        public GameController()
        {
            //
            // Initialize game variables
            //
            _playingGame = true;
            _playingRound = true;

            _gameView.DisplayWelcomeScreen();

            PlayGame();
        }

        #endregion

        #region METHODS

        /// <summary>
        /// Game Loop
        /// </summary>
        public void PlayGame()
        {
            //
            // Display game area
            //
            _gameView.DisplayGameArea();

            //
            // Initialize gameboard status
            //
            _gameboard.CurrentGameState = Gameboard.GameState.NewGame;

            //
            // Create PlayerPiece and GameboardPosition objects
            //
            Gameboard.PlayerPiece currentPlayerPiece;
            GameboardPosition gameboardPosition = new GameboardPosition();

            while (_playingGame)
            {
                while (_playingRound)
                {
                    _gameboard.UpdateGameboardState();

                    _gameView.DisplayGameArea();

                    switch (_gameboard.CurrentGameState)
                    {
                        case Gameboard.GameState.NewGame:
                            //
                            // The new game status should not be an option here
                            //
                            break;

                        case Gameboard.GameState.PlayerXTurn:
                            currentPlayerPiece = Gameboard.PlayerPiece.X;

                            GetPlayerPositionChoice(gameboardPosition);

                            if (_gameView.CurrentViewState == ConsoleView.ViewState.Active)
                            {
                                _gameboard.SetPlayerPiece(gameboardPosition, currentPlayerPiece);
                            }
                            break;

                        case Gameboard.GameState.PlayerOTurn:
                            currentPlayerPiece = Gameboard.PlayerPiece.O;
                            
                            GetPlayerPositionChoice(gameboardPosition);

                            if (_gameView.CurrentViewState == ConsoleView.ViewState.Active)
                            {
                                _gameboard.SetPlayerPiece(gameboardPosition, currentPlayerPiece);
                            }
                            break;

                        case Gameboard.GameState.PlayerXWin:
                            _playingRound = false;
                            break;

                        case Gameboard.GameState.PlayerOWin:
                            _playingRound = false;
                            break;

                        case Gameboard.GameState.CatsGame:
                            _playingRound = false;
                            break;

                        default:
                            break;
                    }
                }

                //
                // Player Was Unable to Enter Valid Position
                // TODO Handle with an exception
                //
                if (_gameView.CurrentViewState == ConsoleView.ViewState.PlayerUsedMaxAttempts)
                {
                    _gameView.DisplayMaxAttemptsMessage();

                    //
                    // Terminate the Game
                    //
                    Environment.Exit(0);
                }
            }

            Console.ReadLine();
        }

        /// <summary>
        /// validate player's move choice
        /// </summary>
        /// <param name="gameboardPosition"></param>
        /// <returns></returns>
        //
        // TODO: move to ConsoleView
        //
        private GameboardPosition GetPlayerPositionChoice(GameboardPosition gameboardPosition)
        {
            _gameView.GetPlayerPositionChoice(gameboardPosition);

            //
            // Player input a position that was taken
            //
            while (!_gameboard.IsValidMove(gameboardPosition))
            {
                _gameView.DisplayMessage("That position on the board is not available.",
                    "Please enter a new position.",
                    "");

                _gameView.GetPlayerPositionChoice(gameboardPosition);
            }

            return gameboardPosition;
        }

        #endregion
    }
}
