using System;
using System.Collections;
using System.Data;
using System.Data.SqlTypes;
using System.IO;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;
using System.Xml;

#if FW2
using System.Collections.Generic;
using KeyValue = System.Collections.Generic.KeyValuePair<System.Type,System.Type>;
#else
using KeyValue = BLToolkit.Common.CompoundValue;
#endif

using BLToolkit.Common;
using BLToolkit.Reflection;
using BLToolkit.Reflection.Extension;

namespace BLToolkit.Mapping
{
	public abstract class MappingSchema
	{
		#region Constructors

		public MappingSchema()
		{
			InitNullValues();
		}

		#endregion

		#region ObjectMapper Support

		private Hashtable _mappers = new Hashtable();

		public ObjectMapper GetObjectMapper(Type type)
		{
			ObjectMapper om = (ObjectMapper)_mappers[type];

			if (om == null)
			{
				lock (_mappers.SyncRoot)
				{
					om = (ObjectMapper)_mappers[type];

					if (om == null)
					{
						om = CreateObjectMapper(type);

						if (om == null)
							throw new MappingException(
								string.Format("Cannot create object mapper for the '{0}' type.", type.FullName));

						SetObjectMapper(type, om);

						om.Init(this, type);
					}
				}
			}

			return om;
		}

		public void SetObjectMapper(Type type, ObjectMapper om)
		{
			if (type == null) throw new ArgumentNullException("type");

			_mappers[type] = om;

			if (type.IsAbstract)
				_mappers[TypeAccessor.GetAccessor(type).Type] = om;
		}

		protected virtual ObjectMapper CreateObjectMapper(Type type)
		{
			Attribute attr = TypeHelper.GetFirstAttribute(type, typeof(ObjectMapperAttribute));
			return attr == null? CreateObjectMapperInstance(type): ((ObjectMapperAttribute)attr).ObjectMapper;
		}

		protected virtual ObjectMapper CreateObjectMapperInstance(Type type)
		{
			return new ObjectMapper();
		}

		#endregion

		#region Public Members

		private ExtensionList _extensions;
		public  ExtensionList  Extensions
		{
			get { return _extensions;  }
			set { _extensions = value; }
		}
	
		#endregion

		#region Convert

		public virtual void InitNullValues()
		{
			_defaultSByteNullValue     = (SByte)    GetNullValue(typeof(SByte));
			_defaultInt16NullValue     = (Int16)    GetNullValue(typeof(Int16));
			_defaultInt32NullValue     = (Int32)    GetNullValue(typeof(Int32));
			_defaultInt64NullValue     = (Int64)    GetNullValue(typeof(Int64));
			_defaultByteNullValue      = (Byte)     GetNullValue(typeof(Byte));
			_defaultUInt16NullValue    = (UInt16)   GetNullValue(typeof(UInt16));
			_defaultUInt32NullValue    = (UInt32)   GetNullValue(typeof(UInt32));
			_defaultUInt64NullValue    = (UInt64)   GetNullValue(typeof(UInt64));
			_defaultCharNullValue      = (Char)     GetNullValue(typeof(Char));
			_defaultSingleNullValue    = (Single)   GetNullValue(typeof(Single));
			_defaultDoubleNullValue    = (Double)   GetNullValue(typeof(Double));
			_defaultBooleanNullValue   = (Boolean)  GetNullValue(typeof(Boolean));

			_defaultStringNullValue    = (String)   GetNullValue(typeof(String));
			_defaultDateTimeNullValue  = (DateTime) GetNullValue(typeof(DateTime));
			_defaultDecimalNullValue   = (Decimal)  GetNullValue(typeof(Decimal));
			_defaultGuidNullValue      = (Guid)     GetNullValue(typeof(Guid));
			_defaultStreamNullValue    = (Stream)   GetNullValue(typeof(Stream));
			_defaultXmlReaderNullValue = (XmlReader)GetNullValue(typeof(XmlReader));
		}

		#region Primitive Types

		private SByte _defaultSByteNullValue;
		[CLSCompliant(false)]
		public  SByte  DefaultSByteNullValue
		{
			get { return _defaultSByteNullValue;  }
			set { _defaultSByteNullValue = value; }
		}

		[CLSCompliant(false)]
		public virtual SByte ConvertToSByte(object value)
		{
			return
				value is SByte ? (SByte)value :
				value == null ? _defaultSByteNullValue :
#if FW2
					Convert<SByte, object>.From(value);
#else
					Convert.ToSByte(value);
#endif
		}

		private Int16 _defaultInt16NullValue;
		public  Int16  DefaultInt16NullValue
		{
			get { return _defaultInt16NullValue;  }
			set { _defaultInt16NullValue = value; }
		}

		public virtual Int16 ConvertToInt16(object value)
		{
			return
				value is Int16? (Int16)value:
				value == null?  _defaultInt16NullValue:
#if FW2
					Convert<Int16, object>.From(value);
#else
					Convert.ToInt16(value);
#endif
		}

		private Int32 _defaultInt32NullValue;
		public  Int32  DefaultInt32NullValue
		{
			get { return _defaultInt32NullValue;  }
			set { _defaultInt32NullValue = value; }
		}

		public virtual Int32 ConvertToInt32(object value)
		{
			return
				value is Int32? (Int32)value:
				value == null?  _defaultInt32NullValue:
#if FW2
					Convert<Int32, object>.From(value);
#else
					Convert.ToInt32(value);
#endif
		}

		private Int64 _defaultInt64NullValue;
		public  Int64  DefaultInt64NullValue
		{
			get { return _defaultInt64NullValue;  }
			set { _defaultInt64NullValue = value; }
		}

		public virtual Int64 ConvertToInt64(object value)
		{
			return
				value is Int64? (Int64)value:
				value == null?  _defaultInt64NullValue:
#if FW2
					Convert<Int64, object>.From(value);
#else
					Convert.ToInt64(value);
#endif
		}

		private Byte _defaultByteNullValue;
		public  Byte  DefaultByteNullValue
		{
			get { return _defaultByteNullValue;  }
			set { _defaultByteNullValue = value; }
		}

		public virtual Byte ConvertToByte(object value)
		{
			return
				value is Byte? (Byte)value:
				value == null? _defaultByteNullValue:
#if FW2
					Convert<Byte, object>.From(value);
#else
					Convert.ToByte(value);
#endif
		}

		private UInt16 _defaultUInt16NullValue;
		[CLSCompliant(false)]
		public  UInt16  DefaultUInt16NullValue
		{
			get { return _defaultUInt16NullValue;  }
			set { _defaultUInt16NullValue = value; }
		}

		[CLSCompliant(false)]
		public virtual UInt16 ConvertToUInt16(object value)
		{
			return
				value is UInt16? (UInt16)value:
				value == null?   _defaultUInt16NullValue:
#if FW2
					Convert<UInt16, object>.From(value);
#else
					Convert.ToUInt16(value);
#endif
		}

		private UInt32 _defaultUInt32NullValue;
		[CLSCompliant(false)]
		public  UInt32  DefaultUInt32NullValue
		{
			get { return _defaultUInt32NullValue;  }
			set { _defaultUInt32NullValue = value; }
		}

		[CLSCompliant(false)]
		public virtual UInt32 ConvertToUInt32(object value)
		{
			return
				value is UInt32? (UInt32)value:
				value == null?   _defaultUInt32NullValue:
#if FW2
					Convert<UInt32, object>.From(value);
#else
					Convert.ToUInt32(value);
#endif
		}

		private UInt64 _defaultUInt64NullValue;
		[CLSCompliant(false)]
		public  UInt64  DefaultUInt64NullValue
		{
			get { return _defaultUInt64NullValue;  }
			set { _defaultUInt64NullValue = value; }
		}

		[CLSCompliant(false)]
		public virtual UInt64 ConvertToUInt64(object value)
		{
			return
				value is UInt64? (UInt64)value:
				value == null?   _defaultUInt64NullValue:
#if FW2
					Convert<UInt64, object>.From(value);
#else
					Convert.ToUInt64(value);
#endif
		}

		private Char _defaultCharNullValue;
		public  Char  DefaultCharNullValue
		{
			get { return _defaultCharNullValue;  }
			set { _defaultCharNullValue = value; }
		}

		public virtual Char ConvertToChar(object value)
		{
			return
				value is Char? (Char)value:
				value == null? _defaultCharNullValue:
#if FW2
					Convert<Char, object>.From(value);
#else
					Convert.ToChar(value);
#endif
		}

		private Single _defaultSingleNullValue;
		public  Single  DefaultSingleNullValue
		{
			get { return _defaultSingleNullValue;  }
			set { _defaultSingleNullValue = value; }
		}

		public virtual Single ConvertToSingle(object value)
		{
			return
				value is Single? (Single)value:
				value == null?   _defaultSingleNullValue:
#if FW2
					Convert<Single, object>.From(value);
#else
					Convert.ToSingle(value);
#endif
		}

		private Double _defaultDoubleNullValue;
		public  Double  DefaultDoubleNullValue
		{
			get { return _defaultDoubleNullValue;  }
			set { _defaultDoubleNullValue = value; }
		}

		public virtual Double ConvertToDouble(object value)
		{
			return
				value is Double? (Double)value:
				value == null?   _defaultDoubleNullValue:
#if FW2
					Convert<Double, object>.From(value);
#else
					Convert.ToDouble(value);
#endif
		}

		private Boolean _defaultBooleanNullValue;
		public  Boolean  DefaultBooleanNullValue
		{
			get { return _defaultBooleanNullValue;  }
			set { _defaultBooleanNullValue = value; }
		}

		public virtual Boolean ConvertToBoolean(object value)
		{
			return
				value is Boolean? (Boolean)value:
				value == null?    _defaultBooleanNullValue:
#if FW2
					Convert<Boolean, object>.From(value);
#else
					Convert.ToBoolean(value);
#endif
		}

		#endregion

		#region Simple Types

		private string _defaultStringNullValue;
		public  string  DefaultStringNullValue
		{
			get { return _defaultStringNullValue;  }
			set { _defaultStringNullValue = value; }
		}

		public virtual String ConvertToString(object value)
		{
			return
				value is String? (String)value :
				value == null?   _defaultStringNullValue:
#if FW2
					Convert<String, object>.From(value);
#else
					Convert.ToString(value);
#endif
		}

		private DateTime _defaultDateTimeNullValue;
		public  DateTime  DefaultDateTimeNullValue
		{
			get { return _defaultDateTimeNullValue;  }
			set { _defaultDateTimeNullValue = value; }
		}

		public virtual DateTime ConvertToDateTime(object value)
		{
			return
				value is DateTime? (DateTime)value:
				value == null?     _defaultDateTimeNullValue:
#if FW2
					Convert<DateTime, object>.From(value);
#else
					Convert.ToDateTime(value);
#endif
		}

		private decimal _defaultDecimalNullValue;
		public  decimal  DefaultDecimalNullValue
		{
			get { return _defaultDecimalNullValue;  }
			set { _defaultDecimalNullValue = value; }
		}

		public virtual Decimal ConvertToDecimal(object value)
		{
			return
				value is Decimal? (Decimal)value:
				value == null?    _defaultDecimalNullValue:
#if FW2
					Convert<Decimal, object>.From(value);
#else
					Convert.ToDecimal(value);
#endif
		}

		private Guid _defaultGuidNullValue;
		public  Guid  DefaultGuidNullValue
		{
			get { return _defaultGuidNullValue;  }
			set { _defaultGuidNullValue = value; }
		}

		public virtual Guid ConvertToGuid(object value)
		{
			return
				value is Guid? (Guid)value:
				value == null?  _defaultGuidNullValue:
#if FW2
					Convert<Guid, object>.From(value);
#else
					new Guid(value.ToString());
#endif
		}

		private Stream _defaultStreamNullValue;
		public  Stream  DefaultStreamNullValue
		{
			get { return _defaultStreamNullValue; }
			set { _defaultStreamNullValue = value; }
		}

		public virtual Stream ConvertToStream(object value)
		{
			if (value is Stream) return (Stream)value;
			if (value == null)   return _defaultStreamNullValue;
#if FW2
			return Convert<Stream, object>.From(value);
#else
			if (value is SqlBinary) return new MemoryStream(((SqlBinary)value).Value);
			if (value is byte[])    return new MemoryStream((byte[])value);

			throw new MappingException(value.GetType(), typeof(Stream));
#endif
		}

