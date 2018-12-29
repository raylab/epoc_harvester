using System;
using System.Collections.Generic;
using Emotiv;
using System.IO;
using System.Threading;
using System.Reflection;
using System.Linq;
using System.Web.Script.Serialization;

namespace Epoc_harvister
{
    static class My_License
    {

        public static void activateLicense()
        {

            string licenseKey = "d8a7953c-e088-40a9-b9cc-a41bb7cfbb2b";
            int debitNum = 1000000;
            string userName = "raylab";
            string password = "DauLab1953";

            //Authorize

            if (EmotivCloudClient.EC_Connect() != EdkDll.EDK_OK)
            {
               My_Program.myForm.textBox1.Text = "Cannot connect to Emotiv.";
                Thread.Sleep(2000);
                return;
            }


            if (EmotivCloudClient.EC_Login(userName, password) != EdkDll.EDK_OK)
            {
                My_Program.myForm.textBox1.Text = "Your login failed.";
                Thread.Sleep(2000);
                return;
            }

            My_Program.myForm.textBox1.Text = "Logged in as " + userName;

            int userCloudID = 0;
            if (EmotivCloudClient.EC_GetUserDetail(ref userCloudID) != EdkDll.EDK_OK)
                return;


            int result = EdkDll.IEE_AuthorizeLicense(licenseKey, debitNum);
            if (result == EdkDll.EDK_OK || result == EdkDll.EDK_LICENSE_REGISTERED)
            {
                My_Program.myForm.textBox1.Text = "License activated.";
            }
            else My_Program.myForm.textBox1.Text = "License Error:" + result;
        }
    }
}