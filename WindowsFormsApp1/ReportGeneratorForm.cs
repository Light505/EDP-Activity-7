using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Drawing;
using System.Globalization;
using System.IO.Packaging;
using System.Windows.Forms;
using System.Xml;
using MySql.Data.MySqlClient;

namespace InformationSystem
{
    public partial class ReportGeneratorForm : Form
    {
        DbConnection db = new DbConnection();
        private string lastReportName;
        private DataTable lastReportData;

        public ReportGeneratorForm()
        {
            InitializeComponent();
            cmbReport.SelectedIndex = 0;
            UpdateExportAvailability();
        }

        private string GetSelectedQuery()
        {
            switch (cmbReport.Text)
            {
                case "Course Enrollment Summary":
                    return "SELECT * FROM v_course_enrollment_summary";

                case "Department GPA Report":
                    return "SELECT * FROM v_department_gpa_report";

                case "Student Schedule":
                    return "SELECT * FROM v_student_schedule";

                case "Enrollment Transactions":
                    return @"
                        SELECT e.enrollment_id,
                               s.student_no,
                               CONCAT(s.last_name, ', ', s.first_name) AS student_name,
                               c.course_code,
                               c.course_title,
                               e.term,
                               e.status,
                               e.enrolled_at
                        FROM enrollments e
                        INNER JOIN students s ON s.student_id = e.student_id
                        INNER JOIN courses c ON c.course_id = e.course_id
                        ORDER BY e.enrolled_at DESC";

                case "Dropped Enrollments":
                    return @"
                        SELECT e.enrollment_id,
                               s.student_no,
                               CONCAT(s.last_name, ', ', s.first_name) AS student_name,
                               c.course_code,
                               c.course_title,
                               e.term,
                               e.status,
                               e.enrolled_at
                        FROM enrollments e
                        INNER JOIN students s ON s.student_id = e.student_id
                        INNER JOIN courses c ON c.course_id = e.course_id
                        WHERE e.status = 'DROPPED'
                        ORDER BY e.enrolled_at DESC";

                case "Grades Report":
                    return @"
                        SELECT g.grade_id,
                               e.enrollment_id,
                               s.student_no,
                               CONCAT(s.last_name, ', ', s.first_name) AS student_name,
                               c.course_code,
                               c.course_title,
                               e.term,
                               g.grade,
                               g.remarks
                        FROM grades g
                        INNER JOIN enrollments e ON e.enrollment_id = g.enrollment_id
                        INNER JOIN students s ON s.student_id = e.student_id
                        INNER JOIN courses c ON c.course_id = e.course_id
                        ORDER BY e.enrolled_at DESC";

                case "Users List":
                    return "SELECT user_id, username, email FROM users";

                case "Students List":
                    return "SELECT student_id, student_no, first_name, last_name, email, year_level, status FROM students";

                case "Professors List":
                    return "SELECT professorId, firstName, lastName, monthlySalary, yearlyBonus, departmentId FROM professors";

                default:
                    return "";
            }
        }

        private void LoadReport()
        {
            string query = GetSelectedQuery();

            if (query == "")
            {
                MessageBox.Show("Please select a report.");
                return;
            }

            using (MySqlConnection conn = db.GetConnection())
            {
                try
                {
                    conn.Open();

                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvReport.DataSource = dt;
                    lastReportName = cmbReport.Text;
                    lastReportData = dt;
                    UpdateExportAvailability();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading report: " + ex.Message);
                }
            }
        }

