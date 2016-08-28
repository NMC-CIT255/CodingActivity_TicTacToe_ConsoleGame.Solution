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

        private int _playerXWins;
        private int _playerOWins;
        private int _catsGames;

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



        #endregion

        #region METHODS

        /// <summary>
        /// Initialize the multi-round game
        /// </summary>
        public void InitializeGame()
        {
            //
            // Initialize game variables
            //
            _playingGame = true;
            _playingRound = true;
            _roundNumber = 0;
            _playerOWins = 0;
            _playerXWins = 0;
            _catsGames = 0;

            //
            // Initialize game board status
            //
            _gameboard.InitializeGameboard();
        }


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

                _gameView.DisplayCurrentGameStatus(_roundNumber, _playerXWins, _playerOWins);

                if (_gameView.DisplayNewRoundPrompt())
                {
                    _gameboard.InitializeGameboard();
                    _gameView.InitializeView();
                    _playingRound = true;
                }
                else
                {
                    _playingGame = false;
                }
            }
            _gameView.DisplayClosingScreen();
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

                    //GameboardPosition gameboardPosition = new GameboardPosition();

                    switch (_gameboard.CurrentGameState)
                    {
                        case Gameboard.GameState.NewRound:
                            _roundNumber++;
                            _gameboard.CurrentGameState = Gameboard.GameState.PlayerXTurn;
                            break;

                        case Gameboard.GameState.PlayerXTurn:
                            ManagePlayerTurn(Gameboard.PlayerPiece.X);
                            break;

                        case Gameboard.GameState.PlayerOTurn:
                            ManagePlayerTurn(Gameboard.PlayerPiece.O);
                            break;

                        case Gameboard.GameState.PlayerXWin:
                            _playerXWins++;
                            _playingRound = false;
                            break;

                        case Gameboard.GameState.PlayerOWin:
                            _playerOWins++;
                            _playingRound = false;
                            break;

                        case Gameboard.GameState.CatsGame:
                            _catsGames++;
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

        /// <summary>
        /// Attempt to get a valid player move. 
        /// </summary>
        /// <param name="currentPlayerPiece">identify as either the X or O player</param>
        private void ManagePlayerTurn(Gameboard.PlayerPiece currentPlayerPiece)
        {
            GameboardPosition gameboardPosition = _gameView.GetPlayerPositionChoice();

            if (_gameView.CurrentViewState == ConsoleView.ViewState.Active)
            {
                _gameboard.SetPlayerPiece(gameboardPosition, currentPlayerPiece);
            }
        }

        #endregion
    }
}
