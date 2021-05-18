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
            bool result = program.Dummy();
            bool expected = true;
            Assert.Equal(expected, result);
        }
    }
}
