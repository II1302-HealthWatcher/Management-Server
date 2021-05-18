using ManagementServer.ConsoleApp.model;
using Xunit;

namespace ManagementServer.Tests.model
{
    public class EntryIndexManagerTests
    {
        [Fact]
        public void ChechkEntryIndexTest()
        {
            EntryIndexManager entryIndexManager = new EntryIndexManager("EIMTests.bin");
            int entryIndex = entryIndexManager.GetEntryIndexAndIncrement("HW-52341");
            int secondEntryIndex = entryIndexManager.GetEntryIndexAndIncrement("HW-52341");
            int expectedEntry = entryIndex + 1;
            Assert.Equal(expectedEntry, secondEntryIndex);
        }

        [Fact]
        public void ResetIndexTest()
        {
            EntryIndexManager entryIndexManager = new EntryIndexManager("EIMTests.bin");
            int entryIndex = entryIndexManager.GetEntryIndexAndIncrement("HW-52341");
            entryIndexManager.ResetDeviceEntryIndex("HW-52341");
            int entryIndexAfterReset = entryIndexManager.GetEntryIndexAndIncrement("HW-52341");
            int expectedEntryIndex = 0;
            Assert.Equal(expectedEntryIndex, entryIndexAfterReset);
        }
    }
}