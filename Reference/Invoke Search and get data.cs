
// Set BO adapter
this.custAdapter = new CustomerAdapter(this.oTrans);
this.custAdapter.BOConnect();

// Prepare single Where clause
    string whereClause = "CustID = \'" + custID + custSuffix.ToString("D3") + "\'";

// Set Hashtable Where clauses
    System.Collections.Hashtable whereClauses = new System.Collections.Hashtable(1);
    whereClauses.Add("Customer", whereClause);

// Invoke Search with Hashtable Where clauses
    SearchOptions searchOptions = SearchOptions.CreateRuntimeSearch(whereClauses, DataSetMode.RowsDataSet);
    this.custAdapter.InvokeSearch(searchOptions);
    
// Get Data Row
    DataRow dataRow = this.adapterName.TableNameData.TableName.Rows[row index];
    dataRow["Column Name"]
    

// Simple Search Function List Loopup
    bool recSelected;
    string whereClause = string.Empty;
    System.Data.DataSet dsCustomerAdapter = Ice.UI.FormFunctions.SearchFunctions.listLookup(this.oTrans, "CustomerAdapter", out recSelected, true, whereClause);
    if (recSelected)
    {
        System.Data.DataRow adapterRow = dsCustomerAdapter.Tables[0].Rows[0];

        // Map Search Fields to Application Fields
        EpiDataView edvCustomer = ((EpiDataView)(this.oTrans.EpiDataViews["Customer"]));
        System.Data.DataRow edvCustomerRow = edvCustomer.CurrentDataRow;
        if ((edvCustomerRow != null))
        {
            edvCustomerRow.BeginEdit();
            edvCustomerRow["ParentCustNum_c"] = adapterRow["CustNum"];
            edvCustomerRow["ParentCustID_c"] = adapterRow["CustID"];
            edvCustomerRow.EndEdit();
            
        }
    }