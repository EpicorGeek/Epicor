// **************************************************
// Custom code for NewCustIDForm
// Created: 10/16/2017 8:03:08 AM
// **************************************************

extern alias Erp_Contracts_BO_Customer;
extern alias Erp_Contracts_BO_BankAcct;
extern alias Erp_Contracts_BO_ShipTo;

using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Text.RegularExpressions;
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
	private CustomerAdapter custAdapter;
	private EpiDataView custDataView;
	private EpiTextBox et_newCustID;
	public void InitializeCustomCode()
	{
		// ** Wizard Insert Location - Do not delete 'Begin/End Wizard Added Variable Initialization' lines **
		// Begin Wizard Added Variable Initialization

		// End Wizard Added Variable Initialization

		// Begin Wizard Added Custom Method Calls

		this.btnNextID.Click += new System.EventHandler(this.btnNextID_Click);
		// End Wizard Added Custom Method Calls
	}

	public void DestroyCustomCode()
	{
		// ** Wizard Insert Location - Do not delete 'Begin/End Wizard Added Object Disposal' lines **
		// Begin Wizard Added Object Disposal

		this.btnNextID.Click -= new System.EventHandler(this.btnNextID_Click);
		// End Wizard Added Object Disposal

		// Begin Custom Code Disposal

		// End Custom Code Disposal
	}

	private void btnNextID_Click(object sender, System.EventArgs args)
	{
		// ** Place Event Handling Code Here **
		this.custAdapter = new CustomerAdapter(this.oTrans);
		this.custAdapter.BOConnect();
		this.custDataView = (EpiDataView)(this.oTrans.EpiDataViews["Customer"]);
		DataRow currentCustomer = this.custDataView.CurrentDataRow;
		
	// Add Prefix
		string custID = formatNewID(currentCustomer["Name"].ToString());

	// Set Suffix
		int custSuffix = 0; // 000
	
	// Iterate until suffix exists with prefix
		do {
			string whereClause = "CustID = \'" + custID + custSuffix.ToString("D3") + "\'";
			System.Collections.Hashtable whereClauses = new System.Collections.Hashtable(1);
			whereClauses.Add("Customer", whereClause);
			
			SearchOptions searchOptions = SearchOptions.CreateRuntimeSearch(whereClauses, DataSetMode.RowsDataSet);
			this.custAdapter.InvokeSearch(searchOptions);
	
			if ((this.custAdapter.CustomerData.Customer.Rows.Count > 0))
			{
				custSuffix++;
			} 
		} while(this.custAdapter.CustomerData.Customer.Rows.Count > 0);

	// Set new custID
		string newCustID = custID + custSuffix.ToString("D3");
		MessageBox.Show("New CustID: " + newCustID);
		
		et_newCustID = ((EpiTextBox)csm.GetNativeControlReference("eb06f6cc-8342-4610-8c12-bdfb93e78f8d")); 
		et_newCustID.Text = newCustID;
	}


	/* Format new ID to only contain uppercase letters */
	private static string formatNewID(string name) {

		string prefix = name;
		string cleanPrefix = "";
		foreach(Char i in prefix) {
			if(Char.IsLetter(i)) {
				cleanPrefix = cleanPrefix + i;
			}
		}

		while( cleanPrefix.Length < 3){
			 cleanPrefix =  cleanPrefix + "0";
		}
		string newPrefix = cleanPrefix.ToUpper().Substring(0,3);

		/*
		if(Regex.IsMatch(newCustID, @"^[a-zA-Z]+$")) {
			return newCustID;
		}
		*/
		return newPrefix;
	}

}
