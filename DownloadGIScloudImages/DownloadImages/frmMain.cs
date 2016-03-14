using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net;
using System.Collections.Specialized; // NameValueCollection
using System.IO;             
using OfficeOpenXml;         // EEPlus
using OfficeOpenXml.Style;   // EEPlus
using Microsoft.Win32;  // This is for the registry
using System.Threading; // For the progress bar

namespace DownloadImages
{
    public partial class frmMain : Form
    {

        // API Key " 30c8e7ae25d5e6fa853d96f33d8978ef "
        private string strFilePath = "";

        public frmMain()
        {
            InitializeComponent();
            // Set the last used value for the "multi select mode" checkbox
            try
            {
                string value = (string)Registry.GetValue(@"HKEY_CURRENT_USER\Software\QCData\DowloadGIScloudImages", "OpenExplorer", "Checked");

                if (value == "Checked")
                {
                    chkExplorer.CheckState = CheckState.Checked;
                }
                else
                {
                    chkExplorer.CheckState = CheckState.Unchecked;
                }
            }
            catch
            { /*This is just here for the first time the tool is run on a worstation. */}
        }

        /*************************************************************************************/

        private void button2_Click(object sender, EventArgs e)
        {
            int iCounter = 0;
            bool bHasLocation = true;

            // Show the dialog and get result.
            openFileDialog1.Filter = "Excel (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                // Test result.
                //String File = openFileDialog1.FileName;
                //MessageBox.Show(File);
            }
            else
            {
                return;
            }

            FileInfo FEIfile = new FileInfo(openFileDialog1.FileName);
            if (Path.GetExtension(openFileDialog1.FileName).ToUpper() != ".XLSX")
            {
                MessageBox.Show("This tool only processes .xlsx files.");
                return;
            }

            strFilePath = Path.GetDirectoryName(openFileDialog1.FileName);

            progressBar1.Style = ProgressBarStyle.Continuous;
            progressBar1.Value = 0;

            /* Open and read the XlSX file. */