		private XmlReader _defaultXmlReaderNullValue;
		public  XmlReader  DefaultXmlReaderNullValue
		{
			get { return _defaultXmlReaderNullValue; }
			set { _defaultXmlReaderNullValue = value; }
		}

		public virtual XmlReader ConvertToXmlReader(object value)
		{
			if (value is XmlReader) return (XmlReader)value;
			if (value == null)      return _defaultXmlReaderNullValue;
#if FW2
			return Convert<XmlReader, object>.From(value);
#else
			throw new MappingException(value.GetType(), typeof(XmlReader));
#endif
		}

		public virtual byte[] ConvertToByteArray(object value)
		{
			if (value is byte[]) return (byte[])value;
			if (value == null)   return null;
#if FW2
			return Convert<Byte[], object>.From(value);
#else
			if (value is SqlBinary) return ((SqlBinary)value).Value;

			throw new MappingException(value.GetType(), typeof(byte[]));
#endif
		}

		public virtual char[] ConvertToCharArray(object value)
		{
			if (value is char[])    return (char[])value;
			if (value == null)      return null;
#if FW2
			return Convert<Char[], object>.From(value);
#else
			if (value is string)    return ((string)value).ToCharArray();
			if (value is SqlString) return ((SqlString)value).Value.ToCharArray();

			throw new MappingException(value.GetType(), typeof(char[]));
#endif
		}

		#endregion

#if FW2

		#region Nullable Types

		[CLSCompliant(false)]
		public virtual SByte? ConvertToNullableSByte(object value)
		{
			if (value is SByte?) return (SByte?)value;
			if (value == null)   return null;

			return Convert<SByte?, object>.From(value);
		}

		public virtual Int16? ConvertToNullableInt16(object value)
		{
			if (value is Int16?) return (Int16?)value;
			if (value == null)   return null;

			return Convert<Int16?, object>.From(value);
		}

		public virtual Int32? ConvertToNullableInt32(object value)
		{
			if (value is Int32?) return (Int32?)value;
			if (value == null)   return null;

			return Convert<Int32?, object>.From(value);
		}

		public virtual Int64? ConvertToNullableInt64(object value)
		{
			if (value is Int64?) return (Int64?)value;
			if (value == null)   return null;

			return Convert<Int64?, object>.From(value);
		}

		public virtual Byte? ConvertToNullableByte(object value)
		{
			if (value is Byte?) return (Byte?)value;
			if (value == null)  return null;

			return Convert<Byte?, object>.From(value);
		}

		[CLSCompliant(false)]
		public virtual UInt16? ConvertToNullableUInt16(object value)
		{
			if (value is UInt16?) return (UInt16?)value;
			if (value == null)    return null;

			return Convert<UInt16?, object>.From(value);
		}

		[CLSCompliant(false)]
		public virtual UInt32? ConvertToNullableUInt32(object value)
		{
			if (value is UInt32?) return (UInt32?)value;
			if (value == null)    return null;

			return Convert<UInt32?, object>.From(value);
		}

		[CLSCompliant(false)]
		public virtual UInt64? ConvertToNullableUInt64(object value)
		{
			if (value is UInt64?) return (UInt64?)value;
			if (value == null)    return null;

			return Convert<UInt64?, object>.From(value);
		}

		public virtual Char? ConvertToNullableChar(object value)
		{
			if (value is Char?) return (Char?)value;
			if (value == null)  return null;

			return Convert<Char?, object>.From(value);
		}

		public virtual Double? ConvertToNullableDouble(object value)
		{
			if (value is Double?) return (Double?)value;
			if (value == null)    return null;

			return Convert<Double?, object>.From(value);
		}

		public virtual Single? ConvertToNullableSingle(object value)
		{
			if (value is Single?) return (Single?)value;
			if (value == null)    return null;

			return Convert<Single?, object>.From(value);
		}

		public virtual Boolean? ConvertToNullableBoolean(object value)
		{
			if (value is Boolean?) return (Boolean?)value;
			if (value == null)     return null;

			return Convert<Boolean?, object>.From(value);
		}

		public virtual DateTime? ConvertToNullableDateTime(object value)
		{
			if (value is DateTime?) return (DateTime?)value;
			if (value == null)      return null;

			return Convert<DateTime?, object>.From(value);
		}

		public virtual Decimal? ConvertToNullableDecimal(object value)
		{
			if (value is Decimal?) return (Decimal?)value;
			if (value == null)     return null;

			return Convert<Decimal?, object>.From(value);
		}

		public virtual Guid? ConvertToNullableGuid(object value)
		{
			if (value is Guid?) return (Guid?)value;
			if (value == null)  return null;

			return Convert<Guid?, object>.From(value);
		}

		#endregion

#endif

		#region SqlTypes

		public virtual SqlByte ConvertToSqlByte(object value)
		{
			return
				value == null?    SqlByte.Null :
				value is SqlByte? (SqlByte)value:
#if FW2
					Convert<SqlByte, object>.From(value);
#else
					new SqlByte(ConvertToByte(value));
#endif
		}

		public virtual SqlInt16 ConvertToSqlInt16(object value)
		{
			return
				value == null?     SqlInt16.Null:
				value is SqlInt16? (SqlInt16)value:
#if FW2
					Convert<SqlInt16, object>.From(value);
#else
					new SqlInt16(ConvertToInt16(value));
#endif
		}

		public virtual SqlInt32 ConvertToSqlInt32(object value)
		{
			return
				value == null?     SqlInt32.Null:
				value is SqlInt32? (SqlInt32)value:
#if FW2
					Convert<SqlInt32, object>.From(value);
#else
					new SqlInt32(ConvertToInt32(value));
#endif
		}

		public virtual SqlInt64 ConvertToSqlInt64(object value)
		{
			return
				value == null?     SqlInt64.Null:
				value is SqlInt64? (SqlInt64)value:
#if FW2
					Convert<SqlInt64, object>.From(value);
#else
					new SqlInt64(ConvertToInt64(value));
#endif
		}

		public virtual SqlSingle ConvertToSqlSingle(object value)
		{
			return
				value == null?      SqlSingle.Null:
				value is SqlSingle? (SqlSingle)value:
#if FW2
					Convert<SqlSingle, object>.From(value);
#else
					new SqlSingle(ConvertToSingle(value));
#endif
		}

		public virtual SqlBoolean ConvertToSqlBoolean(object value)
		{
			return
				value == null?       SqlBoolean.Null:
				value is SqlBoolean? (SqlBoolean)value:
#if FW2
					Convert<SqlBoolean, object>.From(value);
#else
					new SqlBoolean(ConvertToBoolean(value));
#endif
		}

		public virtual SqlDouble ConvertToSqlDouble(object value)
		{
			return
				value == null?      SqlDouble.Null:
				value is SqlDouble? (SqlDouble)value:
#if FW2
					Convert<SqlDouble, object>.From(value);
#else
					new SqlDouble(ConvertToDouble(value));
#endif
		}

		public virtual SqlDateTime ConvertToSqlDateTime(object value)
		{
			return
				value == null?        SqlDateTime.Null:
				value is SqlDateTime? (SqlDateTime)value:
#if FW2
					Convert<SqlDateTime, object>.From(value);
#else
					new SqlDateTime(ConvertToDateTime(value));
#endif
		}

		public virtual SqlDecimal ConvertToSqlDecimal(object value)
		{
			return
				value == null?       SqlDecimal.Null:
				value is SqlDecimal? (SqlDecimal)value:
				value is SqlMoney?   ((SqlMoney)value).ToSqlDecimal():
#if FW2
					Convert<SqlDecimal, object>.From(value);
#else
					new SqlDecimal(ConvertToDecimal(value));
#endif
		}

		public virtual SqlMoney ConvertToSqlMoney(object value)
		{
			return
				value == null?       SqlMoney.Null:
				value is SqlMoney?   (SqlMoney)value:
				value is SqlDecimal? ((SqlDecimal)value).ToSqlMoney():
#if FW2
					Convert<SqlMoney, object>.From(value);
#else
					new SqlMoney(ConvertToDecimal(value));
#endif
		}

		public virtual SqlString ConvertToSqlString(object value)
		{
			return
				value == null?      SqlString.Null:
				value is SqlString? (SqlString)value:
#if FW2
					Convert<SqlString, object>.From(value);
#else
					new SqlString(ConvertToString(value));
#endif
		}

		public virtual SqlBinary ConvertToSqlBinary(object value)
		{
			if (value == null)      return SqlBinary.Null;
			if (value is SqlBinary) return (SqlBinary)value;
#if FW2
			return Convert<SqlBinary, object>.From(value);
#else
			if (value is byte[])    return new SqlBinary((byte[])value);

			throw new MappingException(value.GetType(), typeof(SqlBinary));
#endif
		}

		public virtual SqlGuid ConvertToSqlGuid(object value)
		{
			if (value == null)      return SqlGuid.Null;
			if (value is SqlGuid)   return (SqlGuid)value;

#if FW2
			return Convert<SqlGuid, object>.From(value);
#else
			if (value is Guid)      return new SqlGuid((Guid)value);
			if (value is byte[])    return new SqlGuid((byte[])value);
			if (value is string)    return new SqlGuid((string)value);
			if (value is SqlBinary) return ((SqlBinary)value).ToSqlGuid();

			throw new MappingException(value.GetType(), typeof(SqlGuid));
#endif
		}

#if FW2
		public virtual SqlBytes ConvertToSqlBytes(object value)
		{
			if (value == null)      return SqlBytes.Null;
			if (value is SqlBytes)  return (SqlBytes)value;

			return Convert<SqlBytes, object>.From(value);
		}

		public virtual SqlChars ConvertToSqlChars(object value)
		{
			if (value == null)      return SqlChars.Null;
			if (value is SqlChars)  return (SqlChars)value;

			return Convert<SqlChars, object>.From(value);
		}

		public virtual SqlXml ConvertToSqlXml(object value)
		{
			if (value == null)      return SqlXml.Null;
			if (value is SqlXml)    return (SqlXml)value;

			return Convert<SqlXml, object>.From(value);
		}
#endif

		#endregion

		#region General case

		public virtual object ConvertChangeType(object value, Type conversionType)
		{
			bool isNullable = false;
#if FW2
			if (conversionType.IsArray)
			{
				Type t = conversionType;

				do
				{
					t = t.GetElementType();
				}
				while (t.IsArray);

				isNullable = t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>);
			}
			else
			{
				isNullable = conversionType.IsGenericType &&
					conversionType.GetGenericTypeDefinition() == typeof(Nullable<>);
			}
#endif
			return ConvertChangeType(value, conversionType, isNullable);
		}

