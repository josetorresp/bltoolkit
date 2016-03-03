using System;

namespace BLToolkit
{
	/// <summary>
	/// Global library constants.
	/// </summary>
	public static partial class BLToolkitConstants
	{
		/// <summary>
		/// Major component of version.
		/// </summary>
		public const string MajorVersion = "4";

		/// <summary>
		/// Minor component of version.
		/// </summary>
		public const string MinorVersion = "3";

		/// <summary>
		/// Build component of version.
		/// </summary>
		public const string Build = "4";

		/// <summary>
		/// Full version string.
		/// </summary>
		public const string FullVersionString = MajorVersion + "." + MinorVersion + "." + Build + "." + Revision;

		/// <summary>
		/// Full BLT version.
		/// </summary>
		public static readonly Version FullVersion = new Version(FullVersionString);

		public const string ProductName        = "BLToolkit";
		public const string ProductDescription = "Business Logic Toolkit for .NET";
		public const string Copyright          = "\xA9 2002-2016 www.bltoolkit.net";
	}
}
