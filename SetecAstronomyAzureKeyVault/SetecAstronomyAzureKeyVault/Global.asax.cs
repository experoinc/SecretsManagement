﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.Azure.KeyVault;
using System.Web.Configuration;

namespace SetecAstronomyAzureKeyVault
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // I put my GetToken method in a Utils class. Change for wherever you placed your method. 
            var kv = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(Util.GetToken));

            var sec = kv.GetSecretsAsync(WebConfigurationManager.AppSettings["VaultUri"]).GetAwaiter().GetResult();

            //I put a variable in a Utils class to hold the secret for general  application use. 
            Util.EncryptSecret = new List<KeyValuePair<string, string>>();

            foreach (var item in sec.Value)
            {
                var value = kv.GetSecretAsync(item.Id).GetAwaiter().GetResult().Value;
                Util.EncryptSecret.Add(new KeyValuePair<string, string>(item.Identifier.Name, value));
            }
        }
    }
}
