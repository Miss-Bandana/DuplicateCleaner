using McTools.Xrm.Connection;
using McTools.Xrm.Connection.WinForms;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Description;
using System.Windows.Forms;
using XrmToolBox.Extensibility;





namespace Duplicate_Cleaner
{
    public partial class MyPluginControl : PluginControlBase
    {
        private Dictionary<string, string> fieldAliasMap = new Dictionary<string, string>();

        private Settings mySettings;

        public MyPluginControl()
        {
            InitializeComponent();
        }

        private List<List<Entity>> duplicateSets = new List<List<Entity>>();

        private void MyPluginControl_Load(object sender, EventArgs e)
        {
            ShowInfoNotification("This is a notification that can lead to XrmToolBox repository", new Uri("https://github.com/MscrmTools/XrmToolBox"));

            // Loads or creates the settings for the plugin
            if (!SettingsManager.Instance.TryLoad(GetType(), out mySettings))
            {
                mySettings = new Settings();

                LogWarning("Settings not found => a new settings file has been created!");
            }
            else
            {
                LogInfo("Settings found and loaded");
            }

            // ✅ Ensure NumChars column starts as ReadOnly
            if (SelectfieldsGrid.Columns.Contains("NumberofCharacters"))
            {
                SelectfieldsGrid.Columns["NumberofCharacters"].ReadOnly = true;
            }

            // ✅ Hook up editing control event (for Criteria changes)
            SelectfieldsGrid.EditingControlShowing += SelectfieldsGrid_EditingControlShowing;


        }

        private void SelectfieldsGrid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (SelectfieldsGrid.CurrentCell.ColumnIndex == SelectfieldsGrid.Columns["Criteria"].Index)
            {
                if (e.Control is ComboBox combo)
                {
                    combo.SelectedIndexChanged -= Criteria_SelectedIndexChanged;
                    combo.SelectedIndexChanged += Criteria_SelectedIndexChanged;
                }
            }
        }

