using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shopping.Contexts.Procurement.Services;
using Shopping.Contexts.Procurement.Applications.Interfaces;

namespace BuyAnyWhere.Test.Contexts.Procurement
{
    [TestClass]
    public class NotificationServiceTest
    {
        [TestMethod]
        public void NotifyTest()
        {
            INotificationService notificationService = new NotificationService();
            notificationService.Notify("Hello", "World", "cPhklk8t4k4:APA91bFa4yU1cquAiwKywioX_hksPMZy-zrqbOkMiV5vHk6FFu6Q98FAQUjIEJOhDVPrS5hotYi-TeVacHZiY3U1CjzwHaE6T6rsatJPRP0JfLuQWbQquDvbB5Aq3LpEQVbtgMXsz53K");

        }
    }
}
