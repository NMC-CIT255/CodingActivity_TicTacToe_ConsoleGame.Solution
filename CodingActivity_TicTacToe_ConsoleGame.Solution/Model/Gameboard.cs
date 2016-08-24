using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

//using TicTacToe.ConsoleApp.Model;

namespace CodingActivity_TicTacToe_ConsoleGame
{
    public class Gameboard
    {
        #region ENUMS

        public enum PlayerPiece
        {
            X,
            O,
            None
        }

        public enum GameState
        {
            NewGame,
            PlayerXTurn,
            PlayerOTurn,
            PlayerXWin,
            PlayerOWin,
            CatsGame
        }

        #endregion

        #region FIELDS

        private const int MAX_NUM_OF_ROWS_COLUMNS = 3;

        private PlayerPiece[,] _positionState;

        private GameState _currentGameState;

        #endregion

        #region PROPERTIES

        public int MaxNumOfRowsColumns
        {
            get { return MAX_NUM_OF_ROWS_COLUMNS; }
        }

        public PlayerPiece[,] PositionState
        {
            get { return _positionState; }
            set { _positionState = value; }
        }

        public GameState CurrentGameState
        {
            get { return _currentGameState; }
            set { _currentGameState = value; }
        }
        #endregion

        #region CONSTRUCTORS

        public Gameboard()
        {
            _positionState = new PlayerPiece[MAX_NUM_OF_ROWS_COLUMNS, MAX_NUM_OF_ROWS_COLUMNS];

            _currentGameState = GameState.NewGame;

            InitializeGameboard();
        }

        #endregion

        #region METHODS

        public void InitializeGameboard()
        {
            //
            // Set all PlayerPiece array values to "None"
            //
            for (int row = 0; row < 3; row++)
            {
                for (int column = 0; column < 3; column++)
                {
                    _positionState[row, column] = PlayerPiece.None;
                }
            }
        }

        public bool IsValidMove(GameboardPosition gameboardPosition)
        {
            //
            // Confirm that the board position is empty
            // Note: gameboardPosition converted to array index by subtracting 1
            //
            if (_positionState[gameboardPosition.Row - 1, gameboardPosition.Column - 1] == PlayerPiece.None)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void UpdateGameboardState()
        {
            //
            // All positions filled
            //
            if (IsCatsGame())
            {
                _currentGameState = GameState.CatsGame;
            }
            //
            // A player X has won
            //
            else if (ThreeInARow(PlayerPiece.X))
            {
                _currentGameState = GameState.PlayerXWin;
            }
            //
            // A player O has won
            //
            else if (ThreeInARow(PlayerPiece.O))
            {
                _currentGameState = GameState.PlayerOWin;
            }
            //
            // Player O's turn
            //
            else if (_currentGameState == GameState.PlayerXTurn)
            {
                _currentGameState = GameState.PlayerOTurn;
            }
            //
            // Player X's turn
            //
            else if (_currentGameState == GameState.PlayerOTurn)
            {
                _currentGameState = GameState.PlayerXTurn;
            }
            //
            // New game (start with player X)
            //
            else
            {
                _currentGameState = GameState.PlayerXTurn;
            }
        }

        public bool IsCatsGame()
        {
            //
            // All positions on board are filled and no winner
            //
            for (int row = 0; row < 3; row++)
            {
                for (int column = 0; column < 3; column++)
                {
                    if (_positionState[row, column] == PlayerPiece.None)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool ThreeInARow(PlayerPiece playerPieceToCheck)
        {
            //
            // Check rows for player win
            //
            for (int row = 0; row < 3; row++)
            {
                if (_positionState[row, 0] == playerPieceToCheck &&
                    _positionState[row, 1] == playerPieceToCheck &&
                    _positionState[row, 2] == playerPieceToCheck)
                {
                    return true;
                }
            }

            //
            // Check columns for player win
            //
            for (int column = 0; column < 3; column++)
            {
                if (_positionState[0, column] == playerPieceToCheck &&
                    _positionState[1, column] == playerPieceToCheck &&
                    _positionState[2, column] == playerPieceToCheck)
                {
                    return true;
                }
            }

            //
            // Check diagonals for player win
            //
            if (
                (_positionState[0, 0] == playerPieceToCheck &&
                _positionState[1, 1] == playerPieceToCheck &&
                _positionState[2, 2] == playerPieceToCheck)
                ||
                (_positionState[0, 2] == playerPieceToCheck &&
                _positionState[1, 1] == playerPieceToCheck &&
                _positionState[2, 0] == playerPieceToCheck)
                )
            {
                return true;
            }

            //
            // No Player Has Won
            //

            return false;
        }

        /// <summary>
        /// add player's move to the game board
        /// </summary>
        /// <param name="gameboardPosition"></param>
        /// <param name="PlayerPiece"></param>
        public void SetPlayerPiece(GameboardPosition gameboardPosition, PlayerPiece PlayerPiece)

        {
            //
            // Row and column value adjusted to match array structure
            // Note: gameboardPosition converted to array index by subtracting 1
            //
            _positionState[gameboardPosition.Row - 1, gameboardPosition.Column - 1] = PlayerPiece;
        }

        #endregion
    }
}
