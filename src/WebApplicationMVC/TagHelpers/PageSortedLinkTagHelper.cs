using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Store.WebApplicationMVC.Models;
using Microsoft.AspNetCore.Mvc.TagHelpers;

namespace Store.WebApplicationMVC.TagHelpers
{
    [HtmlTargetElement("a", Attributes = "[sorted-page-link]", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class PageSortedLinkTagHelper : TagHelper
    {
        IUrlHelperFactory _urlHelperFactory;
        public PageSortedLinkTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory;
        }

        public string PageRouteWhenSelected { get; set; }
        public string PageRouteWhenUnselected { get; set; }
        public string PageCurrentRoute { get; set; }
        public string SortingCriteriaSelectedClass { get; set; }
        public string SortingCriteriaNotSelectedClass { get; set; }
        public string SortingCriteriaBaseClass { get; set; }

        [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
        public Dictionary<string, object> PageUrlValues { get; set; } = new Dictionary<string, object>();
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext? ViewContext { get; set; }
        private string GetHrefAttributeForPageRoute(string pageRoute)
        {
            IUrlHelper urlHelper = _urlHelperFactory.GetUrlHelper(this.ViewContext);
            return urlHelper.RouteUrl(routeName: pageRoute, values: this.PageUrlValues) + this.ViewContext.HttpContext.Request.QueryString;
        }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (this.ViewContext != null)
            {

                if (PageCurrentRoute == PageRouteWhenUnselected)
                {
                    output.Attributes.SetAttribute("href", GetHrefAttributeForPageRoute(PageRouteWhenSelected));
                    output.Attributes.SetAttribute("class", $"{SortingCriteriaBaseClass} {SortingCriteriaSelectedClass}");
                }
                else
                {
                    output.Attributes.SetAttribute("href", GetHrefAttributeForPageRoute(PageRouteWhenUnselected));
                    output.Attributes.SetAttribute("class", $"{SortingCriteriaBaseClass} {SortingCriteriaNotSelectedClass}");
                }
            }
        }
    }
}
