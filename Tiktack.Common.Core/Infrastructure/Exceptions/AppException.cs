using System;

namespace Tiktack.Common.Core.Infrastructure.Exceptions
{
    public class AppException : Exception
    {
        public ExceptionEventType ExceptionEvent { get; set; }

        public AppException(string message, ExceptionEventType exceptionEvent) : base(message)
        {
            ExceptionEvent = exceptionEvent;
        }

        public AppException(ExceptionEventType exceptionEvent) : this(nameof(exceptionEvent), exceptionEvent)
        {
        }
    }
}
