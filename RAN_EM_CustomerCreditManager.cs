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
		
	private EpiBaseAdapter oTrans_adapter;
	// End Wizard Added Module Level Variables **

	// Add Custom Module Level Variables Here **
	private EpiDataView custCredView;
	private EpiDateTimeEditor date;
	public void InitializeCustomCode()
	{
		// ** Wizard Insert Location - Do not delete 'Begin/End Wizard Added Variable Initialization' lines **
		// Begin Wizard Added Variable Initialization

		this.CMCustomer_Column.ColumnChanged += new DataColumnChangeEventHandler(this.CMCustomer_AfterFieldChange);
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

		this.CMCustomer_Column.ColumnChanged -= new DataColumnChangeEventHandler(this.CMCustomer_AfterFieldChange);
		this.oTrans_adapter.BeforeAdapterMethod -= new BeforeAdapterMethod(this.oTrans_adapter_BeforeAdapterMethod);
		this.oTrans_adapter = null;
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
				//args.Row["LastCreditHold_c"] = System.DateTime.Today;
				break;
		}
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

				date = ((EpiDateTimeEditor)csm.GetNativeControlReference("58b174f5-b539-42ab-8e1c-1007c7b85541")); 
				int length = date.Text.Length;
				if(length == 0)
				{
					custCredView = ((EpiDataView)(this.oTrans.EpiDataViews["Customer"]));
					custCredView.dataView[this.custCredView.Row]["LastCreditHold_c"] = DBNull.Value;
				}
				break;
		}

	}
}
