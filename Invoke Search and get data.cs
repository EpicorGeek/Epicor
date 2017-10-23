
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