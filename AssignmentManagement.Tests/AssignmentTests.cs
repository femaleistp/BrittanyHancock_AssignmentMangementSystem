namespace AssignmentManagement.Tests
{
    using AssignmentManagement.Core;
    using Moq;

    public class AssignmentTests
    {
        [Fact]
        public void Constructor_ValidInput_ShouldCreateAssignment()
        {
            var assignment = new Assignment("Read Chapter 2", "Summarize key points", DateTime.Now.AddDays(3), AssignmentPriority.Medium, "Notes 1");
            Assert.Equal("Read Chapter 2", assignment.Title);
            Assert.Equal("Summarize key points", assignment.Description);
            Assert.False(assignment.IsCompleted);
        }

        [Fact]
        public void Constructor_BlankTitle_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() => new Assignment("", "Valid description", DateTime.Now.AddDays(3), AssignmentPriority.Medium, "Notes 1"));
        }

        [Fact]
        public void Update_BlankDescription_ShouldThrowException()
        {
            var assignment = new Assignment("Read Chapter 2", "Summarize key points", DateTime.Now.AddDays(3), AssignmentPriority.Medium, "Notes 1");
            Assert.Throws<ArgumentException>(() => assignment.Update("Valid title", ""));
        }

        [Fact]
        public void MarkComplete_SetsIsCompletedToTrue()
        {
            var assignment = new Assignment("Task", "Complete the lab", DateTime.Now.AddDays(3), AssignmentPriority.Medium, "Notes 1");
            assignment.MarkComplete();
            Assert.True(assignment.IsCompleted);
        }

        // BUG-2025-341: Exposes failure to assign Notes in constructor
        [Fact]
        public void Constructor_WithNotes_ShouldAssignNotes()
        {
            // Arrange
            var expectedNotes = "Review Chapters 3–5 before next lecture";

            // Act
            var assignment = new Assignment(
                "Reading: Clean Code",
                "Chapters 3–5",
                DateTime.Now.AddDays(2),
                AssignmentPriority.Medium,
                expectedNotes);

            // Assert
            Assert.Equal(expectedNotes, assignment.Notes);
        }

        // BUG-2025-342: Verifies that ToString includes Notes when present
        [Fact]
        public void ToString_ShouldIncludeNotes()
        {
            // Arrange
            var notes = "Respond to at least two classmates";
            var assignment = new Assignment(
                "Week 6 Discussion",
                "Share your thoughts on software modularity",
                DateTime.Now.AddDays(1),
                AssignmentPriority.High,
                notes);

            // Act
            var result = assignment.ToString();

            // Assert
            Assert.Contains(notes, result);
        }

        // BUG-2025-343: Exposes logic error in IsOverdue for completed assignments
        [Fact]
        public void IsOverdue_ShouldReturnFalse_WhenAssignmentIsCompleted()
        {
            // Arrange
            var assignment = new Assignment(
                "Quiz: Test-Driven Development",
                "Complete all 10 questions",
                DateTime.Now.AddDays(-1),  // due yesterday
                AssignmentPriority.Medium,
                "Submitted during lab");

            assignment.MarkComplete();

            // Act
            var result = assignment.IsOverdue();

            // Assert
            Assert.False(result);
        }

        // Edge Case: Assignment with null Notes should not break ToString or Add logic
        [Fact]
        public void Assignment_WithNullNotes_ShouldAddAndDisplayGracefully()
        {
            // Arrange
            var formatter = new AssignmentFormatter();
            var logger = new Mock<IAppLogger>();
            var service = new AssignmentService(formatter, logger.Object);

            var assignment = new Assignment(
                "Reading: Design Patterns",
                "Skim chapters 3–5",
                DateTime.Now.AddDays(2),
                AssignmentPriority.Medium,
                null // Notes intentionally null
            );

            // Act
            var added = service.AddAssignment(assignment);
            var formatted = formatter.Format(assignment);

            // Assert
            Assert.True(added);
            Assert.Contains("Reading: Design Patterns", formatted);
            Assert.DoesNotContain("Notes: ", formatted); // It shouldn't break or show 'Notes: null'
        }
    }
}
