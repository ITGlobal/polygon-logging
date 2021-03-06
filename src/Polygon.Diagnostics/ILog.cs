
// <auto-generated>
//      This code was generated using T4 text template
//      Generated at 07/07/2017 16:31:56
//
//      Changes to this file may cause incorrect behaviour and will be lost 
//      if the code is regenerated.
// </auto-generated>

using JetBrains.Annotations;

namespace Polygon.Diagnostics
{
    /// <summary>
    ///     ������
    /// </summary>
    [PublicAPI]
    public interface ILog
    {

        /// <summary>
        ///     �������� �� ������� ����������� <see cref="LogLevel.Trace"/>.
        /// </summary>
        bool IsTraceEnabled { get; }

        /// <summary>
        ///     �������� � ��� ��������� � ������� <see cref="LogLevel.Trace"/>.
        /// </summary>
		[NotNull]
		ITraceLogPrinter Trace([System.Runtime.CompilerServices.CallerLineNumber] int line = 0, [System.Runtime.CompilerServices.CallerMemberName] string caller = null);

        /// <summary>
        ///     �������� �� ������� ����������� <see cref="LogLevel.Debug"/>.
        /// </summary>
        bool IsDebugEnabled { get; }

        /// <summary>
        ///     �������� � ��� ��������� � ������� <see cref="LogLevel.Debug"/>.
        /// </summary>
		[NotNull]
		IDebugLogPrinter Debug([System.Runtime.CompilerServices.CallerLineNumber] int line = 0, [System.Runtime.CompilerServices.CallerMemberName] string caller = null);

        /// <summary>
        ///     �������� �� ������� ����������� <see cref="LogLevel.Info"/>.
        /// </summary>
        bool IsInfoEnabled { get; }

        /// <summary>
        ///     �������� � ��� ��������� � ������� <see cref="LogLevel.Info"/>.
        /// </summary>
		[NotNull]
		IInfoLogPrinter Info([System.Runtime.CompilerServices.CallerLineNumber] int line = 0, [System.Runtime.CompilerServices.CallerMemberName] string caller = null);

        /// <summary>
        ///     �������� �� ������� ����������� <see cref="LogLevel.Warn"/>.
        /// </summary>
        bool IsWarnEnabled { get; }

        /// <summary>
        ///     �������� � ��� ��������� � ������� <see cref="LogLevel.Warn"/>.
        /// </summary>
		[NotNull]
		IWarnLogPrinter Warn([System.Runtime.CompilerServices.CallerLineNumber] int line = 0, [System.Runtime.CompilerServices.CallerMemberName] string caller = null);

        /// <summary>
        ///     �������� �� ������� ����������� <see cref="LogLevel.Error"/>.
        /// </summary>
        bool IsErrorEnabled { get; }

        /// <summary>
        ///     �������� � ��� ��������� � ������� <see cref="LogLevel.Error"/>.
        /// </summary>
		[NotNull]
		IErrorLogPrinter Error([System.Runtime.CompilerServices.CallerLineNumber] int line = 0, [System.Runtime.CompilerServices.CallerMemberName] string caller = null);

        /// <summary>
        ///     �������� �� ������� ����������� <see cref="LogLevel.Fatal"/>.
        /// </summary>
        bool IsFatalEnabled { get; }

        /// <summary>
        ///     �������� � ��� ��������� � ������� <see cref="LogLevel.Fatal"/>.
        /// </summary>
		[NotNull]
		IFatalLogPrinter Fatal([System.Runtime.CompilerServices.CallerLineNumber] int line = 0, [System.Runtime.CompilerServices.CallerMemberName] string caller = null);
	}


	/// <summary>
	///		Fluent-��������� ��� ������ ������� � ������� <see cref="LogLevel.Trace"/>.
	/// </summary>
	[PublicAPI]
    public interface ITraceLogPrinter
	{
        /// <summary>
        ///     �������� �� ������� ����������� <see cref="LogLevel.Trace"/>.
        /// </summary>
        bool IsEnabled { get; }

