using OpenCvSharp;

namespace FHPathfinder.RecognitionService;

public class NumberFieldsRecognizer
{
    private readonly DebugOptions _debugOptions;

    public NumberFieldsRecognizer(DebugOptions debugOptions)
    {
        _debugOptions = debugOptions;
    }

    public List<Rect> RecognizePotentialNumberFields(Mat image)
    {
        using var preparedImage = PrepareImage(image);

        preparedImage.FindContours(out var cnts, out _, RetrievalModes.External,
            ContourApproximationModes.ApproxSimple);
        


        const int minArea = 2000;
        const int maxArea = 4000;

        var recognizedNumberFields = new List<Rect>();

        foreach (var c in cnts)
        {
            var area = Cv2.ContourArea(c);
            var peri = Cv2.ArcLength(c, true);
            var approx = Cv2.ApproxPolyDP(c, 0.1 * peri, true);
            if (area is > minArea and < maxArea && approx.Length == 4 && Cv2.IsContourConvex(c))
            {
                var rect = Cv2.BoundingRect(c);

                recognizedNumberFields.Add(rect);
            }
        }

        if (_debugOptions.HasFlag(DebugOptions.ShowAllNumberFields))
        {
            Helpers.ShowMat(image, x =>
            {
                foreach (var rect in recognizedNumberFields)
                {
                    x.Rectangle(rect, Scalar.LimeGreen);
                }
            });
        }

        return recognizedNumberFields;
    }

    private Mat PrepareImage(Mat image)
    {
        // Threshold of gray in HSV space
        using var hsv = image.CvtColor(ColorConversionCodes.BGR2HSV);
        var lowerGray = new Scalar(0, 0, 66);
        var upperGray = new Scalar(0, 0, 70);
        using var maskGray = hsv.InRange(lowerGray, upperGray);
        using var masked = new Mat();
        Cv2.BitwiseAnd(image, image, masked, maskGray);
        //Helpers.ShowMat(masked);

        // Blur
        using var gray = masked.CvtColor(ColorConversionCodes.BGR2GRAY);
        using var blur = gray.MedianBlur(5);
        //Helpers.ShowMat(blur);

        using var thresh = blur.Threshold(0, 255, ThresholdTypes.Binary & ThresholdTypes.Otsu);
        //Helpers.ShowMat(thresh);

        using var kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(5, 5));
        var morph = thresh.MorphologyEx(MorphTypes.Close, kernel, null, 2);
        //Helpers.ShowMat(morph);

        return morph;
    }
}