using Mz.Modulators;
using Mz.Numerics;
using SkiaSharp;

namespace Mz.Images
{
    public partial class MzImage
    {
        public void Line(float x0, float y0, float x1, float y1, byte[] rgba, float weight = 2, bool isCapRounded = true)
        {
            var paint = new SKPaint
            {
                StrokeCap = isCapRounded ? SKStrokeCap.Round : SKStrokeCap.Square,
                StrokeJoin = SKStrokeJoin.Round,
                StrokeWidth = weight,
                Color = new SKColor(rgba[0], rgba[1], rgba[2], rgba[3]),
                Style = SKPaintStyle.Stroke,
                IsAntialias = true
            };
            
            var path = new SKPath();
            path.MoveTo(x0, y0);
            path.LineTo(x1, y1);
            path.Close();
            _canvasSk.DrawPath(path, paint);
            
            // push to the pixel buffer
            _canvasSk.Flush();
        }

        public void ParametricCurve(EaseFunction modulator, byte[] rgba, float weight = 2, float interval = 0.01f, float marginFactor = 0.24f)
        {
            var margin = Width > Height ? Height * marginFactor : Width * marginFactor;

            var marginX = margin;
            var xStart = marginX + weight / 2f;
            var width = Width - weight - marginX * 2;
            var xPrevious = xStart;

            var marginY = margin;
            var yStart = Height - weight / 2f - marginY;
            var height = Height - weight - marginY * 2;
            var yPrevious = yStart;

            for (var progress = interval; progress <= 1f; progress += interval)
            {
                var x = xStart + width * progress;
                var y = yStart - modulator(progress) * height;
                
                Line(xPrevious, yPrevious, x, y, rgba, weight);

                xPrevious = x;
                yPrevious = y;
            }
        }
        
        public void Modulation(IModulator modulator, byte[] rgba, float weight = 2, float interval = 0.005f, float marginFactor = 0.24f)
        {
            var marginX = Width * marginFactor;
            var width = Width - weight - marginX * 2;
            var xPrevious = Numbers.NegativeInfinity;

            var marginY = Height * marginFactor;
            var height = Height - weight - marginY * 2;
            var yPrevious = Numbers.NegativeInfinity;
            
            for (var progress = 0f; progress <= 1; progress += interval)
            {
                var metric = modulator.Modulate(progress);
                var x = metric.Values[0] * (width / 2) + (Width / 2f);
                var y = metric.Values[1] * (height / 2) + (Height / 2f);
                
                if (xPrevious > Numbers.NegativeInfinity) Line(xPrevious, yPrevious, x, y, rgba, weight);

                xPrevious = x;
                yPrevious = y;
            }
        }
    }
}