		/// <summary>
        ///     �������� � ��� ��������� � ������� <see cref="LogLevel.Trace"/>.
        /// </summary>
		[LogMethod]
		void Print([NotNull] IPrintable message);



	}

	/// <summary>
    ///     ������-���������� ��� <see cref="ITraceLogPrinter"/>
    /// </summary>
	[PublicAPI]
    public static class TraceLogPrinterExtensions
	{

		/// <summary>
        ///     �������� � ��� ��������� � ������� <see cref="LogLevel.Trace"/>.
        /// </summary>
        [LogMethod]
		public static void Print([NotNull] this ITraceLogPrinter printer, [NotNull] string message) 
		{
			if(!printer.IsEnabled)
			{
				return;
			}

			var printerImpl = printer as LogPrinterBase;
			if(printerImpl != null)
			{
				printerImpl.PrintImpl(LogMessage.Preformatted(message));
			}
		}

		/// <summary>
        ///     �������� � ��� ��������� � ������� <see cref="LogLevel.Trace"/>.
        /// </summary>
		[LogMethod]
		public static void Print([NotNull] this ITraceLogPrinter printer, [NotNull] string message, params ILogField[] fields) 
		{
			if(!printer.IsEnabled)
			{
				FormattingHelper.ReleaseUnused(fields);
				return;
			}

			var printerImpl = printer as LogPrinterBase;
			if(printerImpl != null)
			{
				var formattedMessage = LogMessage.MakeString(message, fields);
				printerImpl.PrintImpl(formattedMessage);
			}
		}

		/// <summary>
        ///     �������� � ��� ��������� � ������� <see cref="LogLevel.Trace"/>.
        /// </summary>
		[LogMethod, StringFormatMethod("message")]
		public static void PrintFormat([NotNull] this ITraceLogPrinter printer, [NotNull] string message, params object[] args) 
		{
			if(!printer.IsEnabled)
			{
				FormattingHelper.ReleaseUnused(args);
				return;
			}

			var printerImpl = printer as LogPrinterBase;
			if(printerImpl != null)
			{
				var formattedMessage = LogMessage.FormatString(message, args);
				printerImpl.PrintImpl(formattedMessage);
			}
		}

	
	}


	/// <summary>
	///		Fluent-��������� ��� ������ ������� � ������� <see cref="LogLevel.Debug"/>.
	/// </summary>
	[PublicAPI]
    public interface IDebugLogPrinter
	{
        /// <summary>
        ///     �������� �� ������� ����������� <see cref="LogLevel.Debug"/>.
        /// </summary>
        bool IsEnabled { get; }

		/// <summary>
        ///     �������� � ��� ��������� � ������� <see cref="LogLevel.Debug"/>.
        /// </summary>
		[LogMethod]
		void Print([NotNull] IPrintable message);

		/// <summary>
        ///     �������� � ��� ��������� � ������� <see cref="LogLevel.Debug"/>.
        /// </summary>
        [LogMethod]
		void Print([NotNull] System.FormattableString message);

		/// <summary>
        ///     �������� � ��� ��������� � ������� <see cref="LogLevel.Debug"/>.
        /// </summary>
        [LogMethod]
		void Print([NotNull] System.FormattableString message, params ILogField[] fields);


	}

	/// <summary>
    ///     ������-���������� ��� <see cref="IDebugLogPrinter"/>
    /// </summary>
	[PublicAPI]
    public static class DebugLogPrinterExtensions
	{

		/// <summary>
        ///     �������� � ��� ��������� � ������� <see cref="LogLevel.Debug"/>.
        /// </summary>
        [LogMethod]
		public static void Print([NotNull] this IDebugLogPrinter printer, [NotNull] string message) 
		{
			if(!printer.IsEnabled)
			{
				return;
			}

			var printerImpl = printer as LogPrinterBase;
			if(printerImpl != null)
			{
				printerImpl.PrintImpl(LogMessage.Preformatted(message));
			}
		}

