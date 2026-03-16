using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace KumariCinemas
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SetActiveNavItem();
        }

        private void SetActiveNavItem()
        {
            string path = Request.AppRelativeCurrentExecutionFilePath ?? string.Empty;
            path = path.ToLowerInvariant();

            var navMap = new Dictionary<string, HtmlAnchor>(StringComparer.OrdinalIgnoreCase)
            {
                { "~/default", navHome },
                { "~/", navHome },
                { "~/userdetails", navUsers },
                { "~/theatercityhalldetails", navTheaters },
                { "~/moviedetails", navMovies },
                { "~/showtimesdetails", navShowtimes },
                { "~/ticketdetails", navTickets },
                { "~/userticket", navUserTicket },
                { "~/theatercityhallmovie", navTheaterMovie },
                { "~/movietheatercityhalloccupancyperformer", navOccupancy }
            };

            string normalized = path.EndsWith(".aspx", StringComparison.OrdinalIgnoreCase)
                ? path.Substring(0, path.Length - 5)
                : path;

            if (normalized == "~")
                normalized = "~/";

            HtmlAnchor activeLink;
            if (navMap.TryGetValue(normalized, out activeLink))
                MarkAsActive(activeLink);
        }

        private static void MarkAsActive(HtmlAnchor link)
        {
            string existingClass = link.Attributes["class"] ?? string.Empty;
            if (existingClass.IndexOf("active", StringComparison.OrdinalIgnoreCase) >= 0)
                return;

            link.Attributes["class"] = (existingClass + " active").Trim();
            link.Attributes["aria-current"] = "page";
        }
    }
}