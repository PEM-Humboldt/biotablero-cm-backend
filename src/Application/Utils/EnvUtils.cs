namespace IAVH.BioTablero.CM.Application.Utils;

using System;

/// <summary>
/// Environment utils.
/// </summary>
public static class EnvUtils
{
    /// <summary>
    /// Get required environment variable.
    /// </summary>
    /// <param name="name">Variable name.</param>
    /// <returns>Variable value.</returns>
    /// <exception cref="InvalidOperationException">Not defined environment variable.</exception>
    public static string GetRequiredEnv(string name) =>
        Environment.GetEnvironmentVariable(name) ?? throw new InvalidOperationException($"Environment variable '{name}' not defined.");
}
