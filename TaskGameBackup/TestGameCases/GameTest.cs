using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Game.Tests
{
    [TestClass]
    public class GameBoardTests
    {
        [TestMethod]
        public void Initialize_BoardInitializedWithSpacesExceptPlayerPosition()
        {
            // Arrange
            int boardSize = 5;
            ITaskGameBoard gameBoard = new TaskGameBoard(boardSize);

            // Act (Call Initialize)
            gameBoard.Initialize();

            // Assert (After Initialize)
            char[,] board = gameBoard.Board;

            // Check that the player's initial position is 'P'
            Assert.AreEqual('P', board[0, 0]);

            // Check that all other positions are spaces
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    if (i != 0 || j != 0)
                    {
                        Assert.AreEqual(' ', board[i, j]);
                    }
                }
            }
        }

        [TestMethod]
        public void PlaceMines_MinesArePlaced()
        {
            // Arrange
            int boardSize = 5;
            ITaskGameBoard gameBoard = new TaskGameBoard(boardSize);

            // Act
            gameBoard.PlaceMines();

            // Assert
            char[,] board = gameBoard.Board;
            int mineCount = 0;
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    if (board[i, j] == 'X')
                    {
                        mineCount++;
                    }
                }
            }

            Assert.AreEqual(boardSize, mineCount); // Assuming you want one mine per row
        }

        [TestMethod]
        public void IsValidMove_ValidMove_ReturnsTrue()
        {
            // Arrange
            int boardSize = 5;
            ITaskGameBoard gameBoard = new TaskGameBoard(boardSize);

            // Act
            bool isValidMove = gameBoard.IsValidMove(1, 1);

            // Assert
            Assert.IsTrue(isValidMove);
        }

        [TestMethod]
        public void IsValidMove_InvalidMove_ReturnsFalse()
        {
            // Arrange
            int boardSize = 5;
            ITaskGameBoard gameBoard = new TaskGameBoard(boardSize);

            // Act
            bool isValidMove = gameBoard.IsValidMove(-1, -1);

            // Assert
            Assert.IsFalse(isValidMove);
        }

        [TestMethod]
        public void FindPlayerPosition_PlayerPositionFound_ReturnsPosition()
        {
            // Arrange
            int boardSize = 5;
            ITaskGameBoard gameBoard = new TaskGameBoard(boardSize);
            gameBoard.Initialize();

            // Act
            var playerPosition = gameBoard.FindPlayerPosition();

            // Assert
            Assert.IsNotNull(playerPosition);
            Assert.AreEqual(0, playerPosition.Item1);
            Assert.AreEqual(0, playerPosition.Item2);
        }

  
        [TestMethod]
        public void MovePlayer_ValidMove_PlayerPositionUpdated()
        {
            // Arrange
            int boardSize = 5;
            ITaskGameBoard gameBoard = new TaskGameBoard(boardSize);
            gameBoard.Initialize();

            // Act
            gameBoard.MovePlayer(1, 1);
            var playerPosition = gameBoard.FindPlayerPosition();

            // Assert
            Assert.IsNotNull(playerPosition);
            Assert.AreEqual(1, playerPosition.Item1);
            Assert.AreEqual(1, playerPosition.Item2);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void MovePlayer_InvalidMove_ThrowsInvalidOperationException()
        {
            // Arrange
            int boardSize = 5;
            ITaskGameBoard gameBoard = new TaskGameBoard(boardSize);
            gameBoard.Initialize();

            // Act (Try to move outside the board)
            gameBoard.MovePlayer(-1, -1);

            // Assert (Expecting an InvalidOperationException to be thrown)
        }

        // Add more test methods for other TaskGameBoard methods as needed.
    }
}
