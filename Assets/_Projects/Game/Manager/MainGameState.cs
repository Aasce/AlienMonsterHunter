namespace Asce.MainGame.Managers
{
    public enum MainGameState
    {
        None = 0,
        Initialize = 1,
        Creating = 2,
        Loading = 3,

        Playing = 5,
        Pausing = 6,

        Completed = 10,
        Failed = 11,

        Exiting = 15,
    }
}