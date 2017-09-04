using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections;
using System.IO;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Asg2_dds160030
{


    public partial class RebateForm : Form
    {
        //FilePath of the data file
        const string FilePath = "CS6326Asg2.txt";

        //declaration for sorting 
        private ListViewSorter lvwColumnSorter;

        //Record of a person
        private MyObject record;

        //Previous record read from data file
        private MyObject readrecord;

       //List of MyObjects
        List<MyObject> myobjects = new List<MyObject>();

        //start time to calculate time to save data
        DateTime startTime;


        public RebateForm()

        {
            //Set the state of Window to Maximized, User can Minimize
            this.WindowState = FormWindowState.Maximized;

            InitializeComponent();

            //Create instance of class ListViewSorter for Column Sort
            lvwColumnSorter = new ListViewSorter();

            //Create instance of column header
            ColumnHeader columnheader = new ColumnHeader();

            // Size the columns with respect to data in them, Can be done in Form Load Event but refresh frequency is too much
            foreach (ColumnHeader ch in this.listNames.Columns)
            {
                ch.Width = -2;
            }

            //Let user move columns around
            this.listNames.AllowColumnReorder = true;

            //Use custom sorter to sort the listView
            this.listNames.ListViewItemSorter = lvwColumnSorter;

            //Creates a file, if not present. Otherwise, nothing.
            StreamWriter w = File.AppendText(FilePath);
            w.Close();

            //Read all lines from File to check if any data is there in text file and store in array records.
            string[] records = System.IO.File.ReadAllLines(@FilePath);

            //Parse the lines into their individual fields.
            //For example, the part associated with FirstName goes to MyObject.FirstName
            foreach (string x in records)
            {
                string[] data = x.Split('\t');
                readrecord = new MyObject();
                readrecord.MyObjectID = data[0] + " " + data[2] + " " + data[1];
                readrecord.FirstName = data[0];
                readrecord.Minit = data[2];
                readrecord.LastName = data[1];
                readrecord.Add1 = data[3];
                readrecord.Add2 = data[4];
                readrecord.City = data[5];
                readrecord.State = data[6];
                readrecord.ZipCode = data[7];
                readrecord.Phone = data[8];
                readrecord.Email = data[9];
                if (data[10] == "True")
                    readrecord.ProofofPurchase = true;
                else
                    readrecord.ProofofPurchase = false;
                readrecord.Date = data[11];
                readrecord.startTime = data[12];
                readrecord.endTime = data[13];

                //Add these fields to myobjects array of MyObject
                myobjects.Add(readrecord);

                //Print the item in current listView
                string Phone = readrecord.Phone;
                string Name = readrecord.FirstName + " " + readrecord.Minit + " " + readrecord.LastName;

                //Adds new listview item for previous records
                ListViewItem item = new ListViewItem(Name);
                item.SubItems.Add(Phone);
                listNames.Items.Add(item);
            }



        }
        //Write method to the text file
        //param List of MyObjects
        //Writes all the records in myobjects depending on how the ToString() method of MyObject class is defined
        public void writetotextClear(List<MyObject> ItemArray)
        {
            using (StreamWriter sw = new StreamWriter(FilePath))
            {
                foreach (MyObject x in ItemArray)
                {
                    //calls ToString of MyObject
                    sw.WriteLine(x.ToString());
                }
                sw.Close();
            }

        }

        //String array of states of U.S.A.
        string[] states = { "AL", "AK", "AS", "AZ", "AR", "CA", "CO", "CT", "DE", "DC", "FL", "GA", "GU", "HI", "ID", "IL", "IN", "IA", "KS", "KY", "LA", "ME", "MD", "MH", "MA", "MI", "FM", "MN", "MS", "MO", "MT", "NE", "NV", "NH", "NJ", "NM", "NY", "NC", "ND", "MP", "OH", "OK", "OR", "PW", "PA", "PR", "RI", "SC", "SD", "TN", "TX", "UT", "VT", "VA", "VI", "WA", "WV", "WI", "WY" };

        //Check if the State entered is Valid
        //param state string
        public bool IsValidState(string state)
        {
            bool checkstate = false;

            for (int i = 0; i < states.Length; i++)
            {
                //Compare Strings state states[i] ignoring Case
                //true if equal
                if (string.Equals(state, states[i], StringComparison.OrdinalIgnoreCase) == true)
                {
                    checkstate = true;
                }
            }

            return checkstate;

        }

        //Check if Number-Only String fast
        //param string
        public bool IsDigitsOnly(string num)
        {
            
            foreach (char c in num)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }


        //Check the Name is Valid
        //param string
        //I initially had no numbers, but in light of Assignment 3, I have added numbers as acceptable input
        // "." and " " is for names such as Martin Luther King JR.
        public bool IsValidName(string Name)
        {
            //check if A-Z, 0-9, "." , " "
            //I initially had no numbers, but in light of Assignment 3, I have added numbers as acceptable input
            bool namecheck = Regex.IsMatch(Name, @"^[ a-zA-Z0-9.-]+$");
            return namecheck;

        }

        //Check if Address is Valid
        //param address string
        //Allows characters, A-Z and numbers and spaces and "/" (Addresses like 43/10 XYZ Road) , "." (When abbreviating like st.)
        public bool IsValidAddress(string Address)
        {
            bool namecheck = Regex.IsMatch(Address, @"^[ a-zA-Z0-9,./]+$");
            return namecheck;

        }

        //Check if valid email address
        //param string
        //allow of type abc@d.fgh
        public static bool IsValidEmailAddress(string email)
        {
            bool emailcheck = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
            return emailcheck;
        }

        //Check if Save is creating a Duplicate on Save
        //param List of MyObjects
        public bool IsDuplicateSave(List<MyObject> myobjects)
        {
            string Name = ctlFirstName.Text + " " + ctlMinit.Text + " " + ctlLastName.Text;
            string Phone = ctlPhone.Text;
            bool duplicatecheck = false;
            for (int j = 0; j < myobjects.Count; j++)
            {
                if ((string.Equals(Name, myobjects[j].MyObjectID, StringComparison.OrdinalIgnoreCase) == true) && (string.Equals(Phone, myobjects[j].Phone, StringComparison.OrdinalIgnoreCase) == true))
                {
                    duplicatecheck = true;
                }
            }
            return duplicatecheck;
        }

        //If the user has entered a Name and Phone number already in the list, we allow him or her to update
        //Check if this is true
        //Check if update should be allowed
        //Sometimes a clerk may not want to select/find the item from Listview if it is too big
        //S/He can just type the Full name in the respective FirstName, LastName and Minit fields
        //We will find if this record exists and the data entered by the clerk is now new data
        //To be sure that clerk knows this, we will display, Duplicate Entry - Cannot Save/Can Update

        public bool IsDuplicateUpdate()
       {
            bool allowUpdate = false;
            foreach (MyObject x in myobjects)
            {
                if (ctlFirstName.Text + " " + ctlMinit.Text + " " + ctlLastName.Text == x.MyObjectID && ctlPhone.Text == x.Phone)
                {
                    allowUpdate = true;
                }
            }
                return allowUpdate;
       }
     

        //General Form Load Event, text modified event
        //checks for valid inputs
        private void ctlFirstName_TextChanged(object sender, EventArgs e)
        {
            //General Message
            StatusBar.Refresh();
            stripstatus.Text = "Waiting for user's input";

            //Check First name
            //returns false if invalid name
            bool namecheck = IsValidName(ctlFirstName.Text);
            if (namecheck != true && ctlFirstName.TextLength > 0)
            {
                StatusBar.Refresh();
                stripstatus.Text = "Invalid First Name";
            }

            //Check Middle Initial
            //returns false if invalid
            bool namecheck1 = IsValidName(ctlMinit.Text);
            if (namecheck1 != true && ctlMinit.TextLength > 0)
            {
                StatusBar.Refresh();
                stripstatus.Text = "Invalid Middle Name";
            }

            //Check Last Name
            //returns false if invalid

            bool namecheck2 = IsValidName(ctlLastName.Text);
            if (namecheck2 != true && ctlLastName.TextLength > 0)
            {
                StatusBar.Refresh();
                stripstatus.Text = "Invalid Last Name";
            }

            //Check valid Addresses for Address Lines 1 and 2
            //param text fields of Address Lines 1 and 2
            //returns false if Invalid address
            bool Addresscheck = IsValidAddress(ctlAdd1.Text);
            bool Addresscheck1 = IsValidAddress(ctlAdd2.Text);
            if ((Addresscheck != true || Addresscheck1 != true) && (ctlAdd1.TextLength > 0 && ctlAdd2.TextLength > 0))
            {
                StatusBar.Refresh();
                stripstatus.Text = "Invalid Address";
            }

            //Check valid City 
            //param text fields of City
            //returns false if Invalid city
            bool citycheck = IsValidName(ctlCity.Text);
            if (citycheck != true && ctlCity.TextLength > 0)
            {
                StatusBar.Refresh();
                stripstatus.Text = "Invalid City";
            }

            //Check if State entered is Valid State of USA
            bool statecheck = IsValidState(ctlState.Text);

            if (statecheck == false && ctlState.TextLength > 0)
            {
                btnSave.Enabled = false;
                btnUpdate.Enabled = false;
                StatusBar.Refresh();
                stripstatus.Text = "Invalid State";
            }


            //Check valid Zip Code 
            //param text fields of ZipCode
            //returns false if Invalid ZipCode
            //Zipcode must also be either 5 digits or 9 digits as per US Standard
            bool Zipcodecheck = IsDigitsOnly(ctlZipCode.Text) && (ctlZipCode.TextLength == 5 || ctlZipCode.TextLength == 9);
            if (Zipcodecheck != true && ctlZipCode.TextLength > 0)
            {
                StatusBar.Refresh();
                stripstatus.Text = "Invalid ZipCode";
            }

            //Check if valid Phone number
            //Allows 0-9 and "+" for international numbers
            bool Phonecheck = Regex.IsMatch(ctlPhone.Text, @"^[0-9\+]+$");
            if (Phonecheck != true && ctlPhone.TextLength > 0)
            {
                StatusBar.Refresh();
                stripstatus.Text = "Invalid Phone";

            }


            //Check valid email by calling IsValidEmailAddress
            //param text field of Email 
            //returns true if valid
            bool emailcheck = IsValidEmailAddress(ctlEmail.Text);
            if (emailcheck == false && ctlEmail.TextLength > 0)
            {
                btnSave.Enabled = false;
                btnUpdate.Enabled = false;
                StatusBar.Refresh();
                stripstatus.Text = "Invalid email address";
            }


            //Check if record is there in list before
            //If there, return true
            //param list of MyObjects
            bool duplicatecheckSave = IsDuplicateSave(this.myobjects);

            //Check if update should be allowed
            //Sometimes a clerk may not want to select/find the item from Listview if it is too big
            //S/He can just type the Full name in the respective FirstName, LastName and Minit fields
            //We will find if this record exists and the data entered by the clerk is now new data
            //To be sure that clerk knows this, we will display, Duplicate Entry - Cannot Save/Can Update
            bool allowUpdate = IsDuplicateUpdate();
            if (allowUpdate != true )
            {
                btnUpdate.Enabled = false;
            }
            if (duplicatecheckSave == true && allowUpdate == true)
            {
                btnSave.Enabled = false;
                StatusBar.Refresh();
                stripstatus.Text = "Duplicate Person - Cannot Save/Can Update";
            }

            //Delete button should be disabled if nothing is Selected!
            if (listNames.SelectedItems.Count == 0)
            {
             btnDelete.Enabled = false;

            }
            else
            {
                btnDelete.Enabled = true;
            }



            //Update button should be enabled 
            //if changes are made
            //if person entered a person who was in list before and made changes
            //if all data is valid
            //4 cases because we want to leave the Minit and Add2 as non compulsory fields
            //In such case the update should not check for Text in those fields
            if (ctlMinit.TextLength > 0 && ctlAdd2.TextLength == 0)
                btnUpdate.Enabled = (allowUpdate == true || listNames.SelectedItems.Count > 0) && namecheck == true && namecheck1 == true && namecheck2 == true && citycheck == true && Phonecheck == true && Zipcodecheck == true  && Addresscheck == true && statecheck == true && ctlFirstName.TextLength > 0 && ctlLastName.TextLength > 0 && ctlAdd1.TextLength > 0 && ctlCity.TextLength > 0 && ctlEmail.TextLength > 0 && emailcheck == true && ctlPhone.TextLength > 0 && ctlZipCode.TextLength > 0;

            else if (ctlAdd2.TextLength > 0 && ctlMinit.TextLength == 0)
                btnUpdate.Enabled = (allowUpdate == true || listNames.SelectedItems.Count > 0) && namecheck == true && namecheck2 == true && citycheck == true && Phonecheck == true && Zipcodecheck == true && Addresscheck1 == true && Addresscheck == true && statecheck == true && ctlFirstName.TextLength > 0 && ctlLastName.TextLength > 0 && ctlAdd1.TextLength > 0 && ctlCity.TextLength > 0 && ctlEmail.TextLength > 0 && emailcheck == true && ctlPhone.TextLength > 0 && ctlZipCode.TextLength > 0;

            else if (ctlAdd2.TextLength > 0 && ctlMinit.TextLength > 0)
                btnUpdate.Enabled = (allowUpdate == true || listNames.SelectedItems.Count > 0) && namecheck == true && namecheck1 == true && namecheck2 == true && citycheck == true && Phonecheck == true && Zipcodecheck == true && Addresscheck1 == true && Addresscheck == true && statecheck == true && ctlFirstName.TextLength > 0 && ctlLastName.TextLength > 0 && ctlAdd1.TextLength > 0 && ctlCity.TextLength > 0 && ctlEmail.TextLength > 0 && emailcheck == true && ctlPhone.TextLength > 0 && ctlZipCode.TextLength > 0;

            else
                btnUpdate.Enabled = (allowUpdate == true || listNames.SelectedItems.Count > 0) && namecheck == true  && namecheck2 == true && citycheck == true && Phonecheck == true && Zipcodecheck == true && Addresscheck == true && statecheck == true && ctlFirstName.TextLength > 0 && ctlLastName.TextLength > 0 && ctlAdd1.TextLength > 0 && ctlCity.TextLength > 0 && ctlEmail.TextLength > 0 && emailcheck == true && ctlPhone.TextLength > 0 && ctlZipCode.TextLength > 0;



            //Save button should be enabled 
            //if new person with valid data is added
            //disable if person entered a person who was in list before and made changes
            //4 cases because we want to leave the Minit and Add2 as non compulsory fields
            //In such case the update should not check for Text in those fields

            if (ctlMinit.TextLength > 0 && ctlAdd2.TextLength == 0)
            btnSave.Enabled = namecheck ==true && namecheck1==true && namecheck2==true && Addresscheck == true  && citycheck == true && Zipcodecheck == true && Phonecheck ==true && duplicatecheckSave == false && statecheck == true && ctlFirstName.TextLength > 0 && ctlLastName.TextLength > 0 && ctlAdd1.TextLength > 0 && ctlCity.TextLength > 0 && ctlEmail.TextLength > 0 && emailcheck == true && ctlPhone.TextLength > 0 && ctlZipCode.TextLength > 0;

            else if(ctlAdd2.TextLength > 0 && ctlMinit.TextLength == 0)
                btnSave.Enabled = namecheck == true && namecheck2 == true && Addresscheck == true && Addresscheck1 == true && citycheck == true && Zipcodecheck == true && Phonecheck == true && duplicatecheckSave == false && statecheck == true && ctlFirstName.TextLength > 0 && ctlLastName.TextLength > 0 && ctlAdd1.TextLength > 0 && ctlCity.TextLength > 0 && ctlEmail.TextLength > 0 && emailcheck == true && ctlPhone.TextLength > 0 && ctlZipCode.TextLength > 0;

            else if (ctlAdd2.TextLength > 0 && ctlMinit.TextLength > 0)
                btnSave.Enabled = namecheck == true && namecheck1 == true && namecheck2 == true && Addresscheck == true && Addresscheck1 == true && citycheck == true && Zipcodecheck == true && Phonecheck == true && duplicatecheckSave == false && statecheck == true && ctlFirstName.TextLength > 0 && ctlLastName.TextLength > 0 && ctlAdd1.TextLength > 0 && ctlCity.TextLength > 0 && ctlEmail.TextLength > 0 && emailcheck == true && ctlPhone.TextLength > 0 && ctlZipCode.TextLength > 0;

            else
                btnSave.Enabled = namecheck == true  && namecheck2 == true && Addresscheck == true  && citycheck == true && Zipcodecheck == true && Phonecheck == true && duplicatecheckSave == false && statecheck == true && ctlFirstName.TextLength > 0 && ctlLastName.TextLength > 0 && ctlAdd1.TextLength > 0 && ctlCity.TextLength > 0 && ctlEmail.TextLength > 0 && emailcheck == true && ctlPhone.TextLength > 0 && ctlZipCode.TextLength > 0;


   }



        //Upon event of Update button click
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //if no person from list is selected
            //Sometimes a clerk may not want to select/find the item from Listview if it is too big
            //S/He can just type the Full name in the respective FirstName, LastName and Minit fields
            //We will find if this record exists and the data entered by the clerk is now new data
            //To be sure that clerk knows this, we will display, Duplicate Entry - Cannot Save/Can Update

            if (listNames.SelectedItems.Count == 0)
            { 
            for (int i = 0; i < myobjects.Count; i++)
            {
                    //check both Name as well as Phone for identifying a record
                if (ctlFirstName.Text + " " + ctlMinit.Text + " " + ctlLastName.Text == myobjects[i].MyObjectID)
                {
                    //sets the new values for object
                    myobjects[i].MyObjectID = ctlFirstName.Text + " " + ctlMinit.Text + " " + ctlLastName.Text;
                    myobjects[i].FirstName = ctlFirstName.Text;
                    myobjects[i].Minit = ctlMinit.Text;
                    myobjects[i].LastName = ctlLastName.Text;
                    myobjects[i].Add1 = ctlAdd1.Text;
                    myobjects[i].Add2 = ctlAdd2.Text;
                    myobjects[i].City = ctlCity.Text;
                    myobjects[i].State = ctlState.Text;
                    myobjects[i].ZipCode = ctlZipCode.Text;
                    myobjects[i].Phone = ctlPhone.Text;
                    myobjects[i].Email = ctlEmail.Text;
                    myobjects[i].Date = ctlDate.Text;
                    if (checkboxProof.Checked == true)
                        myobjects[i].ProofofPurchase = true;
                    else
                        myobjects[i].ProofofPurchase = false;

                }
                //Acknowledge Entry Update
                    StatusBar.Refresh();
                    stripstatus.Text = "Entry Updated";

                }
        }
            //If item is selected from list, we update the fields of that item
            //check if selected item count is greater than 0
            else if (listNames.SelectedItems.Count > 0)
            {
                string Name = listNames.SelectedItems[0].SubItems[0].Text;
                string Phone = listNames.SelectedItems[0].SubItems[1].Text;

                for (int i = 0; i < myobjects.Count; i++)
                {
                //Update only if the Full Name and Phone of person matches to avoid overwriting someone else's record
                     if (Name == myobjects[i].MyObjectID && Phone == myobjects[i].Phone)
                    {
                        myobjects[i].MyObjectID = ctlFirstName.Text + " " + ctlMinit.Text + " " + ctlLastName.Text;
                        myobjects[i].FirstName = ctlFirstName.Text;
                        myobjects[i].Minit = ctlMinit.Text;
                        myobjects[i].LastName = ctlLastName.Text;
                        myobjects[i].Add1 = ctlAdd1.Text;
                        myobjects[i].Add2 = ctlAdd2.Text;
                        myobjects[i].City = ctlCity.Text;
                        myobjects[i].State = ctlState.Text;
                        myobjects[i].ZipCode = ctlZipCode.Text;
                        myobjects[i].Phone = ctlPhone.Text;
                        myobjects[i].Email = ctlEmail.Text;
                        myobjects[i].Date = ctlDate.Text;
                        if (checkboxProof.Checked == true)
                            myobjects[i].ProofofPurchase = true;
                        else
                            myobjects[i].ProofofPurchase = false;

                        listNames.SelectedItems[0].SubItems[0].Text = ctlFirstName.Text + " " + ctlMinit.Text + " " + ctlLastName.Text;
                        listNames.SelectedItems[0].SubItems[1].Text = (ctlPhone.Text);

                        StatusBar.Refresh();
                        stripstatus.Text = "Entry Updated";

                    }

                }

                //after update write down the entire file again
                writetotextClear(myobjects);
            }
         
            else
            {
                ctlFirstName.Clear();
                ctlLastName.Clear();
                ctlAdd1.Clear();
                ctlAdd2.Clear();
                ctlMinit.Clear();
                ctlEmail.Clear();
                ctlCity.Clear();
                ctlPhone.Clear();
                ctlState.Clear();
                ctlZipCode.Clear();
                ctlDate.ResetText();
                checkboxProof.Checked = true;
                btnSave.Enabled = false;
            }

        }

        //Event if delete button is pressed
        //Check if item is there
        //If it is there, select it and find in list of MyObjects
        //Delete record and rewrite the program again so no gaps are present
        private void btnDelete_Click(object sender, EventArgs e)
        {
            ListViewItem li = listNames.SelectedItems[0];
            string Name = listNames.SelectedItems[0].SubItems[0].Text;
            string Phone = listNames.SelectedItems[0].SubItems[1].Text;

            for (int i = 0; i < myobjects.Count; i++)
            {
                if (Name == myobjects[i].MyObjectID && Phone == myobjects[i].Phone)
                {
                    listNames.Items.Remove(li);
                    myobjects.RemoveAt(i);
                    StatusBar.Refresh();
                    stripstatus.Text = "Entry Deleted";

                    writetotextClear(myobjects);
                }
            }
        }
        private void listNames_ColumnClick(object sender, ColumnClickEventArgs e)
        {

            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            this.listNames.Sort();

        }

        //Checks if the item selected has changed
        //If it has we must dipslay the data from the data file for the item selected for user to see what values the fields took
        
        private void listNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if item is selected i.e. count > 0
            if (listNames.SelectedItems.Count > 0)
            {
                string Name = listNames.SelectedItems[0].SubItems[0].Text;
                string Phone = listNames.SelectedItems[0].SubItems[1].Text;

                for (int i = 0; i < myobjects.Count; i++)
                {
                    if (myobjects[i].MyObjectID == Name && myobjects[i].Phone == Phone)
                    {
                        //If the Full name and Phone of the person match the record in data file, pull up his details
                        //fill the fields with the values from the data file
                        string id = ctlFirstName.Text + " " + ctlMinit.Text + " " + ctlLastName.Text;
                        id = myobjects[i].MyObjectID;
                        ctlFirstName.Text = myobjects[i].FirstName;
                        ctlMinit.Text = myobjects[i].Minit;
                        ctlLastName.Text = myobjects[i].LastName;
                        ctlAdd1.Text = myobjects[i].Add1;
                        ctlAdd2.Text = myobjects[i].Add2;
                        ctlCity.Text = myobjects[i].City;
                        ctlState.Text = myobjects[i].State;
                        ctlZipCode.Text = myobjects[i].ZipCode;
                        ctlPhone.Text = myobjects[i].Phone;
                        ctlEmail.Text = myobjects[i].Email;
                        ctlDate.Text = myobjects[i].Date;
                        if (myobjects[i].ProofofPurchase == true)
                            checkboxProof.Checked = true;
                        else
                            checkboxProof.Checked = false;
                    }

                }
            }

            else
            {
                ctlFirstName.Clear();
                ctlLastName.Clear();
                ctlAdd1.Clear();
                ctlAdd2.Clear();
                ctlMinit.Clear();
                ctlEmail.Clear();
                ctlCity.Clear();
                ctlPhone.Clear();
                ctlState.Clear();
                ctlZipCode.Clear();
                ctlDate.ResetText();
                checkboxProof.Checked = true;
                btnSave.Enabled = false;
            }
        }

        //This event is used to track the starting time
        //When the key is pressed on the first name field for first time, start time is noted
        private void ctlFirstName_KeyPress(object sender, KeyPressEventArgs e)
        {
              startTime = DateTime.Now;
            
        }

        //This is used to save the data records
        private void btnSave_Click(object sender, EventArgs e)
        {

            //Takes out only the time component of DateTime and convert to long HH:MM:SS format as required
            string startingTime = startTime.ToLongTimeString();

            //When the Save was clicked we take down the time, as the time to enter the entry ends here
            string endTime = DateTime.Now.ToLongTimeString();
       
            //Save the details from the text fields to MyObject as a record
            record = new MyObject();
            record.MyObjectID = ctlFirstName.Text + " " + ctlMinit.Text + " " + ctlLastName.Text;
            record.FirstName = ctlFirstName.Text;
            record.LastName = ctlLastName.Text;
            record.Phone = ctlPhone.Text;
            record.Minit = ctlMinit.Text;
            record.Add1 = ctlAdd1.Text;
            record.Add2 = ctlAdd2.Text;
            record.City = ctlCity.Text;
            record.State = ctlState.Text;
            record.ZipCode = ctlZipCode.Text;
            record.Email = ctlEmail.Text;
            record.ProofofPurchase = checkboxProof.Checked;
            record.Date = ctlDate.Text;
            record.startTime = startingTime;
            record.endTime = endTime;
            myobjects.Add(record);
            
            //Add to ListView and make the item focused
            ListViewItem li = listNames.Items.Add(ctlFirstName.Text + " " + ctlMinit.Text + " " + ctlLastName.Text);
            li.SubItems.Add(ctlPhone.Text);

            //if some other item is selected, remove selection on that and select current item
            if (listNames.SelectedItems.Count != 0)
            {
                listNames.SelectedItems[0].Selected = false;
            }
            listNames.FocusedItem = li;
            li.Selected = true;
            //Find the item with the Text and get the index of the item
            var item = listNames.FindItemWithText(ctlFirstName.Text + " " + ctlMinit.Text + " " + ctlLastName.Text);
            int i = 0;
            if (item != null)
            {
                i = listNames.Items.IndexOf(item);
               
                //Make sure it is visible to the user by scrolling to it
                listNames.EnsureVisible(i);
            }

           

            //write to the data file
            writetotextClear(myobjects);
           
            
            //Clear the fields afterwards
            ctlFirstName.Clear();
            ctlLastName.Clear();
            ctlAdd1.Clear();
            ctlAdd2.Clear();
            ctlMinit.Clear();
            ctlEmail.Clear();
            ctlCity.Clear();
            ctlPhone.Clear();
            ctlState.Clear();
            ctlZipCode.Clear();
            ctlDate.ResetText();
            checkboxProof.Checked = true;

            //acknowledge entry has been saved
            StatusBar.Refresh();
            stripstatus.Text = "Entry Saved";
            StatusBar.Refresh();

            //make button false again
            btnSave.Enabled = false;


        }
    }
}
