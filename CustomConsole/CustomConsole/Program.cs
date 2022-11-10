using SkiaSharp;
using Spectre.Console;

await AnsiConsole.Live(Text.Empty)
    .StartAsync(async ctx =>
    {
        var stream = new SKManagedStream(new FileStream("gengar-gengar-pokemon.gif", FileMode.Open),true);

        var codec = SKCodec.Create(stream);
        var frames = codec.FrameInfo;

        var info = codec.Info;
        var bitmap = new SKBitmap(info);

        for (var frame = 0; frame < frames.Length; frame++)
        {
            var opts = new SKCodecOptions(frame);

            if (codec?.GetPixels(info, bitmap.GetPixels(), opts) != SKCodecResult.Success) continue;

            using var memStream = new MemoryStream();
            using var wStream = new SKManagedWStream(memStream);
            bitmap.Encode(wStream, SKEncodedImageFormat.Jpeg, 100);
            memStream.Position = 0;
            var data = memStream.ToArray();

            var canvasImage = new CanvasImage(data).MaxWidth(100);
            ctx.UpdateTarget(canvasImage);

            var duration = frames[frame].Duration;
            if (duration <= 0)
            {
                duration = 100;
            }

            await Task.Delay(duration);
        }
    });
//AnsiConsole.Markup("[underline red]Hello[/], World!");