		public virtual object ConvertChangeType(object value, Type conversionType, bool isNullable)
		{
			if (conversionType.IsArray)
			{
				if (null == value)
					return null;
				
				Type srcType = value.GetType();

				if (srcType == conversionType)
					return value;

				if (srcType.IsArray)
				{
					Type srcElementType = srcType.GetElementType();
					Type dstElementType = conversionType.GetElementType();

					if (srcElementType.IsArray != dstElementType.IsArray
						|| (srcElementType.IsArray &&
							srcElementType.GetArrayRank() != dstElementType.GetArrayRank()))
					{
						throw new MappingException(srcType, conversionType);
					}

					Array srcArray = (Array)value;
					Array dstArray;

					int rank = srcArray.Rank;

					if (rank == 1 && 0 == srcArray.GetLowerBound(0))
					{
						int arrayLength = srcArray.Length;

						dstArray = Array.CreateInstance(dstElementType, arrayLength);

						// Int32 is assignable from UInt32, SByte from Byte and so on.
						//
						if (dstElementType.IsAssignableFrom(srcElementType))
							Array.Copy(srcArray, dstArray, arrayLength);
						else
							for (int i = 0; i < arrayLength; ++i)
								dstArray.SetValue(ConvertChangeType(srcArray.GetValue(i), dstElementType, isNullable), i);
					}
					else
					{
						int arrayLength = 1;
						int[] dimensions = new int[rank];
						int[] indices = new int[rank];
						int[] lbounds = new int[rank];

						for (int i = 0; i < rank; ++i)
						{
							arrayLength *= (dimensions[i] = srcArray.GetLength(i));
							lbounds[i] = srcArray.GetLowerBound(i);
						}

						dstArray = Array.CreateInstance(dstElementType, dimensions, lbounds);
						for (int i = 0; i < arrayLength; ++i)
						{
							int index = i;
							for (int j = rank - 1; j >= 0; --j)
							{
								indices[j] = index % dimensions[j] + lbounds[j];
								index /= dimensions[j];
							}

							dstArray.SetValue(ConvertChangeType(srcArray.GetValue(indices), dstElementType, isNullable), indices);
						}
					}

					return dstArray;
				}
			}

#if FW2
			if (isNullable)
			{
				switch (Type.GetTypeCode(conversionType))
				{
					case TypeCode.Boolean:  return ConvertToNullableBoolean (value);
					case TypeCode.Byte:     return ConvertToNullableByte    (value);
					case TypeCode.Char:     return ConvertToNullableChar    (value);
					case TypeCode.DateTime: return ConvertToNullableDateTime(value);
					case TypeCode.Decimal:  return ConvertToNullableDecimal (value);
					case TypeCode.Double:   return ConvertToNullableDouble  (value);
					case TypeCode.Int16:    return ConvertToNullableInt16   (value);
					case TypeCode.Int32:    return ConvertToNullableInt32   (value);
					case TypeCode.Int64:    return ConvertToNullableInt64   (value);
					case TypeCode.SByte:    return ConvertToNullableSByte   (value);
					case TypeCode.Single:   return ConvertToNullableSingle  (value);
					case TypeCode.UInt16:   return ConvertToNullableUInt16  (value);
					case TypeCode.UInt32:   return ConvertToNullableUInt32  (value);
					case TypeCode.UInt64:   return ConvertToNullableUInt64  (value);
				}

				if (typeof(Guid) == conversionType) return ConvertToNullableGuid(value);
			}
#endif

			switch (Type.GetTypeCode(conversionType))
			{
				case TypeCode.Boolean:  return ConvertToBoolean (value);
				case TypeCode.Byte:     return ConvertToByte    (value);
				case TypeCode.Char:     return ConvertToChar    (value);
				case TypeCode.DateTime: return ConvertToDateTime(value);
				case TypeCode.Decimal:  return ConvertToDecimal (value);
				case TypeCode.Double:   return ConvertToDouble  (value);
				case TypeCode.Int16:    return ConvertToInt16   (value);
				case TypeCode.Int32:    return ConvertToInt32   (value);
				case TypeCode.Int64:    return ConvertToInt64   (value);
				case TypeCode.SByte:    return ConvertToSByte   (value);
				case TypeCode.Single:   return ConvertToSingle  (value);
				case TypeCode.String:   return ConvertToString  (value);
				case TypeCode.UInt16:   return ConvertToUInt16  (value);
				case TypeCode.UInt32:   return ConvertToUInt32  (value);
				case TypeCode.UInt64:   return ConvertToUInt64  (value);
			}

			if (typeof(Guid)   == conversionType)      return ConvertToGuid  (value);
			if (typeof(Stream) == conversionType)      return ConvertToStream(value);
			if (typeof(XmlReader) == conversionType)   return ConvertToXmlReader(value);
			if (typeof(byte[]) == conversionType)      return ConvertToByteArray(value);
			if (typeof(char[]) == conversionType)      return ConvertToCharArray(value);

			if (typeof(SqlInt32)    == conversionType) return ConvertToSqlInt32   (value);
			if (typeof(SqlString)   == conversionType) return ConvertToSqlString  (value);
			if (typeof(SqlDecimal)  == conversionType) return ConvertToSqlDecimal (value);
			if (typeof(SqlDateTime) == conversionType) return ConvertToSqlDateTime(value);
			if (typeof(SqlBoolean)  == conversionType) return ConvertToSqlBoolean (value);
			if (typeof(SqlMoney)    == conversionType) return ConvertToSqlMoney   (value);
			if (typeof(SqlGuid)     == conversionType) return ConvertToSqlGuid    (value);
			if (typeof(SqlDouble)   == conversionType) return ConvertToSqlDouble  (value);
			if (typeof(SqlByte)     == conversionType) return ConvertToSqlByte    (value);
			if (typeof(SqlInt16)    == conversionType) return ConvertToSqlInt16   (value);
			if (typeof(SqlInt64)    == conversionType) return ConvertToSqlInt64   (value);
			if (typeof(SqlSingle)   == conversionType) return ConvertToSqlSingle  (value);
			if (typeof(SqlBinary)   == conversionType) return ConvertToSqlBinary  (value);
#if FW2
			if (typeof(SqlBytes)    == conversionType) return ConvertToSqlBytes   (value);
			if (typeof(SqlChars)    == conversionType) return ConvertToSqlChars   (value);
			if (typeof(SqlXml)      == conversionType) return ConvertToSqlXml     (value);
#endif

			try
			{
				return Convert.ChangeType(value, conversionType);
			}
			catch (InvalidCastException ex)
			{
				throw new MappingException(ex.Message, ex);
			}
		}

		#endregion
		
		#endregion

		#region Protected Members

		protected virtual DataReaderMapper CreateDataReaderMapper(IDataReader dataReader)
		{
			return new DataReaderMapper(this, dataReader);
		}

		protected virtual DataReaderListMapper CreateDataReaderListMapper(IDataReader reader)
		{
			return new DataReaderListMapper(CreateDataReaderMapper(reader));
		}

		protected virtual DataReaderMapper CreateDataReaderMapper(
			IDataReader          dataReader,
			NameOrIndexParameter nameOrIndex)
		{
			return new ScalarDataReaderMapper(this, dataReader, nameOrIndex);
		}

		protected virtual DataReaderListMapper CreateDataReaderListMapper(
			IDataReader          reader,
			NameOrIndexParameter nameOrIndex)
		{
			return new DataReaderListMapper(CreateDataReaderMapper(reader, nameOrIndex));
		}

		protected virtual DataRowMapper CreateDataRowMapper(DataRow row, DataRowVersion version)
		{
			return new DataRowMapper(row, version);
		}

		protected virtual DataTableMapper CreateDataTableMapper(DataTable dataTable, DataRowVersion version)
		{
			return new DataTableMapper(dataTable, CreateDataRowMapper(null, version));
		}

		protected virtual DictionaryMapper CreateDictionaryMapper(IDictionary dictionary)
		{
			return new DictionaryMapper(dictionary);
		}

		protected virtual DictionaryListMapper CreateDictionaryListMapper(
			IDictionary dic, NameOrIndexParameter keyField, ObjectMapper objectMapper)
		{
			return new DictionaryListMapper(dic, keyField, objectMapper);
		}
		
		protected virtual DictionaryIndexListMapper CreateDictionaryListMapper(
			IDictionary dic, MapIndex index, ObjectMapper objectMapper)
		{
			return new DictionaryIndexListMapper(dic, index, objectMapper);
		}

#if FW2
		protected virtual DictionaryListMapper<K, T> CreateDictionaryListMapper<K, T>(
			IDictionary<K, T> dic, NameOrIndexParameter keyField, ObjectMapper objectMapper)
		{
			return new DictionaryListMapper<K, T>(dic, keyField, objectMapper);
		}

		protected virtual DictionaryIndexListMapper<T> CreateDictionaryListMapper<T>(
			IDictionary<CompoundValue,T> dic, MapIndex index, ObjectMapper objectMapper)
		{
			return new DictionaryIndexListMapper<T>(dic, index, objectMapper);
		}
#endif

		protected virtual EnumeratorMapper CreateEnumeratorMapper(IEnumerator enumerator)
		{
			return new EnumeratorMapper(enumerator);
		}

		protected virtual ObjectListMapper CreateObjectListMapper(IList list, ObjectMapper objectMapper)
		{
			return new ObjectListMapper(list, objectMapper);
		}

		protected virtual ScalarListMapper CreateScalarListMapper(IList list, Type type)
		{
			return new ScalarListMapper(list, type);
		}

		protected virtual SimpleDestinationListMapper CreateScalarListListMapper(IList list, Type type)
		{
			return new SimpleDestinationListMapper(CreateScalarListMapper(list, type));
		}

#if FW2
		protected virtual ScalarListMapper<T> CreateScalarListMapper<T>(IList<T> list)
		{
			return new ScalarListMapper<T>(this, list);
		}

		protected virtual SimpleDestinationListMapper CreateScalarListListMapper<T>(IList<T> list)
		{
			return new SimpleDestinationListMapper(CreateScalarListMapper<T>(list));
		}
#endif

		#endregion

		#region GetNullValue

		public virtual object GetNullValue(Type type)
		{
			return TypeAccessor.GetNullValue(type);
		}

		public virtual bool IsNull(object value)
		{
			return TypeAccessor.IsNull(value);
		}

		#endregion

		#region GetMapValues

		private Hashtable _mapValues = new Hashtable();

		internal static ArrayList GetExtensionMapValues(TypeExtension typeExt, Type type)
		{
			AttributeExtensionCollection extList = typeExt.Attributes["MapValue"];

			if (extList == AttributeExtensionCollection.Null)
				return null;

			ArrayList attrs = new ArrayList(extList.Count);

			foreach (AttributeExtension ext in extList)
			{
				object origValue = ext["OrigValue"];

				if (origValue != null)
				{
					origValue = TypeExtension.ChangeType(origValue, type);
					attrs.Add(new MapValue(origValue, ext.Value));
				}
			}

			return attrs;
		}

		private MapValue[] GetMapValuesFromExtension(Type type)
		{
			TypeExtension typeExt = TypeExtension.GetTypeExtenstion(type, Extensions);

			if (typeExt == TypeExtension.Null)
				return null;

			ArrayList list = null;

			if (type.IsEnum)
				list = GetEnumMapValuesFromExtension(typeExt, type);

			if (list == null)
				list = GetExtensionMapValues(typeExt, type);

			return list != null? (MapValue[])list.ToArray(typeof(MapValue)): null;
		}

		private MapValue[] GetMapValuesFromType(Type type)
		{
			ArrayList list = null;

			if (type.IsEnum)
				list = GetEnumMapValuesFromType(type);

			object[] attrs = TypeHelper.GetAttributes(type, typeof(MapValueAttribute));

			if (attrs != null && attrs.Length != 0)
			{
				if (list == null)
					list = new ArrayList(attrs.Length);

				for (int i = 0; i < attrs.Length; i++)
				{
					MapValueAttribute a = (MapValueAttribute)attrs[i];
					list.Add(new MapValue(a.OrigValue, a.Values));
				}
			}

			return list != null? (MapValue[])list.ToArray(typeof(MapValue)): null;
		}

		public virtual MapValue[] GetMapValues(Type type)
		{
			if (type == null) throw new ArgumentNullException("type");

			MapValue[] mapValues = (MapValue[])_mapValues[type];

			if (mapValues != null || _mapValues.Contains(type))
				return mapValues;

			mapValues = GetMapValuesFromExtension(type);

			if (mapValues == null)
				mapValues = GetMapValuesFromType(type);

			_mapValues[type] = mapValues;

			return mapValues;
		}

		const FieldAttributes EnumField = FieldAttributes.Public | FieldAttributes.Static | FieldAttributes.Literal;

		internal static ArrayList GetEnumMapValuesFromExtension(TypeExtension typeExt, Type type)
		{
			ArrayList   mapValues = null;
			FieldInfo[] fields    = type.GetFields();

			foreach (FieldInfo fi in fields)
			{
				if ((fi.Attributes & EnumField) == EnumField)
				{
					AttributeExtensionCollection attrExt = typeExt[fi.Name]["MapValue"];

					ArrayList list      = new ArrayList(attrExt.Count);
					object    origValue = Enum.Parse(type, fi.Name);

					foreach (AttributeExtension ae in attrExt)
						if (ae.Value != null)
							list.Add(ae.Value);

					if (list.Count > 0)
					{
						if (mapValues == null)
							mapValues = new ArrayList(fields.Length);

						mapValues.Add(new MapValue(origValue, list.ToArray()));
					}
				}
			}

			return mapValues;
		}

		private ArrayList GetEnumMapValuesFromType(Type type)
		{
			ArrayList   mapValues = null;
			FieldInfo[] fields    = type.GetFields();

			foreach (FieldInfo fi in fields)
			{
				if ((fi.Attributes & EnumField) == EnumField)
				{
					Attribute[] enumAttributes = Attribute.GetCustomAttributes(fi, typeof(MapValueAttribute));

					foreach (MapValueAttribute attr in enumAttributes)
					{
						if (mapValues == null)
							mapValues = new ArrayList(fields.Length);

						object origValue = Enum.Parse(type, fi.Name);

						mapValues.Add(new MapValue(origValue, attr.Values));
					}
				}
			}

			return mapValues;
		}

		#endregion

		#region GetDefaultValue

		private Hashtable _defaultValues = new Hashtable();

		internal static object GetExtensionDefaultValue(TypeExtension typeExt, Type type)
		{
			object value = null;

