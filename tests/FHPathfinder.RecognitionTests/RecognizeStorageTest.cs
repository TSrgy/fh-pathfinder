using System.Collections.Generic;
using FHPathfinder.RecognitionService;
using FHPathfinder.RecognitionService.Data;
using OpenCvSharp;
using Xunit;

namespace FHPathfinder.RecognitionTests
{
    public class RecognizeStorageTest
    {
        private readonly StorageRecognizer _storageRecognizer;

        public RecognizeStorageTest()
        {
            var debugOptions = DebugOptions.ShowNothing;
            var iconsRecognizer = new IconsRecognizer(debugOptions);
            var numberFieldsRecognizer = new NumberFieldsRecognizer(debugOptions);
            _storageRecognizer = new StorageRecognizer(debugOptions, iconsRecognizer, numberFieldsRecognizer);
        }

        [Fact]
        public void ShouldDetectOilWellNumberFields()
        {
            using var screenshot = new Mat("Screenshots/OilWell.png", ImreadModes.Unchanged);
            var storage = _storageRecognizer.RecognizeStorage(screenshot);
            Assert.Contains(storage, x => x.Key == StorageItemType.CrudeOil && x.Value == 2);
            Assert.Contains(storage, x => x.Key == StorageItemType.Diesel && x.Value == 1_000);
            Assert.Contains(storage, x => x.Key == StorageItemType.Petrol && x.Value == 765);
        }
    }
}