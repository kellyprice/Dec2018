private void searchgrid_TextChanged(List<DAField> fields)
        {
            try
            {
                DataTable MyDataTable = ((System.Data.DataTable)dataGridView1.DataSource); 

                DataView dv = MyDataTable.DefaultView;

                _textFilters = ""; //reset filters
                bool first = true; //to handle the " and "
                foreach (DAField f in fields)
                {
                    if (f.Value.Length > 0) //only if there is a value to filter for
                    {
                        if (!first) 
                            _textFilters += " and ";

                        if (f.Value.ToUpper() == "NULL")
                        {
                            _textFilters += f.Field + " is NULL";
                        }
                        else
                        {
                            if (f.dType != "String")
                            {
                                if (f.Value.StartsWith("<") || f.Value.StartsWith(">"))
                                {
                                    string FValue = f.Value.Replace("=", "");
                                    FValue = FValue.Replace(" ", "");
                                    _textFilters += f.Field + " " + FValue[0] + "= '" + FValue.Substring(1) + "'";
                                }
                                else
                                {
                                    string FValue = f.Value.Replace("=", "");
                                    FValue = FValue.Replace(" ", "");
                                    _textFilters += f.Field + " = '" + FValue + "'";
                                }
                            }
                            else if (f.dType == "String")
                            {
                                _textFilters += f.Field + " like '%" + f.Value + "%'";
                            }
                        }
                        first = false;
                    }
                }
                dv.RowFilter = _textFilters;
                this.labelFilter.Text = _textFilters;
                _filters = _textFilters;

                _rowcount = dv.Count;
                lblRowCount.Text = "Row Count: " + _rowcount.ToString();
            }
            catch (Exception Ex)
            {
            }
        }