			if (type.IsEnum)
				value = GetEnumDefaultValueFromExtension(typeExt, type);

			return value != null? value: typeExt.Attributes["DefaultValue"].Value;
		}

		public virtual object GetDefaultValue(Type type)
		{
			if (type == null) throw new ArgumentNullException("type");

			object defaultValue = _defaultValues[type];

			if (defaultValue != null || _defaultValues.Contains(type))
				return defaultValue;

			TypeExtension typeExt = TypeExtension.GetTypeExtenstion(type, Extensions);

			defaultValue = GetExtensionDefaultValue(typeExt, type);

			if (defaultValue == null)
			{
				if (type.IsEnum)
					defaultValue = GetEnumDefaultValueFromType(type);

				if (defaultValue == null)
				{
					object[] attrs = TypeHelper.GetAttributes(type, typeof(DefaultValueAttribute));

					if (attrs != null && attrs.Length != 0)
						defaultValue = ((DefaultValueAttribute)attrs[0]).Value;
				}
			}

			_defaultValues[type] = defaultValue = TypeExtension.ChangeType(defaultValue, type);

			return defaultValue;
		}

		private static object GetEnumDefaultValueFromExtension(TypeExtension typeExt, Type type)
		{
			FieldInfo[] fields = type.GetFields();

			foreach (FieldInfo fi in fields)
				if ((fi.Attributes & EnumField) == EnumField)
					if (typeExt[fi.Name]["DefaultValue"].Value != null)
						return Enum.Parse(type, fi.Name);

			return null;
		}

		private static object GetEnumDefaultValueFromType(Type type)
		{
			FieldInfo[] fields = type.GetFields();

			foreach (FieldInfo fi in fields)
			{
				if ((fi.Attributes & EnumField) == EnumField)
				{
					Attribute[] attrs = Attribute.GetCustomAttributes(fi, typeof(DefaultValueAttribute));

					if (attrs.Length > 0)
						return Enum.Parse(type, fi.Name);
				}
			}

			return null;
		}

		#endregion

		#region GetDataSource, GetDataDestination

		[CLSCompliant(false)]
		public virtual IMapDataSource GetDataSource(object obj)
		{
			if (obj == null) throw new ArgumentNullException("obj");

			if (obj is IMapDataSource)
				return (IMapDataSource)obj;

			if (obj is IDataReader)
				return CreateDataReaderMapper((IDataReader)obj);

			if (obj is DataRow)
				return CreateDataRowMapper((DataRow)obj, DataRowVersion.Default);

			if (obj is DataRowView)
				return CreateDataRowMapper(
					((DataRowView)obj).Row,
					((DataRowView)obj).RowVersion);

			if (obj is DataTable)
				return CreateDataRowMapper(((DataTable)(obj)).Rows[0], DataRowVersion.Default);

			if (obj is IDictionary)
				return CreateDictionaryMapper((IDictionary)obj);

			return GetObjectMapper(obj.GetType());
		}

		[CLSCompliant(false)]
		public virtual IMapDataDestination GetDataDestination(object obj)
		{
			if (obj == null) throw new ArgumentNullException("obj");

			if (obj is IMapDataDestination)
				return (IMapDataDestination)obj;

			if (obj is DataRow)
				return CreateDataRowMapper((DataRow)obj, DataRowVersion.Default);

			if (obj is DataRowView)
				return CreateDataRowMapper(
					((DataRowView)obj).Row,
					((DataRowView)obj).RowVersion);

			if (obj is DataTable)
			{
				DataTable dt = obj as DataTable;
				DataRow   dr = dt.NewRow();

				dt.Rows.Add(dr);

				return CreateDataRowMapper(dr, DataRowVersion.Default);
			}

			if (obj is IDictionary)
				return CreateDictionaryMapper((IDictionary)obj);

			return GetObjectMapper(obj.GetType());
		}

		#endregion

		#region ValueMapper

		[CLSCompliant(false)]
		public virtual IValueMapper DefaultValueMapper
		{
			get { return ValueMapping.DefaultMapper; }
		}

		private Hashtable _sameTypeMappers      = new Hashtable();
		private Hashtable _differentTypeMappers = new Hashtable();

		[CLSCompliant(false)]
		public void SetValueMapper(Type sourceType, Type destType, IValueMapper mapper)
		{
			if (sourceType == null) sourceType = typeof(object);
			if (destType   == null) destType   = typeof(object);

			if (sourceType == destType)
			{
				if (mapper == null)
					_sameTypeMappers.Remove(sourceType);
				else
					_sameTypeMappers[sourceType] = mapper;
			}
			else
			{
				KeyValue key = new KeyValue(sourceType, destType);

				if (mapper == null)
					_differentTypeMappers.Remove(key);
				else
					_differentTypeMappers[key] = mapper;
			}
		}

		[CLSCompliant(false)]
		protected virtual IValueMapper GetValueMapper(Type sourceType, Type destType)
		{
			return ValueMapping.GetMapper(sourceType, destType);
		}

		[CLSCompliant(false)]
		protected IValueMapper[] GetValueMappers(
			IMapDataSource source, IMapDataDestination dest, int[] index)
		{
			IValueMapper[] mappers = new IValueMapper[index.Length];

			for (int i = 0; i < index.Length; i++)
			{
				int n = index[i];

				if (n < 0)
					continue;

				if (!source.SupportsTypedValues(i) || !dest.SupportsTypedValues(n))
				{
					mappers[i] = DefaultValueMapper;
					continue;
				}

				Type sourceType = source.GetFieldType(i);
				Type destType   = dest.  GetFieldType(n);

				if (sourceType == null) sourceType = typeof(object);
				if (destType   == null) destType   = typeof(object);

				if (sourceType == destType)
				{
					IValueMapper t = (IValueMapper)_sameTypeMappers[sourceType];

					if (t == null)
						_sameTypeMappers[sourceType] = t = GetValueMapper(sourceType, destType);

					mappers[i] = t;
				}
				else
				{
					KeyValue     key = new KeyValue(sourceType, destType);
					IValueMapper t   = (IValueMapper)_differentTypeMappers[key];

					if (t == null)
						_differentTypeMappers[key] = t = GetValueMapper(sourceType, destType);

					mappers[i] = t;
				}
			}

			return mappers;
		}

		#endregion

		#region Base Mapping

		[CLSCompliant(false)]
		protected static int[] GetIndex(IMapDataSource source, IMapDataDestination dest)
		{
			int   count = source.Count;
			int[] index = new int[count];

			for (int i = 0; i < count; i++)
				index[i] = dest.GetOrdinal(source.GetName(i));

			return index;
		}

		[CLSCompliant(false), Obsolete]
		protected static void MapInternal(
			IMapDataSource      source, object sourceObject,
			IMapDataDestination dest,   object destObject,
			int[]               index)
		{
			for (int i = 0; i < index.Length; i++)
			{
				int n = index[i];

				if (n >= 0)
					dest.SetValue(destObject, n, source.GetValue(sourceObject, i));
			}
		}

		[CLSCompliant(false)]
		protected static void MapInternal(
			IMapDataSource      source, object sourceObject,
			IMapDataDestination dest,   object destObject,
			int[]               index,
			IValueMapper[]      mappers)
		{
			for (int i = 0; i < index.Length; i++)
			{
				int n = index[i];

				if (n >= 0)
					mappers[i].Map(source, sourceObject, i, dest, destObject, n);
			}
		}

		[CLSCompliant(false)]
		protected virtual void MapInternal(
			InitContext         initContext,
			IMapDataSource      source, object sourceObject, 
			IMapDataDestination dest,   object destObject,
			params object[]     parameters)
		{
			ISupportMapping smSource = sourceObject as ISupportMapping;
			ISupportMapping smDest   = destObject   as ISupportMapping;

			if (smSource != null)
			{
				if (initContext == null)
				{
					initContext = new InitContext();

					initContext.MappingSchema = this;
					initContext.DataSource    = source;
					initContext.SourceObject  = sourceObject;
					initContext.ObjectMapper  = dest as ObjectMapper;
					initContext.Parameters    = parameters;
				}

				initContext.IsSource = true;
				smSource.BeginMapping(initContext);
				initContext.IsSource = false;

				if (initContext.StopMapping)
					return;
			}

			if (smDest != null)
			{
				if (initContext == null)
				{
					initContext = new InitContext();

					initContext.MappingSchema = this;
					initContext.DataSource    = source;
					initContext.SourceObject  = sourceObject;
					initContext.ObjectMapper  = dest as ObjectMapper;
					initContext.Parameters    = parameters;
				}

				smDest.BeginMapping(initContext);

				if (initContext.StopMapping)
					return;

				if (dest != initContext.ObjectMapper && initContext.ObjectMapper != null)
					dest = initContext.ObjectMapper;
			}

			//MapInternal(source, sourceObject, dest, destObject, GetIndex(source, dest));

			int[]          index   = GetIndex       (source, dest);
			IValueMapper[] mappers = GetValueMappers(source, dest, index);

			MapInternal(source, sourceObject, dest, destObject, index, mappers);

			if (smDest != null)
				smDest.EndMapping(initContext);

			if (smSource != null)
			{
				initContext.IsSource = true;
				smSource.EndMapping(initContext);
				initContext.IsSource = false;
			}
		}

		protected virtual object MapInternal(InitContext initContext)
		{
			object dest = initContext.ObjectMapper.CreateInstance(initContext);

			if (initContext.StopMapping == false)
			{
				MapInternal(initContext,
					initContext.DataSource, initContext.SourceObject,
					initContext.ObjectMapper, dest,
					initContext.Parameters);
			}

			return dest;
		}

		[CLSCompliant(false)]
		public void MapSourceToDestination(
			IMapDataSource      source, object sourceObject, 
			IMapDataDestination dest,   object destObject,
			params object[]     parameters)
		{
			MapInternal(null, source, sourceObject, dest, destObject, parameters);
		}

		public void MapSourceToDestination(object sourceObject, object destObject, params object[] parameters)
		{
			IMapDataSource      source = GetDataSource     (sourceObject);
			IMapDataDestination dest   = GetDataDestination(destObject);

			MapInternal(null, source, sourceObject, dest, destObject, parameters);
		}

		private static ObjectMapper _nullMapper = new ObjectMapper();

		[CLSCompliant(false)]
		public virtual void MapSourceListToDestinationList(
			IMapDataSourceList      dataSourceList,
			IMapDataDestinationList dataDestinationList,
			params object[]         parameters)
		{
			if (dataSourceList      == null) throw new ArgumentNullException("dataSourceList");
			if (dataDestinationList == null) throw new ArgumentNullException("dataDestinationList");

			InitContext ctx = new InitContext();

			ctx.MappingSchema = this;
			ctx.Parameters    = parameters;

			dataSourceList.     InitMapping(ctx); if (ctx.StopMapping) return;
			dataDestinationList.InitMapping(ctx); if (ctx.StopMapping) return;

			int[]               index   = null;
			IValueMapper[]      mappers = null;
			ObjectMapper        current = _nullMapper;
			IMapDataDestination dest    = dataDestinationList.GetDataDestination(ctx);
			ObjectMapper        om      = dest as ObjectMapper;

			while (dataSourceList.SetNextDataSource(ctx))
			{
				ctx.ObjectMapper = om;

				object destObject = dataDestinationList.GetNextObject(ctx);

				if (ctx.StopMapping) continue;

				ISupportMapping smSource = ctx.SourceObject as ISupportMapping;
				ISupportMapping smDest   = destObject       as ISupportMapping;

				if (smSource != null)
				{
					ctx.IsSource = true;
					smSource.BeginMapping(ctx);
					ctx.IsSource = false;

					if (ctx.StopMapping)
						continue;
				}

				if (smDest != null)
				{
					smDest.BeginMapping(ctx);

					if (ctx.StopMapping)
						continue;
				}

				IMapDataDestination currentDest = current != null ? current : dest;

				if (current != ctx.ObjectMapper)
				{
					current     = ctx.ObjectMapper;
					currentDest = current != null ? current : dest;
					index       = GetIndex(ctx.DataSource, currentDest);
					mappers     = GetValueMappers(ctx.DataSource, currentDest, index);
				}

				MapInternal(
					ctx.DataSource,
					ctx.SourceObject,
					currentDest,
					destObject,
					index,
					mappers);

				if (smDest != null)
					smDest.EndMapping(ctx);

				if (smSource != null)
				{
					ctx.IsSource = true;
					smSource.EndMapping(ctx);
					ctx.IsSource = false;
				}
			}

			dataDestinationList.EndMapping(ctx);
			dataSourceList.     EndMapping(ctx);
		}

