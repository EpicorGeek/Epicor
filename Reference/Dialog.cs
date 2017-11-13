DialogResult dialogResult = EpiMessageBox.Show("Cancel Update?", "Cancel", MessageBoxButtons.YesNo);

if ((dialogResult == DialogResult.Yes)) {
    args.Cancel = true;
} 
else {
    // Do something
}