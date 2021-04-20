using System;
using Xunit;
using ManagementServer.ConsoleApp;
namespace ManagementServer.Tests
{
    public class ProgramTests
    {
        [Fact]
        public void DummyTest1()
        {
            Program program = new Program();
            int expected = 1;
            int result = program.dummy();
            Assert.Equal(expected, result);
        }
    }
}