		/// <summary>
        ///     �������� � ��� ��������� � ������� <see cref="LogLevel.Debug"/>.
        /// </summary>
		[LogMethod]
		public static void Print([NotNull] this IDebugLogPrinter printer, [NotNull] string message, params ILogField[] fields) 
		{
			if(!printer.IsEnabled)
			{
				FormattingHelper.ReleaseUnused(fields);
				return;
			}

			var printerImpl = printer as LogPrinterBase;
			if(printerImpl != null)
			{
				var formattedMessage = LogMessage.MakeString(message, fields);
				printerImpl.PrintImpl(formattedMessage);
			}
		}

		/// <summary>
        ///     �������� � ��� ��������� � ������� <see cref="LogLevel.Debug"/>.
        /// </summary>
		[LogMethod, StringFormatMethod("message")]
		public static void PrintFormat([NotNull] this IDebugLogPrinter printer, [NotNull] string message, params object[] args) 
		{
			if(!printer.IsEnabled)
			{
				FormattingHelper.ReleaseUnused(args);
				return;
			}

			var printerImpl = printer as LogPrinterBase;
			if(printerImpl != null)
			{
				var formattedMessage = LogMessage.FormatString(message, args);
				printerImpl.PrintImpl(formattedMessage);
			}
		}

	
	}


	/// <summary>
	///		Fluent-��������� ��� ������ ������� � ������� <see cref="LogLevel.Info"/>.
	/// </summary>
	[PublicAPI]
    public interface IInfoLogPrinter
	{
        /// <summary>
        ///     �������� �� ������� ����������� <see cref="LogLevel.Info"/>.
        /// </summary>
        bool IsEnabled { get; }

		/// <summary>
        ///     �������� � ��� ��������� � ������� <see cref="LogLevel.Info"/>.
        /// </summary>
		[LogMethod]
		void Print([NotNull] IPrintable message);

		/// <summary>
        ///     �������� � ��� ��������� � ������� <see cref="LogLevel.Info"/>.
        /// </summary>
        [LogMethod]
		void Print([NotNull] System.FormattableString message);

		/// <summary>
        ///     �������� � ��� ��������� � ������� <see cref="LogLevel.Info"/>.
        /// </summary>
        [LogMethod]
		void Print([NotNull] System.FormattableString message, params ILogField[] fields);


	}

	/// <summary>
    ///     ������-���������� ��� <see cref="IInfoLogPrinter"/>
    /// </summary>
	[PublicAPI]
    public static class InfoLogPrinterExtensions
	{

		/// <summary>
        ///     �������� � ��� ��������� � ������� <see cref="LogLevel.Info"/>.
        /// </summary>
        [LogMethod]
		public static void Print([NotNull] this IInfoLogPrinter printer, [NotNull] string message) 
		{
			if(!printer.IsEnabled)
			{
				return;
			}

			var printerImpl = printer as LogPrinterBase;
			if(printerImpl != null)
			{
				printerImpl.PrintImpl(LogMessage.Preformatted(message));
			}
		}

		/// <summary>
        ///     �������� � ��� ��������� � ������� <see cref="LogLevel.Info"/>.
        /// </summary>
		[LogMethod]
		public static void Print([NotNull] this IInfoLogPrinter printer, [NotNull] string message, params ILogField[] fields) 
		{
			if(!printer.IsEnabled)
			{
				FormattingHelper.ReleaseUnused(fields);
				return;
			}

			var printerImpl = printer as LogPrinterBase;
			if(printerImpl != null)
			{
				var formattedMessage = LogMessage.MakeString(message, fields);
				printerImpl.PrintImpl(formattedMessage);
			}
		}

