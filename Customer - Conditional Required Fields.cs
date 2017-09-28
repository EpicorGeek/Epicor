// **************************************************
// Custom code for CustomerEntryForm
// Created: 7/25/2017 12:12:42 PM
// **************************************************

extern alias Erp_Contracts_BO_Customer;
extern alias Erp_Contracts_BO_BankAcct;
extern alias Erp_Contracts_BO_ShipTo;

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

	private EpiBaseAdapter oTrans_customerAdapter;
	private static EpiCheckBox activeDistributor;
	private static EpiCombo metroArea;
	private static EpiCombo primaryLineVE;

	private static EpiCombo category;
	private static EpiCombo fleetSize;
	// End Wizard Added Module Level Variables **

	// Add Custom Module Level Variables Here **

	public void InitializeCustomCode()
	{
		// ** Wizard Insert Location - Do not delete 'Begin/End Wizard Added Variable Initialization' lines **
		// Begin Wizard Added Variable Initialization

		this.oTrans_customerAdapter = ((EpiBaseAdapter)(this.csm.TransAdaptersHT["oTrans_customerAdapter"]));
		this.oTrans_customerAdapter.BeforeAdapterMethod += new BeforeAdapterMethod(this.oTrans_customerAdapter_BeforeAdapterMethod);
		// End Wizard Added Variable Initialization

		// Begin Wizard Added Custom Method Calls

		// End Wizard Added Custom Method Calls
	}

	public void DestroyCustomCode()
	{
		// ** Wizard Insert Location - Do not delete 'Begin/End Wizard Added Object Disposal' lines **
		// Begin Wizard Added Object Disposal

		this.oTrans_customerAdapter.BeforeAdapterMethod -= new BeforeAdapterMethod(this.oTrans_customerAdapter_BeforeAdapterMethod);
		this.oTrans_customerAdapter = null;
		// End Wizard Added Object Disposal

		// Begin Custom Code Disposal

		// End Custom Code Disposal
	}

	


	private void oTrans_customerAdapter_BeforeAdapterMethod(object sender, BeforeAdapterMethodArgs args)
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
				activeDistributor = ((EpiCheckBox)csm.GetNativeControlReference("4ff5eda0-9888-4703-a1b1-5185e494758e")); 	
				metroArea = ((EpiCombo)csm.GetNativeControlReference("c0355971-9f0a-4126-905a-7d5eca9fb7c5")); 	
				primaryLineVE = ((EpiCombo)csm.GetNativeControlReference("bc535f05-1878-40e0-8c80-ddd3a6d70a0f"));
				if(activeDistributor.Checked)
				{
					if(metroArea.Text.Length == 0 || primaryLineVE.Text.Length == 0)
						throw new UIException("Metro Area and Primary Line Van Equip must be filled for an Active distributor.");
				}
				
				category = ((EpiCombo)csm.GetNativeControlReference("f76dd0e7-ba1f-4fff-aaec-9cc5e3f15156"));
				fleetSize = ((EpiCombo)csm.GetNativeControlReference("8228301d-6ad4-4b54-8834-5a9a50535bf1"));
				if(category.Text == "Fleet" && fleetSize .Text.Length == 0)
				{
					throw new UIException("Fleet size must be filled for Category: Fleet");
				}

				break;
		}
	
	}
}
