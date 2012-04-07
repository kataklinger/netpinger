
/*
 * 
 * Copyright (c) 2006-2010 Mladen Jankovic. All rights reserved.
 * 
 * contact: kataklinger@gmail[dot]com
 * 
 *	- Redistributions of source code must retain the above copyright notice,
 *	  this list of conditions and the following disclaimer. 
 * 
 *	- Redistributions in binary form must reproduce the above copyright notice,
 *	  this list of conditions and the following disclaimer in the documentation
 *	  and/or other materials provided with the distribution.
 * 
 *  - The names of contributors may not be used to endorse or promote products
 *    derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE
 * LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
 * SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
 * INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
 * CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
 * POSSIBILITY OF SUCH DAMAGE.
 *
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LiveGraph
{
	public partial class AddGraphDlg : Form
	{
		public string GraphName
		{
			get { return _txtGraphName.Text; }
			set { _txtGraphName.Text = value; }
		}

		public long Resolution
		{
			get { return (long)_spnResolution.Value; }
			set { _spnResolution.Value = value / TimeSpan.TicksPerMillisecond; }
		}

		public bool ShowTimeLabels
		{
			get { return _chkShowTime.Checked; }
			set { _chkShowTime.Checked = value; }
		}

		public AddGraphDlg()
		{
			InitializeComponent();

			_spnResolution.Maximum = TimeSpan.TicksPerHour / TimeSpan.TicksPerMillisecond;
			_spnResolution.Value = TimeSpan.TicksPerSecond / TimeSpan.TicksPerMillisecond;
		}

	}
}
