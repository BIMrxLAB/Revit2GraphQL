using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using System.Configuration;
using System.Globalization;
using System.Windows.Forms;
using System.Net;
using System.IdentityModel.Tokens.Jwt;

namespace RevitMarconiCommand.Helpers
{
    public class MsalAuthHelper
    {

        private IPublicClientApplication _app;

        private static readonly string ClientId = "294c8f2a-d3cf-48af-be60-83b7b94552ec";
        private static readonly string Authority = "https://BIMrxB2C.b2clogin.com/tfp/BIMrxB2C.onmicrosoft.com/B2C_1_SignUpSignIn";
        private static readonly string SpListScope = "https://BIMrxB2C.onmicrosoft.com/72cf8896-fd75-46cf-966a-1b6d14cb6483/API.Access";
        private static readonly string[] Scopes = { SpListScope };

        private IList<IAccount> accounts;
        public AuthenticationResult AuthenticationResult;

        public IAccount ActiveAccount
        {
            get
            {
                return accounts.FirstOrDefault();
            }
        }

        public async Task<string> GetTokenAsync()
        {
            var aAccount = accounts.FirstOrDefault();
            if (aAccount == null)
            {
                return "";
            }
            else
            {
                AuthenticationResult = await _app.AcquireTokenSilent(Scopes, aAccount)
                    .ExecuteAsync()
                    .ConfigureAwait(false);
                return AuthenticationResult.AccessToken;
            }
        }

        public MsalAuthHelper()
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            _app = PublicClientApplicationBuilder.Create(ClientId)
                .WithB2CAuthority(Authority)
                //.WithRedirectUri("msal294c8f2a-d3cf-48af-be60-83b7b94552ec://auth")               
                .WithRedirectUri("http://localhost")               
                .Build();
            TokenCacheHelper.EnableSerialization(_app.UserTokenCache);
            accounts = _app.GetAccountsAsync().Result.ToList();
        }

        public async Task SignInAsync()
        {
            accounts = (await _app.GetAccountsAsync()).ToList();
            AuthenticationResult = null;
            try
            {
                AuthenticationResult = await _app.AcquireTokenInteractive(Scopes)
                    .WithUseEmbeddedWebView(false)
                    .WithAccount(accounts.FirstOrDefault())
                    //.WithPrompt(Prompt.SelectAccount)
                    .ExecuteAsync();

                accounts = (await _app.GetAccountsAsync()).ToList();

            }
            catch (MsalUiRequiredException)
            {

            }
            catch (MsalException ex)
            {
                // An unexpected error occurred.
                string message = ex.Message;
                if (ex.InnerException != null)
                {
                    message += "Error Code: " + ex.ErrorCode + "Inner Exception : " + ex.InnerException.Message;
                }
                MessageBox.Show(message);
            }


        }

        public async Task SignOutAsync()
        {
            // clear the cache
            while (accounts.Any())
            {
                await _app.RemoveAsync(accounts.First());
                accounts = (await _app.GetAccountsAsync()).ToList();
            }
            AuthenticationResult = null;
        }

        public bool IsSignedIn()
        {
            if (accounts.Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal async Task<object> GetNameOfActiveAccountAsync()
        {
            //using System.IdentityModel.Tokens.Jwt;
            var handler = new JwtSecurityTokenHandler();
            var tokenContent = (JwtSecurityToken)handler.ReadToken(await GetTokenAsync());
            var email = tokenContent.Claims.FirstOrDefault(x => x.Type == "emails")?.Value;
            var name = tokenContent.Claims.FirstOrDefault(x => x.Type == "name")?.Value;
            return name;
        }
    }
}
