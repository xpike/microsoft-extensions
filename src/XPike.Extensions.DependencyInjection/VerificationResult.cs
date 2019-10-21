using Microsoft.Extensions.DependencyInjection;
using System;

namespace XPike.Extensions.DependencyInjection
{
    /// <summary>
    /// Encapsulates the result of a failed attempt to verify a registered type in the container.
    /// </summary>
    [Serializable]
    public class VerificationResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VerificationResult"/> class.
        /// </summary>
        /// <param name="descriptor">The ServiceDescriptor being verified.</param>
        /// <param name="exception">The exception thrown, if any.</param>
        /// <param name="message">The verification message.</param>
        /// <exception cref="ArgumentNullException">descriptor</exception>
        public VerificationResult(ServiceDescriptor descriptor, Exception exception = null, string message = null)
        {
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));

            this.Exception = exception;
            Message = message ?? $"Service type {descriptor.ServiceType} failed to resolve. See the related Exception for more details.";
        }

        /// <summary>
        /// Gets the verification message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; }

        /// <summary>
        /// Gets the exception that caused the verification to fail.
        /// </summary>
        /// <value>The exception.</value>
        public Exception Exception { get; }
    }
}
