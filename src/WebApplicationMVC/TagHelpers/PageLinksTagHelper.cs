using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Store.WebApplicationMVC.Models;

namespace Store.WebApplicationMVC.TagHelpers
{
    [HtmlTargetElement("div", Attributes="page-model", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class PageLinksTagHelper : TagHelper
    {
        IUrlHelperFactory _urlHelperFactory;
        public PageLinksTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory;
        }

        public string? PageRoute { get; set; }

        [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
        public Dictionary<string, object> PageUrlValues { get; set; } = new Dictionary<string, object>();
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext? ViewContext { get; set; }
        public PagingInfo? PageModel { get; set; }
        public string PageLeftClass { get; set; }
        public string PageRightClass { get; set; }
        public string PageSelectedClass { get; set; }
        public string PageLeftSymbol { get; set; }
        public string PageRightSymbol { get; set; }
        public string? PageAction { get; set; }
        public string PageClass { get; set; }
        private string GetHrefAttributeForPageLink(int page)
        {
            IUrlHelper urlHelper = _urlHelperFactory.GetUrlHelper(this.ViewContext);
            this.PageUrlValues["page"] = page;
            return (this.PageAction is not null ?
                    urlHelper.Action(action: this.PageAction, values: this.PageUrlValues) :
                    urlHelper.RouteUrl(routeName: this.PageRoute, values: this.PageUrlValues)) + this.ViewContext.HttpContext.Request.QueryString;
        }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (this.ViewContext != null && this.PageModel != null)
            {
                TagBuilder result = new TagBuilder("div");

                TagBuilder pageLinkLeft = new TagBuilder("a");
                if (PageModel.HasPreviousPage)
                    pageLinkLeft.Attributes["href"] = GetHrefAttributeForPageLink(PageModel.Page - 1);

                pageLinkLeft.AddCssClass(PageClass);
                pageLinkLeft.AddCssClass(this.PageLeftClass);
                TagBuilder pageLinkLeftImage = new TagBuilder("i");
                pageLinkLeftImage.AddCssClass(PageLeftSymbol);
                pageLinkLeft.InnerHtml.AppendHtml(pageLinkLeftImage);

                TagBuilder pageLabel = new TagBuilder("span");
                pageLabel.AddCssClass(PageSelectedClass);
                pageLabel.InnerHtml.AppendHtml(PageModel.Page.ToString());

                TagBuilder pageLinkRight = new TagBuilder("a");
                if (PageModel.HasNextPage)
                    pageLinkRight.Attributes["href"] = GetHrefAttributeForPageLink(PageModel.Page + 1);

                pageLinkRight.AddCssClass(PageClass);
                pageLinkRight.AddCssClass(PageRightClass);
                TagBuilder pageLinkRightImage = new TagBuilder("i");
                pageLinkRightImage.AddCssClass(PageRightSymbol);
                pageLinkRight.InnerHtml.AppendHtml(pageLinkRightImage);

                result.InnerHtml.AppendHtml(pageLinkLeft);
                result.InnerHtml.AppendHtml(pageLabel);
                result.InnerHtml.AppendHtml(pageLinkRight);

                output.Content.AppendHtml(result.InnerHtml);
            }
        }
    }
}
