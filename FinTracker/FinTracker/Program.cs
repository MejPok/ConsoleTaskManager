
namespace FinTracker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            EntryPoint();
        }

        static void EntryPoint()
        {
            IServiceLogic logic = new ServiceLogic();
            var console = new ConsoleControl(logic);    

        }
    }

    class ConsoleControl
    {
        private readonly IServiceLogic _logic;
        public ConsoleControl(IServiceLogic logic)
        {
            _logic = logic;
            StartGreet();
            MainUIcycle();
        }

        void StartGreet()
        {
            Console.WriteLine("Welcome to my task manager");
        }

        void MainUIcycle()
        {
            while (true) {
                ListChoices();
            }
        }

        void ListChoices()
        {
            Console.WriteLine("Type a number of a choice u want to make: ");
            Console.WriteLine("1. Add Task");
            Console.WriteLine("2. Remove Task");
            Console.WriteLine("3. Show list");
            Console.WriteLine("4. Change completion of task");

            CompareChoices(GetInputInt());
        }

        void CompareChoices(int choice)
        {
            switch (choice) {
                case 1: 
                    AddTaskShow();
                    break;
                case 2:
                    RemoveTaskShow();
                    break;
                case 3:
                    ShowList();
                    break;
                case 4:
                    ChangeTaskStateShow();
                    break;
            }
        }

        void ChangeTaskStateShow()
        {
            Console.Clear();
            DisplayTasks();

            Console.WriteLine("In which task do u want to change completion?");

            int taskIdChosen = GetInputInt();

            Console.WriteLine("Do u want to set the completion to either, true or false");

            string choice = GetInputString();

            if (choice.ToLower() == "true")
            {
                _logic.MarkTaskState(taskIdChosen,true);
            } else
            {
                _logic.MarkTaskState(taskIdChosen, false);
            }

            Console.Clear();
            Console.WriteLine("Change was applied");
            Console.WriteLine();
        }


        void DisplayTasks()
        {
            foreach (TaskItem task in _logic.GetAllTasks())
            {
                Console.WriteLine(task.ToString());
            }
        }
        void DisplayTasks(List<TaskItem> tasks)
        {
            foreach (TaskItem task in tasks)
            {
                Console.WriteLine(task.ToString());
            }
        }

        void AddTaskShow()
        {
            Console.Clear();
            Console.WriteLine("Please add the task description");
            if (_logic.AddTask(GetInputString()))
            {   
                Console.Clear();
                Console.WriteLine("New Task added");
            } else
            {
                Console.WriteLine("Issue adding new task");
            }
            
        }
        void RemoveTaskShow()
        {
            Console.Clear();

            DisplayTasks();

            Console.WriteLine("Which task item do u want to delete, use the id");
            int taskIdChosen = GetInputInt();
            if (_logic.DeleteTask(taskIdChosen))
            {
                Console.Clear();
                Console.WriteLine("Task deleted");
            }
        }

        void ShowList()
        {
            Console.Clear();
            var tasks = _logic.GetAllTasks();
            if(tasks == null)
            {
                Console.WriteLine("Something went wrong");
                return;
            }

            DisplayTasks(tasks);

            if(!tasks.Any())
            {
                Console.WriteLine("No tasks found");
                
            }

            Console.WriteLine();
        }
        string GetInputString()
        {
            string input = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(input))
            {   
                Console.WriteLine("An error occured, try again");
                input = Console.ReadLine();
            }

            return input;
            
        }
        int GetInputInt()
        {
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out var result))
                    return result;

                Console.WriteLine("Invalid number, try again.");
            }
        }

    }

    class TaskItem {
        public int Id { get; }

        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public TaskItem(string description, int Id, bool isDone = false)
        {
            this.Id = Id;
            Description = description;
            IsCompleted = isDone;
        }

        public override string ToString()
        {
            return $"{Id}. Done: {IsCompleted}. {Description}.";
        }
    }


    interface IServiceLogic {
        bool AddTask(string description);
        bool DeleteTask(int taskId);
        bool MarkTaskState(int id, bool done);
        List<TaskItem> GetAllTasks();
    }





    class ServiceLogic : IServiceLogic
    {
        static int ID = 1;
        List<TaskItem> _tasks = [];

        public bool AddTask(string description)
        {
            if(description == null) return false;

            TaskItem newTask = new TaskItem(description, ID++, false);
            _tasks.Add(newTask);

            return true;
        }

        public bool DeleteTask(int taskId)
        {
            TaskItem item = _tasks.Find(x => x.Id == taskId);
            if(item == null) return false;

            _tasks.Remove(item);

            return true;
        }

        public List<TaskItem> GetAllTasks()
        {
            return _tasks;
        }

        public bool MarkTaskState(int taskId, bool done)
        {
            TaskItem item = _tasks.Find(x => x.Id == taskId);
            if(item == null) return false;

            item.IsCompleted = done;
            return true;
        }
    }


}
