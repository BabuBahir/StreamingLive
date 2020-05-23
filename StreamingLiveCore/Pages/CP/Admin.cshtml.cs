﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Data;

namespace StreamingLiveCore.Pages.CP
{
    public class AdminModel : PageModel
    {
        public string Upcoming = "";
        public StreamingLiveLib.Sites RecentSites;


        public void OnGet()
        {
            if (!AppUser.Current.IsSiteAdmin) Response.Redirect("/cp/");
            PopulateUpcoming();
            RecentSites = StreamingLiveLib.Sites.LoadRecent();
        }

        public void OnGetAccess()
        {
            int id = Convert.ToInt32(Request.Query["id"]);
            AppUser.Current.Site = StreamingLiveLib.Site.Load(id);
            AppUser.Current.Role = new StreamingLiveLib.Role() { Name = "admin", SiteId = id, UserId = AppUser.Current.UserData.Id };
            Response.Redirect("/cp/");
        }

        private void PopulateUpcoming()
        {
            StringBuilder sb = new StringBuilder();
            DataTable dt = StreamingLiveLib.Services.LoadUpcoming();
            foreach (DataRow row in dt.Rows)
            {
                DateTime serviceTime = Convert.ToDateTime(row["ServiceTime"]).AddHours(-5);
                sb.Append($"<tr><td><a href=\"https://{row["KeyName"]}.streaminglive.church/\" target=\"_blank\">{row["KeyName"]}</a></td><td>{serviceTime}</td></tr>");
            }
            Upcoming = sb.ToString();
        }

        



    }
}