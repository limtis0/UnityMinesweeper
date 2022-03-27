using UnityEngine;

public struct Cell
{
    public enum Type
    {
        Invalid, // Default value
        Empty,
        Mine,
        Number,
    }

    public Type type;
    public Vector3Int position; // Use Vector3Int with tilemaps
    public int number;
    public bool revealed;
    public bool flagged;
    public bool exploded;
}