        private void SearchReport()
        {
            string search = txtSearch.Text.Trim();

            if (search == "")
            {
                LoadReport();
                return;
            }

            string query = "";

            switch (cmbReport.Text)
            {
                case "Course Enrollment Summary":
                    query = "SELECT * FROM v_course_enrollment_summary WHERE course_code LIKE @search OR course_title LIKE @search OR department_name LIKE @search OR term LIKE @search";
                    break;

                case "Department GPA Report":
                    query = "SELECT * FROM v_department_gpa_report WHERE department_code LIKE @search OR department_name LIKE @search OR term LIKE @search";
                    break;

                case "Student Schedule":
                    query = "SELECT * FROM v_student_schedule WHERE student_no LIKE @search OR student_name LIKE @search OR course_code LIKE @search OR course_title LIKE @search OR term LIKE @search";
                    break;

                case "Enrollment Transactions":
                    query = @"
                        SELECT e.enrollment_id,
                               s.student_no,
                               CONCAT(s.last_name, ', ', s.first_name) AS student_name,
                               c.course_code,
                               c.course_title,
                               e.term,
                               e.status,
                               e.enrolled_at
                        FROM enrollments e
                        INNER JOIN students s ON s.student_id = e.student_id
                        INNER JOIN courses c ON c.course_id = e.course_id
                        WHERE s.student_no LIKE @search
                           OR s.first_name LIKE @search
                           OR s.last_name LIKE @search
                           OR c.course_code LIKE @search
                           OR c.course_title LIKE @search
                           OR e.term LIKE @search
                           OR e.status LIKE @search
                        ORDER BY e.enrolled_at DESC";
                    break;

                case "Dropped Enrollments":
                    query = @"
                        SELECT e.enrollment_id,
                               s.student_no,
                               CONCAT(s.last_name, ', ', s.first_name) AS student_name,
                               c.course_code,
                               c.course_title,
                               e.term,
                               e.status,
                               e.enrolled_at
                        FROM enrollments e
                        INNER JOIN students s ON s.student_id = e.student_id
                        INNER JOIN courses c ON c.course_id = e.course_id
                        WHERE e.status = 'DROPPED'
                          AND (s.student_no LIKE @search
                           OR s.first_name LIKE @search
                           OR s.last_name LIKE @search
                           OR c.course_code LIKE @search
                           OR c.course_title LIKE @search
                           OR e.term LIKE @search)
                        ORDER BY e.enrolled_at DESC";
                    break;

                case "Grades Report":
                    query = @"
                        SELECT g.grade_id,
                               e.enrollment_id,
                               s.student_no,
                               CONCAT(s.last_name, ', ', s.first_name) AS student_name,
                               c.course_code,
                               c.course_title,
                               e.term,
                               g.grade,
                               g.remarks
                        FROM grades g
                        INNER JOIN enrollments e ON e.enrollment_id = g.enrollment_id
                        INNER JOIN students s ON s.student_id = e.student_id
                        INNER JOIN courses c ON c.course_id = e.course_id
                        WHERE s.student_no LIKE @search
                           OR s.first_name LIKE @search
                           OR s.last_name LIKE @search
                           OR c.course_code LIKE @search
                           OR c.course_title LIKE @search
                           OR e.term LIKE @search
                           OR CAST(g.grade AS CHAR) LIKE @search
                           OR g.remarks LIKE @search
                        ORDER BY e.enrolled_at DESC";
                    break;

                case "Users List":
                    query = "SELECT user_id, username, email FROM users WHERE username LIKE @search OR email LIKE @search";
                    break;

                case "Students List":
                    query = "SELECT student_id, student_no, first_name, last_name, email, year_level, status FROM students WHERE student_no LIKE @search OR first_name LIKE @search OR last_name LIKE @search OR email LIKE @search";
                    break;

                case "Professors List":
                    query = "SELECT professorId, firstName, lastName, monthlySalary, yearlyBonus, departmentId FROM professors WHERE firstName LIKE @search OR lastName LIKE @search";
                    break;

                default:
                    MessageBox.Show("Please select a report.");
                    return;
            }

            using (MySqlConnection conn = db.GetConnection())
            {
                try
                {
                    conn.Open();

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@search", "%" + search + "%");

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvReport.DataSource = dt;
                    lastReportName = cmbReport.Text;
                    lastReportData = dt;
                    UpdateExportAvailability();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error searching report: " + ex.Message);
                }
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            LoadReport();
        }

        private void btnSearch_Click_1(object sender, EventArgs e)
        {
            SearchReport();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            ExportReport();
        }

        private void cmbReport_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgvReport.DataSource = null;
            lastReportData = null;
            lastReportName = null;
            UpdateExportAvailability();
        }

        private void dgvReport_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void UpdateExportAvailability()
        {
            string templatePath = GetTemplatePath(cmbReport.Text);
            btnExport.Enabled = !string.IsNullOrWhiteSpace(templatePath) && File.Exists(templatePath);
        }

