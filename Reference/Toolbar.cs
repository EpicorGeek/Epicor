this.baseToolbarsManager.Tools["EpiAddNewNewPartRev"].SharedProps.Visible = false;


foreach (ToolBase tool in baseToolbarsManager.Tools)
{
    if(tool.Key.ToString() == "<Key>") {
        tool.SharedProps.Visible = false; 
    }
}


/* All Toolsbars (Toolbase) */
try
{
	 string message = "";
	 foreach (UltraToolbar toolbar in baseToolbarsManager.Toolbars)
{
	// add the toolbar name
	message+="Tool Key: "+tool.Key+ "\tTool Type: "+tool.GetType().Name+"\n";
}
MessageBox.Show(message);
} catch (Exception e)
{
	ExceptionBox.Show(e);
}

/* All Tools (Toolbase) */
try
{
	 string message = "";
	 foreach (ToolBase tool in baseToolbarsManager.Tools)
{
	// add the toolbar name
	message+="Tool Key: "+tool.Key+ "\tTool Type: "+tool.GetType().Name+"\n";
}
MessageBox.Show(message);
} catch (Exception e)
{
	ExceptionBox.Show(e);
}