		/// <summary>
        ///     �������� � ��� ��������� � ������� <see cref="LogLevel.Info"/>.
        /// </summary>
		[LogMethod, StringFormatMethod("message")]
		public static void PrintFormat([NotNull] this IInfoLogPrinter printer, [NotNull] string message, params object[] args) 
		{
			if(!printer.IsEnabled)
			{
				FormattingHelper.ReleaseUnused(args);
				return;
			}

			var printerImpl = printer as LogPrinterBase;
			if(printerImpl != null)
			{
				var formattedMessage = LogMessage.FormatString(message, args);
				printerImpl.PrintImpl(formattedMessage);
			}
		}

	
	}


	/// <summary>
	///		Fluent-��������� ��� ������ ������� � ������� <see cref="LogLevel.Warn"/>.
	/// </summary>
	[SupportExceptionLogging]
	[PublicAPI]
    public interface IWarnLogPrinter
	{
        /// <summary>
        ///     �������� �� ������� ����������� <see cref="LogLevel.Warn"/>.
        /// </summary>
        bool IsEnabled { get; }

		/// <summary>
        ///     �������� � ��� ��������� � ������� <see cref="LogLevel.Warn"/>.
        /// </summary>
		[LogMethod]
		void Print([NotNull] IPrintable message);

		/// <summary>
        ///     �������� � ��� ��������� � ������� <see cref="LogLevel.Warn"/>.
        /// </summary>
        [LogMethod]
		void Print([NotNull] System.FormattableString message);

		/// <summary>
        ///     �������� � ��� ��������� � ������� <see cref="LogLevel.Warn"/>.
        /// </summary>
        [LogMethod]
		void Print([NotNull] System.FormattableString message, params ILogField[] fields);

		/// <summary>
        ///     �������� � ��� ��������� � ������� <see cref="LogLevel.Warn"/>.
        /// </summary>
		[LogExceptionMethod]
		void Print([NotNull] System.Exception exception, [NotNull] IPrintable message);

        /// <summary>
        ///     �������� � ��� ��������� � ������� <see cref="LogLevel.Warn"/>.
        /// </summary>
        [LogExceptionMethod]
		void Print([NotNull] System.Exception exception, [NotNull] System.FormattableString message);

        /// <summary>
        ///     �������� � ��� ��������� � ������� <see cref="LogLevel.Warn"/>.
        /// </summary>
        [LogExceptionMethod]
		void Print([NotNull] System.Exception exception, [NotNull] System.FormattableString message, params ILogField[] fields);
	}

	/// <summary>
    ///     ������-���������� ��� <see cref="IWarnLogPrinter"/>
    /// </summary>
	[PublicAPI]
    public static class WarnLogPrinterExtensions
	{

		/// <summary>
        ///     �������� � ��� ��������� � ������� <see cref="LogLevel.Warn"/>.
        /// </summary>
        [LogMethod]
		public static void Print([NotNull] this IWarnLogPrinter printer, [NotNull] string message) 
		{
			if(!printer.IsEnabled)
			{
				return;
			}

			var printerImpl = printer as LogPrinterBase;
			if(printerImpl != null)
			{
				printerImpl.PrintImpl(LogMessage.Preformatted(message));
			}
		}

		/// <summary>
        ///     �������� � ��� ��������� � ������� <see cref="LogLevel.Warn"/>.
        /// </summary>
		[LogMethod]
		public static void Print([NotNull] this IWarnLogPrinter printer, [NotNull] string message, params ILogField[] fields) 
		{
			if(!printer.IsEnabled)
			{
				FormattingHelper.ReleaseUnused(fields);
				return;
			}

			var printerImpl = printer as LogPrinterBase;
			if(printerImpl != null)
			{
				var formattedMessage = LogMessage.MakeString(message, fields);
				printerImpl.PrintImpl(formattedMessage);
			}
		}

