using System;
using System.Collections.Generic;
using System.Linq;

namespace AssignmentManagement.Core
{
    public class AssignmentService : IAssignmentService
    {
        private readonly List<Assignment> _assignments = new();
        private readonly IAssignmentFormatter _formatter;
        private readonly IAppLogger _logger;

        public AssignmentService(IAssignmentFormatter formatter, IAppLogger logger)
        {
            _formatter = formatter;
            _logger = logger;
        }

        public virtual bool AddAssignment(Assignment assignment)
        {
            try
            {
                if (_assignments.Any(a=>a.Title.Equals(assignment.Title, StringComparison.OrdinalIgnoreCase)))
                {
                    _logger.Log($"Duplicate assignment title rejected: {assignment.Title}");
                    return false;
                }
                _assignments.Add(assignment);
                LogAssignmentAction("Added", assignment);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Log($"Error adding assignment: {ex.Message}");
                return false;
            }
        }

        public bool DeleteAssignment(string title)
        {
            var toRemove = _assignments.FirstOrDefault(a => a.Title == title);
            if (toRemove != null)
            {
                _assignments.Remove(toRemove);
                LogAssignmentAction("Deleted", toRemove);
                return true;
            }
            return false;
        }

        public List<Assignment> ListAll() => _assignments;

        public List<Assignment> ListIncomplete() => _assignments.Where(a => !a.IsCompleted).ToList();

        public List<string> ListFormatted() => _assignments.Select(a => _formatter.Format(a)).ToList();

        public Assignment FindByTitle(string title) => _assignments.FirstOrDefault(a => a.Title == title);

        public bool UpdateAssignment(string title, string newTitle, string newDescription)
        {
            var assignment = FindByTitle(title);
            if (assignment != null)
            {
                assignment.Update(newTitle, newDescription);
                return true;
            }
            return false;
        }

        public bool MarkComplete(string title)
        {
            var assignment = FindByTitle(title);
            if (assignment != null)
            {
                assignment.MarkComplete();
                return true;
            }
            return false;
        }

        public Assignment FindAssignmentByTitle(string title)
        {
            return _assignments.FirstOrDefault(a => a.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        }

        public bool MarkAssignmentComplete(string title)
        {
            var assignment = FindAssignmentByTitle(title);
            if (assignment != null)
            {
                assignment.MarkComplete();
                LogAssignmentAction("Marked complete", assignment);
                return true;
            }

            _logger.Log($"Attempted to mark complete but assignment not found: {title}");
            return false;
        }

        // BUG-2025-344: No logging when overdue status is checked
        public bool CheckIfOverdue(string title)
        {
            var assignment = FindByTitle(title);
            if (assignment != null)
            {
                var isOverdue = assignment.IsOverdue();
                LogAssignmentAction("Checked if", assignment, $"is overdue: {isOverdue}");
                return isOverdue;
            }

            _logger.Log($"Attempted overdue check on non-existent assignment: {title}");
            return false;
        }

        // Helper method to log assignment actions
        private void LogAssignmentAction(string action, Assignment assignment, string? extraInfo = null) 
        {
            var message = $"{action} Assignment [{assignment.Id}]: {assignment.Title}";
            if (!string.IsNullOrWhiteSpace(extraInfo))
                message += $" - {extraInfo}";
            _logger.Log(message);
        }
    }
}