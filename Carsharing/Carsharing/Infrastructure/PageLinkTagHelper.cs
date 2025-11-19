using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using Carsharing.Models.ViewModels;

namespace Carsharing.Infrastructure
{
    [HtmlTargetElement("ul", Attributes = "page-model,page-action")]
    public class PageLinkTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory urlHelperFactory;
        public PageLinkTagHelper(IUrlHelperFactory factory)
        {
            urlHelperFactory = factory;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; } = default!;

        public PagingInfo PageModel { get; set; } = new PagingInfo();
        public string PageAction { get; set; } = string.Empty;

        [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
        public Dictionary<string, object> PageUrlValues { get; set; } = new();

        public bool PageClassesEnabled { get; set; } = true;
        public string PageItemClass { get; set; } = "page-item";
        public string PageItemClassSelected { get; set; } = "active";
        public string PageItemClassDisabled { get; set; } = "disabled";
        public string PageLinkClass { get; set; } = "page-link";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);

            output.TagName = "ul";
            var ulClasses = output.Attributes["class"]?.Value?.ToString();
            output.Attributes.SetAttribute("class", $"pagination {(ulClasses ?? string.Empty)}".Trim());

            TagBuilder BuildPageItem(string text, int? page, bool isDisabled = false, bool isActive = false)
            {
                var li = new TagBuilder("li");
                if (PageClassesEnabled)
                {
                    li.AddCssClass(PageItemClass);
                    if (isDisabled) li.AddCssClass(PageItemClassDisabled);
                    if (isActive) li.AddCssClass(PageItemClassSelected);
                }

                var a = new TagBuilder("a");
                a.InnerHtml.Append(text);
                a.AddCssClass(PageLinkClass);

                if (page.HasValue && !isDisabled)
                {
                    var routeValues = new Dictionary<string, object>(PageUrlValues)
                    {
                        ["page"] = page.Value
                    };
                    a.Attributes["href"] = urlHelper.Action(PageAction, routeValues) ?? "#";
                }
                else
                {
                    a.Attributes["href"] = "#";
                    a.Attributes["tabindex"] = "-1";
                    a.Attributes["aria-disabled"] = "true";
                }

                li.InnerHtml.AppendHtml(a);
                return li;
            }

            output.Content.AppendHtml(BuildPageItem("«", 1, PageModel.CurrentPage == 1));
            output.Content.AppendHtml(BuildPageItem("‹", PageModel.CurrentPage - 1, PageModel.CurrentPage == 1));

            int total = PageModel.TotalPages;
            int current = PageModel.CurrentPage;
            if (total <= 7)
            {
                for (int i = 1; i <= total; i++)
                {
                    output.Content.AppendHtml(BuildPageItem(i.ToString(), i, false, i == current));
                }
            }
            else
            {
                output.Content.AppendHtml(BuildPageItem("1", 1, false, current == 1));
                if (current > 3)
                {
                    output.Content.AppendHtml(BuildEllipsis());
                }

                int start = System.Math.Max(2, current - 1);
                int end = System.Math.Min(total - 1, current + 1);
                for (int i = start; i <= end; i++)
                {
                    output.Content.AppendHtml(BuildPageItem(i.ToString(), i, false, i == current));
                }

                if (current < total - 2)
                {
                    output.Content.AppendHtml(BuildEllipsis());
                }
                output.Content.AppendHtml(BuildPageItem(total.ToString(), total, false, current == total));
            }

            output.Content.AppendHtml(BuildPageItem("›", PageModel.CurrentPage + 1, PageModel.CurrentPage == total));
            output.Content.AppendHtml(BuildPageItem("»", total, PageModel.CurrentPage == total));

            TagBuilder BuildEllipsis()
            {
                var li = new TagBuilder("li");
                if (PageClassesEnabled)
                {
                    li.AddCssClass(PageItemClass);
                    li.AddCssClass(PageItemClassDisabled);
                }
                var span = new TagBuilder("span");
                span.InnerHtml.Append("…");
                span.AddCssClass(PageLinkClass);
                li.InnerHtml.AppendHtml(span);
                return li;
            }
        }
    }
}