		#endregion

		#region ValueToEnum, EnumToValue

		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		public object MapValueToEnum(object value, Type type)
		{
			if (value == null)
				return GetNullValue(type);

			MapValue[] mapValues = GetMapValues(type);

			if (mapValues != null)
			{
				IComparable comp = (IComparable)value;

				foreach (MapValue mv in mapValues)
				foreach (object mapValue in mv.MapValues)
				{
					try
					{
						if (comp.CompareTo(mapValue) == 0)
							return mv.OrigValue;
					}
					catch
					{
					}
				}

				// Default value.
				//
				object defaultValue = GetDefaultValue(type);

				if (defaultValue != null)
					return defaultValue;
			}

			return value;
		}

		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		public object MapEnumToValue(object value)
		{
			if (value == null)
				return null;

			Type type = value.GetType();

			object nullValue = GetNullValue(type);

			if (nullValue != null)
			{
				IComparable comp = (IComparable)value;

				try
				{
					if (comp.CompareTo(nullValue) == 0)
						return null;
				}
				catch
				{
				}
			}

			MapValue[] mapValues = GetMapValues(type);

			if (mapValues != null)
			{
				IComparable comp = (IComparable)value;

				foreach (MapValue mv in mapValues)
				{
					try
					{
						if (comp.CompareTo(mv.OrigValue) == 0)
							return mv.MapValues[0];
					}
					catch
					{
					}
				}
			}

			return value;
		}

#if FW2
		public T MapValueToEnum<T>(object value)
		{
			return (T)MapValueToEnum(value, typeof(T));
		}
#endif

		#endregion

		#region Object

		#region MapObjectToObject

		public object MapObjectToObject(object sourceObject, object destObject, params object[] parameters)
		{
			if (sourceObject == null) throw new ArgumentNullException("sourceObject");
			if (destObject   == null) throw new ArgumentNullException("destObject");

			MapInternal(
				null,
				GetObjectMapper(sourceObject.GetType()), sourceObject,
				GetObjectMapper(destObject.  GetType()), destObject,
				parameters);

			return destObject;
		}

		public object MapObjectToObject(object sourceObject, Type destObjectType, params object[] parameters)
		{
			if (sourceObject == null) throw new ArgumentNullException("sourceObject");

			InitContext ctx = new InitContext();

			ctx.MappingSchema = this;
			ctx.DataSource    = GetObjectMapper(sourceObject.GetType());
			ctx.SourceObject  = sourceObject;
			ctx.ObjectMapper  = GetObjectMapper(destObjectType);
			ctx.Parameters    = parameters;

			return MapInternal(ctx);
		}

#if FW2
		public T MapObjectToObject<T>(object sourceObject, params object[] parameters)
		{
			return (T)MapObjectToObject(sourceObject, typeof(T), parameters);
		}
#endif

		#endregion

		#region MapObjectToDataRow

		public DataRow MapObjectToDataRow(object sourceObject, DataRow destRow)
		{
			if (sourceObject == null) throw new ArgumentNullException("sourceObject");

			MapInternal(
				null,
				GetObjectMapper    (sourceObject.GetType()), sourceObject,
				CreateDataRowMapper(destRow, DataRowVersion.Default), destRow,
				null);

			return destRow;
		}

		public DataRow MapObjectToDataRow(object sourceObject, DataTable destTable)
		{
			if (destTable    == null) throw new ArgumentNullException("destTable");
			if (sourceObject == null) throw new ArgumentNullException("sourceObject");

			DataRow destRow = destTable.NewRow();

			destTable.Rows.Add(destRow);

			MapInternal(
				null,
				GetObjectMapper    (sourceObject.GetType()), sourceObject,
				CreateDataRowMapper(destRow, DataRowVersion.Default), destRow,
				null);

			return destRow;
		}

		#endregion

		#region MapObjectToDictionary

		public IDictionary MapObjectToDictionary(object sourceObject, IDictionary destDictionary)
		{
			if (sourceObject == null) throw new ArgumentNullException("sourceObject");

			MapInternal(
				null,
				GetObjectMapper       (sourceObject.GetType()), sourceObject,
				CreateDictionaryMapper(destDictionary),         destDictionary,
				null);

			return destDictionary;
		}

		public Hashtable MapObjectToDictionary(object sourceObject)
		{
			if (sourceObject == null) throw new ArgumentNullException("sourceObject");

			ObjectMapper om = GetObjectMapper(sourceObject.GetType());

			Hashtable destDictionary = new Hashtable(om.Count);

			MapInternal(
				null,
				om, sourceObject,
				CreateDictionaryMapper(destDictionary), destDictionary,
				null);

			return destDictionary;
		}

		#endregion

		#endregion

		#region DataRow

		#region MapDataRowToObject

		public object MapDataRowToObject(DataRow dataRow, object destObject, params object[] parameters)
		{
			if (destObject == null) throw new ArgumentNullException("destObject");

			MapInternal(
				null,
				CreateDataRowMapper(dataRow, DataRowVersion.Default), dataRow,
				GetObjectMapper(destObject.  GetType()), destObject,
				parameters);

			return destObject;
		}

		public object MapDataRowToObject(
			DataRow dataRow, DataRowVersion version, object destObject, params object[] parameters)
		{
			if (destObject == null) throw new ArgumentNullException("destObject");

			MapInternal(
				null,
				CreateDataRowMapper(dataRow, version), dataRow,
				GetObjectMapper(destObject.  GetType()), destObject,
				parameters);

			return destObject;
		}

		public object MapDataRowToObject(DataRow dataRow, Type destObjectType, params object[] parameters)
		{
			InitContext ctx = new InitContext();

			ctx.MappingSchema = this;
			ctx.DataSource    = CreateDataRowMapper(dataRow, DataRowVersion.Default);
			ctx.SourceObject  = dataRow;
			ctx.ObjectMapper  = GetObjectMapper(destObjectType);
			ctx.Parameters    = parameters;

			return MapInternal(ctx);
		}

		public object MapDataRowToObject(
			DataRow dataRow, DataRowVersion version, Type destObjectType, params object[] parameters)
		{
			InitContext ctx = new InitContext();

			ctx.MappingSchema = this;
			ctx.DataSource    = CreateDataRowMapper(dataRow, version);
			ctx.SourceObject  = dataRow;
			ctx.ObjectMapper  = GetObjectMapper(destObjectType);
			ctx.Parameters    = parameters;

			return MapInternal(ctx);
		}

#if FW2
		public T MapDataRowToObject<T>(DataRow dataRow, params object[] parameters)
		{
			return (T)MapDataRowToObject(dataRow, typeof(T), parameters);
		}

		public T MapDataRowToObject<T>(DataRow dataRow, DataRowVersion version, params object[] parameters)
		{
			return (T)MapDataRowToObject(dataRow, version, typeof(T), parameters);
		}
#endif

		#endregion

		#region MapDataRowToDataRow

		public DataRow MapDataRowToDataRow(DataRow sourceRow, DataRow destRow)
		{
			MapInternal(
				null,
				CreateDataRowMapper(sourceRow, DataRowVersion.Default), sourceRow,
				CreateDataRowMapper(destRow,   DataRowVersion.Default), destRow,
				null);

			return destRow;
		}

		public DataRow MapDataRowToDataRow(DataRow sourceRow, DataRowVersion version, DataRow destRow)
		{
			MapInternal(
				null,
				CreateDataRowMapper(sourceRow, version), sourceRow,
				CreateDataRowMapper(destRow, DataRowVersion.Default), destRow,
				null);

			return destRow;
		}

		public DataRow MapDataRowToDataRow(DataRow sourceRow, DataTable destTable)
		{
			if (destTable == null) throw new ArgumentNullException("destTable");

			DataRow destRow = destTable.NewRow();

			destTable.Rows.Add(destRow);

			MapInternal(
				null,
				CreateDataRowMapper(sourceRow, DataRowVersion.Default), sourceRow,
				CreateDataRowMapper(destRow,   DataRowVersion.Default), destRow,
				null);

			return destRow;
		}

		public DataRow MapDataRowToDataRow(DataRow sourceRow, DataRowVersion version, DataTable destTable)
		{
			if (destTable == null) throw new ArgumentNullException("destTable");

			DataRow destRow = destTable.NewRow();

			destTable.Rows.Add(destRow);

			MapInternal(
				null,
				CreateDataRowMapper(sourceRow, version), sourceRow,
				CreateDataRowMapper(destRow, DataRowVersion.Default), destRow,
				null);

			return destRow;
		}

		#endregion

		#region MapDataRowToDictionary

		public IDictionary MapDataRowToDictionary(DataRow sourceRow, IDictionary destDictionary)
		{
			MapInternal(
				null,
				CreateDataRowMapper   (sourceRow, DataRowVersion.Default), sourceRow,
				CreateDictionaryMapper(destDictionary),                    destDictionary,
				null);

			return destDictionary;
		}

		public Hashtable MapDataRowToDictionary(DataRow sourceRow)
		{
			if (sourceRow == null) throw new ArgumentNullException("sourceRow");

			Hashtable destDictionary = new Hashtable(sourceRow.Table.Columns.Count);

			MapInternal(
				null,
				CreateDataRowMapper   (sourceRow, DataRowVersion.Default), sourceRow,
				CreateDictionaryMapper(destDictionary),                    destDictionary,
				null);

			return destDictionary;
		}

		public IDictionary MapDataRowToDictionary(DataRow sourceRow, DataRowVersion version, IDictionary destDictionary)
		{
			MapInternal(
				null,
				CreateDataRowMapper   (sourceRow, version), sourceRow,
				CreateDictionaryMapper(destDictionary),     destDictionary,
				null);

			return destDictionary;
		}

		public Hashtable MapDataRowToDictionary(DataRow sourceRow, DataRowVersion version)
		{
			if (sourceRow == null) throw new ArgumentNullException("sourceRow");

			Hashtable destDictionary = new Hashtable(sourceRow.Table.Columns.Count);

			MapInternal(
				null,
				CreateDataRowMapper   (sourceRow, version), sourceRow,
				CreateDictionaryMapper(destDictionary),     destDictionary,
				null);

			return destDictionary;
		}

		#endregion

		#endregion

		#region DataReader

		#region MapDataReaderToObject

		public object MapDataReaderToObject(IDataReader dataReader, object destObject, params object[] parameters)
		{
			if (destObject == null) throw new ArgumentNullException("destObject");

			MapInternal(
				null,
				CreateDataReaderMapper(dataReader), dataReader,
				GetObjectMapper(destObject. GetType()), destObject,
				parameters);

			return destObject;
		}

		public object MapDataReaderToObject(IDataReader dataReader, Type destObjectType, params object[] parameters)
		{
			InitContext ctx = new InitContext();

			ctx.MappingSchema = this;
			ctx.DataSource    = CreateDataReaderMapper(dataReader);
			ctx.SourceObject  = dataReader;
			ctx.ObjectMapper  = GetObjectMapper(destObjectType);
			ctx.Parameters    = parameters;

			return MapInternal(ctx);
		}

#if FW2
		public T MapDataReaderToObject<T>(IDataReader dataReader, params object[] parameters)
		{
			return (T)MapDataReaderToObject(dataReader, typeof(T), parameters);
		}
#endif

		#endregion

		#region MapDataReaderToDataRow

		public DataRow MapDataReaderToDataRow(IDataReader dataReader, DataRow destRow)
		{
			MapInternal(
				null,
				CreateDataReaderMapper(dataReader), dataReader,
				CreateDataRowMapper(destRow, DataRowVersion.Default), destRow,
				null);

			return destRow;
		}

		public DataRow MapDataReaderToDataRow(IDataReader dataReader, DataTable destTable)
		{
			if (destTable == null) throw new ArgumentNullException("destTable");

			DataRow destRow = destTable.NewRow();

			destTable.Rows.Add(destRow);

			MapInternal(
				null,
				CreateDataReaderMapper(dataReader), dataReader,
				CreateDataRowMapper(destRow, DataRowVersion.Default), destRow,
				null);

			return destRow;
		}

		#endregion

		#region MapDataReaderToDictionary

		public IDictionary MapDataReaderToDictionary(IDataReader dataReader, IDictionary destDictionary)
		{
			MapInternal(
				null,
				CreateDataReaderMapper(dataReader),     dataReader,
				CreateDictionaryMapper(destDictionary), destDictionary,
				null);

			return destDictionary;
		}