        // Enable/disable NumChars depending on Criteria
        private void Criteria_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is ComboBox combo && SelectfieldsGrid.CurrentCell != null)
            {
                string selected = combo.SelectedItem?.ToString();
                int rowIndex = SelectfieldsGrid.CurrentCell.RowIndex;

                if (selected == "Same First Characters" || selected == "Same Last Characters")
                {
                    SelectfieldsGrid.Rows[rowIndex].Cells["NumberofCharacters"].ReadOnly = false;
                }
                else
                {
                    SelectfieldsGrid.Rows[rowIndex].Cells["NumberofCharacters"].Value = null;
                    SelectfieldsGrid.Rows[rowIndex].Cells["NumberofCharacters"].ReadOnly = true;
                }
            }
        }




        private void tsbClose_Click(object sender, EventArgs e)
        {
            CloseTool();
        }


        /// <summary>
        /// This event occurs when the plugin is closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyPluginControl_OnCloseTool(object sender, EventArgs e)
        {
            // Before leaving, save the settings
            SettingsManager.Instance.Save(GetType(), mySettings);
        }

        /// <summary>
        /// This event occurs when the connection has been updated in XrmToolBox
        /// </summary>
        public override void UpdateConnection(IOrganizationService newService, ConnectionDetail detail, string actionName, object parameter)
        {
            base.UpdateConnection(newService, detail, actionName, parameter);

            if (mySettings != null && detail != null)
            {
                mySettings.LastUsedOrganizationWebappUrl = detail.WebApplicationUrl;
                LogInfo("Connection has changed to: {0}", detail.WebApplicationUrl);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

            SelectEntities.Text = string.Empty;

            // Clear SelectFieldsGrid
            SelectfieldsGrid.Rows.Clear();
            SelectfieldsGrid.Refresh();

            // Clear DuplicateSetsGrid
            DuplicateSetsGrid.Columns.Clear();
            DuplicateSetsGrid.Rows.Clear();
            DuplicateSetsGrid.Refresh();

            // Clear Potential Duplicates Grid
            PotentialsDuplecatesGrid.Columns.Clear();
            PotentialsDuplecatesGrid.Rows.Clear();
            PotentialsDuplecatesGrid.Refresh();

            // Reset all checkboxes
            checkBox1.Checked = true;
            checkBox2.Checked = true;
            //checkBox3.Checked = false;

            //Add default row
            SelectfieldsGrid.Rows.Add();



            WorkAsync(new WorkAsyncInfo
            {
                Message = "Loading entities...",
                Work = (worker, args) =>
                {
                    // Retrieve metadata for all entities
                    var request = new Microsoft.Xrm.Sdk.Messages.RetrieveAllEntitiesRequest
                    {
                        EntityFilters = Microsoft.Xrm.Sdk.Metadata.EntityFilters.Entity,
                        RetrieveAsIfPublished = true
                    };

                    var response = (Microsoft.Xrm.Sdk.Messages.RetrieveAllEntitiesResponse)Service.Execute(request);
                    args.Result = response.EntityMetadata;
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var entities = args.Result as Microsoft.Xrm.Sdk.Metadata.EntityMetadata[];
                    if (entities == null) return;

                    // Bind to the ComboBox (SelectEntities)
                    SelectEntities.Items.Clear();
                    foreach (var entity in entities.OrderBy(e1 => e1.LogicalName))
                    {
                        string displayName = entity.DisplayName?.UserLocalizedLabel?.Label ?? entity.LogicalName;
                        SelectEntities.Items.Add($"{displayName} ({entity.LogicalName})");
                    }

                    //MessageBox.Show("Entities loaded successfully!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            });


        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private bool IsSupportedForGrouping(AttributeMetadata attr)
        {
            return attr.AttributeType == AttributeTypeCode.String
                || attr.AttributeType == AttributeTypeCode.Integer
                || attr.AttributeType == AttributeTypeCode.BigInt
                || attr.AttributeType == AttributeTypeCode.Boolean
                || attr.AttributeType == AttributeTypeCode.DateTime
                || attr.AttributeType == AttributeTypeCode.Picklist
                || attr.AttributeType == AttributeTypeCode.State
                || attr.AttributeType == AttributeTypeCode.Status
                || attr.AttributeType == AttributeTypeCode.Lookup;
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectEntities.SelectedItem == null) return;



            // Clear SelectFieldsGrid
            SelectfieldsGrid.Rows.Clear();
            SelectfieldsGrid.Refresh();

            // Clear DuplicateSetsGrid
            DuplicateSetsGrid.Columns.Clear();
            DuplicateSetsGrid.Rows.Clear();
            DuplicateSetsGrid.Refresh();

            // Clear Potential Duplicates Grid
            PotentialsDuplecatesGrid.Columns.Clear();
            PotentialsDuplecatesGrid.Rows.Clear();
            PotentialsDuplecatesGrid.Refresh();

            // Reset all checkboxes
            checkBox1.Checked = true;
            checkBox2.Checked = true;

            //Add default row
            SelectfieldsGrid.Rows.Add();

            // Extract logical name (inside parentheses)
            string selectedEntity = SelectEntities.SelectedItem.ToString();
            string logicalName = selectedEntity.Substring(selectedEntity.LastIndexOf('(') + 1).TrimEnd(')');

            WorkAsync(new WorkAsyncInfo
            {
                Message = $"Loading fields for {logicalName}...",
                Work = (worker, args) =>
                {
                    var request = new RetrieveEntityRequest
                    {
                        EntityFilters = EntityFilters.Attributes,
                        LogicalName = logicalName,
                        RetrieveAsIfPublished = true
                    };

                    var response = (RetrieveEntityResponse)Service.Execute(request);
                    args.Result = response.EntityMetadata.Attributes;
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var attributes = args.Result as AttributeMetadata[];
                    if (attributes == null) return;

                    // Bind attributes to your DataGridView ComboBox column
                    var fieldColumn = (DataGridViewComboBoxColumn)SelectfieldsGrid.Columns["LogicalName"];
                    fieldColumn.Items.Clear();

                    foreach (var attr in attributes.OrderBy(a => a.LogicalName))
                    {
                        if (!IsSupportedForGrouping(attr)) continue; // ✅ skip unsupported

                        string displayName = attr.DisplayName?.UserLocalizedLabel?.Label ?? attr.LogicalName;
                        // store both display + logical safely
                        fieldColumn.Items.Add($"{displayName} ({attr.LogicalName})");
                    }


                    //MessageBox.Show($"Fields loaded for {logicalName}!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            });

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {


        }

        private void Export_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (DuplicateSetsGrid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a duplicate set row first.");
                return;
            }

            var selectedRow = DuplicateSetsGrid.SelectedRows[0];

            // ✅ get the group index directly from the hidden GroupId column
            int groupIndex = Convert.ToInt32(selectedRow.Cells["GroupId"].Value);

            // ✅ fetch the correct group
            var selectedGroup = duplicateSets[groupIndex];

            if (selectedGroup == null)
            {
                MessageBox.Show("Could not find duplicate group for the selected row.");
                return;
            }

            if (selectedGroup.Count <= 1)
            {
                MessageBox.Show("This group has only one record, nothing to delete.");
                return;
            }

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Deleting oldest records...",
                Work = (worker, args) =>
                {
                    // Order by CreatedOn (newest first)
                    var ordered = selectedGroup
                        .OrderByDescending(x => (DateTime)x["createdon"])
                        .ToList();

                    var keep = ordered.First(); // newest one stays
                    var deleteThese = ordered.Skip(1).ToList();

                    var failed = new List<string>();

                    foreach (var rec in deleteThese)
                    {
                        try
                        {
                            Service.Delete(rec.LogicalName, rec.Id);
                        }
                        catch (Exception ex)
                        {
                            failed.Add($"{rec.LogicalName} {rec.Id}: {ex.Message}");
                        }
                    }

                    args.Result = failed;
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show("Error while deleting oldest records: " + args.Error.Message);
                        return;
                    }

                    var failed = args.Result as List<string>;
                    if (failed != null && failed.Count > 0)
                    {
                        MessageBox.Show("Some records failed to delete:\n" + string.Join("\n", failed));
                    }
                    else
                    {
                        MessageBox.Show("Oldest records deleted successfully. Latest record kept.");
                    }
                }
            });
        }


        private void PotentialsDuplecatesGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {


        }

        private void button1_Click(object sender, EventArgs e)
        {
            SelectfieldsGrid.Rows.Add();

        }

        private void SearchRecords_Click(object sender, EventArgs e)
        {
            if (SelectEntities.SelectedItem == null)
            {
                MessageBox.Show("Please select an entity first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SelectfieldsGrid.EndEdit();


            string selectedEntity = SelectEntities.SelectedItem.ToString();
            string logicalName = selectedEntity.Substring(selectedEntity.LastIndexOf('(') + 1).TrimEnd(')');

            var selectedFields = new List<string>();
            foreach (DataGridViewRow row in SelectfieldsGrid.Rows)
            {
                if (row.IsNewRow) continue;
                var fieldRaw = row.Cells["LogicalName"].Value?.ToString();
                if (string.IsNullOrEmpty(fieldRaw)) continue;

                string fieldLogical = fieldRaw;
                int start = fieldRaw.LastIndexOf('(');
                if (start >= 0 && fieldRaw.EndsWith(")"))
                {
                    fieldLogical = fieldRaw.Substring(start + 1, fieldRaw.Length - start - 2);
                }
                selectedFields.Add(fieldLogical);
            }

            if (!selectedFields.Any())
            {
                MessageBox.Show("Please select at least one valid field.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Ensure every row has criteria
            bool allHaveCriteria = SelectfieldsGrid.Rows.Cast<DataGridViewRow>()
                .All(r =>
                    r.Cells["Criteria"] != null &&
                    r.Cells["Criteria"].Value != null &&
                    !string.IsNullOrWhiteSpace(r.Cells["Criteria"].Value.ToString())
                );

            if (!allHaveCriteria)
            {
                MessageBox.Show("Please select criteria for chosen fields.",
                                "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }



            WorkAsync(new WorkAsyncInfo
            {
                Message = "Searching duplicates...",
                Work = (worker, args) =>
                {
                    string fetch = $@" <fetch distinct='false' aggregate='true'> <entity name='{logicalName}'>";


                    fieldAliasMap.Clear();

                    foreach (var field in selectedFields)
                    {
                        string alias = field + "_alias";
                        fieldAliasMap[field] = alias;

                        // === Trim unnecessary spaces ===
                        if (checkBox1.Checked)
                        {
                            // NOTE: FetchXML does not support TRIM directly, so we just trim after fetch
                        }

                        fetch += $@"<attribute name='{field}' groupby='true' alias='{alias}' />";

                        // Retrieve attribute metadata for type checking
                        var attr = Service.Execute(new RetrieveAttributeRequest
                        {
                            EntityLogicalName = logicalName,
                            LogicalName = field,
                            RetrieveAsIfPublished = true
                        }) as RetrieveAttributeResponse;

                        // Exclude blank values if checkbox checked
                        if (checkBox2.Checked)
                        {
                            if (attr?.AttributeMetadata.AttributeType == AttributeTypeCode.Picklist ||
                                attr?.AttributeMetadata.AttributeType == AttributeTypeCode.State ||
                                attr?.AttributeMetadata.AttributeType == AttributeTypeCode.Status)
                            {
                                // OptionSet / Status / State: just not-null
                                fetch += $@"<filter type='and'>
                                    <condition attribute='{field}' operator='not-null' />
                                </filter>";
                            }
                            else
                            {
                                // Other fields: not-null AND not empty
                                fetch += $@"<filter type='and'>
                                    <condition attribute='{field}' operator='not-null' />
                                    <condition attribute='{field}' operator='ne' value='' />
                                </filter>";
                            }
                        }
                    }


                    fetch += $@"<attribute name='{logicalName}id' aggregate='count' alias='recordcount' />";

                    fetch += "<order alias='recordcount' descending='true' />";
                    fetch += "</entity></fetch>";

                    args.Result = Service.RetrieveMultiple(new FetchExpression(fetch));
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var result = args.Result as EntityCollection;
                    if (result == null || result.Entities.Count == 0)
                    {
                        MessageBox.Show("No duplicates found.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    DuplicateSetsGrid.Columns.Clear();
                    DuplicateSetsGrid.Rows.Clear();

                    DuplicateSetsGrid.Columns.Add("RecordCount", "Record Count");

                    foreach (var field in selectedFields)
                    {
                        DuplicateSetsGrid.Columns.Add(field, field);
                    }

                    // Temporary dictionary to accumulate counts after normalization
                    var bucketCounts = new Dictionary<string, int>();
                    var bucketValues = new Dictionary<string, List<string>>();


                    foreach (var entity in result.Entities)
                    {
                        int count = (int)((AliasedValue)entity["recordcount"]).Value;

                        // Normalize field value(s) here
                        var normalizedValues = new List<string>();
                        foreach (var field in selectedFields)
                        {
                            object value = null;
                            string alias = field + "_alias";
                            if (entity.Contains(alias))
                            {
                                var aliased = entity[alias] as AliasedValue;
                                value = aliased?.Value;
                            }

                            string strVal = string.Empty;

                            // Handle different attribute types appropriately
                            if (value is OptionSetValue optionSetValue)
                            {
                                // For OptionSet, use the integer value for comparison
                                strVal = optionSetValue.Value.ToString();
                            }
                            else if (value is EntityReference entityRef)
                            {
                                // For Lookups, use the GUID
                                strVal = entityRef.Id.ToString();
                            }
                            else if (value != null)
                            {
                                strVal = value.ToString();

                                // Only apply string operations to actual string types
                                if (value is string)
                                {
                                    if (checkBox1.Checked) strVal = strVal.Trim();
                                    // Don't lowercase OptionSet values as they're numeric
                                }
                            }

                            var fieldRow = SelectfieldsGrid.Rows.Cast<DataGridViewRow>()
                                .FirstOrDefault(r => r.Cells["LogicalName"].Value != null &&
                                                     r.Cells["LogicalName"].Value.ToString().Contains(field));
                            if (fieldRow != null)
                            {
                                string criteria = fieldRow.Cells["Criteria"].Value?.ToString();
                                int.TryParse(fieldRow.Cells["NumberofCharacters"].Value?.ToString(), out int numChars);


                                // Default to 1 if First/Last selected but no number entered
                                if (!string.IsNullOrEmpty(criteria) && (criteria.Contains("First") || criteria.Contains("Last")) && numChars <= 0)
                                {
                                    numChars = 1;
                                }

                                if (!string.IsNullOrEmpty(criteria) && numChars > 0)
                                {
                                    if (criteria.Contains("First"))
                                    {
                                        strVal = strVal.Length >= numChars ? strVal.Substring(0, numChars) : strVal;
                                    }
                                    else if (criteria.Contains("Last"))
                                    {
                                        strVal = strVal.Length >= numChars ? strVal.Substring(strVal.Length - numChars) : strVal;
                                    }
                                    strVal = strVal.Trim().ToLower();
                                }
                                else
                                {
                                    strVal = strVal.Trim().ToLower();
                                }



                                // Skip this field entirely if strVal is null
                                if (strVal == null)
                                    continue; // do not add this field to normalizedValues

                                normalizedValues.Add(strVal);


                            }


                        }

                        string key = string.Join("|", normalizedValues);

                        if (!bucketCounts.ContainsKey(key))
                        {
                            bucketCounts[key] = 0;
                            bucketValues[key] = normalizedValues;
                        }

                        bucketCounts[key] += count; // ✅ always add count, even if =1
                    }

                    // Now display only buckets where total > 1
                    DuplicateSetsGrid.Columns.Clear();
                    DuplicateSetsGrid.Rows.Clear();
                    DuplicateSetsGrid.Columns.Add("RecordCount", "Record Count");
                    foreach (var field in selectedFields)
                        DuplicateSetsGrid.Columns.Add(field, "Display Name");//displayname

                    if (!DuplicateSetsGrid.Columns.Contains("RecordId"))//hiddenrecordid bb
                    {
                        DuplicateSetsGrid.Columns.Add("RecordId", "Record Id");
                        DuplicateSetsGrid.Columns["RecordId"].Visible = false;
                    }
                    // ✅ Add hidden GroupId column for tracking
                    if (!DuplicateSetsGrid.Columns.Contains("GroupId"))
                    {
                        DuplicateSetsGrid.Columns.Add("GroupId", "Group Id");
                        DuplicateSetsGrid.Columns["GroupId"].Visible = false;
                    }

                    // Reset duplicateSets before building new groups
                    duplicateSets.Clear();

                    foreach (var kv in bucketCounts.Where(x => x.Value > 1))
                    {


                        // Build query for this duplicate group
                        var conditions = new List<ConditionExpression>();
                        int i = 0;


                        //first/Last criteria
                        foreach (var field in selectedFields)
                        {
                            string value = bucketValues[kv.Key][i];
                            if (!string.IsNullOrEmpty(value))
                            {
                                var fieldRow = SelectfieldsGrid.Rows
                                    .Cast<DataGridViewRow>()
                                    .FirstOrDefault(r => r.Cells["LogicalName"].Value != null &&
                                                         r.Cells["LogicalName"].Value.ToString().Contains(field));

                                string criteria = fieldRow?.Cells["Criteria"].Value?.ToString();
                                int.TryParse(fieldRow?.Cells["NumberofCharacters"].Value?.ToString(), out int numChars);
                                // Default to 1 if First/Last selected but empty
                                if (!string.IsNullOrEmpty(criteria) && (criteria.Contains("First") || criteria.Contains("Last")) && numChars <= 0)
                                {
                                    numChars = 1;
                                }


                                if (!string.IsNullOrEmpty(criteria) && numChars > 0)
                                {
                                    string subVal = value.Length >= numChars ? value.Substring(0, numChars) : value;

                                    if (criteria.Contains("First"))
                                    {
                                        // Ensure trimmed value
                                        subVal = subVal.Trim().ToLower(); ;
                                        conditions.Add(new ConditionExpression(field, ConditionOperator.BeginsWith, subVal));
                                    }
                                    else if (criteria.Contains("Last"))
                                    {
                                        subVal = subVal.Trim().ToLower(); ;
                                        conditions.Add(new ConditionExpression(field, ConditionOperator.EndsWith, subVal));
                                    }
                                    else if (criteria.Contains("Exact"))
                                    {
                                        conditions.Add(new ConditionExpression(field, ConditionOperator.Equal, value.Trim()));
                                    }
                                }

                                else
                                {
                                    // Handle OptionSet values differently
                                    if (value is OptionSetValue)
                                    {
                                        if (int.TryParse(value, out int optionSetValue))
                                        {
                                            conditions.Add(new ConditionExpression(field, ConditionOperator.Equal, optionSetValue));
                                        }
                                    }
                                    else
                                    {
                                        // Exact match fallback for other types
                                        conditions.Add(new ConditionExpression(field, ConditionOperator.Equal, value));
                                    }
                                }

                            }
                            i++;
                        }





                        string primaryName = GetPrimaryNameAttribute(logicalName);

                        var qe = new QueryExpression(logicalName)
                        {
                            ColumnSet = new ColumnSet(
                                selectedFields
                                    .Concat(new[] { primaryName, "createdon" })
                                    .Distinct() // avoid duplicates
                                    .ToArray()
                            )
                        };


                        var filter = new FilterExpression(LogicalOperator.And);
                        foreach (var cond in conditions)
                        {
                            filter.AddCondition(cond);
                        }
                        qe.Criteria.AddFilter(filter);


                        var groupResult = Service.RetrieveMultiple(qe);
                        if (groupResult.Entities.Count > 0)
                            duplicateSets.Add(groupResult.Entities.ToList());
                    }
                    //Duplicate Record not Found Check
                    if (duplicateSets.Count == 0)
                    {
                        MessageBox.Show("No duplicate records found.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }



                    // Now populate DuplicateSetsGrid
                    DuplicateSetsGrid.Columns.Clear();
                    DuplicateSetsGrid.Rows.Clear();
                    DuplicateSetsGrid.Columns.Add("RecordCount", "Record Count");
                    foreach (var field in selectedFields)
                        DuplicateSetsGrid.Columns.Add(field, "Display Name");
                    if (!DuplicateSetsGrid.Columns.Contains("RecordId"))
                    {
                        DuplicateSetsGrid.Columns.Add("RecordId", "Record Id");
                        DuplicateSetsGrid.Columns["RecordId"].Visible = false;
                    }
                    if (!DuplicateSetsGrid.Columns.Contains("GroupId"))
                    {
                        DuplicateSetsGrid.Columns.Add("GroupId", "Group Id");
                        DuplicateSetsGrid.Columns["GroupId"].Visible = false;
                    }

                    // In the SearchRecords_Click method, where you populate DuplicateSetsGrid:
                    for (int groupIndex = 0; groupIndex < duplicateSets.Count; groupIndex++)
                    {
                        var group = duplicateSets[groupIndex];
                        if (group.Count > 1)
                        {
                            var rowValues = new List<object> { group.Count };
                            var firstEntity = group.First();
                            foreach (var field in selectedFields)
                            {
                                object value = firstEntity.Contains(field) ? firstEntity[field] : "";

                                // Convert OptionSetValue to display label
                                if (value is OptionSetValue optionSetValue)
                                {
                                    try
                                    {
                                        var optionMetadata = GetOptionSetMetadata(logicalName, field);
                                        var option = optionMetadata?.Options.FirstOrDefault(o => o.Value == optionSetValue.Value);
                                        value = option?.Label.UserLocalizedLabel?.Label ?? optionSetValue.Value.ToString();
                                    }
                                    catch
                                    {
                                        value = optionSetValue.Value.ToString();
                                    }
                                }
                                // Handle other special types if needed (EntityReference, etc.)

                                rowValues.Add(value);
                            }

                            // Add hidden GroupId at the end
                            rowValues.Add(firstEntity.Id.ToString());
                            rowValues.Add(groupIndex); // GroupId

                            DuplicateSetsGrid.Rows.Add(rowValues.ToArray());
                        }
                    }

                }

            });
        }



        private void DuplicateSetsGrid_SelectionChanged(object sender, EventArgs e)
        {
            // Clear potential duplicates if nothing selected
            PotentialsDuplecatesGrid.Rows.Clear();
            PotentialsDuplecatesGrid.Columns.Clear();

            if (DuplicateSetsGrid.SelectedRows.Count == 0)
            {
                //MessageBox.Show("clear");
                PotentialsDuplecatesGrid.Rows.Clear();

                return;
            }

            var row = DuplicateSetsGrid.SelectedRows[0];

            if (SelectEntities.SelectedItem == null)
            {
                MessageBox.Show("Please select an entity first.");
                return;
            }

            string selectedEntity = SelectEntities.SelectedItem.ToString();
            string logicalName = selectedEntity.Substring(selectedEntity.LastIndexOf('(') + 1).TrimEnd(')');

            // Get entity metadata for type conversion
            EntityMetadata entityMetadata = null;
            try
            {
                var request = new RetrieveEntityRequest
                {
                    LogicalName = logicalName,
                    EntityFilters = EntityFilters.Attributes,
                    RetrieveAsIfPublished = true
                };
                var response = (RetrieveEntityResponse)Service.Execute(request);
                entityMetadata = response.EntityMetadata;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error getting metadata: {ex.Message}");
                return;
            }

            // Collect conditions
            var conditions = new List<ConditionExpression>();
            var potentialFields = new List<string>();

            foreach (DataGridViewColumn column in DuplicateSetsGrid.Columns)
            {
                if (column.Name == "RecordCount" || column.Name == "RecordId" || column.Name == "GroupId")
                    continue;

                string field = column.Name;
                var cell = row.Cells[column.Index];

                object rawValue = cell.Tag ?? cell.Value; // Prefer Tag if set


                if (rawValue != null && !string.IsNullOrEmpty(rawValue.ToString()))
                {
                    string stringVal = rawValue.ToString();

                    // Find matching row in SelectfieldsGrid for this field
                    var fieldRow = SelectfieldsGrid.Rows
                        .Cast<DataGridViewRow>()
                        .FirstOrDefault(r => r.Cells["LogicalName"].Value != null &&
                                             r.Cells["LogicalName"].Value.ToString().Contains(field));

                    string criteria = fieldRow?.Cells["Criteria"].Value?.ToString();

                    int numChars = 0;
                    if (fieldRow != null)
                    {
                        int.TryParse(fieldRow.Cells["NumberofCharacters"].Value?.ToString(), out numChars);

                        // Default to 1 if criteria requires it and user left blank/invalid
                        if (numChars <= 0 && !string.IsNullOrEmpty(criteria) &&
                            (criteria.Contains("First") || criteria.Contains("Last")))
                        {
                            numChars = 1;
                        }
                    }
                    stringVal = stringVal ?? string.Empty;



                    if (!string.IsNullOrEmpty(criteria))
                    {
                        if (criteria.Contains("First"))
                        {
                            string subVal = stringVal.Length >= numChars ? stringVal.Substring(0, numChars) : stringVal;
                            subVal = subVal.Trim().ToLower();
                            conditions.Add(new ConditionExpression(field, ConditionOperator.Like, subVal + "%"));
                        }
                        else if (criteria.Contains("Last"))
                        {
                            string subVal = stringVal.Length >= numChars ? stringVal.Substring(stringVal.Length - numChars) : stringVal;
                            subVal = subVal.Trim().ToLower();
                            conditions.Add(new ConditionExpression(field, ConditionOperator.Like, "%" + subVal));
                        }
                        else
                        {
                            var attr = entityMetadata.Attributes.FirstOrDefault(a => a.LogicalName == field);
                            if (attr?.AttributeType == AttributeTypeCode.Picklist)
                            {
                                int? optionInt = null;

                                if (rawValue is OptionSetValue osv)
                                {
                                    // Already an OptionSetValue object
                                    optionInt = osv.Value;
                                }
                                else if (rawValue is string stringValue)
                                {
                                    // If it's a string, it could be either the label or the value
                                    if (int.TryParse(stringValue, out int intVal))
                                    {
                                        // It's already the integer value
                                        optionInt = intVal;
                                    }
                                    else
                                    {
                                        // It's the display label - we need to look up the integer value
                                        try
                                        {
                                            var optionMetadata = GetOptionSetMetadata(logicalName, field);
                                            var option = optionMetadata?.Options.FirstOrDefault(o =>
                                                o.Label.UserLocalizedLabel?.Label?.Equals(stringValue, StringComparison.OrdinalIgnoreCase) == true);

                                            if (option != null)
                                            {
                                                optionInt = option.Value;
                                            }
                                            else
                                            {
                                                // Try case-insensitive comparison if exact match fails
                                                option = optionMetadata?.Options.FirstOrDefault(o =>
                                                    o.Label.UserLocalizedLabel?.Label?.ToLower() == stringValue.ToLower());
                                                optionInt = option?.Value;
                                            }
                                        }
                                        catch
                                        {
                                            // If metadata lookup fails, try to proceed with string comparison
                                        }
                                    }
                                }

                                if (optionInt.HasValue)
                                {
                                    conditions.Add(new ConditionExpression(field, ConditionOperator.Equal, optionInt.Value));
                                }
                                else
                                {
                                    // Fallback: use the string value (though this may not work for queries)
                                    conditions.Add(new ConditionExpression(field, ConditionOperator.Equal, stringVal));
                                }
                            }
                            else
                            {
                                conditions.Add(new ConditionExpression(field, ConditionOperator.Equal, stringVal?.Trim().ToLower()));
                            }

                        }


                    }
                }



                potentialFields.Add(field);
            }

            if (conditions.Count == 0)
            {
                MessageBox.Show("No valid conditions found for the selected duplicate set.");
                return;
            }

            string primaryName = GetPrimaryNameAttribute(logicalName);

            // 🚀 Load duplicates into grid
            LoadPotentialDuplicates(logicalName, potentialFields, conditions, primaryName);
        }


        private object ConvertCellValueToProperType(object cellValue, string fieldName, EntityMetadata entityMetadata)
        {
            try
            {
                // Find the attribute metadata
                var attribute = entityMetadata.Attributes.FirstOrDefault(a => a.LogicalName == fieldName);
                if (attribute == null) return cellValue;

                string stringValue = cellValue.ToString();

                switch (attribute.AttributeType)
                {
                    case AttributeTypeCode.Picklist:
                    case AttributeTypeCode.State:
                    case AttributeTypeCode.Status:
                        if (int.TryParse(stringValue, out int optionValue))
                            return new OptionSetValue(optionValue);
                        break;

                    case AttributeTypeCode.Lookup:
                        if (Guid.TryParse(stringValue, out Guid lookupId))
                        {
                            // For lookups, cast to LookupAttributeMetadata to get targets
                            if (attribute is LookupAttributeMetadata lookupMetadata)
                            {
                                if (lookupMetadata.Targets != null && lookupMetadata.Targets.Length > 0)
                                {
                                    return new EntityReference(lookupMetadata.Targets[0], lookupId);
                                }
                            }
                            // Fallback if we can't determine the target entity
                            return new EntityReference("none", lookupId);
                        }
                        break;

                    case AttributeTypeCode.Integer:
                        if (int.TryParse(stringValue, out int intValue))
                            return intValue;
                        break;

                    case AttributeTypeCode.Boolean:
                        if (bool.TryParse(stringValue, out bool boolValue))
                            return boolValue;
                        break;

                    case AttributeTypeCode.DateTime:
                        if (DateTime.TryParse(stringValue, out DateTime dateValue))
                            return dateValue;
                        break;

                    case AttributeTypeCode.Decimal:
                        if (decimal.TryParse(stringValue, out decimal decimalValue))
                            return decimalValue;
                        break;

                    case AttributeTypeCode.Money:
                        if (decimal.TryParse(stringValue, out decimal moneyValue))
                            return new Money(moneyValue);
                        break;

                    default:
                        return stringValue; // For string types
                }
            }
            catch
            {
                // If conversion fails, return the original value
            }

            return cellValue;
        }

        private void LoadPotentialDuplicates(string entityName, List<string> potentialFields, List<ConditionExpression> conditions, string primaryName)
        {
            try
            {
                PotentialsDuplecatesGrid.Columns.Clear();
                PotentialsDuplecatesGrid.Rows.Clear();

                // Build query
                var qe = new QueryExpression(entityName)
                {
                    ColumnSet = new ColumnSet(potentialFields.Concat(new[] { primaryName, "createdon" }).ToArray())
                };

                var criteriaFilter = new FilterExpression(LogicalOperator.And);

                foreach (var condition in conditions)
                {
                    // Retrieve metadata
                    var attr = Service.Execute(new RetrieveAttributeRequest
                    {
                        EntityLogicalName = entityName,
                        LogicalName = condition.AttributeName,
                        RetrieveAsIfPublished = true
                    }) as RetrieveAttributeResponse;

                    if (attr?.AttributeMetadata.AttributeType == AttributeTypeCode.Picklist)
                    {
                        var picklist = attr.AttributeMetadata as PicklistAttributeMetadata;

                        // Determine the label or value to match
                        string labelToMatch = null;

                        if (condition.Values[0] is OptionSetValue os)
                        {
                            // Already an OptionSetValue: extract integer value
                            condition.Values[0] = os.Value; // assign integer, not object
                            continue; // skip further mapping
                        }
                        else
                        {
                            labelToMatch = condition.Values[0]?.ToString();
                        }

                        // Match label in OptionSet
                        var option = picklist.OptionSet.Options
                            .FirstOrDefault(o => string.Equals(o.Label?.UserLocalizedLabel?.Label, labelToMatch, StringComparison.OrdinalIgnoreCase));

                        if (option != null)
                        {
                            condition.Values[0] = option.Value; // integer
                        }
                        else if (int.TryParse(labelToMatch, out int intVal))
                        {
                            // Already an integer string
                            condition.Values[0] = intVal;
                        }
                        else
                        {
                            throw new Exception($"Invalid OptionSet value '{condition.Values[0]}' for field {condition.AttributeName}");
                        }
                    }

                    criteriaFilter.Conditions.Add(condition);
                }




                qe.Criteria.AddFilter(criteriaFilter);

                var result = Service.RetrieveMultiple(qe);

                // ✅ Only update the PotentialDuplicatesGrid
                foreach (var field in potentialFields)
                {
                    PotentialsDuplecatesGrid.Columns.Add(field, field);
                }

                PotentialsDuplecatesGrid.Columns.Add("FullName", "Full Name");
                PotentialsDuplecatesGrid.Columns.Add("CreatedOn", "Created On");
                PotentialsDuplecatesGrid.Columns.Add("RecordId", "Record ID");
                PotentialsDuplecatesGrid.Columns["RecordId"].Visible = false;

                foreach (var entity in result.Entities)
                {
                    var rowValues = new List<object>();

                    foreach (var field in potentialFields)
                    {
                        object val = entity.Contains(field) ? entity[field] : null;
                        string displayVal = "";

                        if (val is OptionSetValue opt)
                        {
                            try
                            {
                                var optionMetadata = GetOptionSetMetadata(entity.LogicalName, field);
                                var option = optionMetadata?.Options.FirstOrDefault(o => o.Value == opt.Value);
                                displayVal = option?.Label.UserLocalizedLabel?.Label ?? opt.Value.ToString();
                            }
                            catch
                            {
                                displayVal = opt.Value.ToString();
                            }
                        }
                        else if (val is EntityReference lookup)
                        {
                            displayVal = lookup.Name ?? lookup.Id.ToString();
                        }
                        else if (val is DateTime dt)
                        {
                            displayVal = dt.ToLocalTime().ToString("yyyy-MM-dd HH:mm");
                        }
                        else if (val is Money money)
                        {
                            displayVal = money.Value.ToString("C");
                        }
                        else if (val != null)
                        {
                            displayVal = val.ToString();
                        }

                        rowValues.Add(displayVal); // always add string, never OptionSetValue object
                    }



                    string fullName = entity.Contains(primaryName) ? entity[primaryName].ToString() : "(no name)";
                    rowValues.Add(fullName);
                    //string createdOnVal = entity.Contains("createdon")
                    //    ? ((DateTime)entity["createdon"]).ToString("yyyy-MM-dd HH:mm")
                    //    : "";


                    string createdOnVal = entity.Contains("createdon")
                        ? ((DateTime)entity["createdon"]).ToLocalTime().ToString("yyyy-MM-dd HH:mm")
                        : "";




                    rowValues.Add(createdOnVal);

                    rowValues.Add(entity.Id.ToString());

                    PotentialsDuplecatesGrid.Rows.Add(rowValues.ToArray());
                }

                PotentialsDuplecatesGrid.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading potential duplicates: {ex.Message}\n\n{ex.StackTrace}",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private OptionSetMetadata GetOptionSetMetadata(string entityName, string attributeName)
        {
            var request = new RetrieveAttributeRequest
            {
                EntityLogicalName = entityName,
                LogicalName = attributeName,
                RetrieveAsIfPublished = true
            };

            var response = (RetrieveAttributeResponse)Service.Execute(request);
            return (response.AttributeMetadata as PicklistAttributeMetadata)?.OptionSet;
        }

        private string GetPrimaryNameAttribute(string entityLogicalName)
        {
            var request = new RetrieveEntityRequest
            {
                LogicalName = entityLogicalName,
                EntityFilters = EntityFilters.Entity,
                RetrieveAsIfPublished = true
            };

            var response = (RetrieveEntityResponse)Service.Execute(request);
            return response.EntityMetadata.PrimaryNameAttribute;
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void ClearLatest_Click(object sender, EventArgs e)
        {
            if (DuplicateSetsGrid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a duplicate set first.");
                return;
            }

            var selectedRow = DuplicateSetsGrid.SelectedRows[0];

            // Safe parsing of GroupId
            if (!int.TryParse(selectedRow.Cells["GroupId"].Value?.ToString(), out int groupIndex))
            {
                MessageBox.Show("Invalid group selected.");
                return;
            }

            if (duplicateSets == null || groupIndex >= duplicateSets.Count)
            {
                MessageBox.Show("Duplicate set not found.");
                return;
            }

            var group = duplicateSets[groupIndex];
            if (group.Count <= 1)
            {
                MessageBox.Show("This group has only one record, nothing to delete.");
                return;
            }

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Deleting latest records...",
                Work = (worker, args) =>
                {
                    // Order by createdon ascending → oldest first
                    var ordered = group.OrderBy(x => (DateTime)x["createdon"]).ToList();
                    var keep = ordered.First(); // oldest record
                    var deleteThese = ordered.Skip(1).ToList(); // delete newer ones

                    var failed = new List<string>();

                    foreach (var rec in deleteThese)
                    {
                        try
                        {
                            Service.Delete(rec.LogicalName, rec.Id);
                        }
                        catch (Exception ex)
                        {
                            failed.Add($"{rec.LogicalName} {rec.Id}: {ex.Message}");
                        }
                    }

                    args.Result = failed;
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show("Error while deleting latest records: " + args.Error.Message);
                        return;
                    }

                    var failed = args.Result as List<string>;
                    if (failed != null && failed.Count > 0)
                    {
                        MessageBox.Show("Some records failed to delete:\n" + string.Join("\n", failed));
                    }
                    else
                    {
                        MessageBox.Show("Latest records deleted successfully. Only the oldest record was kept.");
                    }
                }
            });
        }


        private void ExportButton_Click(object sender, EventArgs e)
        {
            if (duplicateSets == null || duplicateSets.Count == 0)
            {
                MessageBox.Show("No duplicate sets to export.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Build a list of logical field names
            var selectedFields = new List<string>();
            foreach (DataGridViewRow row in SelectfieldsGrid.Rows)
            {
                if (row.Cells[0].Value != null)
                {
                    string displayText = row.Cells[0].Value.ToString();
                    string logicalName = displayText;

                    int start = displayText.LastIndexOf('(');
                    if (start >= 0 && displayText.EndsWith(")"))
                    {
                        logicalName = displayText.Substring(start + 1, displayText.Length - start - 2);
                    }

                    selectedFields.Add(logicalName);
                }
            }

            // Always include RecordId, GroupID, and FullName
            var headers = new List<string> { "GroupID", "RecordId" };
            headers.AddRange(selectedFields);
            headers.Add("FullName");

            var sb = new StringBuilder();
            sb.AppendLine(string.Join(",", headers));

            int groupId = 1;
            foreach (var group in duplicateSets)
            {
                foreach (var entity in group)
                {
                    var row = new List<string>
            {
                groupId.ToString(),
                entity.Id.ToString()
            };

                    foreach (var field in selectedFields)
                    {
                        if (entity.Contains(field))
                        {
                            object val = entity[field];
                            string displayVal = "";

                            if (val is OptionSetValue opt)
                            {
                                // Try to get the label instead of just the value
                                try
                                {
                                    var optionMetadata = GetOptionSetMetadata(entity.LogicalName, field);
                                    if (optionMetadata != null)
                                    {
                                        var option = optionMetadata.Options.FirstOrDefault(o => o.Value == opt.Value);
                                        displayVal = option?.Label.UserLocalizedLabel?.Label ?? opt.Value.ToString();
                                    }
                                    else
                                    {
                                        displayVal = opt.Value.ToString();
                                    }
                                }
                                catch
                                {
                                    displayVal = opt.Value.ToString();
                                }
                            }
                            else if (val is EntityReference lookup)
                                displayVal = lookup.Name ?? lookup.Id.ToString();
                            else if (val is DateTime dt)
                                displayVal = dt.ToString("yyyy-MM-dd HH:mm");
                            else if (val is Money money)
                                displayVal = money.Value.ToString();
                            else
                                displayVal = val?.ToString() ?? "";

                            // Escape commas for CSV
                            if (displayVal.Contains(",")) displayVal = $"\"{displayVal}\"";

                            row.Add(displayVal);
                        }
                        else
                        {
                            row.Add("");
                        }
                    }

                    string primaryName = GetPrimaryNameAttribute(entity.LogicalName); // dynamically get primary field
                    string displayName = entity.Contains(primaryName) ? entity[primaryName].ToString() : "";
                    row.Add(displayName); // replaces the old 'fullname'


                    sb.AppendLine(string.Join(",", row));
                }
                groupId++;
            }

            // Save file dialog
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSV files (*.csv)|*.csv";
                sfd.FileName = "DuplicateRecords.csv";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(sfd.FileName, sb.ToString());
                    MessageBox.Show("Export completed.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        }


        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void btnDeleteLastRow_Click(object sender, EventArgs e)
        {
            if (SelectfieldsGrid.Rows.Count > 0)
            {
                int lastIndex = SelectfieldsGrid.Rows.Count - 1;
                if (!SelectfieldsGrid.Rows[lastIndex].IsNewRow)
                {
                    SelectfieldsGrid.Rows.RemoveAt(lastIndex);
                }
            }
            else
            {
                MessageBox.Show("No rows available to delete.",
                                "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }




        }
    }
}
