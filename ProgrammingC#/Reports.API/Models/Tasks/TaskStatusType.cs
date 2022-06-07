namespace Reports.API.Models.Tasks
{
    public enum TaskStatusType
    {
        /// <summary>
        /// Task is currently unsolved and not in progress
        /// </summary>
        Open,

        /// <summary>
        /// Task is currently in progress by some member
        /// </summary>
        Active,

        /// <summary>
        /// Task is already solved and closed for modification
        /// </summary>
        Resolved,
    }
}