		public Hashtable MapDataReaderToDictionary(IDataReader dataReader)
		{
			if (dataReader == null) throw new ArgumentNullException("dataReader");

			Hashtable destDictionary = new Hashtable(dataReader.FieldCount);

			MapInternal(
				null,
				CreateDataReaderMapper(dataReader),     dataReader,
				CreateDictionaryMapper(destDictionary), destDictionary,
				null);

			return destDictionary;
		}

		#endregion

		#endregion

		#region Dictionary

		#region MapDictionaryToObject

		public object MapDictionaryToObject(
			IDictionary sourceDictionary, object destObject, params object[] parameters)
		{
			if (destObject == null) throw new ArgumentNullException("destObject");

			MapInternal(
				null,
				CreateDictionaryMapper(sourceDictionary),       sourceDictionary,
				GetObjectMapper       (destObject.  GetType()), destObject,
				parameters);

			return destObject;
		}

		public object MapDictionaryToObject(
			IDictionary sourceDictionary, Type destObjectType, params object[] parameters)
		{
			InitContext ctx = new InitContext();

			ctx.MappingSchema = this;
			ctx.DataSource    = CreateDictionaryMapper(sourceDictionary);
			ctx.SourceObject  = sourceDictionary;
			ctx.ObjectMapper  = GetObjectMapper(destObjectType);
			ctx.Parameters    = parameters;

			return MapInternal(ctx);
		}

#if FW2
		public T MapDictionaryToObject<T>(IDictionary sourceDictionary, params object[] parameters)
		{
			return (T)MapDictionaryToObject(sourceDictionary, typeof(T), parameters);
		}
#endif

		#endregion

		#region MapDictionaryToDataRow

		public DataRow MapDictionaryToDataRow(IDictionary sourceDictionary, DataRow destRow)
		{
			MapInternal(
				null,
				CreateDictionaryMapper(sourceDictionary),                sourceDictionary,
				CreateDataRowMapper   (destRow, DataRowVersion.Default), destRow,
				null);

			return destRow;
		}

		public DataRow MapDictionaryToDataRow(IDictionary sourceDictionary, DataTable destTable)
		{
			if (destTable == null) throw new ArgumentNullException("destTable");

			DataRow destRow = destTable.NewRow();

			destTable.Rows.Add(destRow);

			MapInternal(
				null,
				CreateDictionaryMapper(sourceDictionary),                sourceDictionary,
				CreateDataRowMapper   (destRow, DataRowVersion.Default), destRow,
				null);

			return destRow;
		}

		#endregion

		#endregion

		#region List

		#region MapListToList

		public IList MapListToList(
			ICollection     sourceList,
			IList           destList,
			Type            destObjectType,
			params object[] parameters)
		{
			if (sourceList == null) throw new ArgumentNullException("sourceList");

			MapSourceListToDestinationList(
				CreateEnumeratorMapper(sourceList.GetEnumerator()),
				CreateObjectListMapper(destList, GetObjectMapper(destObjectType)),
				parameters);

			return destList;
		}

		public ArrayList MapListToList(ICollection sourceList, Type destObjectType, params object[] parameters)
		{
			if (sourceList == null) throw new ArgumentNullException("sourceList");

			ArrayList destList = new ArrayList();

			MapSourceListToDestinationList(
				CreateEnumeratorMapper(sourceList.GetEnumerator()),
				CreateObjectListMapper(destList, GetObjectMapper(destObjectType)),
				parameters);

			return destList;
		}

#if FW2
		public List<T> MapListToList<T>(ICollection sourceList, List<T> destList, params object[] parameters)
		{
			MapSourceListToDestinationList(
				CreateEnumeratorMapper(sourceList.GetEnumerator()),
				CreateObjectListMapper(destList, GetObjectMapper(typeof(T))),
				parameters);

			return destList;
		}

		public List<T> MapListToList<T>(ICollection sourceList, params object[] parameters)
		{
			List<T> destList = new List<T>();

			MapSourceListToDestinationList(
				CreateEnumeratorMapper(sourceList.GetEnumerator()),
				CreateObjectListMapper(destList, GetObjectMapper(typeof(T))),
				parameters);

			return destList;
		}
#endif

		#endregion

		#region MapListToDataTable

		public DataTable MapListToDataTable(ICollection sourceList, DataTable destTable)
		{
			if (sourceList == null) throw new ArgumentNullException("sourceList");

			MapSourceListToDestinationList(
				CreateEnumeratorMapper(sourceList.GetEnumerator()),
				CreateDataTableMapper (destTable, DataRowVersion.Default),
				null);

			return destTable;
		}

		[SuppressMessage("Microsoft.Globalization", "CA1306:SetLocaleForDataTypes")]
		public DataTable MapListToDataTable(ICollection sourceList)
		{
			if (sourceList == null) throw new ArgumentNullException("sourceList");

			DataTable destTable = new DataTable();

			MapSourceListToDestinationList(
				CreateEnumeratorMapper(sourceList.GetEnumerator()),
				CreateDataTableMapper (destTable, DataRowVersion.Default),
				null);

			return destTable;
		}

		#endregion

		#region MapListToDictionary

		public IDictionary MapListToDictionary(
			ICollection          sourceList,
			IDictionary          destDictionary,
			NameOrIndexParameter keyField,
			Type                 destObjectType,
			params object[]      parameters)
		{
			if (sourceList == null) throw new ArgumentNullException("sourceList");

			MapSourceListToDestinationList(
				CreateEnumeratorMapper    (sourceList.GetEnumerator()),
				CreateDictionaryListMapper(destDictionary, keyField, GetObjectMapper(destObjectType)),
				parameters);

			return destDictionary;
		}

		public Hashtable MapListToDictionary(
			ICollection          sourceList,
			NameOrIndexParameter keyField,
			Type                 destObjectType,
			params object[]      parameters)
		{
			if (sourceList == null) throw new ArgumentNullException("sourceList");

			Hashtable destDictionary = new Hashtable();

			MapSourceListToDestinationList(
				CreateEnumeratorMapper    (sourceList.GetEnumerator()),
				CreateDictionaryListMapper(destDictionary, keyField, GetObjectMapper(destObjectType)),
				parameters);

			return destDictionary;
		}

#if FW2
		public IDictionary<K,T> MapListToDictionary<K,T>(
			ICollection          sourceList,
			IDictionary<K,T>     destDictionary,
			NameOrIndexParameter keyField,
			params object[]      parameters)
		{
			MapSourceListToDestinationList(
				CreateEnumeratorMapper         (sourceList.GetEnumerator()),
				CreateDictionaryListMapper<K,T>(destDictionary, keyField, GetObjectMapper(typeof(T))),
				parameters);

			return destDictionary;
		}

		public Dictionary<K,T> MapListToDictionary<K,T>(
			ICollection          sourceList,
			NameOrIndexParameter keyField,
			params object[]      parameters)
		{
			Dictionary<K, T> destDictionary = new Dictionary<K,T>();

			MapSourceListToDestinationList(
				CreateEnumeratorMapper          (sourceList.GetEnumerator()),
				CreateDictionaryListMapper<K,T>(destDictionary, keyField, GetObjectMapper(typeof(T))),
				parameters);

			return destDictionary;
		}
#endif

		#endregion

		#region MapListToDictionaryIndex

		public IDictionary MapListToDictionary(
			ICollection     sourceList,
			IDictionary     destDictionary,
			MapIndex        index,
			Type            destObjectType,
			params object[] parameters)
		{
			if (sourceList == null) throw new ArgumentNullException("sourceList");

			MapSourceListToDestinationList(
				CreateEnumeratorMapper    (sourceList.GetEnumerator()),
				CreateDictionaryListMapper(destDictionary, index, GetObjectMapper(destObjectType)),
				parameters);

			return destDictionary;
		}

		public Hashtable MapListToDictionary(
			ICollection     sourceList,
			MapIndex        index,
			Type            destObjectType,
			params object[] parameters)
		{
			if (sourceList == null) throw new ArgumentNullException("sourceList");

			Hashtable destDictionary = new Hashtable();

			MapSourceListToDestinationList(
				CreateEnumeratorMapper    (sourceList.GetEnumerator()),
				CreateDictionaryListMapper(destDictionary, index, GetObjectMapper(destObjectType)),
				parameters);

			return destDictionary;
		}

#if FW2
		public IDictionary<CompoundValue,T> MapListToDictionary<T>(
			ICollection                  sourceList,
			IDictionary<CompoundValue,T> destDictionary,
			MapIndex                     index,
			params object[]              parameters)
		{
			MapSourceListToDestinationList(
				CreateEnumeratorMapper       (sourceList.GetEnumerator()),
				CreateDictionaryListMapper<T>(destDictionary, index, GetObjectMapper(typeof(T))),
				parameters);

			return destDictionary;
		}

		public Dictionary<CompoundValue,T> MapListToDictionary<T>(
			ICollection     sourceList,
			MapIndex        index,
			params object[] parameters)
		{
			Dictionary<CompoundValue, T> destDictionary = new Dictionary<CompoundValue,T>();

			MapSourceListToDestinationList(
				CreateEnumeratorMapper       (sourceList.GetEnumerator()),
				CreateDictionaryListMapper<T>(destDictionary, index, GetObjectMapper(typeof(T))),
				parameters);

			return destDictionary;
		}
#endif

		#endregion

		#endregion

		#region Table

		#region MapDataTableToDataTable

		public DataTable MapDataTableToDataTable(DataTable sourceTable, DataTable destTable)
		{
			MapSourceListToDestinationList(
				CreateDataTableMapper(sourceTable, DataRowVersion.Default),
				CreateDataTableMapper(destTable,   DataRowVersion.Default),
				null);

			return destTable;
		}

		public DataTable MapDataTableToDataTable(DataTable sourceTable, DataRowVersion version, DataTable destTable)
		{
			MapSourceListToDestinationList(
				CreateDataTableMapper(sourceTable, version),
				CreateDataTableMapper(destTable,   DataRowVersion.Default),
				null);

			return destTable;
		}

		public DataTable MapDataTableToDataTable(DataTable sourceTable)
		{
			if (sourceTable == null) throw new ArgumentNullException("sourceTable");

			DataTable destTable = sourceTable.Clone();

			MapSourceListToDestinationList(
				CreateDataTableMapper(sourceTable, DataRowVersion.Default),
				CreateDataTableMapper(destTable,   DataRowVersion.Default),
				null);

			return destTable;
		}

		public DataTable MapDataTableToDataTable(DataTable sourceTable, DataRowVersion version)
		{
			if (sourceTable == null) throw new ArgumentNullException("sourceTable");

			DataTable destTable = sourceTable.Clone();

			MapSourceListToDestinationList(
				CreateDataTableMapper(sourceTable, version),
				CreateDataTableMapper(destTable,   DataRowVersion.Default),
				null);

			return destTable;
		}

		#endregion

		#region MapDataTableToList

		public IList MapDataTableToList(
			DataTable sourceTable, IList list, Type destObjectType, params object[] parameters)
		{
			MapSourceListToDestinationList(
				CreateDataTableMapper (sourceTable, DataRowVersion.Default),
				CreateObjectListMapper(list, GetObjectMapper(destObjectType)),
				parameters);

			return list;
		}

		public IList MapDataTableToList(
			DataTable       sourceTable,
			DataRowVersion  version,
			IList           list,
			Type            destObjectType,
			params object[] parameters)
		{
			MapSourceListToDestinationList(
				CreateDataTableMapper (sourceTable, version),
				CreateObjectListMapper(list, GetObjectMapper(destObjectType)),
				parameters);

			return list;
		}

		public ArrayList MapDataTableToList(DataTable sourceTable, Type destObjectType, params object[] parameters)
		{
			ArrayList list = new ArrayList();

			MapSourceListToDestinationList(
				CreateDataTableMapper (sourceTable, DataRowVersion.Default),
				CreateObjectListMapper(list, GetObjectMapper(destObjectType)),
				parameters);

			return list;
		}

		public ArrayList MapDataTableToList(
			DataTable sourceTable, DataRowVersion version, Type destObjectType, params object[] parameters)
		{
			ArrayList list = new ArrayList();

			MapSourceListToDestinationList(
				CreateDataTableMapper (sourceTable, version),
				CreateObjectListMapper(list, GetObjectMapper(destObjectType)),
				parameters);

			return list;
		}

#if FW2
		public List<T> MapDataTableToList<T>(DataTable sourceTable, List<T> list, params object[] parameters)
		{
			MapSourceListToDestinationList(
				CreateDataTableMapper (sourceTable, DataRowVersion.Default),
				CreateObjectListMapper(list, GetObjectMapper(typeof(T))),
				parameters);

			return list;
		}

