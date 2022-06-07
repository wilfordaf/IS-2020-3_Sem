namespace Reports.UI.ConsoleInterface
{
    public enum Commands
    {
        /// <summary>
        /// 
        /// </summary>
        PrintInfo,

        /// <summary>
        /// 
        /// </summary>
        StaffLogIn,

        /// <summary>
        /// 
        /// </summary>
        StaffLogOut,

        /// <summary>
        /// 
        /// </summary>
        TeamLeadLogIn,

        /// <summary>
        /// 
        /// </summary>
        TeamLeadLogOut,

        /// <summary>
        /// 
        /// </summary>
        CreateStaffAccount,

        /// <summary>
        /// 
        /// </summary>
        CreateTeamLeadAccount,

        /// <summary>
        /// 
        /// </summary>
        PrintAllMembers,

        /// <summary>
        /// 
        /// </summary>
        GetMemberById,

        GetAllTasks,

        GetTaskById,

        GetTasksByCreationTime,

        GetTasksByLastEditTime,

        GetTasksMemberMadeChanges,

        GetEmployeeTasksOfMember,

        GetEmployeeTasksOfTeamLead,

        AddTask,

        ChangeTask,

        CompleteTask,

        AddComment,

        ChangeResponsibleMember,

        SaveReport,

        CreateSprintReport,
    }
}