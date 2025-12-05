using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWorkApplication.Classes
{
    public class KoboFormConfig
    {
        public string City { get; set; }
        public string FormName { get; set; }
        public string FormId { get; set; }

        public string token = "a4042da5462484094f839b65d537eef9fa46d30c";

        public static List<KoboFormConfig> KoboForms = new List<KoboFormConfig>
        {
            //Aleepo
            new KoboFormConfig {
                City = "Aleppo",
                FormName = "Energy Projects - M&E - Aleppo",
                FormId = "ai6SnMT3dqZAEELCfesSqp"     // <-- actual Kobo ID
            },
            new KoboFormConfig {
                City = "Aleppo",
                FormName = "M&E-2025- Aleppo",
                FormId = "ay2c9afff5StTkQdneHHUS"          
            },
            new KoboFormConfig {
                City = "Aleppo",
                FormName = "Vehicles Projects – Opening Visit - Aleppo",
                FormId = "a8Vh8j6CTzHkPezChub5Ez"
            },
            new KoboFormConfig {
                City = "Aleppo",
                FormName = "Monitoring for Vehicles - Aleppo",
                FormId = "a3BwzyULUpD3P8jh9B8ADn"
            }, 
            new KoboFormConfig {
                City = "Aleppo",
                FormName = "Final Visit - Aleppo",
                FormId = "aDGPm94uwS7HYk4LcG46oh"
            }, 
            new KoboFormConfig {
                City = "Aleppo",
                FormName = "Opening Visit-2025-Aleppo",
                FormId = "aMhoH9jiVXwWSTYacGFeEX"
            }, 

            ////Damascus
            //new KoboFormConfig {
            //    City = "Damascus",
            //    FormName = "Energy Projects - M&E-Damas",
            //    FormId = "aZzfVwdN9sFyBCWDzBVqaU"
            //}, 
            //new KoboFormConfig {
            //    City = "Damascus",
            //    FormName = "M&E-2025-Damas",
            //    FormId = "aFqwLzjDcQ4gczMdfUaXNq"
            //}, 
            //new KoboFormConfig {
            //    City = "Damascus",
            //    FormName = "Opening Visit-2025-Damas",
            //    FormId = "aFAaQVePpXeKJTGU89zGgV"
            //},
            //new KoboFormConfig {
            //    City = "Damascus",
            //    FormName = "Vehicles Projects – Opening Visit - Damas",
            //    FormId = "akQA4KSvPFy3Bt9LnCjnBw"
            //},
            //new KoboFormConfig {
            //    City = "Damascus",
            //    FormName = "Monitoring for Vehicles -Damas",
            //    FormId = "atB8PhFWVmYQskk5rNnehB"
            //}, 
            //new KoboFormConfig {
            //    City = "Damascus",
            //    FormName = "Final-Damas",
            //    FormId = "aYCc7VCGeP77BE9zUTMceV"
            //}, 

            ////wadi
            //new KoboFormConfig {
            //    City = "Wadi",
            //    FormName = "Energy Projects_M&E - Wadi",
            //    FormId = "arvdKcen8mFrYkYcWKYRmJ"
            //},
            //new KoboFormConfig {
            //    City = "Wadi",
            //    FormName = "M&E-2025- Wadi",
            //    FormId = "aRwLqiUq5LKhjF4bWH4tcp"
            //},
            //new KoboFormConfig {
            //    City = "Wadi",
            //    FormName = "Opening Visit-2025- Wadi",
            //    FormId = "aHif7QqLBVEYZr2oKWu3eD"
            //},
            //new KoboFormConfig {
            //    City = "Wadi",
            //    FormName = "Vehicles Projects – Opening Visit - Wadi",
            //    FormId = "aAeDhFGNredqJ4uw56iebX"
            //},
            //new KoboFormConfig {
            //    City = "Wadi",
            //    FormName = "Monitoring for Vehicles - Wadi",
            //    FormId = "aZjS5bHmMPARXPRp5Hbr5d"
            //},
            //new KoboFormConfig {
            //    City = "Wadi",
            //    FormName = "Final - wadi",
            //    FormId = "aLiFSfoPAHBjFQRD7eswrL"
            //},

            ////homs
            //new KoboFormConfig {
            //    City = "Homs",
            //    FormName = "Energy Projects - M&E - Homs",
            //    FormId = "aj4fsVSAoDQDtpt4act3mP"
            //},
            //new KoboFormConfig {
            //    City = "Homs",
            //    FormName = "M&E - 2025- Homs",
            //    FormId = "ayz6LYxMSyePXdfs27cJja"
            //},
            //new KoboFormConfig {
            //    City = "Homs",
            //    FormName = "Opening Visit-2025 - Homs",
            //    FormId = "agEWgTGdefWAVHQk6y4URo"
            //},
            //new KoboFormConfig {
            //    City = "Homs",
            //    FormName = "Monitoring for Vehicles - Homs",
            //    FormId = "acHufLsXu5jQQscMhtyeLt"
            //},
            //new KoboFormConfig {
            //    City = "Homs",
            //    FormName = "Vehicles Projects – Opening Visit - Homs",
            //    FormId = "aEmjzQjAX8vDxCnPGdRgC6"
            //},
            //new KoboFormConfig {
            //    City = "Homs",
            //    FormName = "Final - Homs",
            //    FormId = "a3DSoAPvHiuEuXzPbVPaJD"
            //},

            ////latakia
            //new KoboFormConfig {
            //    City = "Latkia",
            //    FormName = "Energy Projects - M&E- Latakia",
            //    FormId = "aMdK7mGpanWHyF39Reu498"
            //},
            //new KoboFormConfig {
            //    City = "Latkia",
            //    FormName = "M&E- 2025- Latakia",
            //    FormId = "aZbSDYzGm327nfCVPjKJ9N"
            //},
            //new KoboFormConfig {
            //    City = "Latkia",
            //    FormName = "Opening Visit-2025-Latakia",
            //    FormId = "as82uKKeJQM3uivD4sn5WA"
            //},
            //new KoboFormConfig {
            //    City = "Latkia",
            //    FormName = "Vehicles Projects – Opening Visit - Latakia",
            //    FormId = "akVk2vuomrPc3buQUsxfNa"
            //},
            //new KoboFormConfig {
            //    City = "Latkia",
            //    FormName = "Monitoring for Vehicles - latakia",
            //    FormId = "a9yjLmEjdpvxcSuUzbEu3m"
            //},
            //new KoboFormConfig {
            //    City = "Latkia",
            //    FormName = "Final - Latakia",
            //    FormId = "a4bFkLTC2iRwht3vkTdbuo"
            //}

        };

        public static Dictionary<string, Func<JObject, VisitsNew>> FormMappers = new Dictionary<string, Func<JObject, VisitsNew>>
        {
            { "Energy Projects - M&E - Aleppo", KoboMappers.Map_EnergyProjects_Aleppo },
            { "M&E-2025- Aleppo", KoboMappers.Map_ME2025_Aleppo },
            { "Vehicles Projects – Opening Visit - Aleppo", KoboMappers.Map_VehiclesOpening_Aleppo },
            { "Monitoring for Vehicles - Aleppo", KoboMappers.Map_VehiclesMonitoring_Aleppo },
            { "Final Visit - Aleppo", KoboMappers.Map_FinalVisit_Aleppo },
            { "Opening Visit-2025-Aleppo", KoboMappers.Map_OpeningVisit2025_Aleppo },
        };

    }
}