		public List<T> MapDataTableToList<T>(
			DataTable       sourceTable,
			DataRowVersion  version,
			List<T>         list,
			params object[] parameters)
		{
			MapSourceListToDestinationList(
				CreateDataTableMapper (sourceTable, version),
				CreateObjectListMapper(list, GetObjectMapper(typeof(T))),
				parameters);

			return list;
		}

		public List<T> MapDataTableToList<T>(DataTable sourceTable, params object[] parameters)
		{
			List<T> list = new List<T>();

			MapSourceListToDestinationList(
				CreateDataTableMapper (sourceTable, DataRowVersion.Default),
				CreateObjectListMapper(list, GetObjectMapper(typeof(T))),
				parameters);

			return list;
		}

		public List<T> MapDataTableToList<T>(DataTable sourceTable, DataRowVersion version, params object[] parameters)
		{
			List<T> list = new List<T>();

			MapSourceListToDestinationList(
				CreateDataTableMapper (sourceTable, version),
				CreateObjectListMapper(list, GetObjectMapper(typeof(T))),
				parameters);

			return list;
		}
#endif

		#endregion

		#region MapDataTableToDictionary

		public IDictionary MapDataTableToDictionary(
			DataTable            sourceTable,
			IDictionary          destDictionary,
			NameOrIndexParameter keyField,
			Type                 destObjectType,
			params object[]      parameters)
		{
			MapSourceListToDestinationList(
				CreateDataTableMapper     (sourceTable,    DataRowVersion.Default),
				CreateDictionaryListMapper(destDictionary, keyField, GetObjectMapper(destObjectType)),
				parameters);

			return destDictionary;
		}

		public Hashtable MapDataTableToDictionary(
			DataTable            sourceTable,
			NameOrIndexParameter keyField,
			Type                destObjectType,
			params object[]     parameters)
		{
			Hashtable destDictionary = new Hashtable();

			MapSourceListToDestinationList(
				CreateDataTableMapper     (sourceTable,    DataRowVersion.Default),
				CreateDictionaryListMapper(destDictionary, keyField, GetObjectMapper(destObjectType)),
				parameters);

			return destDictionary;
		}

#if FW2
		public IDictionary<K,T> MapDataTableToDictionary<K,T>(
			DataTable            sourceTable,
			IDictionary<K,T>     destDictionary,
			NameOrIndexParameter keyField,
			params object[]      parameters)
		{
			MapSourceListToDestinationList(
				CreateDataTableMapper          (sourceTable,    DataRowVersion.Default),
				CreateDictionaryListMapper<K,T>(destDictionary, keyField, GetObjectMapper(typeof(T))),
				parameters);

			return destDictionary;
		}

		public Dictionary<K,T> MapDataTableToDictionary<K,T>(
			DataTable            sourceTable,
			NameOrIndexParameter keyField,
			params object[]      parameters)
		{
			Dictionary<K, T> destDictionary = new Dictionary<K,T>();

			MapSourceListToDestinationList(
				CreateDataTableMapper          (sourceTable,    DataRowVersion.Default),
				CreateDictionaryListMapper<K,T>(destDictionary, keyField, GetObjectMapper(typeof(T))),
				parameters);

			return destDictionary;
		}
#endif

		#endregion

		#region MapDataTableToDictionary (Index)

		public IDictionary MapDataTableToDictionary(
			DataTable       sourceTable,
			IDictionary     destDictionary,
			MapIndex        index,
			Type            destObjectType,
			params object[] parameters)
		{
			MapSourceListToDestinationList(
				CreateDataTableMapper     (sourceTable,    DataRowVersion.Default),
				CreateDictionaryListMapper(destDictionary, index, GetObjectMapper(destObjectType)),
				parameters);

			return destDictionary;
		}

		public Hashtable MapDataTableToDictionary(
			DataTable       sourceTable,
			MapIndex        index,
			Type            destObjectType,
			params object[] parameters)
		{
			Hashtable destDictionary = new Hashtable();

			MapSourceListToDestinationList(
				CreateDataTableMapper     (sourceTable,    DataRowVersion.Default),
				CreateDictionaryListMapper(destDictionary, index, GetObjectMapper(destObjectType)),
				parameters);

			return destDictionary;
		}

#if FW2
		public IDictionary<CompoundValue,T> MapDataTableToDictionary<T>(
			DataTable                    sourceTable,
			IDictionary<CompoundValue,T> destDictionary,
			MapIndex                     index,
			params object[]              parameters)
		{
			MapSourceListToDestinationList(
				CreateDataTableMapper        (sourceTable,    DataRowVersion.Default),
				CreateDictionaryListMapper<T>(destDictionary, index, GetObjectMapper(typeof(T))),
				parameters);

			return destDictionary;
		}

		public Dictionary<CompoundValue,T> MapDataTableToDictionary<T>(
			DataTable       sourceTable,
			MapIndex        index,
			params object[] parameters)
		{
			Dictionary<CompoundValue,T> destDictionary = new Dictionary<CompoundValue,T>();

			MapSourceListToDestinationList(
				CreateDataTableMapper        (sourceTable,    DataRowVersion.Default),
				CreateDictionaryListMapper<T>(destDictionary, index, GetObjectMapper(typeof(T))),
				parameters);

			return destDictionary;
		}
#endif

		#endregion

		#endregion

		#region DataReader

		#region MapDataReaderToList

		public IList MapDataReaderToList(
			IDataReader     reader,
			IList           list,
			Type            destObjectType,
			params object[] parameters)
		{
			MapSourceListToDestinationList(
				CreateDataReaderListMapper(reader),
				CreateObjectListMapper    (list, GetObjectMapper(destObjectType)),
				parameters);

			return list;
		}

		public ArrayList MapDataReaderToList(IDataReader reader, Type destObjectType, params object[] parameters)
		{
			ArrayList list = new ArrayList();

			MapSourceListToDestinationList(
				CreateDataReaderListMapper(reader),
				CreateObjectListMapper    (list, GetObjectMapper(destObjectType)),
				parameters);

			return list;
		}

#if FW2
		public IList<T> MapDataReaderToList<T>(IDataReader reader, IList<T> list, params object[] parameters)
		{
			MapSourceListToDestinationList(
				CreateDataReaderListMapper(reader),
				CreateObjectListMapper    ((IList)list, GetObjectMapper(typeof(T))),
				parameters);

			return list;
		}

		public List<T> MapDataReaderToList<T>(IDataReader reader, params object[] parameters)
		{
			List<T> list = new List<T>();

			MapSourceListToDestinationList(
				CreateDataReaderListMapper(reader),
				CreateObjectListMapper    (list, GetObjectMapper(typeof(T))),
				parameters);

			return list;
		}
#endif

		#endregion

		#region MapDataReaderToScalarList

		public IList MapDataReaderToScalarList(
			IDataReader          reader,
			NameOrIndexParameter nameOrIndex,
			IList                list,
			Type                 type)
		{
			MapSourceListToDestinationList(
				CreateDataReaderListMapper(reader, nameOrIndex),
				CreateScalarListListMapper(list,   type),
				null);

			return list;
		}

		public ArrayList MapDataReaderToScalarList(
			IDataReader          reader,
			NameOrIndexParameter nameOrIndex,
			Type                 type)
		{
			ArrayList list = new ArrayList();

			MapSourceListToDestinationList(
				CreateDataReaderListMapper(reader, nameOrIndex),
				CreateScalarListListMapper(list,   type),
				null);

			return list;
		}

#if FW2
		public IList<T> MapDataReaderToScalarList<T>(
			IDataReader          reader,
			NameOrIndexParameter nameOrIndex,
			IList<T>             list)
		{
			MapSourceListToDestinationList(
				CreateDataReaderListMapper(reader, nameOrIndex),
				CreateScalarListListMapper(list),
				null);

			return list;
		}

		public List<T> MapDataReaderToScalarList<T>(
			IDataReader          reader,
			NameOrIndexParameter nameOrIndex)
		{
			List<T> list = new List<T>();

			MapSourceListToDestinationList(
				CreateDataReaderListMapper(reader, nameOrIndex),
				CreateScalarListListMapper(list),
				null);

			return list;
		}
#endif

		#endregion

		#region MapDataReaderToDataTable

		public DataTable MapDataReaderToDataTable(IDataReader reader, DataTable destTable)
		{
			MapSourceListToDestinationList(
				CreateDataReaderListMapper(reader),
				CreateDataTableMapper     (destTable, DataRowVersion.Default),
				null);

			return destTable;
		}

		[SuppressMessage("Microsoft.Globalization", "CA1306:SetLocaleForDataTypes")]
		public DataTable MapDataReaderToDataTable(IDataReader reader)
		{
			DataTable destTable = new DataTable();

			MapSourceListToDestinationList(
				CreateDataReaderListMapper(reader),
				CreateDataTableMapper     (destTable, DataRowVersion.Default),
				null);

			return destTable;
		}

		#endregion

		#region MapDataReaderToDictionary

		public IDictionary MapDataReaderToDictionary(
			IDataReader          reader,
			IDictionary          destDictionary,
			NameOrIndexParameter keyField,
			Type                 destObjectType,
			params object[]      parameters)
		{
			MapSourceListToDestinationList(
				CreateDataReaderListMapper(reader),
				CreateDictionaryListMapper(destDictionary, keyField, GetObjectMapper(destObjectType)),
				parameters);

			return destDictionary;
		}

		public Hashtable MapDataReaderToDictionary(
			IDataReader          reader,
			NameOrIndexParameter keyField,
			Type                 destObjectType,
			params object[]      parameters)
		{
			Hashtable dest = new Hashtable();

			MapSourceListToDestinationList(
				CreateDataReaderListMapper(reader),
				CreateDictionaryListMapper(dest, keyField, GetObjectMapper(destObjectType)),
				parameters);

			return dest;
		}

#if FW2
		public IDictionary<K,T> MapDataReaderToDictionary<K,T>(
			IDataReader          reader,
			IDictionary<K,T>     destDictionary,
			NameOrIndexParameter keyField,
			params object[]      parameters)
		{
			MapSourceListToDestinationList(
				CreateDataReaderListMapper     (reader),
				CreateDictionaryListMapper<K,T>(destDictionary, keyField, GetObjectMapper(typeof(T))),
				parameters);

			return destDictionary;
		}

		public Dictionary<K,T> MapDataReaderToDictionary<K,T>(
			IDataReader          reader,
			NameOrIndexParameter keyField,
			params object[]      parameters)
		{
			Dictionary<K, T> dest = new Dictionary<K,T>();

			MapSourceListToDestinationList(
				CreateDataReaderListMapper     (reader),
				CreateDictionaryListMapper<K,T>(dest, keyField, GetObjectMapper(typeof(T))),
				parameters);

			return dest;
		}
#endif

		#endregion

		#region MapDataReaderToDictionary (Index)

		public IDictionary MapDataReaderToDictionary(
			IDataReader     reader,
			IDictionary     destDictionary,
			MapIndex        index,
			Type            destObjectType,
			params object[] parameters)
		{
			MapSourceListToDestinationList(
				CreateDataReaderListMapper(reader),
				CreateDictionaryListMapper(destDictionary, index, GetObjectMapper(destObjectType)),
				parameters);

			return destDictionary;
		}

		public Hashtable MapDataReaderToDictionary(
			IDataReader     reader,
			MapIndex        index,
			Type            destObjectType,
			params object[] parameters)
		{
			Hashtable destDictionary = new Hashtable();

			MapSourceListToDestinationList(
				CreateDataReaderListMapper(reader),
				CreateDictionaryListMapper(destDictionary, index, GetObjectMapper(destObjectType)),
				parameters);

			return destDictionary;
		}

#if FW2
		public IDictionary<CompoundValue,T> MapDataReaderToDictionary<T>(
			IDataReader                  reader,
			IDictionary<CompoundValue,T> destDictionary,
			MapIndex                     index,
			params object[]              parameters)
		{
			MapSourceListToDestinationList(
				CreateDataReaderListMapper(reader),
				CreateDictionaryListMapper(destDictionary, index, GetObjectMapper(typeof(T))),
				parameters);

			return destDictionary;
		}

