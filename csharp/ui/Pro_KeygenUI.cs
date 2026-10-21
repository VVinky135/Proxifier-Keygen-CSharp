// WinForms entry point. Build:
//   csc -nologo -codepage:65001 -optimize+ -target:winexe -out:Pro_KeygenUI.exe
//       -r:System.Windows.Forms.dll -r:System.Drawing.dll Keygen.cs Pro_KeygenUI.cs
// C# 5 syntax only (csc.exe from .NET Framework 4.x).

using System;
using System.Drawing;
using System.Windows.Forms;

public class ProKeygenForm : Form
{
    RadioButton rbSetup, rbPortable, rbMac;
    TextBox txtKey;

    public ProKeygenForm()
    {
        Text = "Proxifier Keygen";
        Font = new Font("Microsoft YaHei UI", 9F);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        AutoScaleMode = AutoScaleMode.None;
        ClientSize = new Size(400, 240);

        var grp = new GroupBox();
        grp.Text = "版本";
        grp.Location = new Point(12, 10);
        grp.Size = new Size(376, 80);
        Controls.Add(grp);

        rbSetup = new RadioButton();
        rbSetup.Text = "Setup 安装版";
        rbSetup.AutoSize = true;
        rbSetup.Location = new Point(14, 30);
        rbSetup.Checked = true;
        grp.Controls.Add(rbSetup);

        rbPortable = new RadioButton();
        rbPortable.Text = "Portable 便携版";
        rbPortable.AutoSize = true;
        rbPortable.Location = new Point(145, 30);
        grp.Controls.Add(rbPortable);

        rbMac = new RadioButton();
        rbMac.Text = "Mac 版";
        rbMac.AutoSize = true;
        rbMac.Location = new Point(290, 30);
        grp.Controls.Add(rbMac);

        var btn = new Button();
        btn.Text = "生成 Key";
        btn.Location = new Point(12, 102);
        btn.Size = new Size(376, 34);
        btn.Click += OnGenerate;
        Controls.Add(btn);
        AcceptButton = btn;

        txtKey = new TextBox();
        txtKey.Location = new Point(12, 150);
        txtKey.Size = new Size(376, 28);
        txtKey.ReadOnly = true;
        txtKey.TextAlign = HorizontalAlignment.Center;
        txtKey.Font = new Font("Consolas", 12F);
        Controls.Add(txtKey);
    }

    void OnGenerate(object sender, EventArgs e)
    {
        int product = rbPortable.Checked ? 1 : (rbMac.Checked ? 2 : 0);
        txtKey.Text = Keygen.Generate(product);
        txtKey.SelectAll();
    }
}

static class ProKeygenUI
{
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new ProKeygenForm());
    }
}
