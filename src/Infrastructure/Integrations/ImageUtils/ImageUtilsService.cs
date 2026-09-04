namespace IAVH.BioTablero.CM.Infrastructure.Integrations.ImageUtils;

using System.IO;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices.ImageUtils;
using IAVH.BioTablero.CM.Core.Domain.Utils.Constants;

using Serilog;

using SkiaSharp;

/// <summary>
/// Image Utils service.
/// </summary>
/// <param name="logger">Logger.</param>
public class ImageUtilsService(ILogger logger) : IImageUtilsService
{
    private readonly ILogger logger = logger;

    /// <inheritdoc/>
    public async Task<Stream?> CompressToWebpAsync(Stream input, int quality = 75, CancellationToken ct = default) =>
        await Task.Run(
            () =>
            {
                ct.ThrowIfCancellationRequested();

                input.Position = 0;
                using var inputStream = new SKManagedStream(input, false);
                using var bitmap = SKBitmap.Decode(inputStream);

                if (bitmap == null)
                {
                    logger.Error("Could not decode image: unsupported or invalid format.");
                    return null;
                }

                if (bitmap.Width > FileConstants.WebpMaxDimension || bitmap.Height > FileConstants.WebpMaxDimension)
                {
                    logger.Error("Image is too large to encode WEBP format: {Width}x{Height}", bitmap.Width, bitmap.Height);
                    return null;
                }

                using var image = SKImage.FromBitmap(bitmap);
                using var data = image.Encode(SKEncodedImageFormat.Webp, quality);

                if (data == null)
                {
                    logger.Error("Failed to encode image to WEBP format.");
                    return null;
                }

                var output = new MemoryStream();
                data.SaveTo(output);
                output.Position = 0;

                return (Stream)output;
            },
            ct);
}