        private void ExportReport()
        {
            if (dgvReport.DataSource == null)
            {
                MessageBox.Show("Please generate a report first.");
                return;
            }

            if (!string.Equals(lastReportName, cmbReport.Text, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Please generate the selected report before exporting.");
                return;
            }

            DataTable data = lastReportData ?? dgvReport.DataSource as DataTable;

            if (data == null)
            {
                MessageBox.Show("No data available to export.");
                return;
            }

            if (data.Rows.Count == 0)
            {
                MessageBox.Show("No records to export.");
                return;
            }

            string reportName = cmbReport.Text;
            string templatePath = GetTemplatePath(reportName);

            if (string.IsNullOrWhiteSpace(templatePath) || !File.Exists(templatePath))
            {
                MessageBox.Show("Report template not found. Please configure the template path.");
                return;
            }

            string outputPath = GetOutputPath(reportName);

            try
            {
                ExportToExcel(templatePath, outputPath, data, reportName);

                MessageBox.Show("Report exported successfully to:\n" + outputPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error exporting report: " + ex.Message);
            }
        }

        private string GetTemplatePath(string reportName)
        {
            string fileName = string.Empty;

            switch (reportName)
            {
                case "Enrollment Transactions":
                    fileName = "Interop_EnrollmentTransactionsTemplate.xlsx";
                    break;
                case "Dropped Enrollments":
                    fileName = "Interop_DroppedEnrollmentsTemplate.xlsx";
                    break;
                case "Grades Report":
                    fileName = "Interop_GradesReportTemplate.xlsx";
                    break;
                default:
                    return string.Empty;
            }

            return ResolvePath(Path.Combine("ReportTemplates", fileName));
        }

        private string GetOutputPath(string reportName)
        {
            string outputFolder = ResolveDirectory(Path.Combine("Reports", "Generated"));

            string fileName = reportName.Replace(" ", "_") + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx";
            return Path.Combine(outputFolder, fileName);
        }

        private void ExportToExcel(string templatePath, string outputPath, DataTable data, string reportName)
        {
            File.Copy(templatePath, outputPath, true);

            using (Package package = Package.Open(outputPath, FileMode.Open, FileAccess.ReadWrite))
            {
                List<PackagePart> worksheets = GetWorksheetParts(package);
                if (worksheets.Count == 0)
                {
                    throw new InvalidOperationException("The report template does not contain any worksheets.");
                }

                UpdateReportWorksheet(worksheets[0], data, reportName);
                if (worksheets.Count > 1)
                {
                    UpdateChartWorksheet(package, worksheets[1], data, reportName);
                }
            }
        }

        private string ResolvePath(string relativePath)
        {
            string current = AppDomain.CurrentDomain.BaseDirectory;

            for (int i = 0; i < 5 && !string.IsNullOrWhiteSpace(current); i++)
            {
                string candidate = Path.Combine(current, relativePath);
                if (File.Exists(candidate) || Directory.Exists(candidate))
                {
                    return candidate;
                }

                DirectoryInfo parent = Directory.GetParent(current);
                current = parent?.FullName;
            }

            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
        }

        private string ResolveDirectory(string relativePath)
        {
            string resolved = ResolvePath(relativePath);
            Directory.CreateDirectory(resolved);
            return resolved;
        }

        private List<PackagePart> GetWorksheetParts(Package package)
        {
            PackagePart workbookPart = null;
            foreach (PackageRelationship relationship in package.GetRelationshipsByType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument"))
            {
                workbookPart = package.GetPart(PackUriHelper.ResolvePartUri(new Uri("/", UriKind.Relative), relationship.TargetUri));
                break;
            }

            if (workbookPart == null)
            {
                throw new InvalidOperationException("The report template is not a valid Excel workbook.");
            }

            Dictionary<string, PackageRelationship> worksheetRelationships = new Dictionary<string, PackageRelationship>();
            foreach (PackageRelationship relationship in workbookPart.GetRelationshipsByType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet"))
            {
                worksheetRelationships[relationship.Id] = relationship;
            }

            XmlDocument document = new XmlDocument();
            document.PreserveWhitespace = true;
            using (Stream stream = workbookPart.GetStream(FileMode.Open, FileAccess.Read))
            {
                document.Load(stream);
            }

            XmlNamespaceManager namespaces = new XmlNamespaceManager(document.NameTable);
            namespaces.AddNamespace("x", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
            namespaces.AddNamespace("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");

            List<PackagePart> worksheets = new List<PackagePart>();
            XmlNodeList sheets = document.SelectNodes("/x:workbook/x:sheets/x:sheet", namespaces);
            foreach (XmlNode sheet in sheets)
            {
                XmlElement sheetElement = sheet as XmlElement;
                if (sheetElement == null)
                {
                    continue;
                }

                string relationshipId = sheetElement.GetAttribute("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
                PackageRelationship relationship;
                if (!worksheetRelationships.TryGetValue(relationshipId, out relationship))
                {
                    throw new InvalidOperationException("The report template contains a worksheet with an invalid relationship.");
                }

                Uri worksheetUri = PackUriHelper.ResolvePartUri(workbookPart.Uri, relationship.TargetUri);
                worksheets.Add(package.GetPart(worksheetUri));
            }

            return worksheets;
        }

        private void UpdateReportWorksheet(PackagePart worksheetPart, DataTable data, string reportName)
        {
            const int startRow = 8;

            XmlDocument document = LoadWorksheet(worksheetPart);
            XmlNamespaceManager namespaces = CreateWorksheetNamespaceManager(document);
            XmlElement sheetData = GetSheetData(document, namespaces);

            Dictionary<int, string> headerStyles = GetRowStyles(sheetData, startRow);
            Dictionary<int, string> dataStyles = GetRowStyles(sheetData, startRow + 1);
            string firstHeaderStyle = headerStyles.ContainsKey(1) ? headerStyles[1] : null;

            UpsertTextCell(document, sheetData, 1, 1, "Information Tech.", firstHeaderStyle);
            UpsertTextCell(document, sheetData, 2, 1, reportName, firstHeaderStyle);
            UpsertTextCell(document, sheetData, 4, 1, "Prepared by:", null);
            UpsertTextCell(document, sheetData, 5, 1, "_____________________________", null);
            UpsertTextCell(document, sheetData, 6, 1, "Signature", null);

            RemoveRowsFrom(sheetData, startRow);
            WriteDataRows(document, sheetData, data, startRow, headerStyles, dataStyles);
            ReplaceAutoFilter(document, namespaces, startRow, data.Columns.Count, data.Rows.Count);
            RemoveMergedCellsFrom(document, namespaces, startRow);
            UpdateDimension(document, namespaces, data.Columns.Count, startRow + data.Rows.Count);
            SaveWorksheet(worksheetPart, document);
        }

        private void UpdateChartWorksheet(Package package, PackagePart worksheetPart, DataTable data, string reportName)
        {
            XmlDocument document = LoadWorksheet(worksheetPart);
            XmlNamespaceManager namespaces = CreateWorksheetNamespaceManager(document);
            XmlElement sheetData = GetSheetData(document, namespaces);
            List<ChartPoint> chartPoints = BuildChartPoints(data);

            RemoveRowsFrom(sheetData, 1);
            UpsertTextCell(document, sheetData, 1, 1, reportName + " Graph", null);
            UpsertTextCell(document, sheetData, 3, 1, "Category", null);
            UpsertTextCell(document, sheetData, 3, 2, "Record Count", null);

            for (int i = 0; i < chartPoints.Count; i++)
            {
                UpsertTextCell(document, sheetData, 4 + i, 1, chartPoints[i].Category, null);
                UpsertNumberCell(document, sheetData, 4 + i, 2, chartPoints[i].Value, null);
            }

            int lastRow = Math.Max(4, 3 + chartPoints.Count);
            RemoveAutoFilters(document, namespaces);
            UpdateDimension(document, namespaces, 8, Math.Max(lastRow, 22));
            SaveWorksheet(worksheetPart, document);
            UpdateChartReferences(package, worksheetPart, reportName, lastRow);
        }

        private List<ChartPoint> BuildChartPoints(DataTable data)
        {
            int categoryColumn = GetPreferredCategoryColumn(data);
            Dictionary<string, double> counts = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);

            foreach (DataRow row in data.Rows)
            {
                string category = "Record";
                if (categoryColumn >= 0 && row[categoryColumn] != null && row[categoryColumn] != DBNull.Value)
                {
                    category = Convert.ToString(row[categoryColumn], CultureInfo.CurrentCulture);
                    if (string.IsNullOrWhiteSpace(category))
                    {
                        category = "Blank";
                    }
                }

                if (!counts.ContainsKey(category))
                {
                    counts[category] = 0;
                }

                counts[category]++;
            }

            List<ChartPoint> points = new List<ChartPoint>();
            foreach (KeyValuePair<string, double> count in counts)
            {
                points.Add(new ChartPoint(count.Key, count.Value));
            }

            if (points.Count == 0)
            {
                points.Add(new ChartPoint("No records", 0));
            }

            return points;
        }

        private int GetPreferredCategoryColumn(DataTable data)
        {
            string[] preferredNames = { "status", "remarks", "course_code", "term", "student_name", "student_no" };
            foreach (string preferredName in preferredNames)
            {
                if (data.Columns.Contains(preferredName))
                {
                    return data.Columns[preferredName].Ordinal;
                }
            }

            for (int col = 0; col < data.Columns.Count; col++)
            {
                if (data.Columns[col].DataType == typeof(string))
                {
                    return col;
                }
            }

            return data.Columns.Count > 0 ? 0 : -1;
        }

        private void UpdateChartReferences(Package package, PackagePart worksheetPart, string reportName, int lastRow)
        {
            foreach (PackageRelationship drawingRelationship in worksheetPart.GetRelationshipsByType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/drawing"))
            {
                PackagePart drawingPart = package.GetPart(PackUriHelper.ResolvePartUri(worksheetPart.Uri, drawingRelationship.TargetUri));
                foreach (PackageRelationship chartRelationship in drawingPart.GetRelationshipsByType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/chart"))
                {
                    PackagePart chartPart = package.GetPart(PackUriHelper.ResolvePartUri(drawingPart.Uri, chartRelationship.TargetUri));
                    UpdateChartPart(chartPart, reportName, lastRow);
                }
            }
        }

        private void UpdateChartPart(PackagePart chartPart, string reportName, int lastRow)
        {
            XmlDocument document = new XmlDocument();
            document.PreserveWhitespace = true;
            using (Stream stream = chartPart.GetStream(FileMode.Open, FileAccess.Read))
            {
                document.Load(stream);
            }

            XmlNamespaceManager namespaces = new XmlNamespaceManager(document.NameTable);
            namespaces.AddNamespace("c", "http://schemas.openxmlformats.org/drawingml/2006/chart");

            SetChartFormula(document, namespaces, "//c:cat//c:f", "'Graph'!$A$4:$A$" + lastRow.ToString(CultureInfo.InvariantCulture));
            SetChartFormula(document, namespaces, "//c:val//c:f", "'Graph'!$B$4:$B$" + lastRow.ToString(CultureInfo.InvariantCulture));

            XmlElement titleText = document.SelectSingleNode("//c:ser/c:tx/c:v", namespaces) as XmlElement;
            if (titleText != null)
            {
                titleText.InnerText = reportName;
            }

            using (Stream stream = chartPart.GetStream(FileMode.Create, FileAccess.Write))
            {
                document.Save(stream);
            }
        }

        private void SetChartFormula(XmlDocument document, XmlNamespaceManager namespaces, string xpath, string formula)
        {
            XmlElement element = document.SelectSingleNode(xpath, namespaces) as XmlElement;
            if (element != null)
            {
                element.InnerText = formula;
            }
        }

        private XmlDocument LoadWorksheet(PackagePart worksheetPart)
        {
            XmlDocument document = new XmlDocument();
            document.PreserveWhitespace = true;
            using (Stream stream = worksheetPart.GetStream(FileMode.Open, FileAccess.Read))
            {
                document.Load(stream);
            }

            return document;
        }

        private void SaveWorksheet(PackagePart worksheetPart, XmlDocument document)
        {
            using (Stream stream = worksheetPart.GetStream(FileMode.Create, FileAccess.Write))
            {
                document.Save(stream);
            }
        }

        private XmlNamespaceManager CreateWorksheetNamespaceManager(XmlDocument document)
        {
            XmlNamespaceManager namespaces = new XmlNamespaceManager(document.NameTable);
            namespaces.AddNamespace("x", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
            return namespaces;
        }

        private XmlElement GetSheetData(XmlDocument document, XmlNamespaceManager namespaces)
        {
            XmlElement sheetData = document.SelectSingleNode("/x:worksheet/x:sheetData", namespaces) as XmlElement;
            if (sheetData == null)
            {
                sheetData = document.CreateElement("sheetData", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
                document.DocumentElement.AppendChild(sheetData);
            }

            return sheetData;
        }

        private Dictionary<int, string> GetRowStyles(XmlElement sheetData, int rowNumber)
        {
            Dictionary<int, string> styles = new Dictionary<int, string>();
            XmlElement row = FindRow(sheetData, rowNumber);
            if (row == null)
            {
                return styles;
            }

            foreach (XmlNode child in row.ChildNodes)
            {
                XmlElement cell = child as XmlElement;
                if (cell == null || cell.LocalName != "c")
                {
                    continue;
                }

                int column = GetColumnIndex(cell.GetAttribute("r"));
                string style = cell.GetAttribute("s");
                if (column > 0 && !string.IsNullOrWhiteSpace(style))
                {
                    styles[column] = style;
                }
            }

            return styles;
        }

        private void RemoveRowsFrom(XmlElement sheetData, int startRow)
        {
            List<XmlNode> nodesToRemove = new List<XmlNode>();
            foreach (XmlNode child in sheetData.ChildNodes)
            {
                XmlElement row = child as XmlElement;
                if (row == null || row.LocalName != "row")
                {
                    continue;
                }

                int rowNumber;
                if (int.TryParse(row.GetAttribute("r"), out rowNumber) && rowNumber >= startRow)
                {
                    nodesToRemove.Add(row);
                }
            }

            foreach (XmlNode node in nodesToRemove)
            {
                sheetData.RemoveChild(node);
            }
        }

        private void WriteDataRows(XmlDocument document, XmlElement sheetData, DataTable data, int startRow, Dictionary<int, string> headerStyles, Dictionary<int, string> dataStyles)
        {
            for (int col = 0; col < data.Columns.Count; col++)
            {
                UpsertTextCell(document, sheetData, startRow, col + 1, data.Columns[col].ColumnName, GetStyle(headerStyles, col + 1));
            }

            for (int row = 0; row < data.Rows.Count; row++)
            {
                for (int col = 0; col < data.Columns.Count; col++)
                {
                    UpsertValueCell(document, sheetData, startRow + 1 + row, col + 1, data.Rows[row][col], GetStyle(dataStyles, col + 1));
                }
            }
        }

        private string GetStyle(Dictionary<int, string> styles, int column)
        {
            string style;
            if (styles.TryGetValue(column, out style))
            {
                return style;
            }

            if (styles.TryGetValue(1, out style))
            {
                return style;
            }

            return null;
        }

        private void UpsertValueCell(XmlDocument document, XmlElement sheetData, int row, int col, object value, string style)
        {
            if (value == null || value == DBNull.Value)
            {
                UpsertTextCell(document, sheetData, row, col, string.Empty, style);
                return;
            }

            Type type = value.GetType();
            if (type == typeof(byte) || type == typeof(short) || type == typeof(int) || type == typeof(long) ||
                type == typeof(float) || type == typeof(double) || type == typeof(decimal))
            {
                UpsertNumberCell(document, sheetData, row, col, Convert.ToDouble(value), style);
                return;
            }

            if (type == typeof(DateTime))
            {
                UpsertTextCell(document, sheetData, row, col, ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CurrentCulture), style);
                return;
            }

            UpsertTextCell(document, sheetData, row, col, Convert.ToString(value, CultureInfo.CurrentCulture), style);
        }

        private void UpsertTextCell(XmlDocument document, XmlElement sheetData, int row, int col, string value, string style)
        {
            XmlElement cell = GetOrCreateCell(document, sheetData, row, col);
            cell.SetAttribute("t", "inlineStr");
            SetStyle(cell, style);
            cell.RemoveAll();
            cell.SetAttribute("r", GetCellReference(row, col));
            cell.SetAttribute("t", "inlineStr");
            SetStyle(cell, style);

            XmlElement inlineString = document.CreateElement("is", document.DocumentElement.NamespaceURI);
            XmlElement text = document.CreateElement("t", document.DocumentElement.NamespaceURI);
            text.InnerText = value ?? string.Empty;
            inlineString.AppendChild(text);
            cell.AppendChild(inlineString);
        }

        private void UpsertNumberCell(XmlDocument document, XmlElement sheetData, int row, int col, double value, string style)
        {
            XmlElement cell = GetOrCreateCell(document, sheetData, row, col);
            cell.RemoveAll();
            cell.SetAttribute("r", GetCellReference(row, col));
            SetStyle(cell, style);

            XmlElement cellValue = document.CreateElement("v", document.DocumentElement.NamespaceURI);
            cellValue.InnerText = value.ToString(CultureInfo.InvariantCulture);
            cell.AppendChild(cellValue);
        }

        private void SetStyle(XmlElement cell, string style)
        {
            if (!string.IsNullOrWhiteSpace(style))
            {
                cell.SetAttribute("s", style);
            }
            else
            {
                cell.RemoveAttribute("s");
            }
        }

        private XmlElement GetOrCreateCell(XmlDocument document, XmlElement sheetData, int rowNumber, int columnNumber)
        {
            XmlElement row = GetOrCreateRow(document, sheetData, rowNumber);
            XmlElement existingCell = FindCell(row, columnNumber);
            if (existingCell != null)
            {
                return existingCell;
            }

            XmlElement cell = document.CreateElement("c", document.DocumentElement.NamespaceURI);
            cell.SetAttribute("r", GetCellReference(rowNumber, columnNumber));

            XmlNode insertBefore = null;
            foreach (XmlNode child in row.ChildNodes)
            {
                XmlElement currentCell = child as XmlElement;
                if (currentCell != null && currentCell.LocalName == "c" && GetColumnIndex(currentCell.GetAttribute("r")) > columnNumber)
                {
                    insertBefore = currentCell;
                    break;
                }
            }

            if (insertBefore == null)
            {
                row.AppendChild(cell);
            }
            else
            {
                row.InsertBefore(cell, insertBefore);
            }

            return cell;
        }

        private XmlElement GetOrCreateRow(XmlDocument document, XmlElement sheetData, int rowNumber)
        {
            XmlElement existingRow = FindRow(sheetData, rowNumber);
            if (existingRow != null)
            {
                return existingRow;
            }

            XmlElement row = document.CreateElement("row", document.DocumentElement.NamespaceURI);
            row.SetAttribute("r", rowNumber.ToString(CultureInfo.InvariantCulture));

            XmlNode insertBefore = null;
            foreach (XmlNode child in sheetData.ChildNodes)
            {
                XmlElement currentRow = child as XmlElement;
                int currentRowNumber;
                if (currentRow != null && currentRow.LocalName == "row" &&
                    int.TryParse(currentRow.GetAttribute("r"), out currentRowNumber) && currentRowNumber > rowNumber)
                {
                    insertBefore = currentRow;
                    break;
                }
            }

            if (insertBefore == null)
            {
                sheetData.AppendChild(row);
            }
            else
            {
                sheetData.InsertBefore(row, insertBefore);
            }

            return row;
        }

        private XmlElement FindRow(XmlElement sheetData, int rowNumber)
        {
            foreach (XmlNode child in sheetData.ChildNodes)
            {
                XmlElement row = child as XmlElement;
                int currentRowNumber;
                if (row != null && row.LocalName == "row" &&
                    int.TryParse(row.GetAttribute("r"), out currentRowNumber) && currentRowNumber == rowNumber)
                {
                    return row;
                }
            }

            return null;
        }

        private XmlElement FindCell(XmlElement row, int columnNumber)
        {
            foreach (XmlNode child in row.ChildNodes)
            {
                XmlElement cell = child as XmlElement;
                if (cell != null && cell.LocalName == "c" && GetColumnIndex(cell.GetAttribute("r")) == columnNumber)
                {
                    return cell;
                }
            }

            return null;
        }

        private void ReplaceAutoFilter(XmlDocument document, XmlNamespaceManager namespaces, int headerRow, int columnCount, int rowCount)
        {
            RemoveAutoFilters(document, namespaces);
            if (columnCount == 0)
            {
                return;
            }

            XmlElement autoFilter = document.CreateElement("autoFilter", document.DocumentElement.NamespaceURI);
            autoFilter.SetAttribute("ref", "A" + headerRow.ToString(CultureInfo.InvariantCulture) + ":" + GetColumnName(columnCount) + (headerRow + rowCount).ToString(CultureInfo.InvariantCulture));
            XmlNode sheetData = document.SelectSingleNode("/x:worksheet/x:sheetData", namespaces);
            document.DocumentElement.InsertAfter(autoFilter, sheetData);
        }

        private void RemoveAutoFilters(XmlDocument document, XmlNamespaceManager namespaces)
        {
            XmlNodeList filters = document.SelectNodes("/x:worksheet/x:autoFilter", namespaces);
            foreach (XmlNode filter in filters)
            {
                filter.ParentNode.RemoveChild(filter);
            }
        }

        private void RemoveMergedCellsFrom(XmlDocument document, XmlNamespaceManager namespaces, int startRow)
        {
            XmlElement mergedCells = document.SelectSingleNode("/x:worksheet/x:mergeCells", namespaces) as XmlElement;
            if (mergedCells == null)
            {
                return;
            }

            List<XmlNode> nodesToRemove = new List<XmlNode>();
            foreach (XmlNode child in mergedCells.ChildNodes)
            {
                XmlElement mergeCell = child as XmlElement;
                if (mergeCell == null || mergeCell.LocalName != "mergeCell")
                {
                    continue;
                }

                if (RangeTouchesRowOrBelow(mergeCell.GetAttribute("ref"), startRow))
                {
                    nodesToRemove.Add(mergeCell);
                }
            }

            foreach (XmlNode node in nodesToRemove)
            {
                mergedCells.RemoveChild(node);
            }

            if (mergedCells.ChildNodes.Count == 0)
            {
                mergedCells.ParentNode.RemoveChild(mergedCells);
            }
            else
            {
                mergedCells.SetAttribute("count", mergedCells.ChildNodes.Count.ToString(CultureInfo.InvariantCulture));
            }
        }

        private bool RangeTouchesRowOrBelow(string range, int startRow)
        {
            if (string.IsNullOrWhiteSpace(range))
            {
                return false;
            }

            string[] references = range.Split(':');
            foreach (string reference in references)
            {
                if (GetRowIndex(reference) >= startRow)
                {
                    return true;
                }
            }

            return false;
        }

        private void UpdateDimension(XmlDocument document, XmlNamespaceManager namespaces, int columnCount, int lastRow)
        {
            XmlElement dimension = document.SelectSingleNode("/x:worksheet/x:dimension", namespaces) as XmlElement;
            if (dimension == null)
            {
                dimension = document.CreateElement("dimension", document.DocumentElement.NamespaceURI);
                document.DocumentElement.PrependChild(dimension);
            }

            int safeColumnCount = Math.Max(1, columnCount);
            int safeLastRow = Math.Max(1, lastRow);
            dimension.SetAttribute("ref", "A1:" + GetColumnName(safeColumnCount) + safeLastRow.ToString(CultureInfo.InvariantCulture));
        }

        private int GetColumnIndex(string cellReference)
        {
            if (string.IsNullOrWhiteSpace(cellReference))
            {
                return 0;
            }

            int column = 0;
            foreach (char character in cellReference)
            {
                if (!char.IsLetter(character))
                {
                    break;
                }

                column = (column * 26) + (char.ToUpperInvariant(character) - 'A' + 1);
            }

            return column;
        }

        private int GetRowIndex(string cellReference)
        {
            if (string.IsNullOrWhiteSpace(cellReference))
            {
                return 0;
            }

            string digits = string.Empty;
            foreach (char character in cellReference)
            {
                if (char.IsDigit(character))
                {
                    digits += character;
                }
            }

            int row;
            return int.TryParse(digits, out row) ? row : 0;
        }

        private string GetCellReference(int row, int col)
        {
            return GetColumnName(col) + row.ToString(CultureInfo.InvariantCulture);
        }

        private string GetColumnName(int col)
        {
            string name = string.Empty;
            while (col > 0)
            {
                int remainder = (col - 1) % 26;
                name = Convert.ToChar('A' + remainder) + name;
                col = (col - remainder - 1) / 26;
            }

            return name;
        }

        private class ChartPoint
        {
            public ChartPoint(string category, double value)
            {
                Category = category;
                Value = value;
            }

            public string Category { get; private set; }
            public double Value { get; private set; }
        }
    }
}
