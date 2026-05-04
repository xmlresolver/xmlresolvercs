using System.Collections.Generic;
using XmlResolver.Features;

namespace XmlResolver;

/// <summary>
/// The resolver configuration interface.
/// </summary>
/// <para>With an eye towards supporting an open-ended set of features, this interface
/// accepts <see cref="ResolverFeature"/> objects and values for them. </para>
public interface IResolverConfiguration {
    /// <summary>
    /// Set a feature value.
    /// </summary>
    /// <para>This method may throw a <c>NullReferenceException</c> if the value passed
    /// is <c>null</c>.
    /// <param name="feature">The feature.</param>
    /// <param name="value">The value.</param>
    public void SetFeature(ResolverFeature feature, object value);
        
    /// <summary>
    /// Get a feature value.
    /// </summary>
    /// <param name="resolverFeature">The feature.</param>
    /// <returns>The value of the feature.</returns>
    public object GetFeature(ResolverFeature resolverFeature);
        
    /// <summary>
    /// Get the list of known features.
    /// </summary>
    /// <returns>The list of known features.</returns>
    public List<ResolverFeature> GetFeatures();
        
    public IResourceResponse GetResource(IResourceRequest request);
    public IResourceResponse GetResource(IResourceResponse request);
}
