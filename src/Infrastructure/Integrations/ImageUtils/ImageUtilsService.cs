namespace IAVH.BioTablero.CM.Infrastructure.Integrations.ImageUtils;

using System.IO;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices;

using Serilog;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
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
    public async Task<Stream> CompressToJpegAsync(Stream input, int quality = 75, CancellationToken ct = default)
    {
        try
        {
            var output = new MemoryStream();

            input.Position = 0;

            using var image = await Image.LoadAsync(input, ct);

            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Mode = ResizeMode.Max,
                Size = image.Size,
                Sampler = KnownResamplers.Lanczos3,
            }));

            var encoder = new JpegEncoder
            {
                Quality = quality,
            };

            await image.SaveAsJpegAsync(output, encoder, ct);

            output.Position = 0;

            return output;
        }
        catch (UnknownImageFormatException ex)
        {
            logger.Error(ex, "Image processing error");
        }

        return null;
    }
}
