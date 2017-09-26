using Shopping.App_Start;
using Shopping.Contexts.Auth.Applications.Interfaces;
using Shopping.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using Microsoft.Practices.Unity;


namespace Shopping.Contexts.Auth.Applications.Controllers
{
    public class HelpPageController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public void LoadRoute()
        {
            HttpConfiguration configuration = GlobalConfiguration.Configuration;
            LoadOperation(configuration.Services.GetApiExplorer().ApiDescriptions);
        }

        private string parseLink(string Link)
        {
            string[] linkParts = Link.Split('/');
            for (int i = 0; i < linkParts.Length; i++)
            {
                if (linkParts[i].Contains('{') & linkParts[i].Contains('}'))
                {
                    linkParts[i] = "*";
                }
            }
            return String.Join("/", linkParts);
        }

        private void LoadOperation(Collection<ApiDescription> apiDescriptions)
        {
            using (ShoppingEntities shoppingEntities = new ShoppingEntities())
            {
                using (var transaction = shoppingEntities.Database.BeginTransaction())
                {
                    var rol

                    try
                    {
                        foreach (ApiDescription apiDescription in apiDescriptions)
                        {
                            string Link = parseLink(apiDescription.Route.RouteTemplate);
                            string Method = apiDescription.HttpMethod.ToString();
                            if (Link.Contains("*/*"))
                            {
                                continue;
                            }

                            Api api = new Api();
                            api.Id = Guid.NewGuid();
                            api.Method = Method;
                            api.Uri = Link;
                            api.Type = 1;

                            shoppingEntities.Apis.Add(api);
                        }

                        shoppingEntities.SaveChanges();

                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }
    }
}