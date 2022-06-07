using System;
using System.Collections.Generic;
using Reports.API.Models.Team;
using Reports.API.Models.Accounts;
using Reports.API.Models.Members;
using Reports.API.Models.Reports;
using Reports.API.Models.Tasks;
using Reports.API.Tools;

namespace Reports.UI.ConsoleInterface
{
    public class ConsoleInterface
    {
        private Team _team;

        public ConsoleInterface()
        {
            CommandList = new Dictionary<Commands, Action<string[]>>()
            {
                [Commands.PrintInfo] = PrintInfo,
                [Commands.CreateStaffAccount] = CreateStaffAccount,
                [Commands.StaffLogIn] = StaffLogIn,
                [Commands.StaffLogOut] = StaffLogOut,
                [Commands.TeamLeadLogIn] = TeamLeadLogIn,
                [Commands.TeamLeadLogOut] = TeamLeadLogOut,
                [Commands.CreateTeamLeadAccount] = CreateTeamLeadAccount,
                [Commands.PrintAllMembers] = PrintAllMembers,
                [Commands.GetMemberById] = GetMemberById,
                [Commands.GetAllTasks] = GetAllTasks,
                [Commands.GetTaskById] = GetTaskById,
                [Commands.GetTasksByCreationTime] = GetTasksByCreationTime,
                [Commands.GetTasksByLastEditTime] = GetTasksByLastEditTime,
                [Commands.GetTasksMemberMadeChanges] = GetTasksMemberMadeChanges,
                [Commands.GetEmployeeTasksOfMember] = GetEmployeeTasksOfMember,
                [Commands.GetEmployeeTasksOfTeamLead] = GetEmployeeTasksOfTeamLead,
                [Commands.AddTask] = AddTask,
                [Commands.AddComment] = AddComment,
                [Commands.ChangeTask] = ChangeTask,
                [Commands.CompleteTask] = CompleteTask,
                [Commands.ChangeResponsibleMember] = ChangeResponsibleMember,
                [Commands.SaveReport] = SaveReport,
                [Commands.CreateSprintReport] = CreateSprintReport,
            };
        }

        public Dictionary<Commands, Action<string[]>> CommandList { get; set; }

        private StaffMemberAccount _staffAccount;

        private TeamLeadAccount _teamLeadAccount;

        public Team Team { get; set; }

        public void Start()
        {
            _team = Team.LoadFromDatabase();

            string line;
            while ((line = Console.ReadLine()) != string.Empty)
            {
                if (line is null || line[0] != '/' || line.Length == 1)
                {
                    Console.WriteLine("Incorrect input - not a command.");
                    continue;
                }

                if (line.Split(' ')[0] == "/Exit")
                {
                    break;
                }

                Commands command;

                try
                {
                    command = Parse(line[1..].Split(' ')[0]);
                }
                catch (ReportsException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                string[] args = line.Split(' ')[1..];

                try
                {
                    ExecuteCommand(command, args);
                }
                catch (IndexOutOfRangeException)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Invalid parameters.");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                catch (ReportsException e)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine($"Command execution failed: {e.Message}");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                catch (FormatException)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Invalid type of input parameters.");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
            }

            Console.WriteLine("Saving to database...");
            _team.SaveToDatabase();
        }

        private static Commands Parse(string name)
        {
            if (Enum.TryParse(name, out Commands command))
            {
                return command;
            }

            throw new ReportsException("Invalid command.");
        }

        private void ExecuteCommand(Commands command, string[] args)
        {
            CommandList[command].Invoke(args);
        }

        private void PrintInfo(string[] args)
        {
            if (_staffAccount is null)
            {
                if (_teamLeadAccount is null)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Account to act was not chosen.");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    return;
                }
                Console.WriteLine($"Team lead {_teamLeadAccount.Owner.Name} with id {_teamLeadAccount.Owner.Id}");
                return;
            }

            Console.WriteLine($"Staff member {_staffAccount.Owner.Name} with id {_staffAccount.Owner.Id}");

        }

