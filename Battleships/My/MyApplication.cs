using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Microsoft.VisualBasic.ApplicationServices;

namespace Battleships.My
{
    [GeneratedCode("MyTemplate", "11.0.0.0"), EditorBrowsable(EditorBrowsableState.Never)]
    internal class MyApplication : WindowsFormsApplicationBase
    {
        [EditorBrowsable(EditorBrowsableState.Advanced), DebuggerHidden, STAThread]
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        internal static void Main(string[] args)
        {
            Application.SetCompatibleTextRenderingDefault(UseCompatibleTextRendering);
            MyProject.Application.Run(args);
        }

        [DebuggerStepThrough]
        public MyApplication() : base(AuthenticationMode.Windows)
        {
            IsSingleInstance = false;
            EnableVisualStyles = true;
            SaveMySettingsOnExit = true;
            ShutdownStyle = ShutdownMode.AfterMainFormCloses;
        }

        [DebuggerStepThrough]
        protected override void OnCreateMainForm()
        {
            MainForm = MyProject.Forms.Form1;
        }
    }
}
