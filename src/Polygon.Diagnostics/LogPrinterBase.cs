using System;

namespace Polygon.Diagnostics
{
    internal abstract class LogPrinterBase
    {
        public string LoggerName { get; set; }
        public string MethodName { get; set; }
        public int LineNumber { get; set; }

        protected abstract LogLevel Level { get; }
        protected virtual void Release() { }

        internal void PrintImpl(IPrintable message)
        {
            LogManager.Write(LoggerName, Level, message.Print(PrintOption.Default), null, MethodName, LineNumber);
            (message as IReusable)?.Release();
            Release();
        }

        internal void PrintImpl(Exception exception, IPrintable message)
        {
            LogManager.Write(LoggerName, Level, message.Print(PrintOption.Default), exception, MethodName, LineNumber);
            (message as IReusable)?.Release();
            Release();
        }
    }
}

