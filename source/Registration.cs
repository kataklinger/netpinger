using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
//using LicencingComponent.Protocol;
using System.Threading;

namespace Coolsoft.NetPinger
{
	//delegate void RegistrationComplitedDelegate(ResponseStatus status);

	public partial class Registration : Form
	{

		#region ProxyAddress

		private string _proxyAddress = string.Empty;

		public string ProxyAddress
		{
			get { return _proxyAddress; }
		}

		#endregion

		#region ProxyPort

		private int _proxyPort = 8080;

		public int ProxyPort
		{
			get { return _proxyPort; }
		}

		#endregion

		#region UseProxy

		private bool _useProxy;

		public bool UseProxy
		{
			get { return _useProxy; }
		}

		#endregion

		public Registration()
		{
			InitializeComponent();

			_grpLicenceFile.Enabled = _rbLicenceFile.Checked = false;
			_grpLicenceKey.Enabled = _rbLicenceKey.Checked = false;

			//OnRegistrationComplited += new RegistrationComplitedDelegate(RegistrationComplited);
		}

		private void _rbLicenceKey_CheckedChanged(object sender, EventArgs e)
		{
			_grpLicenceFile.Enabled = _rbLicenceFile.Checked;
			_grpLicenceKey.Enabled = _rbLicenceKey.Checked;
		}

		private void _rbLicenceFile_CheckedChanged(object sender, EventArgs e)
		{
			_grpLicenceFile.Enabled = _rbLicenceFile.Checked;
			_grpLicenceKey.Enabled = _rbLicenceKey.Checked;
		}

		//LicenceFile _licenceFile = null;

		private void _btnBrowse_Click(object sender, EventArgs e)
		{
			//if (_dlgOpenFile.ShowDialog() == DialogResult.OK)
			//{
			//    _txtLicenceFile.Text = _dlgOpenFile.FileName;
			//    using (StreamReader sr = new StreamReader(File.Open(_dlgOpenFile.FileName, FileMode.Open,
			//         FileAccess.Read, FileShare.Read)))
			//    {
			//        LicenceFile file = new LicenceFile(sr.ReadToEnd());
			//        if (file.GetStatus() != ResponseStatus.RS_OK)
			//        {
			//            MessageBox.Show(this, "Wrong licence file format", "Error",
			//                MessageBoxButtons.OK, MessageBoxIcon.Error);
			//        }
			//        else
			//        {
			//            _licenceFile = file;
			//            _txtLicenceKey.Text = file.GetLicenceKey();
			//        }
			//    }
			//}
		}

		private void _lnkRegistration_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			try
			{
				Process.Start(_lnkRegistration.Text);
			}
			catch
			{
				MessageBox.Show("Cannot start web browser!");
			}
		}

		private void _lnkDownload_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			try
			{
				Process.Start(_lnkDownload.Text);
			}
			catch
			{
				MessageBox.Show("Cannot start web browser!");
			}
		}

		private void _btnProxy_Click(object sender, EventArgs e)
		{
			ProxyOptions dlg = new ProxyOptions();
			dlg.ProxyAddress = _proxyAddress;
			dlg.ProxyPort = _proxyPort;
			dlg.UseProxy = _useProxy;

			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				_proxyAddress = dlg.ProxyAddress;
				_proxyPort = dlg.ProxyPort;
				_useProxy = dlg.UseProxy;
			}
		}

		private void _btnHostID_Click(object sender, EventArgs e)
		{
			//HostID dlg = new HostID();
			//dlg.SetHostID(Program.Licencing.GetHostID());
			//dlg.ShowDialog(this);
		}

		private void _txtLicenceKey_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!((e.KeyChar >= 'a' && e.KeyChar <= 'z') ||
				(e.KeyChar >= 'A' && e.KeyChar <= 'Z') ||
				(e.KeyChar >= '0' && e.KeyChar <= '9') || char.IsControl(e.KeyChar)))
				e.Handled = true;
		}

		private void _btnOK_Click(object sender, EventArgs e)
		{
			//if (_rbLicenceKey.Checked)
			//{
			//    if (_useProxy)
			//    {
			//        Program.Licencing.GetHTTPCilent().SetProxyServer(_proxyAddress);
			//        Program.Licencing.GetHTTPCilent().SetProxyServerPort(_proxyPort);
			//    }
			//    else
			//        Program.Licencing.GetHTTPCilent().SetProxyServer(string.Empty);

			//    ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessRegistration), _txtLicenceKey.Text);
			//    if (_showRegistrationDialog)
			//        _registrationProcessDlg.ShowDialog();
			//}
			//else
			//{
			//    if (_licenceFile != null)
			//    {
			//        ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessLicenceFile), _licenceFile);
			//        if (_showRegistrationDialog)
			//            _registrationProcessDlg.ShowDialog();
			//    }
			//    else
			//        MessageBox.Show(this, "You haven't selected valid licence file.", "Error",
			//            MessageBoxButtons.OK, MessageBoxIcon.Error);
			//}
		}

		RegistrationProcess _registrationProcessDlg = new RegistrationProcess();

		//private event RegistrationComplitedDelegate OnRegistrationComplited;

		private void ProcessRegistration(object licenceKey)
		{
			//ResponseStatus status = Program.Licencing.Register((string)licenceKey, false);

			//if (OnRegistrationComplited != null)
			//    OnRegistrationComplited(status);
		}

		private void ProcessLicenceFile(object licenceFile)
		{
			//ResponseStatus status = Program.Licencing.ImportLicenceFile((LicenceFile)licenceFile);

			//if (OnRegistrationComplited != null)
			//    OnRegistrationComplited(status);
		}

		//bool _showRegistrationDialog = true;

		//private void RegistrationComplited(ResponseStatus status)
		//{
		//    if (InvokeRequired)
		//    {
		//        Invoke(new RegistrationComplitedDelegate(RegistrationComplited), status);
		//        return;
		//    }

		//    _showRegistrationDialog = false;
		//    _registrationProcessDlg.Close();

		//    switch (status)
		//    {
		//        case ResponseStatus.RS_WRONG_LICENCE_KEY:
		//        case ResponseStatus.RS_LICENCE_KEY_TAKEN:
		//            MessageBox.Show(this, "Wrong licence key or it is already taken.", "Wrong licence",
		//                MessageBoxButtons.OK, MessageBoxIcon.Warning);
		//            break;

		//        case ResponseStatus.RS_OK:
		//            MessageBox.Show(this, "Registration complited successfully!", "Registration complited",
		//                MessageBoxButtons.OK, MessageBoxIcon.Information);

		//            DialogResult = DialogResult.OK;
		//            Close();
		//            break;

		//        default:
		//            MessageBox.Show(this, "An unknown error occured. Maybe Internet connection settings are incorrect or licencing server is unavailable.",
		//                "Unknown error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
		//            break;
		//    }
		//}

	}
}