// **************************************************
// Custom code for PartForm
// Created: 6/29/2017 1:35:59 PM
// **************************************************

extern alias Erp_Contracts_BO_Part;
extern alias Erp_Contracts_BO_PartPlantSearch;
extern alias Erp_Contracts_BO_PO;
extern alias Erp_Contracts_BO_PartOnHandWhse;
extern alias Erp_Contracts_BO_Vendor;
extern alias Erp_Contracts_BO_VendorPPSearch;

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

	// End Wizard Added Module Level Variables **

	// Add Custom Module Level Variables Here **

	public void InitializeCustomCode()
	{
		// ** Wizard Insert Location - Do not delete 'Begin/End Wizard Added Variable Initialization' lines **
		// Begin Wizard Added Variable Initialization

		// End Wizard Added Variable Initialization

		// Begin Wizard Added Custom Method Calls

		this.checkbox2.Click += new System.EventHandler(this.checkbox2_Click);
		// End Wizard Added Custom Method Calls
	}

	public void DestroyCustomCode()
	{
		// ** Wizard Insert Location - Do not delete 'Begin/End Wizard Added Object Disposal' lines **
		// Begin Wizard Added Object Disposal

		this.checkbox2.Click -= new System.EventHandler(this.checkbox2_Click);
		// End Wizard Added Object Disposal

		// Begin Custom Code Disposal

		// End Custom Code Disposal
	}

	private void checkbox2_Click(object sender, System.EventArgs args)
	{
		// ** Place Event Handling Code Here **
		this.checkbox1.Checked = true;
	}
}
