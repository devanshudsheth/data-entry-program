using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asg2_dds160030
{
    //Create a class of MyObjects
    //This will be used to maintain objects or in our case Person records.
    //They will each have the attributes for the database records
    public class MyObject
    {
        //Empty Constructor
        public MyObject()
        {

        }

        public MyObject(string FirstName, string LastName, string Minit, string Add1, string Add2, string City, string State, string ZipCode, string Phone, string Email, bool ProofofPurchase, string Date)
        {
          
        }
        private string MyObjectIDvalue;

        //An ID associated with each MyObject
        //It is the Full Name of the Person
        public string MyObjectID
        {
            get { return MyObjectIDvalue; }
            set { MyObjectIDvalue = value; }
        }

        private string FirstNamevalue;

        //First Name
        //sets FirstNameValue to FirstName
        //gets FirstNameValue
        public string FirstName
        {
            get { return FirstNamevalue; }
            set { FirstNamevalue = value; }
        }

        private string LastNamevalue;

        //Last Name
        //sets LastNamevalue to LastName
        //gets LastNamevalue
        public string LastName
        {
            get { return LastNamevalue; }
            set { LastNamevalue = value; }
        }


        private string Minitvalue;

        //Miiddle Initial
        //sets MiddleInitvalue to Middle Initial
        //gets MiddleInitvalue
        public string Minit
        {
            get { return Minitvalue; }
            set { Minitvalue = value; }
        }


        private string Add1value;

        //Address Line 1
        //sets Add1value to Add1
        //gets Add1value
        public string Add1
        {
            get { return Add1value; }
            set { Add1value = value; }
        }

        private string Add2value;

        //Address Line 2
        //sets Add2value to Add2
        //gets Add2value
        public string Add2
        {
            get { return Add2value; }
            set { Add2value = value; }
        }

        private string Cityvalue;
        
        //City
        //sets Cityvalue to City
        //gets Cityvalue
        public string City
        {
            get { return Cityvalue; }
            set { Cityvalue = value; }
        }

        private string Statevalue;

        //State
        //sets Statevalue to State
        //gets Statevalue
        public string State
        {
            get { return Statevalue; }
            set { Statevalue = value; }
        }

        private string ZipCodevalue;

        //Zipcode
        //sets ZipCodevalue to ZipCode
        //gets ZipCode
        public string ZipCode
        {
            get { return ZipCodevalue; }
            set { ZipCodevalue = value; }
        }

        private string Phonevalue;


        //Phone
        //sets Phonevalue to Phone
        //gets Phonevalue
        public string Phone
        {
            get { return Phonevalue; }
            set { Phonevalue = value; }
        }

        private string Emailvalue;


        //Eail
        //sets Emailvalue to Email
        //gets Emailvalue
        public string Email
        {
            get { return Emailvalue; }
            set { Emailvalue = value; }
        }

        private string Datevalue;
        //Date
        //sets Datevalue to Date
        //gets Datevalue
        public string Date
        {
            get { return Datevalue; }
            set { Datevalue = value; }
        }

        private bool PoPvalue;
       
        //Proof of Purchase
        //sets PoPvalue to Proof of Purchase checkbox
        //gets PoPvalue
        public bool ProofofPurchase
        {
            get { return PoPvalue; }
            set { PoPvalue = value; }
        }

        private string startTimevalue;

        //Start time
        //sets startTimevalue to start time
        //gets startTime
        public string startTime
        {
            get { return startTimevalue; }
            set { startTimevalue = value; }
        }

        private string endTimevalue;

        //End time
        //sets endTimevalue to end time
        //gets endTime
        public string endTime
        {
            get { return endTimevalue; }
            set { endTimevalue = value; }
        }

        // Override the ToString method for MyObject
        // Print all data as a single TAB spaced Line in data record
        // Each MyObject corresponds to one unique line
        public override string ToString()
        {
            return this.FirstName + "\t" + this.LastName + "\t" + this.Minit + "\t" + this.Add1 + "\t" + this.Add2 + "\t" + this.City + "\t" + this.State + "\t" + this.ZipCode + "\t" + this.Phone + "\t" + this.Email + "\t" + this.ProofofPurchase.ToString() + "\t" + this.Date + "\t" + this.startTime + "\t" + this.endTime;
        }

    }




}
