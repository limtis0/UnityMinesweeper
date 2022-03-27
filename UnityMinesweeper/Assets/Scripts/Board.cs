using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }

    public Tile tileUnknown;
    public Tile tileEmpty;
    public Tile tileMine;
    public Tile tileExploded;
    public Tile tileFlag;
    public Tile tileNum1;
    public Tile tileNum2;
    public Tile tileNum3;
    public Tile tileNum4;
    public Tile tileNum5;
    public Tile tileNum6;
    public Tile tileNum7;
    public Tile tileNum8;

    public Tile tileUnknownDark;
    public Tile tileEmptyDark;
    public Tile tileMineDark;
    public Tile tileExplodedDark;
    public Tile tileFlagDark;
    public Tile tileNum1Dark;
    public Tile tileNum2Dark;
    public Tile tileNum3Dark;
    public Tile tileNum4Dark;
    public Tile tileNum5Dark;
    public Tile tileNum6Dark;
    public Tile tileNum7Dark;
    public Tile tileNum8Dark;

    private bool darkTheme;

    private void Awake()
    {
        darkTheme = PlayerPrefs.GetInt("nightTheme", 0) == 1;
        tilemap = GetComponent<Tilemap>();
    }

    public void Draw(Cell[,] state)
    {
        int width = state.GetLength(0);
        int height = state.GetLength(1);
        Cell cell;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                cell = state[x, y];
                tilemap.SetTile(cell.position, GetTile(cell));
            }
        }
    }

    private Tile GetTile(Cell cell)
    {
        if (!darkTheme)
        {
            if(cell.revealed) 
            {
                return GetRevealedTile(cell);
            } 
            else if (cell.flagged) 
            {
                return tileFlag;
            } 
            else 
            {
                return tileUnknown;
            }
        }
        else
        {
            if(cell.revealed) 
            {
                return GetRevealedTileDark(cell);
            } 
            else if (cell.flagged) 
            {
                return tileFlagDark;
            } 
            else 
            {
                return tileUnknownDark;
            }
        }
    }

    private Tile GetRevealedTile(Cell cell)
    {
        switch (cell.type)
        {
            case Cell.Type.Empty: return tileEmpty;
            case Cell.Type.Mine: return cell.exploded ? tileExploded : tileMine;
            case Cell.Type.Number: return GetNumberTile(cell);
            default: return null;
        }
    }

    private Tile GetRevealedTileDark(Cell cell)
    {
        switch (cell.type)
        {
            case Cell.Type.Empty: return tileEmptyDark;
            case Cell.Type.Mine: return cell.exploded ? tileExplodedDark : tileMineDark;
            case Cell.Type.Number: return GetNumberTileDark(cell);
            default: return null;
        }
    }

    private Tile GetNumberTile(Cell cell)
    {
        switch(cell.number)
        {
            case 1: return tileNum1;
            case 2: return tileNum2;
            case 3: return tileNum3;
            case 4: return tileNum4;
            case 5: return tileNum5;
            case 6: return tileNum6;
            case 7: return tileNum7;
            case 8: return tileNum8;
            default: return null;
        }
    }

    private Tile GetNumberTileDark(Cell cell)
    {
        switch(cell.number)
        {
            case 1: return tileNum1Dark;
            case 2: return tileNum2Dark;
            case 3: return tileNum3Dark;
            case 4: return tileNum4Dark;
            case 5: return tileNum5Dark;
            case 6: return tileNum6Dark;
            case 7: return tileNum7Dark;
            case 8: return tileNum8Dark;
            default: return null;
        }
    }
}
