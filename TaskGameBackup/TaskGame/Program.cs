// Single Responsibility Principle (SRP) - Each class has a single responsibility
public interface ITaskGameBoard
{
    char[,] Board { get; }
    int Size { get; }
    void Initialize();
    void PlaceMines();
    bool IsValidMove(int row, int col);
    Tuple<int, int> FindPlayerPosition();
    void MovePlayer(int row, int col);
}

public class TaskGameBoard : ITaskGameBoard
{
    public char[,] Board { get; private set; }
    public int Size { get; }

    public TaskGameBoard(int size)
    {
        Size = size;
        Initialize();
    }

    public void Initialize()
    {
        Board = new char[Size, Size];
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                Board[i, j] = ' ';
            }
        }
        Board[0, 0] = 'P'; // Initial player position
    }

    public void PlaceMines()
    {
        Random random = new Random();
        int minesPlaced = 0;
        while (minesPlaced < Size)
        {
            int row = random.Next(Size);
            int col = random.Next(Size);
            if (Board[row, col] != 'X' && Board[row, col] != 'P')
            {
                Board[row, col] = 'X';
                minesPlaced++;
            }
        }
    }

    public bool IsValidMove(int row, int col)
    {
        return row >= 0 && row < Size && col >= 0 && col < Size;
    }

    public Tuple<int, int> FindPlayerPosition()
    {
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                if (Board[i, j] == 'P')
                {
                    return Tuple.Create(i, j);
                }
            }
        }
        return null; // Player not found
    }

    public void MovePlayer(int row, int col)
    {
        Tuple<int, int> playerPosition = FindPlayerPosition();

        if (playerPosition != null)
        {
            int currentRow = playerPosition.Item1;
            int currentCol = playerPosition.Item2;

            if (IsValidMove(row, col))
            {
                Board[currentRow, currentCol] = ' ';
                Board[row, col] = 'P';
            }
            else
            {
                throw new InvalidOperationException("Invalid move.");
            }
        }
        else
        {
            throw new InvalidOperationException("Player not found on the board.");
        }
    }
}

// Open-Closed Principle (OCP) - Code is open for extension but closed for modification
public interface ITaskGame
{
    void Play();
}

public class TaskMinesweeperGame : ITaskGame
{
    private readonly ITaskGameBoard gameBoard;
    private int numLives;
    private int moves;

    public TaskMinesweeperGame(ITaskGameBoard board, int lives)
    {
        gameBoard = board;
        numLives = lives;
        moves = 0;
    }

    public void Play()
    {
        gameBoard.PlaceMines();

        while (true)
        {
            PrintBoard();
            string pmove =null; 
            string move;
            if (pmove == null)
            {
                Console.Write("Enter your move (up, down, left, right, or quit): ");
                move = Console.ReadLine();
            }
            else
            {
                move = pmove;
            }
            if (move.ToLower() == "quit")
            {
                Console.WriteLine("Game over! You quit.");
                break;
            }

            if (Array.Exists(new string[] { "up", "down", "left", "right" }, element => element == move.ToLower()))
            {
                Tuple<int, int> playerPosition = gameBoard.FindPlayerPosition();
                int currentRow = playerPosition.Item1;
                int currentCol = playerPosition.Item2;

                int deltaRow = 0, deltaCol = 0;

                if (move.ToLower() == "up")
                {
                    deltaRow = -1;
                }
                else if (move.ToLower() == "down")
                {
                    deltaRow = 1;
                }
                else if (move.ToLower() == "left")
                {
                    deltaCol = -1;
                }
                else if (move.ToLower() == "right")
                {
                    deltaCol = 1;
                }

                int newRow = currentRow + deltaRow;
                int newCol = currentCol + deltaCol;

                try
                {
                    gameBoard.MovePlayer(newRow, newCol);
                    moves++;
                    if (gameBoard.Board[newRow, newCol] == 'X')
                    {
                        numLives--;
                        if (numLives <= 0)
                        {
                            Console.WriteLine("Game over! You hit a mine.");
                            break;
                        }
                    }
                    else if (newRow == gameBoard.Size - 1)
                    {
                        Console.WriteLine("Congratulations! You reached the other side.");
                        break;
                    }
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                Console.WriteLine("Invalid move. Please enter up, down, left, right, or quit.");
            }
        }
    }

    private void PrintBoard()
    {
        Console.Clear();
        for (int i = 0; i < gameBoard.Size; i++)
        {
            for (int j = 0; j < gameBoard.Size; j++)
            {
                Console.Write(gameBoard.Board[i, j] + " ");
            }
            Console.WriteLine();
        }
        Console.WriteLine($"Lives: {numLives}, Moves: {moves}");
    }
}

// Liskov Substitution Principle (LSP) - Subtypes can be substituted for their base types
public class MultiplayerTaskMinesweeperGame : ITaskGame
{
    private readonly ITaskGameBoard gameBoard;
    private int numLives;
    private int moves;
    private int currentPlayer;

    public MultiplayerTaskMinesweeperGame(ITaskGameBoard board, int lives, int players)
    {
        gameBoard = board;
        numLives = lives;
        moves = 0;
        currentPlayer = 1;
    }

