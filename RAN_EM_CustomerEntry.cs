// **************************************************
// Custom code for CustomerEntryForm
// Created: 7/25/2017 12:12:42 PM
// **************************************************
// ECS/NPHAM
// RAN_CustomerEntry_1.2.0  Created: 9/29/2017 
// Project: RAN-17-004
// Add Parent Customer
// **************************************************

extern alias Erp_Contracts_BO_Customer;
extern alias Erp_Contracts_BO_BankAcct;
extern alias Erp_Contracts_BO_ShipTo;
extern alias Erp_Adapters_Customer;

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
	private static EpiCombo countryNum;

	private static EpiCombo category;
	private static EpiCombo fleetSize;
	// End Wizard Added Module Level Variables **

	// Add Custom Module Level Variables Here **
	private CustomerAdapter custAdapter;
	private EpiDataView custDataView;
	private EpiTextBox et_newCustID;

	public void InitializeCustomCode()
	{
		// ** Wizard Insert Location - Do not delete 'Begin/End Wizard Added Variable Initialization' lines **
		// Begin Wizard Added Variable Initialization
			


		
		this.oTrans_customerAdapter = ((EpiBaseAdapter)(this.csm.TransAdaptersHT["oTrans_customerAdapter"]));
		this.custDataView = (EpiDataView)oTrans.EpiDataViews["Customer"];

		this.oTrans_customerAdapter.BeforeAdapterMethod += new BeforeAdapterMethod(this.oTrans_customerAdapter_BeforeAdapterMethod);
		this.Customer_Column.ColumnChanged += new DataColumnChangeEventHandler(this.Customer_AfterFieldChange);
		this.custDataView = ((EpiDataView)(this.oTrans.EpiDataViews["Customer"]));
		this.custDataView.EpiViewNotification += new EpiViewNotification(this.custDataView_EpiViewNotification);
		this.CustCnt_Column.ColumnChanged += new DataColumnChangeEventHandler(this.CustCnt_AfterFieldChange);
		// End Wizard Added Variable Initialization

		// Begin Wizard Added Custom Method Calls
		
		this.btnParentCust.Click += new System.EventHandler(this.btnParentCust_Click);
		this.btnNextCustID.Click += new System.EventHandler(this.btnNextCustID_Click);
		// End Wizard Added Custom Method Calls
	}

	public void DestroyCustomCode()
	{
		// ** Wizard Insert Location - Do not delete 'Begin/End Wizard Added Object Disposal' lines **
		// Begin Wizard Added Object Disposal

		this.oTrans_customerAdapter.BeforeAdapterMethod -= new BeforeAdapterMethod(this.oTrans_customerAdapter_BeforeAdapterMethod);
		this.oTrans_customerAdapter = null;
		this.Customer_Column.ColumnChanged -= new DataColumnChangeEventHandler(this.Customer_AfterFieldChange);
		this.btnParentCust.Click -= new System.EventHandler(this.btnParentCust_Click);
		this.btnNextCustID.Click -= new System.EventHandler(this.btnNextCustID_Click);
		this.custDataView = null;
		this.custDataView.EpiViewNotification -= new EpiViewNotification(this.custDataView_EpiViewNotification);
		this.CustCnt_Column.ColumnChanged -= new DataColumnChangeEventHandler(this.CustCnt_AfterFieldChange);
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
		// EpiMessageBox.Show(args.MethodName);
		string parentcustnum="";
		switch (args.MethodName)
		{
			case "Update":
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

				
				if (custDataView!=null && custDataView.HasRow)
				{
					
					if (custDataView.dataView[custDataView.Row]["ParentCustID_c"].ToString() =="") 
					{
						custDataView.dataView[custDataView.Row]["ParentCustNum_c"] =0;
					}
					else
					{
						
						parentcustnum = GetParentCustNumByCustNum(custDataView.dataView[custDataView.Row]["ParentCustID_c"].ToString());
						
						if (parentcustnum.Length ==0) 
						{
							throw new UIException("Parent Customer ID  doesn't exists.");
						}
						else
						{
							
							custDataView.dataView[custDataView.Row]["ParentCustNum_c"] = parentcustnum;
							
						}
	
					}

				}
				break;
		}
	
	}

	private void Customer_AfterFieldChange(object sender, DataColumnChangeEventArgs args)
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

	private string GetParentCustNumByCustNum(string code)	
	{
		string returnV = "";

		try 
		{

			CustomerAdapter adapterCustomer = new CustomerAdapter(this.oTrans);
			adapterCustomer.BOConnect();
			adapterCustomer.GetByCustID(code,false);

			
			if (adapterCustomer.CustomerData.Customer.Rows.Count == 1) 	
			{	
				DataRow rRow = adapterCustomer.CustomerData.Customer[adapterCustomer.CustomerData.Customer.Rows.Count-1];
				if (rRow!=null )
				{
					returnV = rRow["CustNum"].ToString();
				}
			}	        
			adapterCustomer.Dispose();
		}
		catch (Exception ex)
		{
			//ExceptionBox.Show(ex);
			return "";	
		}

		return returnV;
	}


	private void CustomerEntryForm_Load(object sender, EventArgs args)
	{
		SetVersionNumber(this.oTrans.EpiBaseForm);

	}

    public static void SetVersionNumber(EpiBaseForm frm)
    {
		if(frm.Text.Contains("Version") == true) return;

        string custName = frm.CustomizationName;
        string version = "?.?";

        int pos = custName.LastIndexOf("_");
        if (pos >= 0)
        {
            version = custName.Substring(pos + 1, custName.Length - pos - 1);
        }

        frm.Text = String.Format("{0} - Version {1}", frm.Text, version);
    }

	private void btnParentCust_Click(object sender, System.EventArgs args)
	{
		// ** Place Event Handling Code Here **
		SearchOnCustomerAdapterShowDialog();
	}

	private void SearchOnCustomerAdapterShowDialog()
	{
		// Wizard Generated Search Method
		// You will need to call this method from another method in custom code
		// For example, [Form]_Load or [Button]_Click

		bool recSelected;
		string whereClause = string.Empty;
		System.Data.DataSet dsCustomerAdapter = Ice.UI.FormFunctions.SearchFunctions.listLookup(this.oTrans, "CustomerAdapter", out recSelected, true, whereClause);
		if (recSelected)
		{
			System.Data.DataRow adapterRow = dsCustomerAdapter.Tables[0].Rows[0];

			// Map Search Fields to Application Fields
			EpiDataView custDataView = ((EpiDataView)(this.oTrans.EpiDataViews["Customer"]));
			System.Data.DataRow custDataViewRow = custDataView.CurrentDataRow;
			if ((custDataViewRow != null))
			{
				custDataViewRow.BeginEdit();
				custDataViewRow["ParentCustNum_c"] = adapterRow["CustNum"];
				custDataViewRow["ParentCustID_c"] = adapterRow["CustID"];
				custDataViewRow.EndEdit();
				
			}
		}
	}



	private void btnNextCustID_Click(object sender, System.EventArgs args)
	{
		DialogResult dialogResult = EpiMessageBox.Show("Are you sure you want to get next Cust ID?", "Cancel", MessageBoxButtons.YesNo);

		if ((dialogResult == DialogResult.Yes)) {

			this.custAdapter = new CustomerAdapter(this.oTrans);
			this.custAdapter.BOConnect();
			
			DataRow currentCustomer = this.custDataView.CurrentDataRow;
			
		// Add Prefix
			string custID = formatNewID(currentCustomer["Name"].ToString());
	
		// Set Suffix
			int custSuffix = 0; // 001
			
		// Iterate until suffix exists with prefix
			do {
                custSuffix++;
				try {
					this.custAdapter.GetByCustID(custID + custSuffix.ToString("D3"),false);
				}catch(Exception e) {
					break;
				}
			} while(custSuffix < 1000);
	
		// Set new custID
			string newCustID = custID + custSuffix.ToString("D3");
			//MessageBox.Show("New CustID: " + newCustID);
			
			currentCustomer["CustID"] = newCustID;
			/*et_newCustID = ((EpiTextBox)csm.GetNativeControlReference("3a3fffc1-6dee-4be0-b52e-7fc5f80e7b95")); 
			et_newCustID.Text = newCustID;*/
		}
	}

	/* Format new ID to only contain uppercase letters */
	private static string formatNewID(string name) {

		string prefix = name;
		string cleanPrefix = "";
		foreach(Char i in prefix) {
			if(Char.IsLetter(i) || Char.IsDigit(i)) {
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


	private void custDataView_EpiViewNotification(EpiDataView view, EpiNotifyArgs args)
	{
		// ** Argument Properties and Uses **
		// view.dataView[args.Row]["FieldName"]
		// args.Row, args.Column, args.Sender, args.NotifyType
		// NotifyType.Initialize, NotifyType.AddRow, NotifyType.DeleteRow, NotifyType.InitLastView, NotifyType.InitAndResetTreeNodes
		if ((args.NotifyType == EpiTransaction.NotifyType.AddRow))
		{
			DataRow currentCustomer = this.custDataView.CurrentDataRow;
			if(this.custDataView.CurrentDataRow != null){
				this.btnNextCustID.Enabled = true;
				if(this.custDataView.CurrentDataRow.RowState.ToString() == "Added")
				{
					this.custDataView.CurrentDataRow["CheckDuplicatePO"] = true;
					this.custDataView.CurrentDataRow["WebCustomer"] = true;
					this.custDataView.CurrentDataRow["AllowOTS"] = true;
					this.custDataView.CurrentDataRow["AllowShipTo3"] = true;
					this.custDataView.CurrentDataRow["ECCType"] = "B";
				}
			}
			else{
				this.btnNextCustID.Enabled = false;
			}
			
		}
	}

	private void CustCnt_AfterFieldChange(object sender, DataColumnChangeEventArgs args)
	{
		// ** Argument Properties and Uses **
		// args.Row["FieldName"]
		// args.Column, args.ProposedValue, args.Row
		// Add Event Handler Code
		EpiDataView edvCustomer = ((EpiDataView)(this.oTrans.EpiDataViews["Customer"]));
		System.Data.DataRow edvCustomerRow = edvCustomer.CurrentDataRow;
		string custID = edvCustomerRow["CustID"].ToString();
		
		
		CustomerAdapter custAdapter = new CustomerAdapter(this.oTrans);
		custAdapter.BOConnect();
		custAdapter.GetByCustID(custID,false);
		DataRow row = custAdapter.CustomerData.Customer[0];
		
		int primB = int.Parse(row["PrimBCon"].ToString());
		int primP = int.Parse(row["PrimPCon"].ToString());
		int primS = int.Parse(row["PrimSCon"].ToString());
		
		string leadType = "";
		bool warning = false;

		switch (args.Column.ColumnName)
		{
		    case "PrimaryBilling":
		        if(primB != 0 && args.ProposedValue.ToString() == "True"){
					warning = true;
					leadType = "billing";
				} 
		    	break;
		    case "PrimaryPurchasing":
		        if(primP != 0 && args.ProposedValue.ToString() == "True"){
					warning = true;
					leadType = "purchasing";
				} 
		    	break;
		    case "PrimaryShipping":
		        if(primS != 0 && args.ProposedValue.ToString() == "True"){
					warning = true;
					leadType = "shipping";
				} 
		    	break;
		}
		
		if(warning) 
		{
			DialogResult dialogResult = EpiMessageBox.Show("There exists another contact for " + leadType + "\nContinue?", "Cancel", MessageBoxButtons.YesNo);
			if ((dialogResult == DialogResult.No)) {
			    oTrans.Undo();
			} 
		}
	}
}
