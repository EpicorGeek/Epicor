// **************************************************
// Custom code for CreditManagerForm
// Created: 7/27/2017 7:28:12 AM
// **************************************************
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

		this.CMCustomer_Column.ColumnChanged += new DataColumnChangeEventHandler(this.CMCustomer_AfterFieldChange);
		// End Wizard Added Variable Initialization

		// Begin Wizard Added Custom Method Calls

		// End Wizard Added Custom Method Calls
	}

	public void DestroyCustomCode()
	{
		// ** Wizard Insert Location - Do not delete 'Begin/End Wizard Added Object Disposal' lines **
		// Begin Wizard Added Object Disposal

		this.CMCustomer_Column.ColumnChanged -= new DataColumnChangeEventHandler(this.CMCustomer_AfterFieldChange);
		// End Wizard Added Object Disposal

		// Begin Custom Code Disposal

		// End Custom Code Disposal
	}



	



	private void CMCustomer_AfterFieldChange(object sender, DataColumnChangeEventArgs args)
	{
		// ** Argument Properties and Uses **
		// args.Row["FieldName"]
		// args.Column, args.ProposedValue, args.Row
		// Add Event Handler Code
		switch (args.Column.ColumnName)
		{
			case "CreditHold":
				args.Row["LastCreditHold_c"] = System.DateTime.Today;
				break;
		}
	}
}
