
/********** BO Adapter  - Invoke Search **********/
// Set BO adapter
this.custAdapter = new CustomerAdapter(this.oTrans);
this.custAdapter.BOConnect();


// Prepare single Where clause
string whereClause = "CustID = \'" + custID + custSuffix.ToString("D3") + "\'";

// Set Hashtable Where clauses
System.Collections.Hashtable whereClauses = new System.Collections.Hashtable(1);
whereClauses.Add("Customer", whereClause);


// Create SearchOptions
SearchOptions searchOptions = SearchOptions.CreateRuntimeSearch(whereClauses, DataSetMode.RowsDataSet);


/****************OPTIONS****************/
// Invoke Search with Hashtable Where clauses
this.custAdapter.InvokeSearch(searchOptions);

    or

// Adapter Method
bool morePages;
DataSet dsUD100 = ud100Adapter.GetRows(searchOptions, out morePages);
/********************************************/


    
/****************OPTIONS****************/
// Get Data Row
    DataRow dataRow = this.adapterName.TableNameData.TableName.Rows[row index];
    dataRow["Column Name"]
   
    or
   
// SELECT Data Row
    DataRow[] copyRows = ud100Adapter.UD100Data.UD100A.Select("ChildKey1 = \'" + txtCopyPart.Text.ToString() + "\' and ChildKey2 = \'" + txtCopyRev.Text.ToString() + "\'");
/********************************************/

                    
                    

                    
                    
                    
/********** BO Adapter  - GetByID Search **********/
// Set BO adapter
this.partRevAdapter = new PartRevSearchAdapter(this.oTrans);
this.partRevAdapter.BOConnect();

// Get Data
this.partRevAdapter.GetByID(txtCopyPart.Text, txtCopyRev.Text, "");

// Get Row
DataRow dataRow = this.partRevAdapter.PartRevSearchData.PartRev.Rows[0];

                    
                    

                    
                    
                
/********** Simple Search Function List Lookup **********/
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