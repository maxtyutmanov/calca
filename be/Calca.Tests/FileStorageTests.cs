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

            allTrans.Should().BeEquivalentTo(new[] { addedTran });
        }

        public void Dispose()
        {
            if (Directory.Exists(TmpDirPath))
                Directory.Delete(TmpDirPath, true);
        }
    }
}