		/// <summary>
        ///     �������� � ��� ��������� � ������� <see cref="LogLevel.Warn"/>.
        /// </summary>
		[LogMethod, StringFormatMethod("message")]
		public static void PrintFormat([NotNull] this IWarnLogPrinter printer, [NotNull] string message, params object[] args) 
		{
			if(!printer.IsEnabled)
			{
				FormattingHelper.ReleaseUnused(args);
				return;
			}

			var printerImpl = printer as LogPrinterBase;
			if(printerImpl != null)
			{
				var formattedMessage = LogMessage.FormatString(message, args);
				printerImpl.PrintImpl(formattedMessage);
			}
		}

		/// <summary>
        ///     �������� � ��� ��������� � ������� <see cref="LogLevel.Warn"/>.
        /// </summary>
        [LogExceptionMethod]
		public static void Print([NotNull] this IWarnLogPrinter printer, [NotNull] System.Exception exception, [NotNull] string message) 
		{
			if(!printer.IsEnabled)
			{
				return;
			}

			var printerImpl = printer as LogPrinterBase;
			if(printerImpl != null)
			{
				printerImpl.PrintImpl(exception, LogMessage.Preformatted(message));
			}
		}
				
		/// <summary>
        ///     �������� � ��� ��������� � ������� <see cref="LogLevel.Warn"/>.
        /// </summary>		
		[LogExceptionMethod]
		public static void Print([NotNull] this IWarnLogPrinter printer, [NotNull] System.Exception exception, [NotNull] string message, params ILogField[] fields) 
		{
			if(!printer.IsEnabled)
			{
				FormattingHelper.ReleaseUnused(fields);
				return;
			}

			var printerImpl = printer as LogPrinterBase;
			if(printerImpl != null)
			{
				var formattedMessage = LogMessage.MakeString(message, fields);
				printerImpl.PrintImpl(exception, formattedMessage);
			}
		}

		/// <summary>
        ///     �������� � ��� ��������� � ������� <see cref="LogLevel.Warn"/>.
        /// </summary>
        [LogExceptionMethod, StringFormatMethod("message")]
		public static void PrintFormat([NotNull] this IWarnLogPrinter printer, [NotNull] System.Exception exception, [NotNull] string message, params object[] args) 
		{
			if(!printer.IsEnabled)
			{
				FormattingHelper.ReleaseUnused(args);
				return;
			}

			var printerImpl = printer as LogPrinterBase;
			if(printerImpl != null)
			{
				var formattedMessage = LogMessage.FormatString(message, args);
				printerImpl.PrintImpl(exception, formattedMessage);
			}
		}
	
	}


	/// <summary>
	///		Fluent-��������� ��� ������ ������� � ������� <see cref="LogLevel.Error"/>.
	/// </summary>
	[SupportExceptionLogging]
	[PublicAPI]
    public interface IErrorLogPrinter
	{
        /// <summary>
        ///     �������� �� ������� ����������� <see cref="LogLevel.Error"/>.
        /// </summary>
        bool IsEnabled { get; }

		/// <summary>
        ///     �������� � ��� ��������� � ������� <see cref="LogLevel.Error"/>.
        /// </summary>
		[LogMethod]
		void Print([NotNull] IPrintable message);

		/// <summary>
        ///     �������� � ��� ��������� � ������� <see cref="LogLevel.Error"/>.
        /// </summary>
        [LogMethod]
		void Print([NotNull] System.FormattableString message);

		/// <summary>
        ///     �������� � ��� ��������� � ������� <see cref="LogLevel.Error"/>.
        /// </summary>
        [LogMethod]
		void Print([NotNull] System.FormattableString message, params ILogField[] fields);

		/// <summary>
        ///     �������� � ��� ��������� � ������� <see cref="LogLevel.Error"/>.
        /// </summary>
		[LogExceptionMethod]
		void Print([NotNull] System.Exception exception, [NotNull] IPrintable message);