    public void Play()
    {
        gameBoard.PlaceMines();

        while (true)
        {
            PrintBoard();
            Console.Write($"Player {currentPlayer}, enter your move (up, down, left, right, or quit): ");
            string move = Console.ReadLine();
            if (move.ToLower() == "quit")
            {
                Console.WriteLine("Game over! You quit.");
                break;
            }

            if (Array.Exists(new string[] { "up", "down", "left", "right" }, element => element == move.ToLower()))
            {
                Tuple<int, int> playerPosition = gameBoard.FindPlayerPosition();
                int currentRow = playerPosition.Item1;
                int currentCol = playerPosition.Item2;

                int deltaRow = 0, deltaCol = 0;

                if (move.ToLower() == "up")
                {
                    deltaRow = -1;
                }
                else if (move.ToLower() == "down")
                {
                    deltaRow = 1;
                }
                else if (move.ToLower() == "left")
                {
                    deltaCol = -1;
                }
                else if (move.ToLower() == "right")
                {
                    deltaCol = 1;
                }

                int newRow = currentRow + deltaRow;
                int newCol = currentCol + deltaCol;

                try
                {
                    gameBoard.MovePlayer(newRow, newCol);
                    moves++;
                    if (gameBoard.Board[newRow, newCol] == 'X')
                    {
                        numLives--;
                        if (numLives <= 0)
                        {
                            Console.WriteLine($"Player {currentPlayer} hit a mine. Player {currentPlayer} loses.");
                            break;
                        }
                    }
                    else if (newRow == gameBoard.Size - 1)
                    {
                        Console.WriteLine($"Player {currentPlayer} reached the other side and wins!");
                        break;
                    }
                    currentPlayer = (currentPlayer % 2) + 1; // Switch players
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                Console.WriteLine("Invalid move. Please enter up, down, left, right, or quit.");
            }
        }
    }

    private void PrintBoard()
    {
        Console.Clear();
        for (int i = 0; i < gameBoard.Size; i++)
        {
            for (int j = 0; j < gameBoard.Size; j++)
            {
                Console.Write(gameBoard.Board[i, j] + " ");
            }
            Console.WriteLine();
        }
        Console.WriteLine($"Player {currentPlayer} - Lives: {numLives}, Moves: {moves}");
    }
}

// Interface Segregation Principle (ISP) - Interfaces are kept minimal and clients only depend on what they use
public interface ITaskScorable
{
    int Score { get; }
    void Play(string move);
}

public class TaskScorableMinesweeperGame : ITaskScorable
{
    private readonly ITaskGameBoard gameBoard;
    private int numLives;
    private int moves;
    private int score;

    public TaskScorableMinesweeperGame(ITaskGameBoard board, int lives)
    {
        gameBoard = board;
        numLives = lives;
        moves = 0;
        score = 0;
    }

    public int Score => score;

    public void Play(string pmove)
    {
        gameBoard.PlaceMines();

        while (true)
        {
            PrintBoard();
            string move;
            if (pmove==null)
            { 
            Console.Write("Enter your move (up, down, left, right, or quit): ");
             move = Console.ReadLine();
            }
            else
            {
               move  = pmove;
            }
            if (move.ToLower() == "quit")
            {
                Console.WriteLine("Game over! You quit.");
                break;
            }

            if (Array.Exists(new string[] { "up", "down", "left", "right" }, element => element == move.ToLower()))
            {
                Tuple<int, int> playerPosition = gameBoard.FindPlayerPosition();
                int currentRow = playerPosition.Item1;
                int currentCol = playerPosition.Item2;

                int deltaRow = 0, deltaCol = 0;

                if (move.ToLower() == "up")
                {
                    deltaRow = -1;
                }
                else if (move.ToLower() == "down")
                {
                    deltaRow = 1;
                }
                else if (move.ToLower() == "left")
                {
                    deltaCol = -1;
                }
                else if (move.ToLower() == "right")
                {
                    deltaCol = 1;
                }

                int newRow = currentRow + deltaRow;
                int newCol = currentCol + deltaCol;

                try
                {
                    gameBoard.MovePlayer(newRow, newCol);
                    moves++;
                    if (gameBoard.Board[newRow, newCol] == 'X')
                    {
                        numLives--;
                        if (numLives <= 0)
                        {
                            Console.WriteLine("Game over! You hit a mine.");
                            break;
                        }
                    }
                    else if (newRow == gameBoard.Size - 1)
                    {
                        Console.WriteLine("Congratulations! You reached the other side.");
                        UpdateScore();
                        break;
                    }
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                Console.WriteLine("Invalid move. Please enter up, down, left, right, or quit.");
            }
        }
    }

    private void UpdateScore()
    {
        score = (gameBoard.Size * gameBoard.Size) - moves; // Calculate the score based on moves
    }

    private void PrintBoard()
    {
        Console.Clear();
        for (int i = 0; i < gameBoard.Size; i++)
        {
            for (int j = 0; j < gameBoard.Size; j++)
            {
                Console.Write(gameBoard.Board[i, j] + " ");
            }
            Console.WriteLine();
        }
        Console.WriteLine($"Lives: {numLives}, Moves: {moves}, Score: {score}");
    }
}

// Dependency Inversion Principle (DIP) - High-level modules depend on abstractions
class Program
{
    static void Main(string[] args)
    {
        int boardSize = 5;
        int numLives = 3;

        ITaskGameBoard board = new TaskGameBoard(boardSize);

        // Example 1: Standard Minesweeper Game
        ITaskGame standardGame = new TaskMinesweeperGame(board, numLives);
        standardGame.Play();

        // Example 2: Multiplayer Minesweeper Game
        int numPlayers = 2;
        ITaskGame multiplayerGame = new MultiplayerTaskMinesweeperGame(board, numLives, numPlayers);
        multiplayerGame.Play();

        // Example 3: Scorable Minesweeper Game
        ITaskScorable scorableGame = new TaskScorableMinesweeperGame(board, numLives);
        scorableGame.Play(null);
        Console.WriteLine($"Final Score: {scorableGame.Score}");
    }
}
