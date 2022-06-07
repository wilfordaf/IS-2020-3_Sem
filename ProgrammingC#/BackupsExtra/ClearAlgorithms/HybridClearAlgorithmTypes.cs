namespace BackupsExtra.ClearAlgorithms
{
    public enum HybridClearAlgorithmTypes
    {
        /// <summary>
        /// Adds restore point to deletion list, if it does not satisfy any of given algorithms.
        /// </summary>
        DoesNotSatisfyAny,

        /// <summary>
        /// Adds restore point to deletion list, if it does not satisfy all of given algorithms.
        /// </summary>
        DoesNotSatisfyAll,
    }
}