using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace DBCU_WebApp.Areas.Identity.Pages.Account.Manage
{
    public static class ManageNavPages
    {
        public static string Account => "Account";
        public static string Administrator => "Administrator";
        public static string OperationPlan => "OperationPlan";

        public static string News => "News";
        public static string DataAndFigures => "DataAndFigures";
        public static string Library => "Library";
        public static string ByProvince => "DBCU_WebApp.Controllers.DataFiguresController";
        public static string ByMineAction => "DBCU_WebApp.Controllers.DataFiguresController";
        
        /// <summary>
        /// 
        /// </summary>

        public static string Index => "Index";

        public static string Email => "Email";

        public static string ChangePassword => "ChangePassword";

        public static string DownloadPersonalData => "DownloadPersonalData";

        public static string DeletePersonalData => "DeletePersonalData";

        public static string ExternalLogins => "ExternalLogins";

        public static string PersonalData => "PersonalData";

        public static string TwoFactorAuthentication => "TwoFactorAuthentication";

        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

        public static string EmailNavClass(ViewContext viewContext) => PageNavClass(viewContext, Email);

        public static string ChangePasswordNavClass(ViewContext viewContext) => PageNavClass(viewContext, ChangePassword);

        public static string DownloadPersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, DownloadPersonalData);

        public static string DeletePersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, DeletePersonalData);

        public static string ExternalLoginsNavClass(ViewContext viewContext) => PageNavClass(viewContext, ExternalLogins);

        public static string PersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, PersonalData);

        public static string TwoFactorAuthenticationNavClass(ViewContext viewContext) => PageNavClass(viewContext, TwoFactorAuthentication);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewContext"></param>
        /// <returns></returns>

        public static string RegisterUser => "RegisterUser";
        public static string RegisterClass(ViewContext viewContext) => PageNavClass(viewContext, RegisterUser);
        public static string AdminRole => "AdminRole";
        public static string AdminRoles(ViewContext viewContext) => PageNavClass(viewContext, AdminRole);

        public static string AdminAddRole => "AdminAddRole";
        public static string AdminAddRoles(ViewContext viewContext) => PageNavClass(viewContext, AdminAddRole);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewContext"></param>
        /// <returns></returns>
        public static string AccountManaGement(ViewContext viewContext) => PageClass(viewContext, Account);
        public static string Admin(ViewContext viewContext) => PageClass(viewContext, Administrator);
        public static string OperationPlans(ViewContext viewContext) => PageClass(viewContext, OperationPlan);
        public static string Newss(ViewContext viewContext) => PageClass(viewContext, News);
        public static string DataAndFiguresClass(ViewContext viewContext) => PageClass(viewContext, DataAndFigures);
        public static string LibraryClass(ViewContext viewContext) => PageClass(viewContext, Library);
        public static string ByProvinceClass(ViewContext viewContext) => PageNavClass(viewContext, ByProvince);
        public static string ByMineActionClass(ViewContext viewContext) => PageNavClass(viewContext, ByMineAction);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewContext"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public static string OperationPlanOrg => "OperationPlanOrg";
        public static string OperationPlanOrgClass(ViewContext viewContext) => PageNavClass(viewContext, OperationPlanOrg);
        public static string ActivityMA => "ActivityMA";
        public static string ActivityMAClass(ViewContext viewContext) => PageNavClass(viewContext, ActivityMA);
        public static string OperationPlanWeek => "OperationPlanWeek";
        public static string OperationPlanWeekClasss(ViewContext viewContext) => PageNavClass(viewContext, OperationPlanWeek);
        public static string OperationPlanIndex => "OperationPlanIndex";
        public static string OperationPlansClass(ViewContext viewContext) => PageNavClass(viewContext, OperationPlanIndex);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewContext"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        /// 
        public static string NewsIndex => "NewsIndex";
        public static string NewsIndexClass(ViewContext viewContext) => PageNavClass(viewContext, NewsIndex);

        public static string NewsCategory => "NewsCategory";
        public static string NewsCategoryClass(ViewContext viewContext) => PageNavClass(viewContext, NewsCategory);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewContext"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        private static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
        private static string PageClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePagePar"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
     
    }
}
