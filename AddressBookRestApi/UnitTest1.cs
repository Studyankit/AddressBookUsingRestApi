using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;

namespace AddressBookRestSharp
{
    [TestClass]
    public class UnitTest1
    {
        RestClient client;

        [TestInitialize]
        public void Setup()
        {
            client = new RestClient(" http://localhost:4000");
        }

        private IRestResponse getContacts()
        {
            RestRequest request = new RestRequest("/Contacts", Method.GET);
            IRestResponse response = client.Execute(request);
            return response;
        }

        //Creating method to get contacts
        [TestMethod]
        public void onCallingGETApi_ReturnContactList()
        {
            IRestResponse response = getContacts();

            //assert
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
            List<Contacts> dataResponse = JsonConvert.DeserializeObject<List<Contacts>>(response.Content);
            Assert.AreEqual(15, dataResponse.Count);
            foreach (var item in dataResponse)
            {
                System.Console.WriteLine("id: " + item.Id + "FirstName: " + item.FirstName + "LastName: " + item.LastName + "Address: " + item.Address + "City: " + item.City + "State: " + item.State + "Zip: " + item.Zip + "PhoneNumber: " + item.PhoneNumber);
            }
        }

        //Creating method to add contacts
        [TestMethod]
        public void givenContact_OnPost_ShouldReturnAddedContact()
        {
            RestRequest request = new RestRequest("/Contacts", Method.POST);
            JObject jObjectbody = new JObject();
            jObjectbody.Add("FirstName", "Ankit");
            jObjectbody.Add("LastName", "Ghosh");
            jObjectbody.Add("Address", "Balihari");
            jObjectbody.Add("City", "Dhanbad");
            jObjectbody.Add("State", "Jh");
            jObjectbody.Add("Zip", 904040);
            jObjectbody.Add("PhoneNumber", 90019484849);
            request.AddParameter("application/json", jObjectbody, ParameterType.RequestBody);

            //act
            IRestResponse response = client.Execute(request);
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.Created);
            Contacts dataResponse = JsonConvert.DeserializeObject<Contacts>(response.Content);
            Assert.AreEqual("Ankit", dataResponse.FirstName);
            Assert.AreEqual("Ghosh", dataResponse.LastName);
            Assert.AreEqual("Balihari", dataResponse.Address);
            Assert.AreEqual("Jh", dataResponse.State);
            Assert.AreEqual(904040, dataResponse.Zip);
            Assert.AreEqual("Dhanbad", dataResponse.City);
            Assert.AreEqual(90019484849, dataResponse.PhoneNumber);
        }

        //Creating method to update contacts
        [TestMethod]
        public void givenContact_OnPUT_ShouldReturnUpdatedContact()
        {
            RestRequest request = new RestRequest("/Contacts/3", Method.PUT);
            JObject jObjectbody = new JObject();
            jObjectbody.Add("FirstName", "Milan");
            jObjectbody.Add("LastName", "Yadav");
            jObjectbody.Add("Address", "Balia");
            jObjectbody.Add("City", "Patna");
            jObjectbody.Add("State", "Bihar");
            jObjectbody.Add("Zip", 840022);
            jObjectbody.Add("PhoneNumber", 6200974747);
            request.AddOrUpdateParameter("application/json", jObjectbody, ParameterType.RequestBody);

            //act
            IRestResponse response = client.Execute(request);
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
            Contacts dataResponse = JsonConvert.DeserializeObject<Contacts>(response.Content);
            Assert.AreEqual("Milan", dataResponse.FirstName);
            Assert.AreEqual("Yadav", dataResponse.LastName);

        }

        //Creating method to delete contact
        [TestMethod]
        public void givenContact_OnDELETE_ShouldReturnContact()
        {
            RestRequest request = new RestRequest("/Contacts/6", Method.DELETE);
            JObject jObjectbody = new JObject();
            request.AddParameter("application/json", jObjectbody, ParameterType.RequestBody);

            //act
            IRestResponse response = client.Execute(request);
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.NotFound);
            Contacts dataResponse = JsonConvert.DeserializeObject<Contacts>(response.Content);
            Assert.AreEqual(null, dataResponse.FirstName);
            Assert.AreEqual(null, dataResponse.LastName);

        }
    }
}