this.baseToolbarsManager.Tools["EpiAddNewNewPartRev"].SharedProps.Visible = false;


foreach (ToolBase tool in baseToolbarsManager.Tools)
{
    if(tool.Key.ToString() == "<Key>") {
        tool.SharedProps.Visible = false; 
    }
}