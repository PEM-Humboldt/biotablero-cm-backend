namespace IAVH.BioTablero.CM.Infrastructure.Integrations.ImageUtils;

using System.IO;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices;
using IAVH.BioTablero.CM.Core.Domain.Utils.Constants;

using Serilog;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

/// <summary>
/// Image Utils service.
/// </summary>
public class ImageUtilsService : IImageUtilsService
{
    private readonly ILogger logger;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="logger">Logger.</param>
    public ImageUtilsService(ILogger logger)
    {
        this.logger = logger;
    }

    /// <inheritdoc/>
    public async Task<Stream> CompressToWebpAsync(Stream input, int quality = 75, CancellationToken ct = default)
    {
        try
        {
            var output = new MemoryStream();

            input.Position = 0;

            using var image = await Image.LoadAsync(input, ct);

            if (image.Size.Width > FileConstants.WebpMaxDimension || image.Size.Height > FileConstants.WebpMaxDimension)
            {
                logger.Error("Image is too large to encode WEBP format: {ImageSize}", image.Size);
                return null;
            }

            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Mode = ResizeMode.Max,
                Size = image.Size,
                Sampler = KnownResamplers.Lanczos3,
            }));

            var encoder = new WebpEncoder
            {
                Quality = quality,
            };

            await image.SaveAsWebpAsync(output, encoder, ct);

            output.Position = 0;

            return output;
        }
        catch (UnknownImageFormatException ex)
        {
            logger.Error(ex, "Image processing error");
        }
        catch (ImageFormatException ex)
        {
            logger.Error(ex, "Image format error");
        }

        return null;
    }
}
