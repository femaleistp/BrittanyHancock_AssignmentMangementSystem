
using System;

namespace AssignmentManagement.Core
{
    public class AssignmentFormatter : IAssignmentFormatter
    {
        public string Format(Assignment assignment)
        {
            string status = GetStatus(assignment);
            return $"[{assignment.Id}] {assignment.Title} - {status}";
        }

        private string GetStatus(Assignment assignment)
        {
            if (assignment.IsCompleted)
                return "Completed";
            if (assignment.IsOverdue())
                return "Overdue";
            return "Incomplete";
        }
    }
}
