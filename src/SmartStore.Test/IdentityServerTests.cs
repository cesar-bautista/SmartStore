using IdentityModel.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SmartStore.Test
{
    [TestClass]
    public class IdentityServerTests
    {
        [TestMethod]
        public async Task Login_testAsync()
        {
            var identityServer = await DiscoveryClient.GetAsync("https://localhost:44372"); //discover the IdentityServer
            if (identityServer.IsError)
            {
                System.Console.Write(identityServer.Error);
                return;
            }

            //Get the token
            var tokenClient = new TokenClient(identityServer.TokenEndpoint, "client", "511536EF-F270-4058-80CA-1C89C192F69A");
            //var tokenResponse = await tokenClient.RequestClientCredentialsAsync("api1");
            var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync("admin", "password", "api1");

            //Call the API

            HttpClient client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);

            var response = await client.GetAsync("https://localhost:44361/api/values");
            var content = await response.Content.ReadAsStringAsync();
            System.Console.WriteLine(JArray.Parse(content));
            System.Console.ReadKey();
        }
    }
}
