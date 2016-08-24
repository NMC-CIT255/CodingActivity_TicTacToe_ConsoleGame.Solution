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

        //public void GameLoop()
        //{
        //    //
        //    // Initialize new game
        //    //
        //    InitializeGame();

        //    //
        //    // Display the Welcome Screen with application Quit option
        //    //
        //    DisplayWelcomeScreen();

        //    //
        //    // Display the game rules
        //    //
        //    DisplayRulesScreen();

        //    //
        //    // Game loop
        //    // 
        //    while (playingGame)
        //    {
        //        //
        //        // Initialize new round
        //        //
        //        InitializeRound();

        //        //
        //        // Round loop
        //        // 
        //        while (playingRound)
        //        {
        //            //
        //            // Display the player guess screen and return the player's guess
        //            //
        //            playersGuess = DisplayGetPlayersGuessScreen();

        //            //
        //            // Evaluate the player's guess and provide the player feedback
        //            //
        //            DisplayPlayerGuessFeedback();

        //            //
        //            // Update round variables, process the results and provide player feedback
        //            //
        //            UpdateAndDisplayRoundStatus();
        //        }

        //        //
        //        // Round complete, display player stats and prompt to Continue/Quit
        //        //
        //        DisplayPlayerStats();
        //    }

        //    DisplayClosingScreen();
        //}

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
            // Initialize game board status
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
                    switch (_gameView.CurrentViewState)
                    {
                        case ConsoleView.ViewState.Active:
                            _gameboard.UpdateGameboardState();

                            _gameView.DisplayGameArea();

                            switch (_gameboard.CurrentGameState)
                            {
                                case Gameboard.GameState.NewGame:// The new game status should not be an option here
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
                                case Gameboard.GameState.PlayerOWin:
                                case Gameboard.GameState.CatsGame:
                                    _playingRound = false;
                                    break;

                                default:
                                    break;
                            }
                            break;
                        case ConsoleView.ViewState.PlayerTimedOut:
                            _gameView.DisplayTimedOutScreen();
                            break;
                        case ConsoleView.ViewState.PlayerUsedMaxAttempts:
                            _gameView.DisplayMaxAttemptsReachedScreen();
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
                else
                {
                    _gameView.DisplayCurrentPlayerStatus();
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
            // Player entered a valid location, check to see if it is free.
            if (_gameView.CurrentViewState != ConsoleView.ViewState.PlayerUsedMaxAttempts)
            {
                while (!_gameboard.IsValidMove(gameboardPosition))
                {
                    _gameView.DisplayMessageBox("That position on the board is not available.");

                    _gameView.GetPlayerPositionChoice(gameboardPosition);
                }
            }
            else
            {
                _playingRound = false;
            }

            return gameboardPosition;
        }

        #endregion
    }
}
