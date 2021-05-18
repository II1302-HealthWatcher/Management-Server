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
            bool expected = true;
            bool result = program.Dummy();
            Assert.Equal(expected, result);
        }
    }
}
