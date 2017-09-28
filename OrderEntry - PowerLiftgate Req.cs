// **************************************************
// Custom code for SalesOrderForm
// Created: 7/26/2017 10:58:03 AM
// **************************************************

extern alias Erp_Contracts_BO_AlternatePart;
extern alias Erp_Contracts_BO_SalesOrder;
extern alias Erp_Contracts_BO_Quote;
extern alias Erp_Contracts_BO_Part;
extern alias Erp_Contracts_BO_Customer;
extern alias Erp_Contracts_BO_RMAProc;
extern alias Erp_Contracts_BO_OrderDtlSearch;
extern alias Erp_Contracts_BO_OrderHist;
extern alias Erp_Contracts_BO_QuoteDtlSearch;
extern alias Erp_Contracts_BO_SerialNumberSearch;
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
	// End Wizard Added Module Level Variables **
	private static EpiCheckBox PLG;
	// Add Custom Module Level Variables Here **
	public void InitializeCustomCode()
	{
		// ** Wizard Insert Location - Do not delete 'Begin/End Wizard Added Variable Initialization' lines **
		// Begin Wizard Added Variable Initialization
		
		this.OrderHed_Column.ColumnChanged += new DataColumnChangeEventHandler(this.OrderHed_AfterFieldChange);
		// End Wizard Added Variable Initialization

		// Begin Wizard Added Custom Method Calls

		// End Wizard Added Custom Method Calls
	}

	public void DestroyCustomCode()
	{
		// ** Wizard Insert Location - Do not delete 'Begin/End Wizard Added Object Disposal' lines **
		// Begin Wizard Added Object Disposal
	
		this.OrderHed_Column.ColumnChanged -= new DataColumnChangeEventHandler(this.OrderHed_AfterFieldChange);
		// End Wizard Added Object Disposal

		// Begin Custom Code Disposal

		// End Custom Code Disposal
	}

	private void OrderHed_AfterFieldChange(object sender, DataColumnChangeEventArgs args)
	{
		// ** Argument Properties and Uses **
		// args.Row["FieldName"]
		// args.Column, args.ProposedValue, args.Row
		// Add Event Handler Code
		PLG = ((EpiCheckBox)csm.GetNativeControlReference("f20cdda6-76b4-46df-8e2e-d0680136bb05")); 	
		switch (args.Column.ColumnName)
		{
			case "CustomerCustID":
				if(PLG.Checked)
					MessageBox.Show("        This customer requires a Power Liftgate.\n      	           Add surcharge.", "Power Liftgate");	
				break;
		}
	}

}
