using System.Collections.Generic;
using FHPathfinder.RecognitionService;
using OpenCvSharp;
using Xunit;

namespace FHPathfinder.RecognitionTests
{
    public class RecognizeNumberFieldsTest
    {
        private readonly NumberFieldsRecognizer _numberFieldsRecognizer;

        public RecognizeNumberFieldsTest()
        {
            var debugOptions = DebugOptions.ShowNothing;
            _numberFieldsRecognizer = new NumberFieldsRecognizer(debugOptions);
        }

        [Fact]
        public void ShouldDetectOilWellNumberFields()
        {
            using var screenshot = new Mat("Screenshots/OilWell.png", ImreadModes.Unchanged);
            var result = _numberFieldsRecognizer.RecognizePotentialNumberFields(screenshot);
            Assert.True(result.Count >= 3);
        }
    }
}