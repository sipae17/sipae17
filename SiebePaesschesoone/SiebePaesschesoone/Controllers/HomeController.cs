using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using Sakura.AspNetCore;
using ClassLibraryForSiebePaesschesoone;

namespace SiebePaesschesoone.Controllers
{
    public class HomeController : Controller
    {

        public List<string> serialkeys = new List<string>();
        public List<string> usedSerialKeys = new List<string>();

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //returns the viewpage 'Index'
        [HttpGet]
        public IActionResult Index()
	    {
	        return View("Index");
	    }

        //method that gets called whenever you click the 'Submit' button on index page 
        //this is why we have HttPost in square brackets
        //@param: ClientModel model, so we pass the model, so we can easily trace the data that the user submits
        [HttpPost]
        public ActionResult Index(ClientModel smodel)
	    {
            try
            {
                //create/generate the serial keys (they are represented as the numbers from 0 to 99)
                for (int i = 0; i < 100; i++) { serialkeys.Add(i.ToString()); }

                //this stream is just used to get the used serialkeys (code from 'foreach')
                //we create a list with the strings from the textfile
                string[] textFile;
                StreamReader stream = new StreamReader("/Users/siebepaesschesoone/Desktop/sipae17/SiebePaesschesoone/SiebePaesschesoone/text.txt");
                textFile = stream.ReadToEnd().Split('\n');
		        stream.Close();
		        string[] line;

                //for each serial key that is used, add it to the usedSerialkeys list
                //we do that by reading from the textFile (streams - input and output), described above
                foreach (string str in textFile)
                {
                    line = str.Split(' ');
                    usedSerialKeys.Add(line[0]);
                }

                //validation rule: if the serial key is already used in a previous submission, add error to model
                //later on we show all errors to user
                if (usedSerialKeys.Contains(smodel.SerialKey))
                {
                    ModelState.AddModelError("SerialKey", "Serial key already used.");
                }
		
                //validation rule: if the serial key is not a number from 0 to 99, add error to model
                //later on we show all errors to user
		        if (!serialkeys.Contains(smodel.SerialKey))
		        {
		            ModelState.AddModelError("SerialKey", "Serial key not correct.");
		        }

                //validation rule
                if (string.IsNullOrEmpty(smodel.FirstName))
                {
                    ModelState.AddModelError("FirstName", "First name is required.");
                }

                //validation rule
                if (string.IsNullOrEmpty(smodel.FamilyName))
                {
                    ModelState.AddModelError("FamilyName", "Family name is required.");
                }

                //validation rule
                if (string.IsNullOrEmpty(smodel.EmailAddress))
                {
                    ModelState.AddModelError("EmailAddress", "Email address is required.");
                }

                //validation rule
                if (string.IsNullOrEmpty(smodel.PhoneNumber))
                {
                    ModelState.AddModelError("PhoneNumber", "Phone number is required.");
                }

                //validation rule
                if (string.IsNullOrEmpty(smodel.DateOfBirth))
                {
                    ModelState.AddModelError("DateOfBirth", "Date of birth is required.");
                }

                //if the model is valid, then write to the textfile on computer, save the submitted data
                if (ModelState.IsValid)
                {
                    //get the data
                    ViewData["SerialKey"] = smodel.SerialKey;
                    ViewData["FirstName"] = smodel.FirstName;
                    ViewData["FamilName"] = smodel.FamilyName;
                    ViewData["EmailAddress"] = smodel.EmailAddress;
                    ViewData["PhoneNumber"] = smodel.PhoneNumber;
                    ViewData["DateOfBirth"] = smodel.DateOfBirth;

                    //we use a stream to write to the textfile
                    using (StreamWriter sw = new StreamWriter("/Users/siebepaesschesoone/Desktop/sipae17/SiebePaesschesoone/SiebePaesschesoone/text.txt", true))
                    {
                        sw.WriteLine(smodel.SerialKey + " " + smodel.FirstName + " " + smodel.FamilyName + " " + smodel.EmailAddress +
                                     " " + smodel.PhoneNumber + " " + smodel.DateOfBirth);
                        sw.Dispose();
			            sw.Close();
                    }

                    //pass a message to the view, if we look at the view you will see 'ViewData["submitted"]
                    //whenever that gets called in the view, the user can see the message "Successfully submitted" on the screen
                    ViewData["submitted"] = "Successfully submitted.";
                    return View("Index");
                }

                //if the modelstate is not valid, we just return the 'Index' viewpage again
                else { return View("Index"); }
            }
            //whenever this try-block gets an exception, the exception is 'catched' with this code,
            //the only possibility of getting an exception, is when there is no txt file in the insisted filepath
            catch { return View("Index"); }
	    }

        //this method gets called after there is a valid authentication password given on the validationPage
        [HttpGet]
        public ActionResult Submissions(int page = 1)
        {
            List<ClientModel> myList = new List<ClientModel>();

            //same concept as described above, we use try-catch
            try
            {
                //we must make a list 'myList' with the data from the textfile, in order to pass it to the view
                //so we can display the data in a table in the view
                using (StreamReader stream = new StreamReader("/Users/siebepaesschesoone/Desktop/sipae17/SiebePaesschesoone/SiebePaesschesoone/text.txt", true))
                {
                    string[] textFile = stream.ReadToEnd().Split('\n');
                    foreach (string str in textFile)
                    {
                        string[] elements = str.Split(' ');
                        ClientModel client = new ClientModel();
                        client.SerialKey = elements[0];
                        client.FirstName = elements[1];
                        client.FamilyName = elements[2];
                        client.EmailAddress = elements[3];
                        client.PhoneNumber = elements[4];
                        client.DateOfBirth = elements[5];
                        myList.Add(client);
                    }
                //never forget to close the stream
	            stream.Close();
                }
            }
            catch {RedirectToAction("Index"); }

            //we transform the list into a pagedList (using a nuGet package 'Sakura.AspNetCore')
            //we pass the paged list to the view
            var myPagedList = myList.ToPagedList(10, page);
            return View("Submissions", myPagedList);
        }

        //when the user clicks the 'view all submissions' button on the index page, he gets redirected to this page
        [HttpGet]
        public ActionResult ValidationPage()
        {
            return View("ValidationPage");
        }

        //in order to see the submissions, we need a valid password (in this case 'admin')
        [HttpPost]
        public ActionResult ValidationPage(ValidationModel vm)
        {
            //this is the authentication to be able to see the submissions page
            if (vm.Password != "admin")
            {
                ModelState.AddModelError("Password", "Wrong password, try again.");
            }

            if (ModelState.IsValid)
                return RedirectToAction("Submissions");
            
            return View("ValidationPage");
        }
    }
}