        private void CreateStaffAccount(string[] args)
        {
            string login = args[0];
            string password = args[1];

            Console.WriteLine("Input username:");
            string name = Console.ReadLine();
            Console.WriteLine("Input chiefId:");
            string chiefId = Console.ReadLine();
            var staffMember = new StaffMember(name, chiefId);
            _team.AddStaffMemberAccount(staffMember, login, password);

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"Succesfully created staff account with id {staffMember.Id}");
            Console.ForegroundColor = ConsoleColor.Gray;

        }

        private void CreateTeamLeadAccount(string[] args)
        {
            string login = args[0];
            string password = args[1];

            Console.WriteLine("Input TeamLead name:");
            string name = Console.ReadLine();
            var teamleadMember = new TeamLeadMember(name);
            _team.SetTeamLeadAccount(teamleadMember, login, password);

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"Succesfully created TeamLead account with id {_team.TeamLead.Id}");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private void StaffLogIn(string[] args)
        {
            if (_staffAccount is not null || _teamLeadAccount is not null)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Log out first, please");
                Console.ForegroundColor = ConsoleColor.Gray;
                return;
            }
            _staffAccount = _team.GetStaffMemberAccount(args[0], args[1]);
            _staffAccount.LogIn();

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"Working as {_staffAccount.Id}");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private void StaffLogOut(string[] args)
        {
            _staffAccount.LogOut();

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"Logged out from {_staffAccount.Id}");
            Console.ForegroundColor = ConsoleColor.Gray;

            _staffAccount = null;

        }

        private void TeamLeadLogIn(string[] args)
        {
            if (_staffAccount is not null || _teamLeadAccount is not null)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Log out first, please");
                Console.ForegroundColor = ConsoleColor.Gray;
                return;
            }

            _teamLeadAccount = _team.GetTeamLeadAccount(args[0], args[1]);
            _teamLeadAccount.LogIn();

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"Working as {_teamLeadAccount.Id}");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private void TeamLeadLogOut(string[] args)
        {
            _teamLeadAccount.LogOut();

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"Logged out from {_teamLeadAccount.Id}");
            Console.ForegroundColor = ConsoleColor.Gray;

            _teamLeadAccount = null;
        }

        private void PrintAllMembers(string[] args)
        {
            Console.WriteLine(_team.PrintAllMembers());
        }

        private void GetMemberById(string[] args)
        {
            StaffMember staffMember = _team.GetMemberById(args[0]).Owner;
            Console.WriteLine($"Staff member {staffMember.Name} with id {staffMember.Id}");
        }

        private void GetAllTasks(string[] args)
        {
            _team.TaskManager.Tasks.ForEach(t => Console.WriteLine($"{t.CreationTime} {t.Essense} {t.Id}"));
        }

        private void GetTaskById(string[] args)
        {
            Task t =_team.TaskManager.FindTaskById(args[0]);
            Console.WriteLine($"{t.Essense} {t.Id}");
        }

        private void GetTasksByCreationTime(string[] args)
        {
            _team.TaskManager.FindTasksByCreationTime(DateTime.Parse(args[0])).
                ForEach(t => Console.WriteLine($"{t.Essense} {t.Id}"));
        }

        private void GetTasksByLastEditTime(string[] args)
        {
            _team.TaskManager.FindTaskByLastChangeTime(DateTime.Parse(args[0])).
                ForEach(t => Console.WriteLine($"{t.Essense} {t.Id}"));
        }

        private void GetTasksMemberMadeChanges(string[] args)
        {
            _team.TaskManager.FindTasksMemberMadeChanges(args[0]).
                ForEach(t => Console.WriteLine($"{t.Essense} {t.Id}"));
        }

        private void GetEmployeeTasksOfMember(string[] args)
        {
            StaffMember staffMember = _team.GetMemberById(args[0]).Owner;
            _team.TaskManager.GetAllEmployeersTasksOfMember(staffMember).
                ForEach(t => Console.WriteLine($"{t.Essense} {t.Id}"));
        }

        private void GetEmployeeTasksOfTeamLead(string[] args)
        {
            _team.TaskManager.GetAllEmployeersTasksOfTeamLead(_team.TeamLead.Owner).
                ForEach(t => Console.WriteLine($"{t.Essense} {t.Id}"));
        }

        private void AddTask(string[] args)
        {
            if (_staffAccount is not null)
            {
                var task = new Task(args[0]);
                _staffAccount.AddTask(task);
                task.ResponsibleMember = _staffAccount.Owner;
                _team.TaskManager.Tasks.Add(task);

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"Added {task.Essense} with {task.Id} to {_staffAccount.Id}");
                Console.ForegroundColor = ConsoleColor.Gray;
                return;
            }

            if (_teamLeadAccount is not null)
            {
                Task task = _team.TaskManager.CreateTask(args[0]);

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"Added {task.Essense} with {task.Id}");
                Console.ForegroundColor = ConsoleColor.Gray;
                return;
            }

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Account to act was not chosen.");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private void ChangeTask(string[] args)
        {
            Task task = _team.TaskManager.FindTaskById(args[0]);
            if (task is null)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Task with this id was not found");
                Console.ForegroundColor = ConsoleColor.Gray;
                return;
            }

            _team.TaskManager.ChangeTaskEssense(args[1], task);
        }

        private void CompleteTask(string[] args)
        {
            if (_staffAccount is null)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("There is nothing to complete");
                Console.ForegroundColor = ConsoleColor.Gray;
                return;
            }

            Task task = _team.TaskManager.FindTaskById(args[0]);
            if (task is null)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Task with this id was not found");
                Console.ForegroundColor = ConsoleColor.Gray;
                return;
            }

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"Task {task.Id} completed");
            Console.ForegroundColor = ConsoleColor.Gray;
            _staffAccount.CompleteTask(task);
            
        }

        private void AddComment(string[] args)
        {
            Task task = _team.TaskManager.FindTaskById(args[0]);
            if (task is null)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Task with this id was not found");
                Console.ForegroundColor = ConsoleColor.Gray;
                return;
            }

            Console.WriteLine(_staffAccount);

            if (_staffAccount is not null)
            {
                task.AddComment(args[1], _staffAccount.Id.ToString());
                return;
            }

            if (_teamLeadAccount is not null)
            {
                task.AddComment(args[1], _teamLeadAccount.Id.ToString());
                return;
            }

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Account to act was not chosen.");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private void ChangeResponsibleMember(string[] args)
        {
            Task task = _team.TaskManager.FindTaskById(args[0]);
            if (task is null)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Task with this id was not found");
                Console.ForegroundColor = ConsoleColor.Gray;
                return;
            }

            StaffMember staffMember = _team.GetMemberById(args[1]).Owner;
            if (staffMember is null)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Member with this id was not found");
                Console.ForegroundColor = ConsoleColor.Gray;
                return;
            }

            if (_staffAccount is not null)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("You do not have permission for this operation");
                Console.ForegroundColor = ConsoleColor.Gray;
                return;
            }

            if (_teamLeadAccount is not null)
            {
                _team.TaskManager.ChangeTaskResponsibleMember(staffMember, task);
                return;
            }

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Account to act was not chosen.");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private void CreateSprintReport(string[] args)
        {
            if (_staffAccount is not null)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("You do not have permission for this operation");
                Console.ForegroundColor = ConsoleColor.Gray;
                return;
            }

            if (_teamLeadAccount is not null)
            {
                SprintReport report = _team.InitializeReportFormation();

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("Report successfully created");
                Console.ForegroundColor = ConsoleColor.Gray;

                report.Data.ForEach(l => Console.WriteLine(l));
                _teamLeadAccount.NotifyEmployeersNewSprint();
            }
        }

        private void SaveReport(string[] args)
        {
            if (_staffAccount is null)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("You do not have permission for this operation");
                Console.ForegroundColor = ConsoleColor.Gray;
                return;
            }

            _staffAccount.Owner.SprintReport.ActionsDone.ForEach(l => Console.WriteLine(l));
            _staffAccount.Owner.SprintReport.Save();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"Report of member {_staffAccount.Id} successfully saved");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}