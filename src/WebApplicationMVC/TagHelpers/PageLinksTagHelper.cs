using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Store.WebApplicationMVC.Models;

namespace Store.WebApplicationMVC.TagHelpers
{
    [HtmlTargetElement("div", Attributes = "page-model", TagStructure = TagStructure.NormalOrSelfClosing)]
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
        public string PageClass { get; set; }
        public string PageLeftSymbol { get; set; }
        public string PageRightSymbol { get; set; }
        public string? PageAction { get; set; }
        public string PageArrowClass { get; set; }
        public string IntervalSeperator { get; set; } = "...";
        public int NumberOfPagesFromSelectedPageVisible { get; set; } = 1;//length from selected page that would form an interval, one means  - "1...5 6 7...9", where six is selected page, two means - "1...5 6 7 8 9...11", where seven is selected page
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

                pageLinkLeft.AddCssClass(PageArrowClass);
                pageLinkLeft.AddCssClass(this.PageLeftClass);
                TagBuilder pageLinkLeftImage = new TagBuilder("i");
                pageLinkLeftImage.AddCssClass(PageLeftSymbol);
                pageLinkLeft.InnerHtml.AppendHtml(pageLinkLeftImage);

                //TagBuilder pageLabel = new TagBuilder("span");
                //pageLabel.AddCssClass(PageSelectedClass);
                //pageLabel.InnerHtml.AppendHtml(PageModel.Page.ToString());

                TagBuilder pageLinkRight = new TagBuilder("a");
                if (PageModel.HasNextPage)
                    pageLinkRight.Attributes["href"] = GetHrefAttributeForPageLink(PageModel.Page + 1);

                pageLinkRight.AddCssClass(PageArrowClass);
                pageLinkRight.AddCssClass(PageRightClass);
                TagBuilder pageLinkRightImage = new TagBuilder("i");
                pageLinkRightImage.AddCssClass(PageRightSymbol);
                pageLinkRight.InnerHtml.AppendHtml(pageLinkRightImage);

                result.InnerHtml.AppendHtml(pageLinkLeft);

                TagBuilder pagesLinks = new TagBuilder("div");
                List<int> visiblePagesLinks = new List<int>();

                int begin = Math.Max(1, PageModel.Page - NumberOfPagesFromSelectedPageVisible);
                int end = Math.Min(PageModel.TotalPages, PageModel.Page + NumberOfPagesFromSelectedPageVisible);
                for (int i = begin; i <= end; ++i)
                {
                    visiblePagesLinks.Add(i);
                }

                if (visiblePagesLinks.First() != 1)
                    visiblePagesLinks.Insert(0, 1);

                if (visiblePagesLinks.Last() != PageModel.TotalPages)
                    visiblePagesLinks.Add(PageModel.TotalPages);

                for(int i = 0;i<visiblePagesLinks.Count - 1; ++i)
                {
                    TagBuilder pageLink = new TagBuilder("a");
                    pageLink.AddCssClass(visiblePagesLinks[i] == PageModel.Page ? PageSelectedClass : PageClass);
                    pageLink.InnerHtml.AppendHtml(visiblePagesLinks[i] == PageModel.Page ? $"<b>{visiblePagesLinks[i].ToString()}</b>" : visiblePagesLinks[i].ToString());
                    pageLink.Attributes["href"] = GetHrefAttributeForPageLink(visiblePagesLinks[i]);
                    pagesLinks.InnerHtml.AppendHtml(pageLink);
                    if (visiblePagesLinks[i + 1] - visiblePagesLinks[i] > 1)
                    {
                        TagBuilder separator = new TagBuilder("span");
                        separator.InnerHtml.AppendHtml(IntervalSeperator);
                        pagesLinks.InnerHtml.AppendHtml(separator);
                    }
                }

                TagBuilder endPageLink = new TagBuilder("a");
                endPageLink.AddCssClass(visiblePagesLinks.Last() == PageModel.Page ? PageSelectedClass : PageClass);
                endPageLink.InnerHtml.AppendHtml(visiblePagesLinks.Last() == PageModel.Page ? $"<b>{visiblePagesLinks.Last().ToString()}</b>" : visiblePagesLinks.Last().ToString());
                endPageLink.Attributes["href"] = GetHrefAttributeForPageLink(visiblePagesLinks.Last());
                pagesLinks.InnerHtml.AppendHtml(endPageLink);

                result.InnerHtml.AppendHtml(pagesLinks);

                result.InnerHtml.AppendHtml(pageLinkRight);
                
                output.Content.AppendHtml(result.InnerHtml);
            }
        }
    }
}
