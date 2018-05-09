using PG.Common;
using System.Web.Http.ExceptionHandling;
using PG.Common.Extensions;

namespace PG.Api
{
    public class UnhandledExceptionLogger : ExceptionLogger
    {
        public override void Log(ExceptionLoggerContext context)
        {
            var ex = context.Exception;
            var logger = LoggerManager.GetLogger();
            logger.Error(ex.GetLastInnerExceptionMessage(), ex);
        }
    }
}