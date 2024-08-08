using System.Reflection;

namespace CommonLibrary.Helpers;

/// <summary>
/// Stream helpers that can be used by all projects.
/// </summary>
public static class StreamHelpers
{
    /// <summary>
    /// Reads an embedded resource from the calling assembly as a <see cref="Stream"/>.
    /// </summary>
    /// <param name="resourceName">The name of the resource to read.</param>
    /// <returns>A <see cref="Stream"/> representing the embedded resource.</returns>
    /// <exception cref="ArgumentException">Thrown when the specified resource is not found in the assembly.</exception>
    public static Stream ReadEmbeddedResourceAsStream(this string resourceName)
    {
        var assembly = Assembly.GetCallingAssembly();
        var fullResourceName = $"{assembly.GetName().Name}.{resourceName}";
        var stream = assembly.GetManifestResourceStream(fullResourceName);
        
        if (stream == null)
        {
            throw new ArgumentException($"Resource '{resourceName}' not found in assembly.");
        }

        stream.Position = 0;

        return stream;
    }
}