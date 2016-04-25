using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace WotDossier.Web.Logging
{
    /// <summary>
    /// Summary description for NLogLoggerFactoryExtensions
    /// </summary>
    public static class NLogLoggerFactoryExtensions
    {
        public static ILoggerFactory AddNLog(
            this ILoggerFactory factory,
            global::NLog.LogFactory logFactory)
        {
            factory.AddProvider(new NLogLoggerProvider(logFactory));
            return factory;
        }
    }
}
