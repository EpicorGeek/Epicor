// **************************************************
// Custom code for CRMCallForm
// Created: 7/25/2017 10:30:36 AM
// **************************************************

extern alias Erp_Contracts_BO_Customer;
extern alias Erp_Contracts_BO_Vendor;

using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using Erp.Adapters;
using Erp.UI;
using Ice.Lib;
using Ice.Adapters;
using Ice.Lib.Customization;
using Ice.Lib.ExtendedProps;
using Ice.Lib.Framework;
using Ice.Lib.Searches;
using Ice.UI.FormFunctions;

public class Script
{
	// ** Wizard Insert Location - Do Not Remove 'Begin/End Wizard Added Module Level Variables' Comments! **
	// Begin Wizard Added Module Level Variables **

	private EpiBaseAdapter oTrans_adapter;
	// End Wizard Added Module Level Variables **

	// Add Custom Module Level Variables Here **
	private static EpiCombo callType;
	private static EpiCombo inquiryType;
	public void InitializeCustomCode()
	{
		// ** Wizard Insert Location - Do not delete 'Begin/End Wizard Added Variable Initialization' lines **
		// Begin Wizard Added Variable Initialization

		this.oTrans_adapter = ((EpiBaseAdapter)(this.csm.TransAdaptersHT["oTrans_adapter"]));
		this.oTrans_adapter.BeforeAdapterMethod += new BeforeAdapterMethod(this.oTrans_adapter_BeforeAdapterMethod);
		// End Wizard Added Variable Initialization

		// Begin Wizard Added Custom Method Calls

		// End Wizard Added Custom Method Calls
	}

	public void DestroyCustomCode()
	{
		// ** Wizard Insert Location - Do not delete 'Begin/End Wizard Added Object Disposal' lines **
		// Begin Wizard Added Object Disposal

		this.oTrans_adapter.BeforeAdapterMethod -= new BeforeAdapterMethod(this.oTrans_adapter_BeforeAdapterMethod);
		this.oTrans_adapter = null;
		// End Wizard Added Object Disposal

		// Begin Custom Code Disposal

		// End Custom Code Disposal
	}

	private void oTrans_adapter_BeforeAdapterMethod(object sender, BeforeAdapterMethodArgs args)
	{
		// ** Argument Properties and Uses **
		// ** args.MethodName **
		// ** Add Event Handler Code **

		// ** Use MessageBox to find adapter method name
		// EpiMessageBox.Show(args.MethodName)
		switch (args.MethodName)
		{
			case "Update":
				// DialogResult dialogResult = EpiMessageBox.Show("Cancel Update?", "Cancel", MessageBoxButtons.YesNo);
				// if ((dialogResult == DialogResult.Yes))
				// {
				// 	args.Cancel = true;
				// } else
				// {
				// 	DoSomethingElse();
				// }
				callType = ((EpiCombo)csm.GetNativeControlReference("aa40b07c-65e2-4cec-bc2a-1fc794aee8cb"));
				inquiryType = ((EpiCombo)csm.GetNativeControlReference("25e32f3c-cd04-4c18-8674-3b58b309078f"));
				if(callType.Text.ToLower().Contains("sale") && inquiryType.Text.Trim().Length == 0)
				{
					throw new UIException("Inquiry Type must be filled for new sales.");
				}
				break;
		}

	}
}
