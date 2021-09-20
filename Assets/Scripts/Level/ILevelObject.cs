namespace Level
{
    /// <summary>
    /// Each object in the level can be started and reset
    /// </summary>
    public interface ILevelObject
    {
        void StartSimulation();
        void ResetSimulation();
    }
}
