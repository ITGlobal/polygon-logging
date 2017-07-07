

// <auto-generated>
//      This code was generated using T4 text template
//      Generated at 03/24/2017 12:44:23
//
//      Changes to this file may cause incorrect behaviour and will be lost 
//      if the code is regenerated.
// </auto-generated>

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Polygon.Diagnostics
{
	partial class FormattingHelper
    {
		/// <summary>
		///		��������������� ��������
		/// </summary>
		public static string Format(IPrintable value, string format = null) 
		{
			if(ReferenceEquals(value, null))
			{
				return NullStr;
			}

			return value.Print(PrintOption.Default);
		}
		/// <summary>
		///		��������������� ��������
		/// </summary>
		public static string Format(string value, string format = null) 
		{
			return FormattingHelper.EscapeString(value);
		}
		/// <summary>
		///		��������������� ��������
		/// </summary>
		public static string Format(bool value, string format = null) 
		{
			return value ? "true" : "false";
		}
		/// <summary>
		///		��������������� ��������
		/// </summary>
		public static string Format(bool? nullableValue, string format = null) 
		{
			if(!nullableValue.HasValue)
			{
				return NullStr;
			}

			var value = nullableValue.Value;
			return value ? "true" : "false";
		}
		/// <summary>
		///		��������������� ��������
		/// </summary>
		public static string Format(int value, string format = null) 
		{
			if(format != null)
			{
				return ((IFormattable)value).ToString(format, FormatProvider);
			}
			return string.Format(FormatProvider, "{0:D}", value);
		}
		/// <summary>
		///		��������������� ��������
		/// </summary>
		public static string Format(int? nullableValue, string format = null) 
		{
			if(!nullableValue.HasValue)
			{
				return NullStr;
			}

			var value = nullableValue.Value;
			if(format != null)
			{
				return ((IFormattable)value).ToString(format, FormatProvider);
			}
			return string.Format(FormatProvider, "{0:D}", value);
		}
		/// <summary>
		///		��������������� ��������
		/// </summary>
		public static string Format(uint value, string format = null) 
		{
			if(format != null)
			{
				return ((IFormattable)value).ToString(format, FormatProvider) + "U";
			}
			return string.Format(FormatProvider, "{0:D}U", value);
		}
		/// <summary>
		///		��������������� ��������
		/// </summary>
		public static string Format(uint? nullableValue, string format = null) 
		{
			if(!nullableValue.HasValue)
			{
				return NullStr;
			}

			var value = nullableValue.Value;
			if(format != null)
			{
				return ((IFormattable)value).ToString(format, FormatProvider) + "U";
			}
			return string.Format(FormatProvider, "{0:D}U", value);
		}
		/// <summary>
		///		��������������� ��������
		/// </summary>
		public static string Format(long value, string format = null) 
		{
			if(format != null)
			{
				return ((IFormattable)value).ToString(format, FormatProvider) + "L";
			}
			return string.Format(FormatProvider, "{0:D}L", value);
		}
		/// <summary>
		///		��������������� ��������
		/// </summary>
		public static string Format(long? nullableValue, string format = null) 
		{
			if(!nullableValue.HasValue)
			{
				return NullStr;
			}

			var value = nullableValue.Value;
			if(format != null)
			{
				return ((IFormattable)value).ToString(format, FormatProvider) + "L";
			}
			return string.Format(FormatProvider, "{0:D}L", value);
		}
		/// <summary>
		///		��������������� ��������
		/// </summary>
		public static string Format(ulong value, string format = null) 
		{
			if(format != null)
			{
				return ((IFormattable)value).ToString(format, FormatProvider) + "UL";
			}
			return string.Format(FormatProvider, "{0:D}UL", value);
		}
		/// <summary>
		///		��������������� ��������
		/// </summary>
		public static string Format(ulong? nullableValue, string format = null) 
		{
			if(!nullableValue.HasValue)
			{
				return NullStr;
			}

			var value = nullableValue.Value;
			if(format != null)
			{
				return ((IFormattable)value).ToString(format, FormatProvider) + "UL";
			}
			return string.Format(FormatProvider, "{0:D}UL", value);
		}
		/// <summary>
		///		��������������� ��������
		/// </summary>
		public static string Format(short value, string format = null) 
		{
			if(format != null)
			{
				return ((IFormattable)value).ToString(format, FormatProvider) + "S";
			}
			return string.Format(FormatProvider, "{0:D}S", value);
		}
		/// <summary>
		///		��������������� ��������
		/// </summary>
		public static string Format(short? nullableValue, string format = null) 
		{
			if(!nullableValue.HasValue)
			{
				return NullStr;
			}

			var value = nullableValue.Value;
			if(format != null)
			{
				return ((IFormattable)value).ToString(format, FormatProvider) + "S";
			}
			return string.Format(FormatProvider, "{0:D}S", value);
		}
		/// <summary>
		///		��������������� ��������
		/// </summary>
		public static string Format(ushort value, string format = null) 
		{
			if(format != null)
			{
				return ((IFormattable)value).ToString(format, FormatProvider) + "US";
			}
			return string.Format(FormatProvider, "{0:D}US", value);
		}
		/// <summary>
		///		��������������� ��������
		/// </summary>
		public static string Format(ushort? nullableValue, string format = null) 
		{
			if(!nullableValue.HasValue)
			{
				return NullStr;
			}

			var value = nullableValue.Value;
			if(format != null)
			{
				return ((IFormattable)value).ToString(format, FormatProvider) + "US";
			}
			return string.Format(FormatProvider, "{0:D}US", value);
		}
		/// <summary>
		///		��������������� ��������
		/// </summary>
		public static string Format(byte value, string format = null) 
		{
			if(format != null)
			{
				return ((IFormattable)value).ToString(format, FormatProvider) + "B";
			}
			return string.Format(FormatProvider, "{0:D}B", value);
		}
		/// <summary>
		///		��������������� ��������
		/// </summary>
		public static string Format(byte? nullableValue, string format = null) 
		{
			if(!nullableValue.HasValue)
			{
				return NullStr;
			}

			var value = nullableValue.Value;
			if(format != null)
			{
				return ((IFormattable)value).ToString(format, FormatProvider) + "B";
			}
			return string.Format(FormatProvider, "{0:D}B", value);
		}
		/// <summary>
		///		��������������� ��������
		/// </summary>
		public static string Format(sbyte value, string format = null) 
		{
			if(format != null)
			{
				return ((IFormattable)value).ToString(format, FormatProvider) + "SB";
			}
			return string.Format(FormatProvider, "{0:D}SB", value);
		}
		/// <summary>
		///		��������������� ��������
		/// </summary>
		public static string Format(sbyte? nullableValue, string format = null) 
		{
			if(!nullableValue.HasValue)
			{
				return NullStr;
			}

			var value = nullableValue.Value;
			if(format != null)
			{
				return ((IFormattable)value).ToString(format, FormatProvider) + "SB";
			}
			return string.Format(FormatProvider, "{0:D}SB", value);
		}
		/// <summary>
		///		��������������� ��������
		/// </summary>
		public static string Format(float value, string format = null) 
		{
			if(format != null)
			{
				return ((IFormattable)value).ToString(format, FormatProvider) + "f";
			}
			return string.Format(FormatProvider, "{0:F}f", value);
		}
		/// <summary>
		///		��������������� ��������
		/// </summary>
		public static string Format(float? nullableValue, string format = null) 
		{
			if(!nullableValue.HasValue)
			{
				return NullStr;
			}

			var value = nullableValue.Value;
			if(format != null)
			{
				return ((IFormattable)value).ToString(format, FormatProvider) + "f";
			}
			return string.Format(FormatProvider, "{0:F}f", value);
		}
		/// <summary>
		///		��������������� ��������
		/// </summary>
		public static string Format(double value, string format = null) 
		{
			if(format != null)
			{
				return ((IFormattable)value).ToString(format, FormatProvider) + "d";
			}
			return string.Format(FormatProvider, "{0:F}d", value);
		}
		/// <summary>
		///		��������������� ��������
		/// </summary>
		public static string Format(double? nullableValue, string format = null) 
		{
			if(!nullableValue.HasValue)
			{
				return NullStr;
			}

			var value = nullableValue.Value;
			if(format != null)
			{
				return ((IFormattable)value).ToString(format, FormatProvider) + "d";
			}
			return string.Format(FormatProvider, "{0:F}d", value);
		}
		/// <summary>
		///		��������������� ��������
		/// </summary>
		public static string Format(decimal value, string format = null) 
		{
			if(format != null)
			{
				return ((IFormattable)value).ToString(format, FormatProvider) + "M";
			}
			return string.Format(FormatProvider, "{0:F}M", value);
		}
		/// <summary>
		///		��������������� ��������
		/// </summary>
		public static string Format(decimal? nullableValue, string format = null) 
		{
			if(!nullableValue.HasValue)
			{
				return NullStr;
			}

			var value = nullableValue.Value;
			if(format != null)
			{
				return ((IFormattable)value).ToString(format, FormatProvider) + "M";
			}
			return string.Format(FormatProvider, "{0:F}M", value);
		}
		/// <summary>
		///		��������������� ��������
		/// </summary>
		public static string Format(DateTime value, string format = null) 
		{
			if(format != null)
			{
				return ((IFormattable)value).ToString(format, FormatProvider);
			}
			return string.Format(FormatProvider, "{0:s}", value);
		}
		/// <summary>
		///		��������������� ��������
		/// </summary>
		public static string Format(DateTime? nullableValue, string format = null) 
		{
			if(!nullableValue.HasValue)
			{
				return NullStr;
			}

			var value = nullableValue.Value;
			if(format != null)
			{
				return ((IFormattable)value).ToString(format, FormatProvider);
			}
			return string.Format(FormatProvider, "{0:s}", value);
		}
		/// <summary>
		///		��������������� ��������
		/// </summary>
		public static string Format(TimeSpan value, string format = null) 
		{
			if(format != null)
			{
				return ((IFormattable)value).ToString(format, FormatProvider);
			}
			return string.Format(FormatProvider, "{0:g}", value);
		}
		/// <summary>
		///		��������������� ��������
		/// </summary>
		public static string Format(TimeSpan? nullableValue, string format = null) 
		{
			if(!nullableValue.HasValue)
			{
				return NullStr;
			}

			var value = nullableValue.Value;
			if(format != null)
			{
				return ((IFormattable)value).ToString(format, FormatProvider);
			}
			return string.Format(FormatProvider, "{0:g}", value);
		}
		/// <summary>
		///		��������������� ��������
		/// </summary>
		public static string Format(Guid value, string format = null) 
		{
			if(format != null)
			{
				return ((IFormattable)value).ToString(format, FormatProvider);
			}
			return string.Format(FormatProvider, "{0:D}", value);
		}
		/// <summary>
		///		��������������� ��������
		/// </summary>
		public static string Format(Guid? nullableValue, string format = null) 
		{
			if(!nullableValue.HasValue)
			{
				return NullStr;
			}

			var value = nullableValue.Value;
			if(format != null)
			{
				return ((IFormattable)value).ToString(format, FormatProvider);
			}
			return string.Format(FormatProvider, "{0:D}", value);
		}


		private static string TryPrintKnownType(object obj, string format)
		{
			var valueOfIPrintable = obj as IPrintable;
			if(!ReferenceEquals(valueOfIPrintable, null))
			{
				return Format(valueOfIPrintable, format);
			}
			if(obj is string)
			{
				return Format((string)obj, format);
			}
			if(obj is bool?)
			{
				return Format((bool?)obj, format);
			}
			if(obj is bool)
			{
				return Format((bool)obj, format);
			}
			if(obj is int?)
			{
				return Format((int?)obj, format);
			}
			if(obj is int)
			{
				return Format((int)obj, format);
			}
			if(obj is uint?)
			{
				return Format((uint?)obj, format);
			}
			if(obj is uint)
			{
				return Format((uint)obj, format);
			}
			if(obj is long?)
			{
				return Format((long?)obj, format);
			}
			if(obj is long)
			{
				return Format((long)obj, format);
			}
			if(obj is ulong?)
			{
				return Format((ulong?)obj, format);
			}
			if(obj is ulong)
			{
				return Format((ulong)obj, format);
			}
			if(obj is short?)
			{
				return Format((short?)obj, format);
			}
			if(obj is short)
			{
				return Format((short)obj, format);
			}
			if(obj is ushort?)
			{
				return Format((ushort?)obj, format);
			}
			if(obj is ushort)
			{
				return Format((ushort)obj, format);
			}
			if(obj is byte?)
			{
				return Format((byte?)obj, format);
			}
			if(obj is byte)
			{
				return Format((byte)obj, format);
			}
			if(obj is sbyte?)
			{
				return Format((sbyte?)obj, format);
			}
			if(obj is sbyte)
			{
				return Format((sbyte)obj, format);
			}
			if(obj is float?)
			{
				return Format((float?)obj, format);
			}
			if(obj is float)
			{
				return Format((float)obj, format);
			}
			if(obj is double?)
			{
				return Format((double?)obj, format);
			}
			if(obj is double)
			{
				return Format((double)obj, format);
			}
			if(obj is decimal?)
			{
				return Format((decimal?)obj, format);
			}
			if(obj is decimal)
			{
				return Format((decimal)obj, format);
			}
			if(obj is DateTime?)
			{
				return Format((DateTime?)obj, format);
			}
			if(obj is DateTime)
			{
				return Format((DateTime)obj, format);
			}
			if(obj is TimeSpan?)
			{
				return Format((TimeSpan?)obj, format);
			}
			if(obj is TimeSpan)
			{
				return Format((TimeSpan)obj, format);
			}
			if(obj is Guid?)
			{
				return Format((Guid?)obj, format);
			}
			if(obj is Guid)
			{
				return Format((Guid)obj, format);
			}
			return null;
		}
	}
}