using System.Collections.Generic;
using FHPathfinder.RecognitionService;
using OpenCvSharp;
using Xunit;

namespace FHPathfinder.RecognitionTests
{
    public class RecognizeStorageTest
    {
        private readonly StorageRecognizer _storageRecognizer;

        public RecognizeStorageTest()
        {
            var debugOptions = DebugOptions.ShowStorage;
            var iconsRecognizer = new IconsRecognizer(debugOptions);
            var numberFieldsRecognizer = new NumberFieldsRecognizer(debugOptions);
            _storageRecognizer = new StorageRecognizer(debugOptions, iconsRecognizer, numberFieldsRecognizer);
        }

        [Fact]
        public void ShouldDetectOilWellNumberFields()
        {
            using var screenshot = new Mat("Screenshots/OilWell.png", ImreadModes.Unchanged);
            _storageRecognizer.RecognizeStorage(screenshot);
        }
    }
}