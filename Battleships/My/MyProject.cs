using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.VisualBasic.CompilerServices;

namespace Battleships.My
{
	[StandardModule, HideModuleName, GeneratedCode("MyTemplate", "11.0.0.0")]
	internal sealed class MyProject
	{
		[MyGroupCollection("System.Windows.Forms.Form", "Create__Instance__", "Dispose__Instance__", "My.MyProject.Forms"), EditorBrowsable(EditorBrowsableState.Never)]
		internal sealed class MyForms
		{
			[ThreadStatic]
			private static Hashtable _mFormBeingCreated;

			[EditorBrowsable(EditorBrowsableState.Never)]
			public Form1 MForm1;

			public Form1 Form1
			{
				get
				{
					MForm1 = Create__Instance__(MForm1);
					return MForm1;
				}
				set
				{
					if (value != MForm1)
					{
						if (value != null)
						{
							throw new ArgumentException("Property can only be set to Nothing");
						}
						Dispose__Instance__(ref MForm1);
					}
				}
			}

			[DebuggerHidden]
			private static T Create__Instance__<T>(T instance) where T : Form, new()
			{
				T result;
				if (instance == null || instance.IsDisposed)
				{
					if (_mFormBeingCreated != null)
					{
						if (_mFormBeingCreated.ContainsKey(typeof(T)))
						{
							throw new InvalidOperationException(Utils.GetResourceString("WinForms_RecursiveFormCreate"));
						}
					}
					else
					{
						_mFormBeingCreated = new Hashtable();
					}
					_mFormBeingCreated.Add(typeof(T), null);
					try
					{
					    result = Activator.CreateInstance<T>();
					    return result;
					}
					finally
					{
						_mFormBeingCreated.Remove(typeof(T));
					}
				}
				result = instance;
				return result;
			}

			[DebuggerHidden]
			private static void Dispose__Instance__<T>(ref T instance) where T : Form
			{
				instance.Dispose();
				instance = default(T);
			}

			[EditorBrowsable(EditorBrowsableState.Never), DebuggerHidden]
			public MyForms()
			{
			}

			[EditorBrowsable(EditorBrowsableState.Never)]
			internal new Type GetType()
			{
				return typeof(MyForms);
			}

			[EditorBrowsable(EditorBrowsableState.Never)]
			public override string ToString()
			{
				return base.ToString();
			}
		}

		[MyGroupCollection("System.Web.Services.Protocols.SoapHttpClientProtocol", "Create__Instance__", "Dispose__Instance__", ""), EditorBrowsable(EditorBrowsableState.Never)]
		internal sealed class MyWebServices
		{
			[EditorBrowsable(EditorBrowsableState.Never), DebuggerHidden]
			internal new Type GetType()
			{
				return typeof(MyWebServices);
			}

			[EditorBrowsable(EditorBrowsableState.Never), DebuggerHidden]
			public MyWebServices()
			{
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never), ComVisible(false)]
		internal sealed class ThreadSafeObjectProvider<T> where T : new()
		{
			[CompilerGenerated, ThreadStatic]
			private static T _mThreadStaticValue;

			internal T GetInstance
			{
				[DebuggerHidden]
				get
				{
					if (_mThreadStaticValue == null)
					{
						_mThreadStaticValue = Activator.CreateInstance<T>();
					}
					return _mThreadStaticValue;
				}
			}

			[EditorBrowsable(EditorBrowsableState.Never), DebuggerHidden]
			public ThreadSafeObjectProvider()
			{
			}
		}

		private static readonly ThreadSafeObjectProvider<MyComputer> MComputerObjectProvider = new ThreadSafeObjectProvider<MyComputer>();

		private static readonly ThreadSafeObjectProvider<MyApplication> MAppObjectProvider = new ThreadSafeObjectProvider<MyApplication>();

		private static readonly ThreadSafeObjectProvider<User> MUserObjectProvider = new ThreadSafeObjectProvider<User>();

		private static readonly ThreadSafeObjectProvider<MyForms> MMyFormsObjectProvider = new ThreadSafeObjectProvider<MyForms>();

		private static readonly ThreadSafeObjectProvider<MyWebServices> MMyWebServicesObjectProvider = new ThreadSafeObjectProvider<MyWebServices>();

		[HelpKeyword("My.Computer")]
		internal static MyComputer Computer
		{
			[DebuggerHidden]
			get
			{
				return MComputerObjectProvider.GetInstance;
			}
		}

		[HelpKeyword("My.Application")]
		internal static MyApplication Application
		{
			[DebuggerHidden]
			get
			{
				return MAppObjectProvider.GetInstance;
			}
		}

		[HelpKeyword("My.User")]
		internal static User User
		{
			[DebuggerHidden]
			get
			{
				return MUserObjectProvider.GetInstance;
			}
		}

		[HelpKeyword("My.Forms")]
		internal static MyForms Forms
		{
			[DebuggerHidden]
			get
			{
				return MMyFormsObjectProvider.GetInstance;
			}
		}

		[HelpKeyword("My.WebServices")]
		internal static MyWebServices WebServices
		{
			[DebuggerHidden]
			get
			{
				return MMyWebServicesObjectProvider.GetInstance;
			}
		}
	}
}
