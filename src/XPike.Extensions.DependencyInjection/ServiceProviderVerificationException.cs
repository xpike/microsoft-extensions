using System;
using System.Collections.Generic;
using System.Linq;

namespace XPike.Extensions.DependencyInjection
{
    /// <summary>
    /// A ServiceProviderVerificationException is thrown when the container fails to verify.
    /// </summary>
    /// <seealso cref="System.Exception" />
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "We want the class to encapsulate the message and not allow consumers to pass in the message.")]
    [Serializable]
    public class ServiceProviderVerificationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceProviderVerificationException"/> class.
        /// </summary>
        /// <param name="results">The exceptions.</param>
        public ServiceProviderVerificationException(IEnumerable<VerificationResult> results)
            : base ("The container failed to validate all required dependencies. You are likely missing a registration. See the Restults collection for details.")
        {
            Results = results.ToArray();
        }

        /// <summary>
        /// Gets a list of <see cref="VerificationResult"/>s that occurred during verification.
        /// </summary>
        /// <value>An array of exceptions.</value>
        public IEnumerable<VerificationResult> Results { get; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceProviderVerificationException"/> class.
        /// </summary>
        /// <param name="serializationInfo">The serialization information.</param>
        /// <param name="streamingContext">The streaming context.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        protected ServiceProviderVerificationException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
        {
            this.Results = (IEnumerable<VerificationResult>)serializationInfo.GetValue("Results", typeof(IEnumerable<VerificationResult>));
        
        }

        /// <summary>
        /// When overridden in a derived class, sets the <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
        /// <exception cref="System.ArgumentNullException">info</exception>
        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            info.AddValue("Results", this.Results, typeof(IEnumerable<VerificationResult>));

            base.GetObjectData(info, context);
        }
    }
}
