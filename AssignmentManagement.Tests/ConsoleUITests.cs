
using Xunit;
using Moq;
using AssignmentManagement.Core;
using AssignmentManagement.Console;
using System.Collections.Generic;
using System.IO;
using AssignmentManagement.UI;

namespace AssignmentManagement.Tests
{
    public class ConsoleUITests
    {
        [Fact]
        public void AddAssignment_Should_Call_Service_Add()
        {
            var mock = new Mock<IAssignmentService>();
            var ui = new ConsoleUI(mock.Object);

            // Correct input: choose menu option 1, enter title, enter description, then exit
            using var input = new StringReader("1\nSample Title\nSample Description\nMedium\n0\n");
            System.Console.SetIn(input);

            ui.Run();

            mock.Verify(s => s.AddAssignment(It.Is<Assignment>(a =>
                a.Title == "Sample Title" &&
                a.Description == "Sample Description"
            )), Times.Once);
        }


        [Fact]
        public void SearchAssignmentByTitle_Should_Display_Assignment()
        {
            var mock = new Mock<IAssignmentService>();
            mock.Setup(s => s.FindByTitle("Sample"))
                .Returns(new Assignment("Sample", "Details", DateTime.Now.AddDays(3), AssignmentPriority.Medium, "Notes 1"));

            var ui = new ConsoleUI(mock.Object);

            using var input = new StringReader("5\nSample\n0\n");
            System.Console.SetIn(input);

            ui.Run();

            mock.Verify(s => s.FindByTitle("Sample"), Times.Once);
        }

        [Fact]
        public void DeleteAssignment_Should_Call_Service_Delete()
        {
            var mock = new Mock<IAssignmentService>();
            mock.Setup(s => s.DeleteAssignment("ToDelete")).Returns(true);

            var ui = new ConsoleUI(mock.Object);

            using var input = new StringReader("7\nToDelete\n0\n");
            System.Console.SetIn(input);

            ui.Run();

            mock.Verify(s => s.DeleteAssignment("ToDelete"), Times.Once);
        }
    }
}
