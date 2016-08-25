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

        private int _roundNumber;

        private static Gameboard _gameboard = new Gameboard();
        private static ConsoleView _gameView = new ConsoleView(_gameboard);


        #endregion

        #region PROPERTIES



        #endregion

        #region CONSTRUCTORS

        public GameController()
        {
            InitializeGame();
            PlayGame();
        }

        public void InitializeGame()
        {
            //
            // Initialize game variables
            //
            _playingGame = true;
            _playingRound = true;
            _roundNumber = 1;

            //
            // Initialize game board status
            //
            _gameboard.CurrentGameState = Gameboard.GameState.NewRound;
        }

        #endregion

        #region METHODS

        /// <summary>
        /// Game Loop
        /// </summary>
        public void PlayGame()
        {
            _gameView.DisplayWelcomeScreen();

            while (_playingGame)
            {
                while (_playingRound)
                {
                    ManageGameStateTasks();
                    _gameboard.UpdateGameboardState();
                }

                _gameView.DisplayCurrentPlayerStatus();

                if (_gameView.DisplayNewRoundPrompt())
                {
                    _gameboard.InitializeGameboard();
                    _gameView.InitializeView();
                    _playingRound = true;
                }
            }
        }

        /// <summary>
        /// manage each new task based on the current game state
        /// </summary>
        private void ManageGameStateTasks()
        {
            switch (_gameView.CurrentViewState)
            {
                case ConsoleView.ViewState.Active:
                    _gameView.DisplayGameArea();

                    Gameboard.PlayerPiece currentPlayerPiece;
                    GameboardPosition gameboardPosition = new GameboardPosition();

                    switch (_gameboard.CurrentGameState)
                    {
                        case Gameboard.GameState.NewRound:
                            _gameboard.CurrentGameState = Gameboard.GameState.PlayerXTurn;
                            break;

                        case Gameboard.GameState.PlayerXTurn:
                            ManagePlayerTurn(Gameboard.PlayerPiece.X);

                            //currentPlayerPiece = Gameboard.PlayerPiece.X;
                            //_gameView.GetPlayerPositionChoice(gameboardPosition);
                            //if (_gameView.CurrentViewState == ConsoleView.ViewState.Active)
                            //{
                            //_gameboard.SetPlayerPiece(gameboardPosition, currentPlayerPiece);
                            //}
                            break;

                        case Gameboard.GameState.PlayerOTurn:
                            currentPlayerPiece = Gameboard.PlayerPiece.O;
                            _gameView.GetPlayerPositionChoice(gameboardPosition);
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
                    _playingRound = false;
                    break;
                case ConsoleView.ViewState.PlayerUsedMaxAttempts:
                    _gameView.DisplayMaxAttemptsReachedScreen();
                    _playingRound = false;
                    break;
                default:
                    break;
            }
        }

        private void ManagePlayerTurn(Gameboard.PlayerPiece currentPlayerPiece)
        {
            GameboardPosition gameboardPosition = new GameboardPosition();

            _gameView.GetPlayerPositionChoice(gameboardPosition);
            if (_gameView.CurrentViewState == ConsoleView.ViewState.Active)
            {
                _gameboard.SetPlayerPiece(gameboardPosition, currentPlayerPiece);
            }
        }

        #endregion
    }
}
