namespace FHPathfinder.RecognitionService;

[Flags]
public enum DebugOptions
{
    ShowNothing = 0,
    ShowEveryIcons = 1,
    ShowAllIcons = 1 << 1,
    ShowAllNumberFields = 1 << 2,
    ShowStorage = 1 << 3,
}