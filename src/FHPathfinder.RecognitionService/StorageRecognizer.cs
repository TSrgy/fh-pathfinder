using FHPathfinder.RecognitionService.Data;
using OpenCvSharp;
using OpenCvSharp.Text;

namespace FHPathfinder.RecognitionService
{
    public class StorageRecognizer
    {
        private readonly DebugOptions _debugOptions;
        private readonly IconsRecognizer _iconsRecognizer;
        private readonly NumberFieldsRecognizer _numberFieldsRecognizer;

        public StorageRecognizer(DebugOptions debugOptions, IconsRecognizer iconsRecognizer, NumberFieldsRecognizer numberFieldsRecognizer)
        {
            _debugOptions = debugOptions;
            _iconsRecognizer = iconsRecognizer;
            _numberFieldsRecognizer = numberFieldsRecognizer;
        }

        public Storage RecognizeStorage(Mat image)
        {
            var recognizedIcon = _iconsRecognizer.RecognizeAllIcons(image);
            var potentialNumberFieldRects = _numberFieldsRecognizer.RecognizePotentialNumberFields(image);

            var numberFieldRects = new Dictionary<StorageItemType, Rect>();
            foreach (var (itemType, iconRect) in recognizedIcon)
            {
                var numberFieldRect = potentialNumberFieldRects
                    .Where(x => x.Contains(new Point(iconRect.X + iconRect.Width * 1.5, iconRect.Y + (iconRect.Height / 2))))
                    .Cast<Rect?>()
                    .FirstOrDefault((Rect?)null);
                if (numberFieldRect.HasValue)
                {
                    numberFieldRects.Add(itemType, numberFieldRect.Value);
                }
            }

            var numberFieldTexts = RecognizeText(image, numberFieldRects);

            if (_debugOptions.HasFlag(DebugOptions.ShowStorage))
            {
                Helpers.ShowMat(image, (x) =>
                {
                    foreach (var (itemType, iconRect) in recognizedIcon)
                    {
                        x.Rectangle(iconRect, Scalar.LimeGreen);
                        if (numberFieldRects.ContainsKey(itemType))
                        {
                            x.Rectangle(numberFieldRects[itemType], Scalar.Red);
                            x.PutText(numberFieldTexts[itemType].ToString(), numberFieldRects[itemType].TopLeft, HersheyFonts.HersheySimplex, 1, Scalar.LimeGreen);
                        }
                    }
                });
            }

            var dict = numberFieldTexts
                .ToDictionary(
                    x => x.Key,
                    x => uint.Parse(x.Value.TrimEnd('+').Replace("k", "000"))
                    );

            return new Storage(dict);
        }

        private IReadOnlyDictionary<StorageItemType, string> RecognizeText(Mat image, IReadOnlyDictionary<StorageItemType, Rect> numberFields)
        {
            using var gray = image.CvtColor(ColorConversionCodes.BGRA2GRAY);
            using var thrash = gray.Threshold(120, 255, ThresholdTypes.BinaryInv);
            //Helpers.ShowMat(thrash);
            using var engine = OCRTesseract.Create("Tessdata/", "eng", "0T23456789k+", 1, 6); // T = 1
            var numberFieldTexts = new Dictionary<StorageItemType, string>();
            foreach (var (itemType, rect) in numberFields)
            {
                using var numberFieldMat = new Mat(thrash, rect);
                engine.Run(numberFieldMat, out var text, out _, out var texts, out _);
                var result = texts?.FirstOrDefault()?.Replace('T', '1') ?? string.Empty;
                numberFieldTexts.Add(itemType, result);
            }
            return numberFieldTexts;
        }
    }
}