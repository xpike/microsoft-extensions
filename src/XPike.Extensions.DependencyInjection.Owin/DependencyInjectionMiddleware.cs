using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin;
using System;
using System.Threading.Tasks;

namespace XPike.Extensions.DependencyInjection.Owin
{
    /// <summary>
    /// Supports using Microsoft.Extensions.DependencyInjection in the Owin middleware pipeline.
    /// Implements the <see cref="Microsoft.Owin.OwinMiddleware" />
    /// This should be the first middleware in the pipeline.
    /// </summary>
    /// <seealso cref="Microsoft.Owin.OwinMiddleware" />
    public class DependencyInjectionMiddleware : OwinMiddleware
    {
        private IServiceProvider provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyInjectionMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        /// <param name="provider">The provider.</param>
        public DependencyInjectionMiddleware(OwinMiddleware next, IServiceProvider provider) 
            : base(next)
        {
            this.provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        /// <summary>
        /// Process an individual request.
        /// </summary>
        /// <param name="context">The context.</param>
        public override async Task Invoke(IOwinContext context)
        {
            using (var scope = provider.CreateScope())
            {
                await Next.Invoke(context).ConfigureAwait(false);
            }
        }
    }
}
