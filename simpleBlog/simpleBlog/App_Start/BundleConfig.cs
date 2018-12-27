﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace simpleBlog
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/admin/styles")
                .Include("~/content/styles/bootstrap.css")
                .Include("~/content/styles/admin.css"));

            bundles.Add(new StyleBundle("~/styles")
                .Include("~/content/styles/bootstrap.css")
                .Include("~/content/styles/site.css"));

            bundles.Add(new ScriptBundle("~/admin/scripts")
                .Include("~/scripts/bootstrap.bundle.js")
                .Include("~/scripts/jquery-3.3.1.js")
                .Include("~/scripts/jquery.validate.js")
                .Include("~/scripts/jquery.validate.unobtrusive.js")
                .Include("~/scripts/bootstrap.js")
                .Include("~/areas/admin/scripts/forms.js"));

            bundles.Add(new ScriptBundle("~/admin/post/scripts")
               .Include("~/areas/admin/scripts/posteditor.js"));

            bundles.Add(new ScriptBundle("~/scripts")
                .Include("~/scripts/bootstrap.bundle.js")
                .Include("~/scripts/bootstrap.js")
               .Include("~/scripts/jquery-3.3.1.js")
               .Include("~/scripts/jquery.validate.js")
               .Include("~/scripts/jquery.validate.unobtrusive.js"));

        }
    }
}