            try
            {
                using (ExcelPackage package = new ExcelPackage(FEIfile))
                {

                    button2.Enabled = false;

                    /* Get the work book in the file */

                    ExcelWorkbook workBook = package.Workbook;
                    if (workBook != null)
                    {
                        if (workBook.Worksheets.Count > 0)
                        {

                            /* Get the first worksheet */

                            //ExcelWorksheet Worksheet = workBook.Worksheets.First();
                            var worksheet = package.Workbook.Worksheets[1];

                            /* Do some checks to make sure the speadsheet format is what we expect */

                            string strCheck = worksheet.Cells["F1"].Value == null ? string.Empty : worksheet.Cells["F1"].Value.ToString();
                            if (strCheck.ToUpper() != "PHOTOLOCATION")
                            {
                                MessageBox.Show("Expected worksheet format - Cell F1 should have the value 'photolocation'. Aborting.");
                                return;
                            }

                            strCheck = worksheet.Cells["G1"].Value == null ? string.Empty : worksheet.Cells["G1"].Value.ToString();
                            if (strCheck.ToUpper() != "PICTURES")
                            {
                                MessageBox.Show("Expected worksheet format - Cell F1 should have the value 'pictures'. Aborting.");
                                return;
                            }
                            
                            strCheck = worksheet.Cells["L1"].Value == null ? string.Empty : worksheet.Cells["L1"].Value.ToString();
                            if (strCheck.ToUpper() != "LAT")
                            {
                                MessageBox.Show("Expected worksheet format - Cell L1 should have the value 'lat'. Aborting.");
                                return;
                            }
                            
                            strCheck = worksheet.Cells["M1"].Value == null ? string.Empty : worksheet.Cells["M1"].Value.ToString();
                            if (strCheck.ToUpper() != "LONG")
                            {
                                MessageBox.Show("Expected worksheet format - Cell M1 should have the value 'long'. Aborting.");
                                return;
                            }

                            /* Find the "real" last used row. */

                            var rowRun = worksheet.Dimension.End.Row;
                            while (rowRun >= 1)
                            {
                                var range = worksheet.Cells[rowRun, 1, rowRun, worksheet.Dimension.End.Column];
                                if (range.Any(c => !string.IsNullOrEmpty(c.Text)))
                                {
                                    break;
                                }
                                rowRun--;
                            }

                            progressBar1.Maximum = rowRun;
                            
                            /* Create the KML file for the users to see where the pole locations are. */

                            /* If the file already exists, get rid of it. Otherwise it would be appended to. */
                            if (File.Exists(strFilePath + @"\Pole_Locations.kml"))
                            {
                                File.Delete(strFilePath + @"\Pole_Locations.kml");
                            }
                            using (StreamWriter writer = new StreamWriter(strFilePath + @"\Pole_Locations.kml", true))
                            {

                                /* First we add the header */
                                writer.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                                writer.WriteLine("<kml xmlns='http://www.opengis.net/kml/2.2'>");
                                writer.WriteLine("<Document>");
                                writer.WriteLine("<name>Pole_Locations</name>");

                                /* Loop through the worksheet and download the files. */

                                for (int row = 2; row <= rowRun; row++)
                                {

                                    progressBar1.Value = row;

                                    /* See if there is a location name/number */

                                    string strLocation = "";
                                    strLocation = worksheet.Cells["F" + row.ToString()].Value == null ? string.Empty : worksheet.Cells["F" + row.ToString()].Value.ToString();
                                    if (strLocation != "")
                                    {
                                        bHasLocation = true;
                                        strLocation = "Location" + strLocation;
                                    }
                                    else
                                    {
                                        bHasLocation = false;
                                        strLocation = "Row" + row.ToString();
                                    }

                                    /* Get the names of the images */

                                    string strTestValue = "";
                                    strTestValue = worksheet.Cells["G" + row.ToString()].Value == null ? string.Empty : worksheet.Cells["G" + row.ToString()].Value.ToString();
                                    if (strTestValue != "")
                                    {

                                        /* Loop through the images and save each one out to the location where the spreadsheet was selecteed */

                                        String[] pictureNames = strTestValue.Split(',');
                                        iCounter = 0;
                                        foreach (string pic in pictureNames)
                                        {
                                            iCounter = iCounter + 1;

                                            /* Create the HTTP GET Request*/

                                            string getVars = "?api_key=30c8e7ae25d5e6fa853d96f33d8978ef";
                                            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(string.Format("https://api.giscloud.com/1/storage/fs/uploads/" + pic + "{0}", getVars));
                                            WebReq.Method = "GET";
                                            HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();

                                            /* Save the image */

                                            try
                                            {
                                                Image img = null;
                                                using (Stream str = WebResp.GetResponseStream())
                                                {
                                                    img = Image.FromStream(str);
                                                }
                                                img.Save(strFilePath + "\\" + strLocation + "_" + iCounter.ToString() + ".jpg");

                                                /* List the image in the appropriate list box*/
                                                if (bHasLocation == true)
                                                {
                                                    lstLocation.Items.Add(strLocation + "_" + iCounter.ToString() + ".jpg");
                                                }
                                                else
                                                {
                                                    lstNoLocation.Items.Add(strLocation + "_" + iCounter.ToString() + ".jpg");
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                MessageBox.Show(ex.ToString());
                                            }
                                            WebResp.Close();
                                        }
                                        lstLocation.Refresh();
                                        lstNoLocation.Refresh();
                                    }
                                    else
                                    {
                                        if (strLocation.StartsWith("Row") == false)
                                        {
                                            lstNoPic.Items.Add(strLocation);
                                        }
                                    }
                                    lstNoPic.Refresh();

                                    /* Get the lat/long for the current pole */

                                    string strLat = worksheet.Cells["L" + row.ToString()].Value == null ? string.Empty : worksheet.Cells["L" + row.ToString()].Value.ToString();
                                    if (strLat == "")
                                    {
                                        MessageBox.Show("Missing Lat value at row " + row.ToString());
                                    }

                                    string strLong = worksheet.Cells["M" + row.ToString()].Value == null ? string.Empty : worksheet.Cells["M" + row.ToString()].Value.ToString();
                                    if (strLong == "")
                                    {
                                        MessageBox.Show("Missing Long value at row " + row.ToString());
                                    }

                                    writer.WriteLine("<Placemark>");
                                    writer.WriteLine("<name>" + strLocation + "</name>");
                                    writer.WriteLine("<description></description>");
                                    writer.WriteLine("<address></address>");
                                    writer.WriteLine("<Point>");
                                    writer.WriteLine("<coordinates>" + strLong + "," + strLat + ",0</coordinates>");
                                    writer.WriteLine("</Point>");
                                    writer.WriteLine("<Style>");
                                    writer.WriteLine("<IconStyle>");
                                    writer.WriteLine("<Icon>");
                                    //writer.WriteLine("<href>http://maps.google.com/mapfiles/kml/shapes/placemark_circle.png</href>");
                                    writer.WriteLine("<href>https://www.qcdata.s3.amazonaws.com/img/earthit/house2.png</href>");
                                    writer.WriteLine("</Icon>");
                                    writer.WriteLine("</IconStyle>");
                                    writer.WriteLine("</Style>");
                                    writer.WriteLine("</Placemark>");

                                }

                                /* Write the footer */
                                writer.WriteLine("</Document>");
                                writer.WriteLine("</kml>");

                            }
                        }
                    }
                    package.Dispose();
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("Error opening spreadsheet. Is it already open? Close it and try again.");
                return;
            }

            /* Log the results so we have the info after the tool is closed */

            try
            {
                using (StreamWriter sw = File.CreateText(strFilePath + "\\Download.log"))
                {
                    sw.WriteLine("Has Location");
                    sw.WriteLine("------------");
                    foreach (var item in lstLocation.Items)
                    {
                        sw.WriteLine(item.ToString());
                    }
                    sw.WriteLine("");
                    sw.WriteLine("No Location");
                    sw.WriteLine("------------");
                    foreach (var item in lstNoLocation.Items)
                    {
                        sw.WriteLine(item.ToString());
                    }
                    sw.WriteLine("");
                    sw.WriteLine("Location No Pics");
                    sw.WriteLine("----------------");
                    foreach (var item in lstNoPic.Items)
                    {
                        sw.WriteLine(item.ToString());
                    }
                }
            }catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            /* If there were any no locations, open the spreadsheet for the user to review */

            if (lstNoLocation.Items.Count != 0)
            {
                System.Diagnostics.Process.Start(openFileDialog1.FileName);
            }

            if (chkExplorer.Checked == true)
            {
                System.Diagnostics.Process.Start("explorer.exe", strFilePath);
            }

            ProcessStartInfo startInfo = new ProcessStartInfo();
            if (File.Exists(@"C:\Program Files (x86)\GDAL\ogr2ogr.exe") == true)
            {
                startInfo.FileName = @"C:\Program Files (x86)\GDAL\ogr2ogr.exe";
            }
            else if (File.Exists(@"C:\Program Files\GDAL\ogr2ogr.exe") == true)
            {
                startInfo.FileName = @"C:\Program Files\GDAL\ogr2ogr.exe";
            }
            else
            {
                MessageBox.Show("Supporting software (GDAL) not found on this computer for converting KML to SHP.");
                goto BeDone;
            }
            startInfo.Arguments = @"-f ""ESRI Shapefile"" " + @"""" + strFilePath + "\\Pole_Locations.shp" + @"""" + " " + @"""" + strFilePath + "\\Pole_Locations.kml" + @"""";
            Process.Start(startInfo);

        BeDone:

            MessageBox.Show("Image download complete!");

        }

        /**********************************************************************************/

        private void cmdExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /**********************************************************************************/

        private void lstLocation_DoubleClick(object sender, EventArgs e)
        {
            if (lstLocation.Items.Count == 0)
            {
                return;
            }

            string text = lstLocation.GetItemText(lstLocation.SelectedItem);
            System.Diagnostics.Process.Start(strFilePath + "\\" + text);
        }

        /**********************************************************************************/

        private void lstNoLocation_DoubleClick(object sender, EventArgs e)
        {
            if (lstNoLocation.Items.Count == 0)
            {
                return;
            }

            string text = lstNoLocation.GetItemText(lstNoLocation.SelectedItem);
            System.Diagnostics.Process.Start(strFilePath + "\\" + text);
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }
        
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 1; i <= 100; i++)
            {
                // Wait 100 milliseconds.
                Thread.Sleep(100);
                // Report progress.
                backgroundWorker1.ReportProgress(i);
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // Change the value of the ProgressBar to the BackgroundWorker progress.
            progressBar1.Value = e.ProgressPercentage;
            // Set the text.
            this.Text = e.ProgressPercentage.ToString();
        }

        private void chkExplorer_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkExplorer_Click(object sender, EventArgs e)
        {
            if (chkExplorer.Checked == false)
            {
                RegistryKey key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"Software\QCData\DowloadGIScloudImages");
                key.SetValue("OpenExplorer", chkExplorer.CheckState);
                key.Close();
            }
            else
            {
                RegistryKey key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"Software\QCData\DowloadGIScloudImages");
                key.SetValue("OpenExplorer", chkExplorer.CheckState);
                key.Close();
            }
        }

        private void frmMain_Load_1(object sender, EventArgs e)
        {

        }

    }
}
