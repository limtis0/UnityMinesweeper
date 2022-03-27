using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class Game : MonoBehaviour
{
    private Board board;
    private Cell[,] state;

    private int revealMouseButton;
    private int flagMouseButton;
    
    private bool gameIsRunning;
    private bool firstMoveMade;

    private int fieldWidth;
    private int fieldHeight;
    
    private int mineCount;
    private int flagCount;

    [SerializeField] private TextMeshProUGUI minesLeft;
    [SerializeField] private TextMeshProUGUI restartText;

    private void Awake()
    {
        board = GetComponentInChildren<Board>();
    }

    private void Start() 
    {
        revealMouseButton = PlayerPrefs.GetInt("invertedControls", 0);
        flagMouseButton = 1 - revealMouseButton;

        fieldWidth = PlayerPrefs.GetInt("fieldWidth", 16);
        fieldHeight = PlayerPrefs.GetInt("fieldHeight", 16);
        mineCount = PlayerPrefs.GetInt("mineCount", 40);

        NewGame();
    }
    
    private void Update()
    {
        if (PauseMenu.gameIsPaused) { return; }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Timer.stopTimer();
            Timer.resetTime();

            NewGame();
        }
        else if (!firstMoveMade)
        {
            if (Input.GetMouseButtonDown(revealMouseButton))
            {
                GenerateMines(GetCellUnderCursor());
                GenerateNumbers();

                Timer.startTimer();
                
                Reveal(GetCellUnderCursor());


                firstMoveMade = true;
            }
        }
        else if (gameIsRunning)
        {
            // On right-click
            if (Input.GetMouseButtonDown(flagMouseButton))
            {
                Flag(GetCellUnderCursor());
            }
            // On left-click
            else if (Input.GetMouseButtonDown(revealMouseButton))
            {
                Reveal(GetCellUnderCursor());
            }
        }
    }

    private void NewGame()
    {
        state = new Cell[fieldWidth, fieldHeight];
        
        restartText.enabled = false;

        gameIsRunning = true;
        firstMoveMade = false;

        flagCount = 0;
        updateMinesLeftText(mineCount);

        GenerateCells();
        board.Draw(state);
    }



    // GENERATION
    private void GenerateCells()
    {
        Cell cell;

        for (int x = 0; x < fieldWidth; x++)
        {
            for (int y = 0; y < fieldHeight; y++)
            {
                cell = new Cell();
                cell.position = new Vector3Int(x, y, 0);
                cell.type = Cell.Type.Empty;
                state[x, y] = cell;
            }
        }
    }

    private void GenerateMines(Cell startingPoint)
    {
        int x, y;
        int sX = startingPoint.position.x;
        int sY = startingPoint.position.y;

        for (int i = 0; i < mineCount; i++)
        {
            x = Random.Range(0, fieldWidth);
            y = Random.Range(0, fieldHeight);

            // Places mine in the next field if random is already filled or the field is inside 3x3 around starting point
            while (state[x, y].type == Cell.Type.Mine || ((x >= sX - 1 && x <= sX + 1) && (y >= sY - 1 && y <= sY + 1)))
            {
                x++;
                if (x >= fieldWidth)
                { 
                    x = 0;
                    y++;
                    if (y >= fieldHeight) { y = 0; }
                }
            }

            state[x, y].type = Cell.Type.Mine;
        }
    }

    private void GenerateNumbers()
    {
        Cell cell;

        for (int x = 0; x < fieldWidth; x++)
        {
            for (int y = 0; y < fieldHeight; y++)
            {
                cell = state[x, y];
                
                if (cell.type == Cell.Type.Mine) { continue; }
                
                cell.number = CountMinesAround(cell);
                if (cell.number > 0) { cell.type = Cell.Type.Number; }

                state[x, y] = cell;
            }
        }
    }



    // ACTION HELPING FUNCTIONS
    public List<Cell> cellsAround(Cell cell)
    {
        int x, y;
        Cell temp;
        List<Cell> cells = new List<Cell>();

        for (int adjX = -1; adjX <= 1; adjX++)
        {
            for (int adjY = -1; adjY <= 1; adjY++)
            {
                if (adjX == 0 && adjY == 0) { continue; }
                
                x = cell.position.x + adjX;
                y = cell.position.y + adjY;
                temp = GetCell(x, y);

                if (temp.type != Cell.Type.Invalid)
                {
                    cells.Add(temp);
                }
            }
        }
        return cells;
    }

    private int CountMinesAround(Cell cell)
    {
        int count = 0;
        foreach (Cell temp in cellsAround(cell))
        {
            if (temp.type == Cell.Type.Mine) { count++; }
        }
        return count;
    }

    private int CountFlagsAround(Cell cell)
    {
        int count = 0;
        foreach (Cell temp in cellsAround(cell))
        {
            if (temp.flagged) { count++; }
        }
        return count;
    }

    private int CountUnrevealedAround(Cell cell)
    {
        int count = 0;
        foreach (Cell temp in cellsAround(cell))
        {
            if (!temp.revealed) { count++; }
        }
        return count;
    }



    // ACTIONS
    private void Flag(Cell cell)
    {

        if (cell.type == Cell.Type.Invalid) { return; }
        
        // Flags cells around if a number is clicked
        if (cell.type == Cell.Type.Number && cell.revealed && CountUnrevealedAround(cell) == cell.number)
        {
            FlagAround(cell);
        }
        else 
        {
            if (cell.revealed) { return; }

            if (cell.flagged)
            {
                flagCount--;
                cell.flagged = false;
            }
            else
            {
                flagCount++;
                cell.flagged = true;
            }

            state[cell.position.x, cell.position.y] = cell;
        }

        updateMinesLeftText(mineCount - flagCount);
        board.Draw(state);
    }

    private void FlagAround(Cell cell)
    {
        foreach (Cell temp in cellsAround(cell))
        {
            if (temp.type != Cell.Type.Invalid && !temp.revealed && !temp.flagged) 
            {
                flagCount++;
                state[temp.position.x, temp.position.y].flagged = true;
            }
        }
    }

    private void Reveal(Cell cell)
    {
        if (cell.type == Cell.Type.Invalid || cell.flagged) { return; }

        switch (cell.type)
        {
            case Cell.Type.Mine:
                Explode(cell);
                break;

            case Cell.Type.Empty:
                Flood(cell);
                CheckWinConditions();
                break;

            case Cell.Type.Number:
                if (!cell.revealed)
                {
                    cell.revealed = true;
                    state[cell.position.x, cell.position.y] = cell;
                    CheckWinConditions();
                }
                else if (CountFlagsAround(cell) == cell.number) // Reveals all tiles around if a number been clicked
                {
                    RevealAround(cell);
                }
                break;
        }

        board.Draw(state);
    }

    private void RevealAround(Cell cell)
    {
        foreach (Cell temp in cellsAround(cell))
        {
            if (!temp.revealed && !temp.flagged) 
            {
                Reveal(temp);
            }
        }
    }


    // GAME LOGIC
    private void Explode(Cell cell)
    {
        restartText.enabled = true;

        gameIsRunning = false;
        Timer.stopTimer();

        cell.revealed = true;
        cell.exploded = true;

        state[cell.position.x, cell.position.y] = cell;

        for (int x = 0; x < fieldWidth; x++)
        {
            for (int y = 0; y < fieldHeight; y++)
            {
                cell = state[x, y];
                if (cell.type == Cell.Type.Mine)
                {
                    cell.revealed = true;
                    state[x, y] = cell;
                }
            }
        }
    }

    // Reveals empty cells, recursively
    private void Flood(Cell cell)
    {
        if (cell.revealed || cell.type == Cell.Type.Mine || cell.type == Cell.Type.Invalid) { return; }

        cell.revealed = true;
        state[cell.position.x, cell.position.y] = cell;

        if (cell.type == Cell.Type.Empty)
        {
            foreach (Cell temp in cellsAround(cell))
            {
                Flood(temp);
            }
        }
    }

    private void CheckWinConditions()
    {
        Cell cell;

        for (int x = 0; x < fieldWidth; x++)
        {
            for (int y = 0; y < fieldHeight; y++)
            {
                cell = state[x, y];
                if (cell.type != Cell.Type.Mine && !cell.revealed) { return; }
            }
        }

        // Win
        Timer.stopTimer();
        gameIsRunning = false;
        
        // Sets all mines as flagged
        for (int x = 0; x < fieldWidth; x++)
        {
            for (int y = 0; y < fieldHeight; y++)
            {
                cell = state[x, y];
                if (cell.type == Cell.Type.Mine)
                {
                    cell.flagged = true;
                    flagCount++;

                    state[x, y] = cell; 
                }
            }
        }

        updateMinesLeftText(0);
        restartText.enabled = true;
    }

    private Cell GetCellUnderCursor()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = board.tilemap.WorldToCell(worldPosition);
        Cell cell = GetCell(cellPosition.x, cellPosition.y);

        return cell;
    }

    private Cell GetCell(int x, int y)
    {   
        // Checks whether coordintes of a cell are inside of the grid
        if (x >= 0 && x < fieldWidth && y >= 0 && y < fieldHeight)
        {
            return state[x, y];
        }
        return new Cell(); // If not: returns a Cell with Type == Invalid
    }

    private void updateMinesLeftText(int value)
    {
        minesLeft.text = "Mines left: " + value.ToString();
    }
}
