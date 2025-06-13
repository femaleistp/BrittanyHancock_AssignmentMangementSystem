using AssignmentManagement.Core;
using AssignmentManagement.UI;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentManagement.Tests
{
    public class DependencyInjectionTests
    {
        // Test to ensure DI container can resolve ConsoleUI with its dependencies
        [Fact]  
        public void DIContainer_ShouldResolve_ConsoleUI_WithDependencies()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddSingleton<IAssignmentFormatter, AssignmentFormatter>();
            services.AddSingleton<IAppLogger, ConsoleAppLogger>();
            services.AddSingleton<IAssignmentService, AssignmentService>();
            services.AddSingleton<ConsoleUI>();

            var provider = services.BuildServiceProvider();

            // Act
            var consoleUI = provider.GetService<ConsoleUI>();

            // Assert
            Assert.NotNull(consoleUI);
        }
    }
}
