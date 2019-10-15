using Calca.Storage;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace Calca.Tests
{
    public class FileStorageTests : IDisposable
    {
        private readonly string TmpDirPath = Path.Combine(Directory.GetCurrentDirectory(), "tmp_storage", Guid.NewGuid().ToString());
        private readonly FileStorage _sut;

        public FileStorageTests()
        {
            _sut = new FileStorage(TmpDirPath);
        }

        [Fact]
        public void AddNewTran_GetAllBack_ShouldReturnSingleTranWithIdOf1()
        {
            var tran = new Model.Tran()
            {
                Amount = 100,
                CollectionId = "croatia2019",
                Consumers = new List<string>() { "jenek", "rinat" },
                Contributors = new List<string>() { "dan" },
                Description = "Dan bought something for jenek and rinat"
            };

            var addedTran = _sut.AppendTran(tran);

            var allTrans = _sut.ReadAllTrans("croatia2019");

            allTrans.Should().BeEquivalentTo(new[] { tran }, opts => opts.Excluding(t => t.Id).Excluding(t => t.AddedAt));
            allTrans[0].Id.Should().Be(1);
        }

        [Fact]
        public void AddNewTran_GetAllBack_ShouldAssignTimestamp()
        {
            var tran = new Model.Tran()
            {
                Amount = 100,
                CollectionId = "croatia2019",
                Consumers = new List<string>() { "jenek", "rinat" },
                Contributors = new List<string>() { "dan" },
                Description = "Dan bought something for jenek and rinat"
            };

            var addedTran = _sut.AppendTran(tran);

            var allTrans = _sut.ReadAllTrans("croatia2019");
            allTrans.Count.Should().Be(1);
            allTrans[0].AddedAt.Should().BeCloseTo(DateTime.UtcNow, 500);
        }

        [Fact]
        public void AddTwoTrans_GetAllBack_ShouldReturnBothWithDifferentIds()
        {
            var tran1 = new Model.Tran()
            {
                Amount = 100,
                CollectionId = "croatia2019",
                Consumers = new List<string>() { "jenek", "rinat" },
                Contributors = new List<string>() { "dan" },
                Description = "Dan bought something for jenek and rinat"
            };

            var tran2 = new Model.Tran()
            {
                Amount = 50,
                CollectionId = "croatia2019",
                Consumers = new List<string>() { "dan" },
                Contributors = new List<string>() { "rinat" },
                Description = "Rinat paid back"
            };

            var addedTran1 = _sut.AppendTran(tran1);
            var addedTran2 = _sut.AppendTran(tran2);

            var allTrans = _sut.ReadAllTrans("croatia2019");

            allTrans.Should().BeEquivalentTo(new[] { tran1, tran2 }, opts => opts.Excluding(t => t.Id).Excluding(t => t.AddedAt));
            allTrans[0].Id.Should().Be(1);
            allTrans[1].Id.Should().Be(2);
        }

        public void Dispose()
        {
            if (Directory.Exists(TmpDirPath))
                Directory.Delete(TmpDirPath, true);
        }
    }
}
