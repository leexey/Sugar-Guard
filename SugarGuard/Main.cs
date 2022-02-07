using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;
using SugarGuard.Properties;
using SugarGuard.Protector;
using SugarGuard.Protector.Enums;

namespace SugarGuard
{
	// Token: 0x02000002 RID: 2
	public partial class Main : Form
	{
		// Token: 0x06000001 RID: 1
		[DllImport("Gdi32.dll")]
		private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

		// Token: 0x06000002 RID: 2
		[DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

		// Token: 0x06000003 RID: 3
		[DllImport("user32.dll")]
		public static extern bool ReleaseCapture();

		// Token: 0x06000004 RID: 4 RVA: 0x00002050 File Offset: 0x00000250
		public Main()
		{
			this.InitializeComponent();
			MaterialSkinManager instance = MaterialSkinManager.Instance;
			instance.Theme = MaterialSkinManager.Themes.DARK;
			instance.ColorScheme = new ColorScheme(Primary.Pink600, Primary.Pink600, Primary.Pink600, Accent.Pink400, TextShade.WHITE);
			base.Region = Region.FromHrgn(Main.CreateRoundRectRgn(0, 0, base.Width, base.Height, 5, 5));
			Control.CheckForIllegalCrossThreadCalls = false;
			this.AllowDrop = true;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020E4 File Offset: 0x000002E4
		private void Main_DragDrop(object sender, DragEventArgs e)
		{
			string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
			foreach (string target in array)
			{
				this.Target = target;
			}
			this.FPath.Text = Path.GetFileName(this.Target);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x0000213C File Offset: 0x0000033C
		private void Main_DragEnter(object sender, DragEventArgs e)
		{
			bool dataPresent = e.Data.GetDataPresent(DataFormats.FileDrop);
			if (dataPresent)
			{
				e.Effect = DragDropEffects.Copy;
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002168 File Offset: 0x00000368
		private void Main_MouseDown(object sender, MouseEventArgs e)
		{
			bool flag = e.Button == MouseButtons.Left;
			if (flag)
			{
				Main.ReleaseCapture();
				Main.SendMessage(base.Handle, 161, 2, 0);
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000021A2 File Offset: 0x000003A2
		private void Close_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000021AB File Offset: 0x000003AB
		private void Protect_Click(object sender, EventArgs e)
		{
			new Task(delegate()
			{
				Sugar sugar = new Sugar(this.Target);
				bool @checked = this.VM.Checked;
				if (@checked)
				{
					sugar.protections.Add(Protections.VM);
				}
				bool checked2 = this.CallConvertion.Checked;
				if (checked2)
				{
					sugar.protections.Add(Protections.CallConvertion);
				}
				bool checked3 = this.AntiDebug.Checked;
				if (checked3)
				{
					sugar.protections.Add(Protections.AntiDebug);
				}
				bool checked4 = this.AntiDump.Checked;
				if (checked4)
				{
					sugar.protections.Add(Protections.AntiDump);
				}
				bool checked5 = this.Constants.Checked;
				if (checked5)
				{
					sugar.protections.Add(Protections.Constants);
					sugar.protections.Add(Protections.PosConstants);
				}
				bool checked6 = this.Mutation.Checked;
				if (checked6)
				{
					sugar.protections.Add(Protections.Mutation);
				}
				bool checked7 = this.ControlFlow.Checked;
				if (checked7)
				{
					sugar.protections.Add(Protections.ControlFlow);
				}
				bool checked8 = this.ReferenceProxy.Checked;
				if (checked8)
				{
					sugar.protections.Add(Protections.ReferenceProxy);
				}
				bool checked9 = this.InvalidOpcodes.Checked;
				if (checked9)
				{
					sugar.protections.Add(Protections.InvalidOpcodes);
				}
				bool checked10 = this.Renamer.Checked;
				if (checked10)
				{
					sugar.protections.Add(Protections.Renamer);
				}
				bool checked11 = this.FakeAttributes.Checked;
				if (checked11)
				{
					sugar.protections.Add(Protections.FakeAttributes);
				}
				bool checked12 = this.WaterMark.Checked;
				if (checked12)
				{
					sugar.protections.Add(Protections.WaterMark);
				}
				sugar.Protect();
				sugar.Save();
				Console.Beep(500, 500);
			}).Start();
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000021C8 File Offset: 0x000003C8
		private void FPath_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			bool flag = openFileDialog.ShowDialog() == DialogResult.OK;
			if (flag)
			{
				this.FPath.Text = openFileDialog.FileName;
				this.Target = this.FPath.Text;
			}
		}

		// Token: 0x04000001 RID: 1
		private string Target = string.Empty;
	}
}
