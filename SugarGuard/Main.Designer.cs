namespace SugarGuard
{
	// Token: 0x02000002 RID: 2
	public partial class Main : global::System.Windows.Forms.Form
	{
		// Token: 0x0600000B RID: 11 RVA: 0x00002210 File Offset: 0x00000410
		protected override void Dispose(bool disposing)
		{
			bool flag = disposing && this.components != null;
			if (flag)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002248 File Offset: 0x00000448
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::SugarGuard.Main));
			this.Constants = new global::MaterialSkin.Controls.MaterialCheckBox();
			this.Protect = new global::MaterialSkin.Controls.MaterialRaisedButton();
			this.VM = new global::MaterialSkin.Controls.MaterialCheckBox();
			this.ReferenceProxy = new global::MaterialSkin.Controls.MaterialCheckBox();
			this.ControlFlow = new global::MaterialSkin.Controls.MaterialCheckBox();
			this.InvalidOpcodes = new global::MaterialSkin.Controls.MaterialCheckBox();
			this.Renamer = new global::MaterialSkin.Controls.MaterialCheckBox();
			this.AntiDebug = new global::MaterialSkin.Controls.MaterialCheckBox();
			this.AntiDump = new global::MaterialSkin.Controls.MaterialCheckBox();
			this.FPath = new global::MaterialSkin.Controls.MaterialSingleLineTextField();
			this.WaterMark = new global::MaterialSkin.Controls.MaterialCheckBox();
			this.CallConvertion = new global::MaterialSkin.Controls.MaterialCheckBox();
			this.pictureBox1 = new global::System.Windows.Forms.PictureBox();
			this.Close = new global::System.Windows.Forms.PictureBox();
			this.FakeAttributes = new global::MaterialSkin.Controls.MaterialCheckBox();
			this.ReferenceOverload = new global::MaterialSkin.Controls.MaterialCheckBox();
			this.Mutation = new global::MaterialSkin.Controls.MaterialCheckBox();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.Close).BeginInit();
			base.SuspendLayout();
			this.Constants.AutoSize = true;
			this.Constants.Depth = 0;
			this.Constants.Font = new global::System.Drawing.Font("Roboto", 10f);
			this.Constants.Location = new global::System.Drawing.Point(15, 104);
			this.Constants.Margin = new global::System.Windows.Forms.Padding(0);
			this.Constants.MouseLocation = new global::System.Drawing.Point(-1, -1);
			this.Constants.MouseState = global::MaterialSkin.MouseState.HOVER;
			this.Constants.Name = "Constants";
			this.Constants.Ripple = true;
			this.Constants.Size = new global::System.Drawing.Size(93, 30);
			this.Constants.TabIndex = 5;
			this.Constants.Text = "Constants";
			this.Constants.UseVisualStyleBackColor = true;
			this.Protect.AutoSize = true;
			this.Protect.AutoSizeMode = global::System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Protect.Depth = 0;
			this.Protect.Icon = null;
			this.Protect.Location = new global::System.Drawing.Point(131, 297);
			this.Protect.MouseState = global::MaterialSkin.MouseState.HOVER;
			this.Protect.Name = "Protect";
			this.Protect.Primary = true;
			this.Protect.Size = new global::System.Drawing.Size(81, 36);
			this.Protect.TabIndex = 7;
			this.Protect.Text = "Protect";
			this.Protect.UseVisualStyleBackColor = true;
			this.Protect.Click += new global::System.EventHandler(this.Protect_Click);
			this.VM.AutoSize = true;
			this.VM.Depth = 0;
			this.VM.Font = new global::System.Drawing.Font("Roboto", 10f);
			this.VM.Location = new global::System.Drawing.Point(248, 301);
			this.VM.Margin = new global::System.Windows.Forms.Padding(0);
			this.VM.MouseLocation = new global::System.Drawing.Point(-1, -1);
			this.VM.MouseState = global::MaterialSkin.MouseState.HOVER;
			this.VM.Name = "VM";
			this.VM.Ripple = true;
			this.VM.Size = new global::System.Drawing.Size(51, 30);
			this.VM.TabIndex = 9;
			this.VM.Text = "VM";
			this.VM.UseVisualStyleBackColor = true;
			this.ReferenceProxy.AutoSize = true;
			this.ReferenceProxy.Depth = 0;
			this.ReferenceProxy.Font = new global::System.Drawing.Font("Roboto", 10f);
			this.ReferenceProxy.Location = new global::System.Drawing.Point(15, 164);
			this.ReferenceProxy.Margin = new global::System.Windows.Forms.Padding(0);
			this.ReferenceProxy.MouseLocation = new global::System.Drawing.Point(-1, -1);
			this.ReferenceProxy.MouseState = global::MaterialSkin.MouseState.HOVER;
			this.ReferenceProxy.Name = "ReferenceProxy";
			this.ReferenceProxy.Ripple = true;
			this.ReferenceProxy.Size = new global::System.Drawing.Size(131, 30);
			this.ReferenceProxy.TabIndex = 10;
			this.ReferenceProxy.Text = "Reference Proxy";
			this.ReferenceProxy.UseVisualStyleBackColor = true;
			this.ControlFlow.AutoSize = true;
			this.ControlFlow.Depth = 0;
			this.ControlFlow.Font = new global::System.Drawing.Font("Roboto", 10f);
			this.ControlFlow.Location = new global::System.Drawing.Point(15, 194);
			this.ControlFlow.Margin = new global::System.Windows.Forms.Padding(0);
			this.ControlFlow.MouseLocation = new global::System.Drawing.Point(-1, -1);
			this.ControlFlow.MouseState = global::MaterialSkin.MouseState.HOVER;
			this.ControlFlow.Name = "ControlFlow";
			this.ControlFlow.Ripple = true;
			this.ControlFlow.Size = new global::System.Drawing.Size(108, 30);
			this.ControlFlow.TabIndex = 11;
			this.ControlFlow.Text = "Control Flow";
			this.ControlFlow.UseVisualStyleBackColor = true;
			this.InvalidOpcodes.AutoSize = true;
			this.InvalidOpcodes.Depth = 0;
			this.InvalidOpcodes.Font = new global::System.Drawing.Font("Roboto", 10f);
			this.InvalidOpcodes.Location = new global::System.Drawing.Point(201, 164);
			this.InvalidOpcodes.Margin = new global::System.Windows.Forms.Padding(0);
			this.InvalidOpcodes.MouseLocation = new global::System.Drawing.Point(-1, -1);
			this.InvalidOpcodes.MouseState = global::MaterialSkin.MouseState.HOVER;
			this.InvalidOpcodes.Name = "InvalidOpcodes";
			this.InvalidOpcodes.Ripple = true;
			this.InvalidOpcodes.Size = new global::System.Drawing.Size(128, 30);
			this.InvalidOpcodes.TabIndex = 12;
			this.InvalidOpcodes.Text = "Invalid Opcodes";
			this.InvalidOpcodes.UseVisualStyleBackColor = true;
			this.Renamer.AutoSize = true;
			this.Renamer.Depth = 0;
			this.Renamer.Font = new global::System.Drawing.Font("Roboto", 10f);
			this.Renamer.Location = new global::System.Drawing.Point(201, 254);
			this.Renamer.Margin = new global::System.Windows.Forms.Padding(0);
			this.Renamer.MouseLocation = new global::System.Drawing.Point(-1, -1);
			this.Renamer.MouseState = global::MaterialSkin.MouseState.HOVER;
			this.Renamer.Name = "Renamer";
			this.Renamer.Ripple = true;
			this.Renamer.Size = new global::System.Drawing.Size(85, 30);
			this.Renamer.TabIndex = 13;
			this.Renamer.Text = "Renamer";
			this.Renamer.UseVisualStyleBackColor = true;
			this.AntiDebug.AutoSize = true;
			this.AntiDebug.Depth = 0;
			this.AntiDebug.Font = new global::System.Drawing.Font("Roboto", 10f);
			this.AntiDebug.Location = new global::System.Drawing.Point(201, 104);
			this.AntiDebug.Margin = new global::System.Windows.Forms.Padding(0);
			this.AntiDebug.MouseLocation = new global::System.Drawing.Point(-1, -1);
			this.AntiDebug.MouseState = global::MaterialSkin.MouseState.HOVER;
			this.AntiDebug.Name = "AntiDebug";
			this.AntiDebug.Ripple = true;
			this.AntiDebug.Size = new global::System.Drawing.Size(98, 30);
			this.AntiDebug.TabIndex = 14;
			this.AntiDebug.Text = "Anti Debug";
			this.AntiDebug.UseVisualStyleBackColor = true;
			this.AntiDump.AutoSize = true;
			this.AntiDump.Depth = 0;
			this.AntiDump.Font = new global::System.Drawing.Font("Roboto", 10f);
			this.AntiDump.Location = new global::System.Drawing.Point(201, 134);
			this.AntiDump.Margin = new global::System.Windows.Forms.Padding(0);
			this.AntiDump.MouseLocation = new global::System.Drawing.Point(-1, -1);
			this.AntiDump.MouseState = global::MaterialSkin.MouseState.HOVER;
			this.AntiDump.Name = "AntiDump";
			this.AntiDump.Ripple = true;
			this.AntiDump.Size = new global::System.Drawing.Size(94, 30);
			this.AntiDump.TabIndex = 15;
			this.AntiDump.Text = "Anti Dump";
			this.AntiDump.UseVisualStyleBackColor = true;
			this.FPath.Depth = 0;
			this.FPath.Hint = "";
			this.FPath.Location = new global::System.Drawing.Point(12, 68);
			this.FPath.MaxLength = 32767;
			this.FPath.MouseState = global::MaterialSkin.MouseState.HOVER;
			this.FPath.Name = "FPath";
			this.FPath.PasswordChar = '\0';
			this.FPath.SelectedText = "";
			this.FPath.SelectionLength = 0;
			this.FPath.SelectionStart = 0;
			this.FPath.Size = new global::System.Drawing.Size(318, 23);
			this.FPath.TabIndex = 17;
			this.FPath.TabStop = false;
			this.FPath.Text = "Drag and drop your assembly here.";
			this.FPath.UseSystemPasswordChar = false;
			this.FPath.Click += new global::System.EventHandler(this.FPath_Click);
			this.WaterMark.AutoSize = true;
			this.WaterMark.Depth = 0;
			this.WaterMark.Font = new global::System.Drawing.Font("Roboto", 10f);
			this.WaterMark.Location = new global::System.Drawing.Point(201, 194);
			this.WaterMark.Margin = new global::System.Windows.Forms.Padding(0);
			this.WaterMark.MouseLocation = new global::System.Drawing.Point(-1, -1);
			this.WaterMark.MouseState = global::MaterialSkin.MouseState.HOVER;
			this.WaterMark.Name = "WaterMark";
			this.WaterMark.Ripple = true;
			this.WaterMark.Size = new global::System.Drawing.Size(101, 30);
			this.WaterMark.TabIndex = 20;
			this.WaterMark.Text = "Water Mark";
			this.WaterMark.UseVisualStyleBackColor = true;
			this.CallConvertion.AutoSize = true;
			this.CallConvertion.Depth = 0;
			this.CallConvertion.Font = new global::System.Drawing.Font("Roboto", 10f);
			this.CallConvertion.Location = new global::System.Drawing.Point(15, 254);
			this.CallConvertion.Margin = new global::System.Windows.Forms.Padding(0);
			this.CallConvertion.MouseLocation = new global::System.Drawing.Point(-1, -1);
			this.CallConvertion.MouseState = global::MaterialSkin.MouseState.HOVER;
			this.CallConvertion.Name = "CallConvertion";
			this.CallConvertion.Ripple = true;
			this.CallConvertion.Size = new global::System.Drawing.Size(124, 30);
			this.CallConvertion.TabIndex = 21;
			this.CallConvertion.Text = "Call Convertion";
			this.CallConvertion.UseVisualStyleBackColor = true;
			this.pictureBox1.Image = global::SugarGuard.Properties.Resources.Logo;
			this.pictureBox1.Location = new global::System.Drawing.Point(12, 12);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new global::System.Drawing.Size(126, 44);
			this.pictureBox1.SizeMode = global::System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 22;
			this.pictureBox1.TabStop = false;
			this.Close.Image = global::SugarGuard.Properties.Resources.Delete;
			this.Close.Location = new global::System.Drawing.Point(301, 19);
			this.Close.Name = "Close";
			this.Close.Size = new global::System.Drawing.Size(29, 30);
			this.Close.SizeMode = global::System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.Close.TabIndex = 4;
			this.Close.TabStop = false;
			this.Close.Click += new global::System.EventHandler(this.Close_Click);
			this.FakeAttributes.AutoSize = true;
			this.FakeAttributes.Depth = 0;
			this.FakeAttributes.Font = new global::System.Drawing.Font("Roboto", 10f);
			this.FakeAttributes.Location = new global::System.Drawing.Point(201, 224);
			this.FakeAttributes.Margin = new global::System.Windows.Forms.Padding(0);
			this.FakeAttributes.MouseLocation = new global::System.Drawing.Point(-1, -1);
			this.FakeAttributes.MouseState = global::MaterialSkin.MouseState.HOVER;
			this.FakeAttributes.Name = "FakeAttributes";
			this.FakeAttributes.Ripple = true;
			this.FakeAttributes.Size = new global::System.Drawing.Size(124, 30);
			this.FakeAttributes.TabIndex = 23;
			this.FakeAttributes.Text = "Fake Attributes";
			this.FakeAttributes.UseVisualStyleBackColor = true;
			this.ReferenceOverload.AutoSize = true;
			this.ReferenceOverload.Depth = 0;
			this.ReferenceOverload.Font = new global::System.Drawing.Font("Roboto", 10f);
			this.ReferenceOverload.Location = new global::System.Drawing.Point(15, 224);
			this.ReferenceOverload.Margin = new global::System.Windows.Forms.Padding(0);
			this.ReferenceOverload.MouseLocation = new global::System.Drawing.Point(-1, -1);
			this.ReferenceOverload.MouseState = global::MaterialSkin.MouseState.HOVER;
			this.ReferenceOverload.Name = "ReferenceOverload";
			this.ReferenceOverload.Ripple = true;
			this.ReferenceOverload.Size = new global::System.Drawing.Size(151, 30);
			this.ReferenceOverload.TabIndex = 24;
			this.ReferenceOverload.Text = "Reference Overload";
			this.ReferenceOverload.UseVisualStyleBackColor = true;
			this.Mutation.AutoSize = true;
			this.Mutation.Depth = 0;
			this.Mutation.Font = new global::System.Drawing.Font("Roboto", 10f);
			this.Mutation.Location = new global::System.Drawing.Point(15, 134);
			this.Mutation.Margin = new global::System.Windows.Forms.Padding(0);
			this.Mutation.MouseLocation = new global::System.Drawing.Point(-1, -1);
			this.Mutation.MouseState = global::MaterialSkin.MouseState.HOVER;
			this.Mutation.Name = "Mutation";
			this.Mutation.Ripple = true;
			this.Mutation.Size = new global::System.Drawing.Size(85, 30);
			this.Mutation.TabIndex = 25;
			this.Mutation.Text = "Mutation";
			this.Mutation.UseVisualStyleBackColor = true;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = global::System.Drawing.Color.FromArgb(33, 33, 33);
			base.ClientSize = new global::System.Drawing.Size(342, 350);
			base.Controls.Add(this.Mutation);
			base.Controls.Add(this.ReferenceOverload);
			base.Controls.Add(this.FakeAttributes);
			base.Controls.Add(this.pictureBox1);
			base.Controls.Add(this.CallConvertion);
			base.Controls.Add(this.WaterMark);
			base.Controls.Add(this.FPath);
			base.Controls.Add(this.AntiDump);
			base.Controls.Add(this.AntiDebug);
			base.Controls.Add(this.Renamer);
			base.Controls.Add(this.InvalidOpcodes);
			base.Controls.Add(this.ControlFlow);
			base.Controls.Add(this.ReferenceProxy);
			base.Controls.Add(this.VM);
			base.Controls.Add(this.Protect);
			base.Controls.Add(this.Constants);
			base.Controls.Add(this.Close);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.None;
			base.Icon = (global::System.Drawing.Icon)componentResourceManager.GetObject("$this.Icon");
			base.Name = "Main";
			base.Opacity = 0.9;
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Form1";
			base.DragDrop += new global::System.Windows.Forms.DragEventHandler(this.Main_DragDrop);
			base.DragEnter += new global::System.Windows.Forms.DragEventHandler(this.Main_DragEnter);
			base.MouseDown += new global::System.Windows.Forms.MouseEventHandler(this.Main_MouseDown);
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.Close).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000002 RID: 2
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x04000003 RID: 3
		private new global::System.Windows.Forms.PictureBox Close;

		// Token: 0x04000004 RID: 4
		private global::MaterialSkin.Controls.MaterialCheckBox Constants;

		// Token: 0x04000005 RID: 5
		private global::MaterialSkin.Controls.MaterialRaisedButton Protect;

		// Token: 0x04000006 RID: 6
		private global::MaterialSkin.Controls.MaterialCheckBox VM;

		// Token: 0x04000007 RID: 7
		private global::MaterialSkin.Controls.MaterialCheckBox ReferenceProxy;

		// Token: 0x04000008 RID: 8
		private global::MaterialSkin.Controls.MaterialCheckBox ControlFlow;

		// Token: 0x04000009 RID: 9
		private global::MaterialSkin.Controls.MaterialCheckBox InvalidOpcodes;

		// Token: 0x0400000A RID: 10
		private global::MaterialSkin.Controls.MaterialCheckBox Renamer;

		// Token: 0x0400000B RID: 11
		private global::MaterialSkin.Controls.MaterialCheckBox AntiDebug;

		// Token: 0x0400000C RID: 12
		private global::MaterialSkin.Controls.MaterialCheckBox AntiDump;

		// Token: 0x0400000D RID: 13
		private global::MaterialSkin.Controls.MaterialSingleLineTextField FPath;

		// Token: 0x0400000E RID: 14
		private global::MaterialSkin.Controls.MaterialCheckBox WaterMark;

		// Token: 0x0400000F RID: 15
		private global::MaterialSkin.Controls.MaterialCheckBox CallConvertion;

		// Token: 0x04000010 RID: 16
		private global::System.Windows.Forms.PictureBox pictureBox1;

		// Token: 0x04000011 RID: 17
		private global::MaterialSkin.Controls.MaterialCheckBox FakeAttributes;

		// Token: 0x04000012 RID: 18
		private global::MaterialSkin.Controls.MaterialCheckBox ReferenceOverload;

		// Token: 0x04000013 RID: 19
		private global::MaterialSkin.Controls.MaterialCheckBox Mutation;
	}
}
