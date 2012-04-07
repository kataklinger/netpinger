
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

namespace NetPinger
{
	public partial class ProgramOptions : Form
	{
		public ProgramOptions()
		{
			InitializeComponent();

			_cbStartWithWindows.Checked = Options.Instance.StartWithWindows;
			_cbShowErrorMessages.Checked = Options.Instance.ShowErrorMessages;
			_cbStartPinging.Checked = Options.Instance.StartPingingOnProgramStart;
			_cbClearTimes.Checked = Options.Instance.ClearTimeStatistics;
		}

		public CheckedListBox SelectedColumns
		{
			get { return _clColumns; }
		}

		public bool StartWithWindows
		{
			get { return _cbStartWithWindows.Checked; }
		}

		public bool ShowErrorMessages
		{
			get { return _cbShowErrorMessages.Checked; }
		}

		public bool StartPingingOnProgramStart
		{
			get { return _cbStartPinging.Checked; }
		}

		public bool ClearTimeStatistics
		{
			get { return _cbClearTimes.Checked; }
		}

	}
}