        /// <summary>
        ///     �������� � ��� ��������� � ������� <see cref="LogLevel.Error"/>.
        /// </summary>
        [LogExceptionMethod]
		void Print([NotNull] System.Exception exception, [NotNull] System.FormattableString message);

        /// <summary>
        ///     �������� � ��� ��������� � ������� <see cref="LogLevel.Error"/>.
        /// </summary>
        [LogExceptionMethod]
		void Print([NotNull] System.Exception exception, [NotNull] System.FormattableString message, params ILogField[] fields);
	}

	/// <summary>
    ///     ������-���������� ��� <see cref="IErrorLogPrinter"/>
    /// </summary>
	[PublicAPI]
    public static class ErrorLogPrinterExtensions
	{

		/// <summary>
        ///     �������� � ��� ��������� � ������� <see cref="LogLevel.Error"/>.
        /// </summary>
        [LogMethod]
		public static void Print([NotNull] this IErrorLogPrinter printer, [NotNull] string message) 
		{
			if(!printer.IsEnabled)
			{
				return;
			}

			var printerImpl = printer as LogPrinterBase;
			if(printerImpl != null)
			{
				printerImpl.PrintImpl(LogMessage.Preformatted(message));
			}
		}

		/// <summary>
        ///     �������� � ��� ��������� � ������� <see cref="LogLevel.Error"/>.
        /// </summary>
		[LogMethod]
		public static void Print([NotNull] this IErrorLogPrinter printer, [NotNull] string message, params ILogField[] fields) 
		{
			if(!printer.IsEnabled)
			{
				FormattingHelper.ReleaseUnused(fields);
				return;
			}

			var printerImpl = printer as LogPrinterBase;
			if(printerImpl != null)
			{
				var formattedMessage = LogMessage.MakeString(message, fields);
				printerImpl.PrintImpl(formattedMessage);
			}
		}

		/// <summary>
        ///     �������� � ��� ��������� � ������� <see cref="LogLevel.Error"/>.
        /// </summary>
		[LogMethod, StringFormatMethod("message")]
		public static void PrintFormat([NotNull] this IErrorLogPrinter printer, [NotNull] string message, params object[] args) 
		{
			if(!printer.IsEnabled)
			{
				FormattingHelper.ReleaseUnused(args);
				return;
			}

			var printerImpl = printer as LogPrinterBase;
			if(printerImpl != null)
			{
				var formattedMessage = LogMessage.FormatString(message, args);
				printerImpl.PrintImpl(formattedMessage);
			}
		}

		/// <summary>
        ///     �������� � ��� ��������� � ������� <see cref="LogLevel.Error"/>.
        /// </summary>
        [LogExceptionMethod]
		public static void Print([NotNull] this IErrorLogPrinter printer, [NotNull] System.Exception exception, [NotNull] string message) 
		{
			if(!printer.IsEnabled)
			{
				return;
			}

			var printerImpl = printer as LogPrinterBase;
			if(printerImpl != null)
			{
				printerImpl.PrintImpl(exception, LogMessage.Preformatted(message));
			}
		}
				
		/// <summary>
        ///     �������� � ��� ��������� � ������� <see cref="LogLevel.Error"/>.
        /// </summary>		
		[LogExceptionMethod]
		public static void Print([NotNull] this IErrorLogPrinter printer, [NotNull] System.Exception exception, [NotNull] string message, params ILogField[] fields) 
		{
			if(!printer.IsEnabled)
			{
				FormattingHelper.ReleaseUnused(fields);
				return;
			}

			var printerImpl = printer as LogPrinterBase;
			if(printerImpl != null)
			{
				var formattedMessage = LogMessage.MakeString(message, fields);
				printerImpl.PrintImpl(exception, formattedMessage);
			}
		}

