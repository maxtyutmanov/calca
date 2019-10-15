using Calca.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Calca.Storage
{
    public class FileStorage
    {
        private readonly object _syncRoot = new object();
        private readonly string _basePath;

        public FileStorage(string basePath)
        {
            _basePath = basePath;
            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);
        }

        public List<Tran> ReadAllTrans(string collectionId)
        {
            var colPath = Path.Join(_basePath, collectionId);
            var trans = GetLinesFromFile(colPath)
                .Select(line => JsonConvert.DeserializeObject<Tran>(line))
                .ToList();
            return trans;
        }

        private IEnumerable<string> GetLinesFromFile(string filePath)
        {
            using (var file = ExclusiveGetFile(filePath))
            using (var reader = new StreamReader(file, Encoding.UTF8, false, 1024, true))
            {
                if (file.Length == 0)
                    yield break;

                file.Position += sizeof(long);

                while (!reader.EndOfStream)
                    yield return reader.ReadLine();
            }
        }

        public Tran AppendTran(Tran tran)
        {
            var colPath = Path.Join(_basePath, tran.CollectionId);
            using (var file = ExclusiveGetFile(colPath))
            {
                // get last used tran ID from the beginning of the file
                var buf = new byte[sizeof(long)];
                var lastTranId = 0L;
                if (file.Length != 0)
                {
                    file.Read(buf, 0, buf.Length);
                    lastTranId = BitConverter.ToInt64(buf);
                }
                var newTranId = lastTranId + 1;

                file.Position = 0;
                buf = BitConverter.GetBytes(newTranId);
                file.Write(buf, 0, buf.Length);
                file.Flush();

                tran.Id = newTranId;
                tran.AddedAt = DateTime.UtcNow;

                file.Position = file.Length;
                using (var writer = new StreamWriter(file, Encoding.UTF8, 1024, true))
                {
                    var line = JsonConvert.SerializeObject(tran, Formatting.None);
                    writer.WriteLine(line);
                }

                file.Flush();

                return tran;
            }
        }

        public Tran DeleteTran(string collectionId, int tranId)
        {
            return AppendTran(new Tran()
            {
                CancelsTranId = tranId,
                CollectionId = collectionId,
                Consumers = new List<string>(),
                Contributors = new List<string>()
            });
        }

        private FileStream ExclusiveGetFile(string filePath)
        {
            Exception lastEx = null;

            for (var i = 0; i < 10; ++i)
            {
                try
                {
                    return new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                }
                catch (IOException e)
                {
                    // access denied error (concurrency)
                    if (e.HResult != -2147024864)
                        throw;
                    lastEx = e;
                    Thread.Sleep(100);
                }
            }

            throw lastEx;
        }
    }
}
