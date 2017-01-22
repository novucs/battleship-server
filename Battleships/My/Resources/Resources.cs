using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Battleships.My.Resources
{
	[StandardModule, HideModuleName, GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0"), DebuggerNonUserCode, CompilerGenerated]
	internal sealed class Resources
	{
		private static ResourceManager _resourceMan;

	    [EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (ReferenceEquals(_resourceMan, null))
				{
					_resourceMan = new ResourceManager("Battleships.Resources", typeof(Resources).Assembly);
				}
				return _resourceMan;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture { get; set; }
	}
}