		/// <summary>
        ///     �������� � ��� ��������� � ������� <see cref="LogLevel.Error"/>.
        /// </summary>
        [LogExceptionMethod, StringFormatMethod("message")]
		public static void PrintFormat([NotNull] this IErrorLogPrinter printer, [NotNull] System.Exception exception, [NotNull] string message, params object[] args) 
		{
			if(!printer.IsEnabled)
			{
				FormattingHelper.ReleaseUnused(args);
				return;
			}

			var printerImpl = printer as LogPrinterBase;
			if(printerImpl != null)
			{
				var formattedMessage = LogMessage.FormatString(message, args);
				printerImpl.PrintImpl(exception, formattedMessage);
			}
		}
	
	}


	/// <summary>
	///		Fluent-��������� ��� ������ ������� � ������� <see cref="LogLevel.Fatal"/>.
	/// </summary>
	[SupportExceptionLogging]
	[PublicAPI]
    public interface IFatalLogPrinter
	{
        /// <summary>
        ///     �������� �� ������� ����������� <see cref="LogLevel.Fatal"/>.
        /// </summary>
        bool IsEnabled { get; }



		/// <summary>
        ///     �������� � ��� ��������� � ������� <see cref="LogLevel.Fatal"/>.
        /// </summary>
		[LogExceptionMethod]
		void Print([NotNull] System.Exception exception, [NotNull] IPrintable message);

        /// <summary>
        ///     �������� � ��� ��������� � ������� <see cref="LogLevel.Fatal"/>.
        /// </summary>
        [LogExceptionMethod]
		void Print([NotNull] System.Exception exception, [NotNull] System.FormattableString message);

        /// <summary>
        ///     �������� � ��� ��������� � ������� <see cref="LogLevel.Fatal"/>.
        /// </summary>
        [LogExceptionMethod]
		void Print([NotNull] System.Exception exception, [NotNull] System.FormattableString message, params ILogField[] fields);
	}

	/// <summary>
    ///     ������-���������� ��� <see cref="IFatalLogPrinter"/>
    /// </summary>
	[PublicAPI]
    public static class FatalLogPrinterExtensions
	{


		/// <summary>
        ///     �������� � ��� ��������� � ������� <see cref="LogLevel.Fatal"/>.
        /// </summary>
        [LogExceptionMethod]
		public static void Print([NotNull] this IFatalLogPrinter printer, [NotNull] System.Exception exception, [NotNull] string message) 
		{
			if(!printer.IsEnabled)
			{
				return;
			}

			var printerImpl = printer as LogPrinterBase;
			if(printerImpl != null)
			{
				printerImpl.PrintImpl(exception, LogMessage.Preformatted(message));
			}
		}
				
		/// <summary>
        ///     �������� � ��� ��������� � ������� <see cref="LogLevel.Fatal"/>.
        /// </summary>		
		[LogExceptionMethod]
		public static void Print([NotNull] this IFatalLogPrinter printer, [NotNull] System.Exception exception, [NotNull] string message, params ILogField[] fields) 
		{
			if(!printer.IsEnabled)
			{
				FormattingHelper.ReleaseUnused(fields);
				return;
			}

			var printerImpl = printer as LogPrinterBase;
			if(printerImpl != null)
			{
				var formattedMessage = LogMessage.MakeString(message, fields);
				printerImpl.PrintImpl(exception, formattedMessage);
			}
		}

		/// <summary>
        ///     �������� � ��� ��������� � ������� <see cref="LogLevel.Fatal"/>.
        /// </summary>
        [LogExceptionMethod, StringFormatMethod("message")]
		public static void PrintFormat([NotNull] this IFatalLogPrinter printer, [NotNull] System.Exception exception, [NotNull] string message, params object[] args) 
		{
			if(!printer.IsEnabled)
			{
				FormattingHelper.ReleaseUnused(args);
				return;
			}

			var printerImpl = printer as LogPrinterBase;
			if(printerImpl != null)
			{
				var formattedMessage = LogMessage.FormatString(message, args);
				printerImpl.PrintImpl(exception, formattedMessage);
			}
		}
	
	}

}
