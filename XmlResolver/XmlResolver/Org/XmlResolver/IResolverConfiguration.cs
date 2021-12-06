using System.Collections.Generic;
using Org.XmlResolver.Features;

namespace Org.XmlResolver {
    /// <summary>
    /// The resolver configuration interface.
    /// </summary>
    /// <para>With an eye towards supporting an open-ended set of features, this inteface
    /// accepts <see cref="ResolverFeature"> objects and values for them. 
    public interface IResolverConfiguration {
        /// <summary>
        /// Set a feature value.
        /// </summary>
        /// <para>This method may throw a <code>NullReferenceException</code> if the value passed
        /// is <code>null</code>.
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
    }
}