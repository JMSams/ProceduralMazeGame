namespace FallingSloth.ProceduralMazeGenerator
{
    [System.Flags]
    public enum Directions : byte
    {
        None    = 0,
        North   = 1,
        East    = 2,
        South   = 4,
        West    = 8
    }
}