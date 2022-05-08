using System.Collections.Generic;
using FHPathfinder.RecognitionService;
using FHPathfinder.RecognitionService.Data;
using FHPathfinder.RecognitionService.Data.StorageItems;
using OpenCvSharp;
using Xunit;

namespace FHPathfinder.RecognitionTests
{
    public class RecognizeIconsTest
    {
        private readonly IconsRecognizer _iconsRecognizer;

        public RecognizeIconsTest()
        {
            var debugOptions = DebugOptions.ShowNothing;
            _iconsRecognizer = new IconsRecognizer(debugOptions);
        }

        [Fact]
        public void ShouldDetectCrudeOilTest()
        {
            using var screenshot = new Mat("Screenshots/OilWell.png", ImreadModes.Unchanged);
            var result = _iconsRecognizer.RecognizeIcon(StorageItemType.CrudeOil, screenshot);
            Assert.NotNull(result);
        }

        [Fact]
        public void ShouldDetectOilWellItemsTest()
        {
            using var screenshot = new Mat("Screenshots/OilWell.png", ImreadModes.Unchanged);
            var result = _iconsRecognizer.RecognizeAllIcons(screenshot);
            Assert.Equal(3, result.Count);
        }
    }
}