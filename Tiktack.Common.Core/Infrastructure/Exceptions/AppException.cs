using System;

namespace Tiktack.Common.Core.Infrastructure.Exceptions
{
    public class AppException : Exception
    {
        public ExceptionEventType ExceptionEvent { get; set; }
    }
}
