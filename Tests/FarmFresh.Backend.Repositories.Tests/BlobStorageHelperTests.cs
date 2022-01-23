using FarmFresh.Backend.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FarmFresh.Backend.Crud.Tests
{
    [Trait("Name", nameof(BlobStorageHelperTests))]
    public class BlobStorageHelperTests
    {

        private readonly BlobStorageHelper _blobStorageHelper;

        public BlobStorageHelperTests(BlobStorageHelper blobStorageHelper)
        {
            _blobStorageHelper = blobStorageHelper;
        }

        [Fact]
        public async Task UploadBlobTest()
        {
            string testFile = @".\Assets\sparta.jpg";
            Assert.True(File.Exists(testFile));

            string extension = Path.GetExtension(testFile);
            string blobName = $"{Guid.NewGuid()}{extension}";

            Stream readStream = File.OpenRead(testFile);
            string url = await _blobStorageHelper.UploadFile(readStream, blobName);
            readStream.Close();
            Assert.NotNull(url);
            Assert.EndsWith(blobName, url);
        }
    }
}