		public Dictionary<CompoundValue,T> MapDataReaderToDictionary<T>(
			IDataReader     reader,
			MapIndex        index,
			params object[] parameters)
		{
			Dictionary<CompoundValue,T> destDictionary = new Dictionary<CompoundValue,T>();

			MapSourceListToDestinationList(
				CreateDataReaderListMapper   (reader),
				CreateDictionaryListMapper<T>(destDictionary, index, GetObjectMapper(typeof(T))),
				parameters);

			return destDictionary;
		}
#endif

		#endregion

		#endregion

		#region Dictionary

		#region MapDictionaryToList

		public IList MapDictionaryToList(
			IDictionary     sourceDictionary,
			IList           destList,
			Type            destObjectType,
			params object[] parameters)
		{
			if (sourceDictionary == null) throw new ArgumentNullException("sourceDictionary");

			MapSourceListToDestinationList(
				CreateEnumeratorMapper(sourceDictionary.Values.GetEnumerator()),
				CreateObjectListMapper(destList, GetObjectMapper(destObjectType)),
				parameters);

			return destList;
		}

		public ArrayList MapDictionaryToList(
			IDictionary     sourceDictionary,
			Type            destObjectType,
			params object[] parameters)
		{
			if (sourceDictionary == null) throw new ArgumentNullException("sourceDictionary");

			ArrayList destList = new ArrayList();

			MapSourceListToDestinationList(
				CreateEnumeratorMapper(sourceDictionary.Values.GetEnumerator()),
				CreateObjectListMapper(destList, GetObjectMapper(destObjectType)),
				parameters);

			return destList;
		}

#if FW2
		public List<T> MapDictionaryToList<T>(
			IDictionary     sourceDictionary,
			List<T>         destList,
			params object[] parameters)
		{
			if (sourceDictionary == null) throw new ArgumentNullException("sourceDictionary");

			MapSourceListToDestinationList(
				CreateEnumeratorMapper(sourceDictionary.Values.GetEnumerator()),
				CreateObjectListMapper(destList, GetObjectMapper(typeof(T))),
				parameters);

			return destList;
		}

		public List<T> MapDictionaryToList<T>(
			IDictionary     sourceDictionary,
			params object[] parameters)
		{
			if (sourceDictionary == null) throw new ArgumentNullException("sourceDictionary");

			List<T> destList = new List<T>();

			MapSourceListToDestinationList(
				CreateEnumeratorMapper(sourceDictionary.Values.GetEnumerator()),
				CreateObjectListMapper(destList, GetObjectMapper(typeof(T))),
				parameters);

			return destList;
		}
#endif

		#endregion

		#region MapDictionaryToDataTable

		public DataTable MapDictionaryToDataTable(IDictionary sourceDictionary, DataTable destTable)
		{
			if (sourceDictionary == null) throw new ArgumentNullException("sourceDictionary");

			MapSourceListToDestinationList(
				CreateEnumeratorMapper(sourceDictionary.Values.GetEnumerator()),
				CreateDataTableMapper (destTable, DataRowVersion.Default),
				null);

			return destTable;
		}

		[SuppressMessage("Microsoft.Globalization", "CA1306:SetLocaleForDataTypes")]
		public DataTable MapDictionaryToDataTable(IDictionary sourceDictionary)
		{
			if (sourceDictionary == null) throw new ArgumentNullException("sourceDictionary");

			DataTable destTable = new DataTable();

			MapSourceListToDestinationList(
				CreateEnumeratorMapper(sourceDictionary.Values.GetEnumerator()),
				CreateDataTableMapper (destTable, DataRowVersion.Default),
				null);

			return destTable;
		}

		#endregion

		#region MapDictionaryToDictionary

		public IDictionary MapDictionaryToDictionary(
			IDictionary          sourceDictionary,
			IDictionary          destDictionary,
			NameOrIndexParameter keyField,
			Type                 destObjectType,
			params object[]      parameters)
		{
			if (sourceDictionary == null) throw new ArgumentNullException("sourceDictionary");

			MapSourceListToDestinationList(
				CreateEnumeratorMapper    (sourceDictionary.Values.GetEnumerator()),
				CreateDictionaryListMapper(destDictionary, keyField, GetObjectMapper(destObjectType)),
				parameters);

			return destDictionary;
		}

		public Hashtable MapDictionaryToDictionary(
			IDictionary          sourceDictionary,
			NameOrIndexParameter keyField,
			Type                 destObjectType,
			params object[]      parameters)
		{
			if (sourceDictionary == null) throw new ArgumentNullException("sourceDictionary");

			Hashtable dest = new Hashtable();

			MapSourceListToDestinationList(
				CreateEnumeratorMapper    (sourceDictionary.Values.GetEnumerator()),
				CreateDictionaryListMapper(dest, keyField, GetObjectMapper(destObjectType)),
				parameters);

			return dest;
		}

#if FW2
		public IDictionary<K,T> MapDictionaryToDictionary<K,T>(
			IDictionary          sourceDictionary,
			IDictionary<K,T>     destDictionary,
			NameOrIndexParameter keyField,
			params object[]      parameters)
		{
			if (sourceDictionary == null) throw new ArgumentNullException("sourceDictionary");

			MapSourceListToDestinationList(
				CreateEnumeratorMapper         (sourceDictionary.Values.GetEnumerator()),
				CreateDictionaryListMapper<K,T>(destDictionary, keyField, GetObjectMapper(typeof(T))),
				parameters);

			return destDictionary;
		}

		public Dictionary<K,T> MapDictionaryToDictionary<K,T>(
			IDictionary          sourceDictionary,
			NameOrIndexParameter keyField,
			params object[]      parameters)
		{
			if (sourceDictionary == null) throw new ArgumentNullException("sourceDictionary");

			Dictionary<K, T> dest = new Dictionary<K,T>();

			MapSourceListToDestinationList(
				CreateEnumeratorMapper         (sourceDictionary.Values.GetEnumerator()),
				CreateDictionaryListMapper<K,T>(dest, keyField, GetObjectMapper(typeof(T))),
				parameters);

			return dest;
		}
#endif

		#endregion

		#region MapDictionaryToDictionary (Index)

		public IDictionary MapDictionaryToDictionary(
			IDictionary     sourceDictionary,
			IDictionary     destDictionary,
			MapIndex        index,
			Type            destObjectType,
			params object[] parameters)
		{
			if (sourceDictionary == null) throw new ArgumentNullException("sourceDictionary");

			MapSourceListToDestinationList(
				CreateEnumeratorMapper    (sourceDictionary.Values.GetEnumerator()),
				CreateDictionaryListMapper(destDictionary, index, GetObjectMapper(destObjectType)),
				parameters);

			return destDictionary;
		}

		public Hashtable MapDictionaryToDictionary(
			IDictionary     sourceDictionary,
			MapIndex        index,
			Type            destObjectType,
			params object[] parameters)
		{
			if (sourceDictionary == null) throw new ArgumentNullException("sourceDictionary");

			Hashtable destDictionary = new Hashtable();

			MapSourceListToDestinationList(
				CreateEnumeratorMapper    (sourceDictionary.Values.GetEnumerator()),
				CreateDictionaryListMapper(destDictionary, index, GetObjectMapper(destObjectType)),
				parameters);

			return destDictionary;
		}

#if FW2
		public IDictionary<CompoundValue,T> MapDictionaryToDictionary<T>(
			IDictionary                  sourceDictionary,
			IDictionary<CompoundValue,T> destDictionary,
			MapIndex                     index,
			params object[]              parameters)
		{
			if (sourceDictionary == null) throw new ArgumentNullException("sourceDictionary");

			MapSourceListToDestinationList(
				CreateEnumeratorMapper       (sourceDictionary.Values.GetEnumerator()),
				CreateDictionaryListMapper<T>(destDictionary, index, GetObjectMapper(typeof(T))),
				parameters);

			return destDictionary;
		}

		public Dictionary<CompoundValue,T> MapDictionaryToDictionary<T>(
			IDictionary     sourceDictionary,
			MapIndex        index,
			params object[] parameters)
		{
			if (sourceDictionary == null) throw new ArgumentNullException("sourceDictionary");

			Dictionary<CompoundValue,T> destDictionary = new Dictionary<CompoundValue,T>();

			MapSourceListToDestinationList(
				CreateEnumeratorMapper       (sourceDictionary.Values.GetEnumerator()),
				CreateDictionaryListMapper<T>(destDictionary, index, GetObjectMapper(typeof(T))),
				parameters);

			return destDictionary;
		}
#endif

		#endregion

		#endregion

		#region MapToResultSet

		public void MapResultSets(MapResultSet[] resultSets)
		{
			Hashtable   initTable     = null;
			object      lastContainer = null;
			InitContext context       = new InitContext();

			context.MappingSchema = this;

			try
			{
				// Map relations.
				//
				foreach (MapResultSet rs in resultSets)
				{
					if (rs.Relations == null)
						continue;

					ObjectMapper masterMapper = rs.ObjectMapper;

					foreach (MapRelation r in rs.Relations)
					{
						// Create hash.
						//
						if (rs.IndexID != r.MasterIndex.ID)
						{
							rs.Hashtable = new Hashtable();
							rs.IndexID   = r.MasterIndex.ID;

							foreach (object o in rs.List)
							{
								object key = r.MasterIndex.GetValueOrIndex(masterMapper, o);
								rs.Hashtable[key] = o;
							}
						}

						// Map.
						//
						MapResultSet slave = r.SlaveResultSet;

						foreach (object o in slave.List)
						{
							object key    = r.SlaveIndex.GetValueOrIndex(slave.ObjectMapper, o);
							object master = rs.Hashtable[key];

							MemberAccessor ma = masterMapper.TypeAccessor[r.ContainerName];

							if (ma == null)
								throw new MappingException(string.Format("Type '{0}' does not contain field '{1}'.",
									masterMapper.TypeAccessor.OriginalType.Name, r.ContainerName));

							object container = ma.GetValue(master);

							if (container is IList)
							{
								if (lastContainer != container)
								{
									lastContainer = container;

									ISupportMapping sm = container as ISupportMapping;

									if (sm != null)
									{
										if (initTable == null)
											initTable = new Hashtable();

										if (initTable.ContainsKey(container) == false)
										{
											sm.BeginMapping(context);
											initTable[container] = sm;
										}
									}
								}

								((IList)container).Add(o);
							}
							else
							{
								masterMapper.TypeAccessor[r.ContainerName].SetValue(o, master);
							}
						}
					}
				}
			}
			finally
			{
				if (initTable != null)
					foreach (ISupportMapping si in initTable.Values)
						si.EndMapping(context);
			}
		}

		public void MapDataReaderToResultSet(IDataReader reader, MapResultSet[] resultSets)
		{
			if (reader == null) throw new ArgumentNullException("reader");

			foreach (MapResultSet rs in resultSets)
			{
				MapDataReaderToList(reader, rs.List, rs.ObjectType, rs.Parameters);

				if (reader.NextResult() == false)
					break;
			}

			MapResultSets(resultSets);
		}

		public void MapDataSetToResultSet(DataSet dataSet, MapResultSet[] resultSets)
		{
			for (int i = 0; i < resultSets.Length && i < dataSet.Tables.Count; i++)
			{
				MapResultSet rs = resultSets[i];

				MapDataTableToList(dataSet.Tables[i], rs.List, rs.ObjectType, rs.Parameters);
			}

			MapResultSets(resultSets);
		}

		public MapResultSet[] Clone(MapResultSet[] resultSets)
		{
			MapResultSet[] output = new MapResultSet[resultSets.Length];

			for (int i = 0; i < resultSets.Length; i++)
				output[i] = new MapResultSet(resultSets[i]);

			return output;
		}

		private int GetResultCount(MapNextResult[] nextResults)
		{
			int n = nextResults.Length;

			foreach (MapNextResult nr in nextResults)
				n += GetResultCount(nr.NextResults);

			return n;
		}

		private int GetResultSets(
			int current, MapResultSet[] output, MapResultSet master, MapNextResult[] nextResults)
		{
			foreach (MapNextResult nr in nextResults)
			{
				output[current] = new MapResultSet(nr.ObjectType);

				master.AddRelation(output[current], nr.SlaveIndex, nr.MasterIndex, nr.ContainerName);

				current += GetResultSets(current + 1, output, output[current], nr.NextResults);
			}

			return current;
		}

		public MapResultSet[] ConvertToResultSet(Type masterType, params MapNextResult[] nextResults)
		{
			MapResultSet[] output = new MapResultSet[1 + GetResultCount(nextResults)];

			output[0] = new MapResultSet(masterType);

			GetResultSets(1, output, output[0], nextResults);

			return output;
		}

		#endregion
	}
}