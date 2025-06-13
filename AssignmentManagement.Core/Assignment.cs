
namespace AssignmentManagement.Core
{
    public class Assignment
    {
        public int Id { get; set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTime? DueDate { get; private set; }
        public AssignmentPriority Priority { get; private set; }
        public bool IsCompleted { get; private set; }
        public string Notes { get; private set; }

        public Assignment(string title, string description, DateTime? dueDate, AssignmentPriority priority, string notes = "")
        {
            if (!Enum.IsDefined(typeof(AssignmentPriority), priority))
                throw new ArgumentException("Invalid priority specified.");

            if (string.IsNullOrWhiteSpace(title)) 
                throw new ArgumentException("Title cannot be blank.", nameof(title));

            // BUG-2025-356: Check for blank description
            if (string.IsNullOrWhiteSpace(description)) 
                throw new ArgumentException("Description cannot be blank.", nameof(description));


            // BUG-2025-356 Enum validation in assignment constructor
            if (!Enum.IsDefined(typeof(AssignmentPriority), priority))  
            {
                throw new ArgumentException($"Invalid priority value: {priority}", nameof(priority));
            }

            Title = title;
            Description = description;
            DueDate = dueDate;
            Priority = priority;
            Notes = notes;  // BUG-2025-341: Notes not assigned in constructor
            IsCompleted = false;
        }

        public void Update(string newTitle, string newDescription)
        {
            if (string.IsNullOrWhiteSpace(newDescription))
                throw new ArgumentException("Description cannot be blank.", nameof(newDescription));

            Title = newTitle;
            Description = newDescription;
        }

        public void MarkComplete()
        {
            IsCompleted = true;
        }

        public bool IsOverdue()
        {
            return DueDate.HasValue && !IsCompleted && DueDate.Value.Date < DateTime.Today; // BUG-2025-343: Logic error in IsOverdue for completed assignments
        }

        public override string ToString()
        {
            var result = $"- {Title} ({Priority}) due {DueDate?.ToShortDateString() ?? "N/A"}\n{Description}";
            if (!string.IsNullOrWhiteSpace(Notes))  // BUG-2025-342: Notes not included in ToString
            {
                result += $"\nNotes: {Notes}";
            }
            return result;
        }
    }

    public enum AssignmentPriority
    {
        Low,
        Medium,
        High
    }
}
